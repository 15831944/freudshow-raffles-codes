using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Timers;
using System.Data.OracleClient;
using DetailInfo.Categery;
using System.Threading;
using System.Diagnostics;


namespace DetailInfo
{
    public delegate void formrefresh();
    /// <summary>
    /// 
    /// </summary>
    public partial class MDIForm : Form
    {
        TaskbarNotifier taskbarNotifier;
        public static MDIForm pMainWin = null;
        public static ToolStrip  tool_strip= null;
        public static ToolStripButton stribtn = null;
        public static TreeView treeview = null;

        //bool rightClickNode = false;
        DateTime startTime;
        TreeNode dragDropTreeNode;
        public MDIForm()
        {
            InitializeComponent();
            pMainWin  =  this;//�����ǳ�ʼ��
            tool_strip = this.toolStrip1;
            treeview = this.treeView1;
            this.skinEngine1.SkinFile = Application.StartupPath + "\\Resources\\office2007.ssk";
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);//��������������������˸
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            for (int i = 0; i < contextMenuStrip1.Items.Count; i++)
            {
                contextMenuStrip1.Items[i].Visible = false;
            }
        }
        
        /// <summary>
        /// ���õ�����Ϣ���ڱ����Լ��¼�
        /// </summary>
        private void SetTaskBar()
        {
            taskbarNotifier = new TaskbarNotifier();
            taskbarNotifier.SetBackgroundBitmap(new Bitmap(User.rootpath + "\\PopupWindow\\skin.bmp"), Color.FromArgb(255, 0, 255));
            taskbarNotifier.SetCloseBitmap(new Bitmap(User.rootpath + "\\PopupWindow\\close.bmp"), Color.FromArgb(255, 0, 255), new Point(187, 8));
            taskbarNotifier.TitleRectangle = new Rectangle(40, 9, 80, 55);
            taskbarNotifier.ContentRectangle = new Rectangle(8, 41, 183, 68);
            taskbarNotifier.TitleClick += new EventHandler(TitleClick);
            taskbarNotifier.CloseClick += new EventHandler(CloseClick);
        }

        private void MDIForm_Load(object sender, EventArgs e)
        {
            System.DateTime dt = System.DateTime.Now;
            SearchtoolStripButton3.Enabled = false;
            ExcelStripButton.Enabled = false;
            SetStatus();

            #region ��������ʾ
            string sql;
            if (User.cur_user == "plm")
                sql = "select * from treenodes_tab t order by t.parent_index";
            else
            {
                string privilegelist = User.FindPrivilegeidList(User.cur_user);
                if (privilegelist == string.Empty)
                {
                    sql = "select * from treenodes_tab t where t.flag='N' order by t.parent_index";
                    MessageBox.Show("��û��Ȩ���������Ա��ϵ");
                }
                else
                    sql = "select * from treenodes_tab t where t.flag='N' or (t.flag='Y' and t.id in (select p.node_id from privilege_node_tab p where p.privilege_id in (" + privilegelist + ")))  order by t.parent_index";
            }
            CreateTreeView(sql, treeView1.Nodes);
            if (treeView1.Nodes.Count != 0)
            {
                treeView1.Nodes[0].Expand();
            }
            //treeView1.ExpandAll();
            #endregion 

        }
        //WINFORM����:
        /**/
        /// <summary>
        /// ��̬����TreeView
        /// </summary>
        /// <param name="sqlText">�����SQL���</param>
        /// <param name="nodes">TreeView�ڵ㼯</param>
        public void CreateTreeView(string sqlText, System.Windows.Forms.TreeNodeCollection nodes)
        {
                DataTable dbTable = new DataTable();//ʵ����һ��DataTable���ݱ����
                try
                {
                    OracleConnection con = new OracleConnection(DataAccess.OIDSConnStr);
                    con.Open();
                    OracleDataAdapter oda = new OracleDataAdapter(sqlText, con);
                    OracleCommandBuilder builder = new OracleCommandBuilder(oda);
                    DataSet ds = new DataSet();
                    oda.Fill(ds);
                    dbTable = ds.Tables[0];
                    con.Close();
                    ds.Dispose();
                }
                catch (OracleException ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                    return;
                }
                //����һ���˵�ȡ������TreeView�Ľڵ�,��Ϊ�ݹ��������ڵݹ��ѯ��TreeView�����нڵ�����
                CreateTreeViewRecursive(nodes, dbTable, 0);
        }
        /**/
        /// <summary>
        /// �ݹ��ѯ
        /// </summary>
        /// <param name="nodes">TreeView�ڵ㼯��</param>
        /// <param name="dataSource">����Դ</param>
        /// <param name="parentid">��һ���˵��ڵ��ʶ��</param>
        public void CreateTreeViewRecursive(System.Windows.Forms.TreeNodeCollection nodes, DataTable dataSource, int parentid)
        {
            string filter;//����һ��������
            filter = string.Format("Parent_Id={0}", parentid);
            DataRow[] drarr = dataSource.Select(filter);//�����˵�ID����������
            TreeNode node;
            foreach (DataRow dr in drarr)//�ݹ�ѭ����ѯ������
            {
                node = new TreeNode();
                node.Text = dr["text"].ToString();
                node.Name = dr["name"].ToString();
                node.ImageIndex = Convert.ToInt32(dr["ImageIndex"]);
                node.SelectedImageIndex = Convert.ToInt32(dr["SelectedImageIndex"]);
                node.Tag = Convert.ToInt32(dr["Id"]);
                nodes.Add(node);//����ڵ�
                CreateTreeViewRecursive(node.Nodes, dataSource, Convert.ToInt32(node.Tag));
            }
        }

        /// <summary>
        /// �ر�������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MDIForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result_close;
            result_close = MessageBox.Show("ȷ��Ҫ�˳�ϵͳ��", "Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result_close == DialogResult.OK)
            {
                if (this.MdiChildren.Length != 0)
                {
                    foreach (Form form in this.MdiChildren)
                    {
                        form.Close();
                        this.SearchtoolStripButton3.Enabled = false;
                    }
                }
                //string sqlStr = "update user_tab set loginstate = 'N' where name = '" + User.cur_user + "'";
                //User.UpdateCon(sqlStr, DataAccess.OIDSConnStr);
                //this.Dispose();
                
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void MDIForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        private void �˳�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("ȷ��Ҫ�˳�ϵͳ��", "�˳�ϵͳ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                if (this.MdiChildren.Length != 0)
                {
                    foreach (Form form in this.MdiChildren)
                    {
                        form.Close();
                        this.SearchtoolStripButton3.Enabled = false;
                    }
                }

                //string sqlStr = "update user_tab set loginstate = 'N' where name = '"+User.cur_user+"'";
                //User.UpdateCon(sqlStr, DataAccess.OIDSConnStr);
                this.Dispose();
            }
            if (result == DialogResult.Cancel)
            {
                return;
            }
        }

        /// <summary>
        /// ������LISTVIEW˫���¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_DoubleClick_1(object sender, EventArgs e)
        {
            #region ��·���ݲ�ѯ����
            //�����СƱ
            if (treeView1.SelectedNode.Name == "DesignSpoolOverView")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "���СƱ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }

                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "���СƱ����";
                sgvf.MdiParent = this;
                sgvf.Show();
                SearchtoolStripButton3.Enabled = true;
            }

            else if (treeView1.SelectedNode.Name == "ECDRAWINGREPORT")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��Ŀͼֽ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                ECDrawingReportFrm ecreport = new ECDrawingReportFrm();
                ecreport.Text = "��Ŀͼֽ����";
                ecreport.MdiParent = this;
                ecreport.Show();
            }

            else if (treeView1.SelectedNode.Name == "FrontPageUpdate")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "���·���")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                InsertBlob insertFrom = new InsertBlob();
                insertFrom.MdiParent = this;
                insertFrom.Show();
            }

            if (treeView1.SelectedNode.Name == "ValveOverView")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "������Ϣ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }

                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "������Ϣ����";
                sgvf.MdiParent = this;
                sgvf.Show();
                SearchtoolStripButton3.Enabled = true;
            }


            //�򿪼ӹ�СƱ
            else if (treeView1.SelectedNode.Name == "MachineSpoolOverView")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "�ӹ�СƱ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }

                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "�ӹ�СƱ����";
                sgvf.MdiParent = this;
                sgvf.Show();
                SearchtoolStripButton3.Enabled = true;
            }




            //�򿪹�ʱͳ��СƱ
            else if (treeView1.SelectedNode.Name == "NormHour")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "�ܼӹ���ʱ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "�ܼӹ���ʱ����";
                sgvf.MdiParent = this;
                sgvf.Show();
                SearchtoolStripButton3.Enabled = true;
            }




            //���ʼ�СƱ
            else if (treeView1.SelectedNode.Name == "QCSpoolOverView")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "�ʼ�СƱ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "�ʼ�СƱ����";
                sgvf.MdiParent = this;
                sgvf.Show();
                SearchtoolStripButton3.Enabled = true;
            }


            //���Ѿ�ͨ���ʼ��СƱ
            else if (treeView1.SelectedNode.Name == "QCCheckedOverView")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "����ͨ��СƱ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "����ͨ��СƱ����";
                sgvf.MdiParent = this;
                sgvf.Show();
                SearchtoolStripButton3.Enabled = true;

            }

            //������Ϣ����
            else if (treeView1.SelectedNode.Name == "MaterialOverView")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "������Ϣ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "������Ϣ����";
                sgvf.MdiParent = this;
                sgvf.Show();
                SearchtoolStripButton3.Enabled = true;
            }
            
            //���Ӽ���Ϣ����
            else if (treeView1.SelectedNode.Name == "ConnectorOverView")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "���Ӽ���Ϣ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "���Ӽ���Ϣ����";
                sgvf.MdiParent = this;
                sgvf.Show();
                SearchtoolStripButton3.Enabled = true;
            }
            
            //�ӹ���Ϣ����
            else if (treeView1.SelectedNode.Name == "MachineOverView")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "�ӹ���Ϣ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "�ӹ���Ϣ����";
                sgvf.MdiParent = this;
                sgvf.Show();
                SearchtoolStripButton3.Enabled = true;
            }

           //������Ϣ����
            else if (treeView1.SelectedNode.Name == "WeldOverView")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "������Ϣ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "������Ϣ����";
                sgvf.MdiParent = this;
                sgvf.Show();
                SearchtoolStripButton3.Enabled = true;
            }
            

            ///��·ͳ��
            else if (treeView1.SelectedNode.Name == "PipeStatistics")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��·ͳ����Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "��·ͳ����Ϣ";
                sgvf.MdiParent = this;
                sgvf.Show();
                SearchtoolStripButton3.Enabled = true;

            }

            ///��·����ͳ��
            else if (treeView1.SelectedNode.Name == "PartStatistics")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��·����ͳ����Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "��·����ͳ����Ϣ";
                sgvf.MdiParent=this;
                sgvf.Show();
                SearchtoolStripButton3.Enabled = true;
            }
            
            
             //��Ŀ����ͳ��
            else if (treeView1.SelectedNode.Name == "ProgressInfo")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��Ŀ��·���ȷ���")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                ProgressStatistics psform = new ProgressStatistics();
                psform.Text = "��Ŀ��·���ȷ���";
                psform.MdiParent = this;
                psform.Show();
                this.toolStrip1.Items[0].Enabled = false;
            }

            //�������������
            if (treeView1.SelectedNode.Name == "MaterialRequirement")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "�����������")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                BatchSheet bsform = new BatchSheet();
                bsform.MdiParent = this;
                bsform.Show();
                SearchtoolStripButton3.Enabled = false;
            }

            if (treeView1.SelectedNode.Name == "AddNewWorker")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��ӳ�����Ա������Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                WorkShopWorkerData wwdform = new WorkShopWorkerData();
                wwdform.MdiParent = this;
                wwdform.Show();
                SearchtoolStripButton3.Enabled = false;
            }

            //��Ŀͼֽ����
            else if (treeView1.SelectedNode.Name == "PROJECTDRAWINGOVERVIEW")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��Ŀͼֽ�Ÿ���")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                PROJECTDRAWINGINFO pdform = new PROJECTDRAWINGINFO();
                pdform.Text = "��Ŀͼֽ�Ÿ���";
                pdform.MdiParent = this;
                pdform.Show();
                this.toolStrip1.Items[0].Enabled = false;
            }

            //�ѷ�ͼֽ���϶����
            else if (treeView1.SelectedNode.Name == "MATERIALRATION")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "�ѷ�ͼֽ���϶����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "�ѷ�ͼֽ���϶����";
                sgvf.MdiParent = this;
                sgvf.Show();
                SearchtoolStripButton3.Enabled = true;
            }

#endregion 

            #region ��̨����ά������
            else if (treeView1.SelectedNode.Name == "Bend")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��ͷ�б�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DataMaintenanceForm dmf = new DataMaintenanceForm();
                dmf.Text = "��ͷ�б�";
                dmf.MdiParent = this;
                dmf.Show();
            }

            else if (treeView1.SelectedNode.Name == "Cabin")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "�����б�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DataMaintenanceForm dmf = new DataMaintenanceForm();
                dmf.Text = "�����б�";
                dmf.MdiParent = this;
                dmf.Show();
            }

            else if (treeView1.SelectedNode.Name == "Connector")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "���Ӽ��б�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DataMaintenanceForm dmf = new DataMaintenanceForm();
                dmf.Text = "���Ӽ��б�";
                dmf.MdiParent = this;
                dmf.Show();
            }

            else if (treeView1.SelectedNode.Name == "ElbowMaterial")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��ͷ���϶����б�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DataMaintenanceForm dmf = new DataMaintenanceForm();
                dmf.Text = "��ͷ���϶����б�";
                dmf.MdiParent = this;
                dmf.Show();
            }

            else if (treeView1.SelectedNode.Name == "PSTAD")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��ģ�б�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DataMaintenanceForm dmf = new DataMaintenanceForm();
                dmf.Text = "��ģ�б�";
                dmf.MdiParent = this;
                dmf.Show();
            }

            else if (treeView1.SelectedNode.Name == "SocketElbow")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "�н���ͷ�б�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DataMaintenanceForm dmf = new DataMaintenanceForm();
                dmf.Text = "�н���ͷ�б�";
                dmf.MdiParent = this;
                dmf.Show();
            }

            else if (treeView1.SelectedNode.Name == "Surface")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "���洦���б�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DataMaintenanceForm dmf = new DataMaintenanceForm();
                dmf.Text = "���洦���б�";
                dmf.MdiParent = this;
                dmf.Show();
            }

            else if (treeView1.SelectedNode.Name == "System")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "ϵͳ�б�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DataMaintenanceForm dmf = new DataMaintenanceForm();
                dmf.Text = "ϵͳ�б�";
                dmf.MdiParent = this;
                dmf.Show();
            }

            else if (treeView1.SelectedNode.Name == "Approver")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "������б�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DataMaintenanceForm dmf = new DataMaintenanceForm();
                dmf.Text = "������б�";
                dmf.MdiParent = this;
                dmf.Show();
            }
           
            else if (treeView1.SelectedNode.Name == "BAITINGNORM_METALPIPE")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "������������ʱ�䶨���б�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DataMaintenanceForm dmf = new DataMaintenanceForm();
                dmf.Text = "������������ʱ�䶨���б�";
                dmf.MdiParent = this;
                dmf.Show();
            }
            else if (treeView1.SelectedNode.Name == "BAITINGNORM_PEPPH")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "PE&PPH�ܹ�ʱ�����б�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DataMaintenanceForm dmf = new DataMaintenanceForm();
                dmf.Text = "PE&PPH�ܹ�ʱ�����б�";
                dmf.MdiParent = this;
                dmf.Show();
            }
            else if (treeView1.SelectedNode.Name == "BEVEL_HOUR_NORM")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "�¿ڼӹ�ʱ�䶨���б�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DataMaintenanceForm dmf = new DataMaintenanceForm();
                dmf.Text = "�¿ڼӹ�ʱ�䶨���б�";
                dmf.MdiParent = this;
                dmf.Show();
            }
            else if (treeView1.SelectedNode.Name == "ELBOW_HOUR_NORM")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "���ʱ�䶨���б�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DataMaintenanceForm dmf = new DataMaintenanceForm();
                dmf.Text = "���ʱ�䶨���б�";
                dmf.MdiParent = this;
                dmf.Show();
            }
            else if (treeView1.SelectedNode.Name == "PIPECHECKING_HOUR_NORM")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "У��ʱ�䶨���б�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DataMaintenanceForm dmf = new DataMaintenanceForm();
                dmf.Text = "У��ʱ�䶨���б�";
                dmf.MdiParent = this;
                dmf.Show();
            }



             #endregion

            #region ����ƻ�
            else if (treeView1.SelectedNode.Name == "WorkShopPlan")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "����ƻ�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                WorkShopPlan wspfrm = new WorkShopPlan();
                wspfrm.Text = "����ƻ�";
                wspfrm.MdiParent = this;
                wspfrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "WorkShopPlanDetails")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "����ƻ���ϸ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                WorkShopPlanDetail wspdfrm = new WorkShopPlanDetail();
                wspdfrm.Text = "����ƻ���ϸ";
                wspdfrm.MdiParent = this;
                wspdfrm.Show();
            }
            #endregion
            else if (treeView1.SelectedNode.Name == "MaterialManagement")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "������Ϣ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                inventoryFrm inventoryform = new inventoryFrm();
                inventoryform.Text = "������Ϣ����";
                inventoryform.MdiParent = this;
                inventoryform.Show();
            }
            #region �����·׷��
            else if (treeView1.SelectedNode.Name == "SpoolTrace")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��·׷��")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                WShopSPlTrace tracefrm = new WShopSPlTrace();
                tracefrm.Text = "��·׷��";
                tracefrm.MdiParent = this;
                tracefrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "MaterialTreaceability")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "���鵥��Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                MaterialTraceabilityFrm traceabilityFrm = new MaterialTraceabilityFrm();
                traceabilityFrm.Text = "���鵥��Ϣ";
                traceabilityFrm.MdiParent = this;
                traceabilityFrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "ModifyStatistics")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��ǰ�����޸���Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                ModifyStatisticsFrm msFrm = new ModifyStatisticsFrm();
                msFrm.Text = "��ǰ�����޸���Ϣ";
                msFrm.MdiParent = this;
                msFrm.Show();
            }
                
            
            #endregion

            #region ���乤ʱ�����
            else if (treeView1.SelectedNode.Name == "MaterialPreparation")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "���Ϲ�ʱ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                WShopManHour manhourfrm = new WShopManHour();
                manhourfrm.Text = "���Ϲ�ʱ����";
                manhourfrm.MdiParent = this;
                manhourfrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "MaterialAssembly")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "װ�乤ʱ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                WShopManHour manhourfrm = new WShopManHour();
                manhourfrm.Text = "װ�乤ʱ����";
                manhourfrm.MdiParent = this;
                manhourfrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "MaterialWeld")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "���ӹ�ʱ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                WShopManHour manhourfrm = new WShopManHour();
                manhourfrm.Text = "���ӹ�ʱ����";
                manhourfrm.MdiParent = this;
                manhourfrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "MaterialQC")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "���鹤ʱ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                WShopManHour manhourfrm = new WShopManHour();
                manhourfrm.Text = "���鹤ʱ����";
                manhourfrm.MdiParent = this;
                manhourfrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "MaterialTransport")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "�ϳ���ʱ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                WShopManHour manhourfrm = new WShopManHour();
                manhourfrm.Text = "�ϳ���ʱ����";
                manhourfrm.MdiParent = this;
                manhourfrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "MaterialPressTest")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "ѹ�����鹤ʱ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                WShopManHour manhourfrm = new WShopManHour();
                manhourfrm.Text = "ѹ�����鹤ʱ����";
                manhourfrm.MdiParent = this;
                manhourfrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "NormalPipeWT")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��̼ͨ�ֹ�(ͭ��)����ͨ����ֹ��ܹ�ʱ����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                NormalPipeWorkingTime normalWTfrm = new NormalPipeWorkingTime();
                normalWTfrm.Text = "��̼ͨ�ֹ�(ͭ��)����ͨ����ֹ��ܹ�ʱ����";
                normalWTfrm.MdiParent = this;
                normalWTfrm.Show();
            }
            #endregion
            #region ECDMS�û�������Ϣ
            else if (treeView1.SelectedNode.Name == "ImportComments")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "���������Ϣ�������")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DetailInfo.ECDMS.ImportComment commentform = new ECDMS.ImportComment();
                commentform.Text = "���������Ϣ�������";
                commentform.MdiParent = this;
                commentform.Show();
            }
            else if (treeView1.SelectedNode.Name == "ImportDrawingPlan")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "ͼֽ��ͼֽ�ƻ�����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                ImportDrawingPlanFrm importDrawPlanfrom = new ImportDrawingPlanFrm();
                importDrawPlanfrom.Text = "ͼֽ��ͼֽ�ƻ�����";
                importDrawPlanfrom.MdiParent = this;
                importDrawPlanfrom.Show();
            }

            else if (treeView1.SelectedNode.Name == "ImportEquipment")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "�豸����")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                ImportEquipmentFrm importEquipmentform = new ImportEquipmentFrm();
                importEquipmentform.Text = "�豸����";
                importEquipmentform.MdiParent = this;
                importEquipmentform.Show();
            }

            else if (treeView1.SelectedNode.Name == "CablesVolume")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "���²�")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                CablesVolumeFrm cablesform = new CablesVolumeFrm();
                cablesform.Text = "���²�";
                cablesform.MdiParent = this;
                cablesform.Show();
            }
            else if (treeView1.SelectedNode.Name == "ImportCable")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "���µ���")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                DetailInfo.ECDMS.ProjectDrawingCableFrm pdcable = new ECDMS.ProjectDrawingCableFrm();
                pdcable.Text = "���µ���";
                pdcable.MdiParent = this;
                pdcable.Show();
            }

            #endregion
            #region ECDMS�û����Ϲ�����Ϣ

            else if (treeView1.SelectedNode.Name == "MatParaSet")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "����Ԥ��")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                M_Estimate matreqadd = new M_Estimate();
                matreqadd.MdiParent = this;
                matreqadd.Text = "����Ԥ��";
                matreqadd.Show();

            }
            else if (treeView1.SelectedNode.Name == "MatParaSummary")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "����Ԥ������")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                frmPartParaSet matreqadd = new frmPartParaSet();
                matreqadd.MdiParent = this;
                matreqadd.Text = "����Ԥ������";
                matreqadd.Show();

            }
            else if (treeView1.SelectedNode.Name == "MatParaConfirm")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "����Ԥ��ȷ��")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                frmPartParaConfirm matreqadd = new frmPartParaConfirm();
                matreqadd.MdiParent = this;
                matreqadd.Text = "����Ԥ��ȷ��";
                matreqadd.Show();

            }

            else if (treeView1.SelectedNode.Name == "MatRequire")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��������ҳ��")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                MaterialManage.MaterialRequireAdd matreqadd = new MaterialManage.MaterialRequireAdd("0","1");
                matreqadd.MdiParent = this;
                matreqadd.Text = "��������ҳ��";
                matreqadd.Show();

            }
            else if (treeView1.SelectedNode.Name == "ERPCode_Manage")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "�������")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                frmPartAdd matadd = new frmPartAdd();
                matadd.MdiParent = this;
                matadd.Text = "�������";
                matadd.Show();

            }
            else if (treeView1.SelectedNode.Name == "ConvertMSS")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "ת����׼��ʽMSS")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                ConvertStandard matadd = new ConvertStandard();
                matadd.MdiParent = this;
                matadd.Text = "ת����׼��ʽMSS";
                matadd.Show();

            }
            else if (treeView1.SelectedNode.Name == "ERP_Inventory")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��Ŀ����ѯ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                frmPartInventory matadd = new frmPartInventory();
                matadd.MdiParent = this;
                matadd.Text = "��Ŀ����ѯ";
                matadd.Show();

            }

            else if (treeView1.SelectedNode.Name == "MatReqSummary")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "MEO��ѯ������")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                MaterialManage.MaterialRequireQuery matadd = new MaterialManage.MaterialRequireQuery();
                matadd.MdiParent = this;
                matadd.Text = "MEO��ѯ������";
                matadd.Show();

            }
            else if (treeView1.SelectedNode.Name == "MatRation")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "�����·�ҳ��")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                MaterialManage.MaterialRation matadd = new MaterialManage.MaterialRation("0","1");
                matadd.MdiParent = this;
                matadd.Text = "�����·�ҳ��";
                matadd.Show();
            }

            #endregion
            #region �ܼӹ�����ϸ��
            else if (treeView1.SelectedNode.Name == "MaterialBlankingInfo")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "������Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                MachinningInfoFrm machineInfoFrm = new MachinningInfoFrm();
                machineInfoFrm.Text = "������Ϣ";
                machineInfoFrm.MdiParent = this;
                machineInfoFrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "MaterialAssembleInfo")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "װ����Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                MachinningInfoFrm machineInfoFrm = new MachinningInfoFrm();
                machineInfoFrm.Text = "װ����Ϣ";
                machineInfoFrm.MdiParent = this;
                machineInfoFrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "MaterialWeldInfo")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "������Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                MachinningInfoFrm machineInfoFrm = new MachinningInfoFrm();
                machineInfoFrm.Text = "������Ϣ";
                machineInfoFrm.MdiParent = this;
                machineInfoFrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "MaterialQCInfo")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "������Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                MachinningInfoFrm machineInfoFrm = new MachinningInfoFrm();
                machineInfoFrm.Text = "������Ϣ";
                machineInfoFrm.MdiParent = this;
                machineInfoFrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "MaterialTreatmentInfo")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "������Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                MachinningInfoFrm machineInfoFrm = new MachinningInfoFrm();
                machineInfoFrm.Text = "������Ϣ";
                machineInfoFrm.MdiParent = this;
                machineInfoFrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "MaterialRecieveInfo")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "�ϳ�������Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                MachinningInfoFrm machineInfoFrm = new MachinningInfoFrm();
                machineInfoFrm.Text = "�ϳ�������Ϣ";
                machineInfoFrm.MdiParent = this;
                machineInfoFrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "MaterialDeliveryInfo")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "������Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                MachinningInfoFrm machineInfoFrm = new MachinningInfoFrm();
                machineInfoFrm.Text = "������Ϣ";
                machineInfoFrm.MdiParent = this;
                machineInfoFrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "MaterialSetInfo")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��װ��Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                MachinningInfoFrm machineInfoFrm = new MachinningInfoFrm();
                machineInfoFrm.Text = "��װ��Ϣ";
                machineInfoFrm.MdiParent = this;
                machineInfoFrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "MaterialTrayClass")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "���̼�������Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                MachinningInfoFrm machineInfoFrm = new MachinningInfoFrm();
                machineInfoFrm.Text = "���̼�������Ϣ";
                machineInfoFrm.MdiParent = this;
                machineInfoFrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "SpoolReportCollection")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "��·�������")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                SpoolReportFrm spoolreportFrm = new SpoolReportFrm();
                spoolreportFrm.Text = "��·�������";
                spoolreportFrm.MdiParent = this;
                spoolreportFrm.Show();
            }

            else if (treeView1.SelectedNode.Name == "MaterialReports")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "���ϱ������")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                MaterialReportsFrm materialreportFrm = new MaterialReportsFrm();
                materialreportFrm.Text = "���ϱ������";
                materialreportFrm.MdiParent = this;
                materialreportFrm.Show();
            }
            else if (treeView1.SelectedNode.Name == "MDrawingInfo")
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Text == "�޸�ɾ����Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        return;
                    }
                }
                ModifyDelInfoFrm mdelfrm = new ModifyDelInfoFrm();
                mdelfrm.Text = "�޸�ɾ����Ϣ";
                mdelfrm.MdiParent = this;
                mdelfrm.Show();
            }
            #endregion



        }

        /// <summary>
        /// ��С�����д���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ������С��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArrayList minarray = new ArrayList();
            foreach (Form form in this.MdiChildren)
            {
                minarray.Add(form);
            }
            if (minarray.Count == 0)
            {
                return;
            }
            if (minarray.Count != 0)
            {
                for (int i = 0; i < minarray.Count; i++)
                {
                    (minarray[i] as Form).WindowState = FormWindowState.Minimized;
                }
            }

        }

        /// <summary>
        /// �ر����д���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void �ر�����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Length != 0)
            {
                foreach (Form form in this.MdiChildren)
                {
                    form.Close();
                    this.SearchtoolStripButton3.Enabled = false;
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// �����Ϣ���������趨
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void ApproveTimer_Tick(object sender, EventArgs e)
        //{
            //ApproveTimer.Start();
            //string sqlstr = "SELECT COUNT(*) FROM SP_SPOOL_TAB T WHERE T.DRAWINGNO IN (SELECT S.DRAWING_NO FROM PLM.PROJECT_DRAWING_TAB S WHERE S.DRAWING_TYPE IS NULL AND DOCTYPE_ID IN (7) AND LASTFLAG = 'Y' AND NEW_FLAG = 'Y' AND DELETE_FLAG = 'N' AND S.RESPONSIBLE_USER = '"+User.cur_user+"') AND T.FLOWSTATUS = 6 AND T.FLAG = 'Y' AND T.LOGNAME='"+User.cur_user+"'";
            //object obj = User.GetScalar(sqlstr, DataAccess.OIDSConnStr);
            ////MessageBox.Show(obj.ToString());

            //if (obj == null || int.Parse(obj.ToString()) == 0)
            //{
            //    return;
            //}

            //if (obj != 0)
            //{
                //SetTaskBar();
                //taskbarNotifier.Show("��������", "���дӼӹ�����������СƱ��Ҫ����", 500, 5000, 500);
            //}














            //object obj = null;
            //string sql = string.Empty;

            //if (UserSecurity.HavingPrivilege(User.cur_user, "SPL_APPROVE_DES"))
            //{
            //    //sql = "SELECT  * FROM SPOOL_APPROVE_TAB WHERE  ASSESOR = '" + User.cur_user + "' AND STATE = 0 AND APPROVENEEDFLAG = 'Y'";
            //    sql = "SELECT  * FROM PIPEAPPROVE_TAB WHERE  ASSESOR = '" + User.cur_user + "' AND STATE = 0 AND APPROVENEEDFLAG = 'Y'";
            //    obj = User.GetScalar(sql, DataAccess.OIDSConnStr);
            //    if (obj != null)
            //    {
            //        SetTaskBar();
            //        taskbarNotifier.ContentClick += new EventHandler(ContentClick);
            //        taskbarNotifier.Show("��������", "���д���СƱ", 500, 5000, 500);
            //    }
            //}

            //else if (UserSecurity.HavingPrivilege(User.cur_user, "SPOOLCONSTRUCTIONDESIGNUSERS"))
            //{
            //    //string sql1 = "SELECT * FROM SPFLOWLOG_TAB WHERE FLOWSTATUS =3 AND USERNAME = '" + User.cur_user + "' AND SPOOLNAME IN (SELECT SPOOLNAME FROM SPFLOWLOG_TAB WHERE FLOWSTATUS = 6) ";
            //    sql = "select * from SP_SPOOL_TAB where spoolname in (SELECT s.SPOOLNAME as СƱ�� from spflowlog_tab S where s.spoolname in (select DISTINCT t.spoolname  from spflowlog_tab t where t.username = '" + User.cur_user + "' and t.flowstatus = " + (int)FlowState.������ + ") and s.flowstatus = " + (int)FlowState.������� + ")  and flag = 'Y' AND flowstatus = 6";
            //    obj = User.GetScalar(sql, DataAccess.OIDSConnStr);
            //    if (obj != null)
            //    {
            //        SetTaskBar();
            //        taskbarNotifier.ContentClick += new EventHandler(ContentClick);
            //        taskbarNotifier.Show("��������", "���дӼӹ�����������СƱ��Ҫ����", 500, 5000, 500);
            //    }

            //}

            //if (UserSecurity.HavingPrivilege(User.cur_user, "SPOOLDESIGNUSERS"))
            //{
            //    //string sql2 = "SELECT * FROM SP_SPOOL_TAB t WHERE t.FLAG = 'Y' AND t.FLOWSTATUS = 5  AND t.SPOOLNAME  in   (SELECT s.SPOOLNAME FROM SPFLOWLOG_TAB s WHERE s.FLOWSTATUS =1 AND s.USERNAME = '" + User.cur_user + "' AND s.SPOOLNAME IN  (SELECT z.SPOOLNAME FROM SPFLOWLOG_TAB z WHERE z.FLOWSTATUS = 5 ) ) ";
            //    //sql = "select * from SP_SPOOL_TAB where SPOOLNAME in ( SELECT  s.SPOOLNAME  from spflowlog_tab S where s.spoolname in (select DISTINCT t.spoolname  from spflowlog_tab t where t.username = '" + User.cur_user + "' and t.flowstatus = " + (int)FlowState.���� + ") and s.flowstatus = " + (int)FlowState.��˷��� + ") and flag = 'Y' and flowstatus = 5";
            //    sql = "SELECT T.* FROM SP_SPOOL_TAB T WHERE T.FLOWSTATUS = 5 AND T.FLAG = 'Y' AND T.DRAWINGNO IN (SELECT S.DRAWINGNO from spflowlog_tab S where S.DRAWINGNO in (select DISTINCT A.DRAWINGNO from spflowlog_tab A where A.username = 'yang.yang' and A.flowstatus = 1) and S.flowstatus = 5) AND T.Projectid IN (SELECT S.Projectid from spflowlog_tab S where S.Projectid in (select DISTINCT A.Projectid from spflowlog_tab A where A.username = '"+User.cur_user+"' and A.flowstatus = 1) and S.flowstatus = 5)";
            //    obj = User.GetScalar(sql, DataAccess.OIDSConnStr);
            //    if (obj != null)
            //    {
            //        SetTaskBar();
            //        taskbarNotifier.ContentClick += new EventHandler(ContentClick);
            //        taskbarNotifier.Show("��������", "�������δͨ����СƱ��Ҫ����", 500, 5000, 500);
            //    }

            //}




        //}


        void PopUpInformation()
        {
            SetTaskBar();
            taskbarNotifier.ContentClick += new EventHandler(ContentClick);
            taskbarNotifier.Show("��������", "���дӼӹ�����������СƱ��Ҫ����", 500, 5000, 500);
        }

        void TitleClick(object obj, EventArgs ea)
        {
        //    MessageBox.Show("Title was Clicked");
        }
        void CloseClick(object obj, EventArgs ea)
        {
            //
        }

        void ContentClick(object obj, EventArgs ea)
        {
            //this.Activate(); this.WindowState = FormWindowState.Maximized;
            //if (UserSecurity.HavingPrivilege(User.cur_user, "SPL_APPROVE_DES"))
            //{
            //    foreach (Form form in this.MdiChildren)
            //    {
            //        if (form.Text == "��˴���")
            //        {
            //            if (form.WindowState == FormWindowState.Minimized)
            //            {
            //                form.WindowState = FormWindowState.Normal;
            //            }
            //            form.Activate();
            //            return;
            //        }
            //    }
            //    ApproveForm af = new ApproveForm();
            //    af.Text = "��˴���";
            //    af.MdiParent = this;
            //    af.Show();
            //}


            //else if (UserSecurity.HavingPrivilege(User.cur_user, "SPOOLCONSTRUCTIONDESIGNUSERS"))
            //{
            //    foreach (Form form in this.MdiChildren)
            //    {
            //        if (form.Name == "MachiningFeedback")
            //        {
            //            if (form.WindowState == FormWindowState.Minimized)
            //            {
            //                form.WindowState = FormWindowState.Normal;

            //            }
            //            form.Activate();
            //            return;
            //        }
            //    }
            //    MachiningFeedback mf = new MachiningFeedback();
            //    mf.Text = "�ܼӹ���������СƱ";
            //    mf.MdiParent = this;
            //    mf.Show();
            //}

            //else if (UserSecurity.HavingPrivilege(User.cur_user, "SPOOLDESIGNUSERS"))
            //{
            //    foreach (Form form in this.MdiChildren)
            //    {
            //        if (form.Text == "��˷���СƱ")
            //        {
            //            if (form.WindowState == FormWindowState.Minimized)
            //            {
            //                form.WindowState = FormWindowState.Normal;

            //            }
            //            form.Activate();
            //            return;
            //        }
            //    }
            //    ApproveForm af = new ApproveForm();
            //    af.Text = "��˷���СƱ";
            //    af.MdiParent = this;
            //    af.Show();

            //}

        }
        #region ��word\excel\notepad�ĵ�
        private void ���ü��±�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe");
        }
        
        private void ����wordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("winword.exe");
        }

        private void ����excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("excel.exe");
        }
        #endregion
        private void ���µ�¼toolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("ȷ��Ҫ���µ�¼��?", "���µ�¼", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //Process process = new Process();
                //process.StartInfo.FileName = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "DetailInfo.exe";
                //process.Start();
                if (this.MdiChildren.Length != 0)
                {
                    foreach (Form form in this.MdiChildren)
                    {
                        form.Close();
                        //this.SearchtoolStripButton3.Enabled = false;
                    }
                }
                this.Dispose();
                Process process = new Process();
                process.StartInfo.FileName = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "DetailInfo.exe";
                process.Start();
                //bool flag = false;

                /////ʵ����һ��ͬ����Ԫ
                //Mutex mutex = new Mutex(true, "QueryMachine.BackManager.LoginForm", out flag);
                //if (flag)
                //{
                //    Application.Restart();

                //}
                //else
                //{
                //    Application.Exit();
                //}

            }
            else
            {
                return;
            }

        }

        /// <summary>
        /// �򿪲�ѯ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchtoolStripButton3_Click(object sender, EventArgs e)
        {
            string tableName = "";
            string name = string.Empty;
            try
            {
                name = this.ActiveMdiChild.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
            switch (name)
            {
                case "���СƱ����": 
                    tableName = "SP_SPOOL_TAB";
                    break;
                case "�ӹ�СƱ����":
                    tableName = "SP_SPOOL_TAB";
                    break;
                case "�ʼ�СƱ����":
                    tableName = "SP_SPOOL_TAB";
                    break;
                case "��װСƱ����":
                    tableName = "SP_SPOOL_TAB";
                    break;
                case "����СƱ����":
                    tableName = "SP_SPOOL_TAB";
                    break;

                case "��·ͳ����Ϣ":
                    tableName = "SPL_VIEW";
                    break;
                case "��·����ͳ����Ϣ":
                    tableName = "MATERIALATTACHMENT_VIEW";
                    break;

                case "������Ϣ����":
                    tableName = "SP_SPOOLMATERIAL_VIEW";
                    break;
                case "���Ӽ���Ϣ����":
                    tableName = "SP_CONNECTOR_VIEW";
                    break;
                case "�ӹ���Ϣ����":
                    tableName = "SP_MACHININGINFO_TAB";
                    break;
                case "������Ϣ����":
                    tableName = "SP_SPOOLWELD_VIEW";
                    break;
                case "������Ϣ����":
                    tableName = "SP_VALVE_VIEW";
                    break;

                case "���Ϲ�ʱ����":
                    tableName = "SP_MATERIALPREPARETIME_VIEW";
                    break;
                case "װ�乤ʱ����":
                    tableName = "SP_SPOOL_TAB";
                    break;
                case "���ӹ�ʱ����":
                    tableName = "SP_SPOOL_TAB";
                    break;
                case "���鹤ʱ����":
                    tableName = "SP_SPOOL_TAB";
                    break;
                case "�ϳ���ʱ����":
                    tableName = "SP_SPOOL_TAB";
                    break;
                case "ѹ�����鹤ʱ����":
                    tableName = "SP_SPOOL_TAB";
                    break;
                case "��̼ͨ�ֹ�(ͭ��)����ͨ����ֹ��ܹ�ʱ����":
                    tableName = "SP_NORMALPIPEWORKINGHOUR_VIEW";
                    break;
                case "������Ϣ":
                    tableName = "SP_WORKSHOPBLANKING_VIEW";
                    break;
                case "װ����Ϣ":
                    tableName = "SP_WORKSHOPASSEMBLY_VIEW";
                    break;
                case "������Ϣ":
                    tableName = "SP_WORKSHOPWELD_VIEW";
                    break;
                case "������Ϣ":
                    tableName = "SP_WORKSHOTOQC_VIEW";
                    break;
                case "������Ϣ":
                    tableName = "SP_WORKSHOPTOTREATMENT_VIEW";
                    break;
                case "�ϳ�������Ϣ":
                    tableName = "SP_WORKSHOPRECIEVE_VIEW";
                    break;
                case "������Ϣ":
                    tableName = "SP_WORKSHOPDELIVERY_VIEW";
                    break;
                case "��װ��Ϣ":
                    tableName = "SP_WORKSHOPINSTALL_VIEW";
                    break;
                case "���̼�������Ϣ":
                    tableName = "SP_WORKSHOPTRAYNOCLASS_VIEW";
                    break;
                case "�ѷ�ͼֽ���϶����":
                    tableName = "SP_MATERIALEQUIPRATION_TAB";
                    break;
                default:
                    this.statusStrip1.Items[0].Enabled = false;
                    break;
            }
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "SearchForm")
                {
                    form.Activate();
                    return;
                }
            }
            SearchForm sf = new SearchForm();
            sf.Table_name = tableName;
            sf.ShowDialog();
        }

        /// <summary>
        /// ����formͼ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuStrip1_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            if (e.Item.Text.Length == 0)
            {
                e.Item.Visible = false;
            }
        }
        #region �ര��ֲ�����
        private void ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void ��������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ����ƽ��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }
        #endregion

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point ClickPoint = new Point(e.X, e.Y);
                TreeNode CurrentNode = treeView1.GetNodeAt(ClickPoint);
                if (CurrentNode != null)//�ж������ǲ���һ���ڵ�
                {
                    if (User.cur_user == "plm")
                    {
                        //��ʾ�Ҽ��˵�
                        for (int i = 0; i < contextMenuStrip1.Items.Count; i++)
                        {
                            contextMenuStrip1.Items[i].Visible = true;
                        }
                    }
                    treeView1.SelectedNode = CurrentNode;//ѡ������ڵ�
                }
                else
                    for (int i = 0; i < contextMenuStrip1.Items.Count; i++)
                    {
                        contextMenuStrip1.Items[i].Visible = false;
                    }
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            TreeNode selectednode = treeView1.SelectedNode;
            switch (name)
            {
                case "���":
                    foreach (Form form in Application.OpenForms)
                    {
                        if (form.Name == "AddNewNode")
                        {
                            form.Activate();
                            return;
                        }
                    }
                    AddNewNode ann = new AddNewNode(new formrefresh(SetReload));
                    ann.MdiParent = this;
                    ann.Text = "����½ڵ�";
                    ann.GetNode(selectednode);
                    ann.GetImagelist(imageList1);
                    ann.Show();
                    break;
                case "ɾ��":
                    DialogResult result = MessageBox.Show("ȷ��Ҫɾ����"+selectednode.Text+"����", "ɾ���ڵ�", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.OK)
                    {
                        //�ж�ѡ���Ľڵ��Ƿ������һ���ڵ�
                        if (selectednode.Nodes.Count == 0)
                        {
                            int id = Convert.ToInt32(selectednode.Tag);
                            selectednode.Remove();//ɾ���ڵ�
                            TreeNodes td=TreeNodes.Find(id);
                            TreeNodes.UpdateParentIndexDel(td.Parentid, td.ParentIndex);
                            string sql = "delete from treenodes_tab t where t.id = " + id;
                            string sqlp = "delete from privilege_node_tab t where t.node_id = " + id;
                            User.UpdateCon(sql, DataAccess.OIDSConnStr);
                            User.UpdateCon(sqlp, DataAccess.OIDSConnStr);
                            MessageBox.Show("ɾ���ɹ���");
                        }
                        else
                            MessageBox.Show("����ɾ���˽ڵ��µ��ӽڵ㣡", "��ʾ��Ϣ", MessageBoxButtons.OK);
                    }
                        
                    break;
                case "�޸�":
                    foreach (Form form in Application.OpenForms)
                    {
                        if (form.Name == "ModifyNode")
                        {
                            form.Activate();
                            return;
                        }
                    }
                    ModifyNode mn = new ModifyNode(new formrefresh(SetReload));
                    mn.MdiParent = this;
                    mn.Text = "�޸Ľڵ�";
                    mn.GetNode(selectednode);
                    mn.GetImagelist(imageList1);
                    mn.Show();
                    break;
                default:
                    break;
            }
        }

        private void SetReload()
        {
            treeView1.Nodes.Clear();
            string sql;
            if (User.cur_user == "plm")
                sql = "select * from treenodes_tab t order by t.parent_index";
            else
            {
                string privilegelist = User.FindPrivilegeidList(User.cur_user);
                sql = "select * from treenodes_tab t where t.flag='N' or (t.flag='Y' and t.id in (select p.node_id from privilege_node_tab p where p.privilege_id in (" + privilegelist + ")))  order by t.parent_index";
            }
            CreateTreeView(sql, treeView1.Nodes);
            treeView1.Nodes[0].Expand();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Start();
            SetStatus();
        }

        /// <summary>
        /// ����״̬��
        /// </summary>
        private void SetStatus()
        {
            int count = this.MdiChildren.Length;
            this.toolStripStatusLabel1.Text = string.Format(" ״̬:  ��ǰ�򿪴��ڹ�:{0}��", count);
            this.toolStripStatusLabel2.Text = string.Format("ϵͳά��:����(2263)");
            this.toolStripStatusLabel3.Text = string.Format("��ǰ��¼�û�:{0}", User.cur_user);
            this.toolStripStatusLabel4.Text = System.DateTime.Now.ToString();

        }

        #region ����Ƥ��
        private void ��ʯ��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.skinEngine1.SkinFile = Application.StartupPath + "\\Resources\\DiamondBlue.ssk";
            User.GetSkinStr("DiamondBlue.ssk");
        }

        private void vistaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.skinEngine1.SkinFile = Application.StartupPath + "\\Resources\\Vista.ssk";
            User.GetSkinStr("RealOne.ssk");
        }

        private void realOneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.skinEngine1.SkinFile = Application.StartupPath + "\\Resources\\RealOne.ssk";
            User.GetSkinStr("Vista.ssk");
        }


        private void office2007ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.skinEngine1.SkinFile = Application.StartupPath + "\\Resources\\office2007.ssk";
            User.GetSkinStr("office2007.ssk");
        }

        private void mP10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.skinEngine1.SkinFile = Application.StartupPath + "\\Resources\\MP10.ssk";
            User.GetSkinStr("MP10.ssk");
        }
        #endregion

        #region ����������
        private Point Position = new Point(0, 0);
        
        /// <summary>
        /// ���û���ʼ�϶���ʱ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left&&User.cur_user=="plm")
                DoDragDrop(e.Item, DragDropEffects.Move);
        }
        /// <summary>
        /// �������Ϲ��ؼ��ı߽�ʱ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            //�������ͣ�� TreeView �ؼ���ʱ��չ���ÿؼ��е� TreeNode   

            Point p = this.treeView1.PointToClient(Control.MousePosition);
            TreeNode tn = this.treeView1.GetNodeAt(p);
            if (tn != null)
            {
                if (this.dragDropTreeNode != tn) //�ƶ����µĽڵ�   
                {
                    if (tn.Nodes.Count > 0 && tn.IsExpanded == false)
                    {
                        this.startTime = DateTime.Now;//�����µ���ʼʱ��   
                    }
                }
                else
                {
                    if (tn.Nodes.Count > 0 && tn.IsExpanded == false && this.startTime != DateTime.MinValue)
                    {
                        TimeSpan ts = DateTime.Now - this.startTime;
                        if (ts.TotalMilliseconds >= 1000) //һ��   
                        {
                            tn.Expand();
                            this.startTime = DateTime.MinValue;
                        }
                    }
                }
            }
            //�����Ϸű�ǩEffect״̬   
            if (tn != null)//&& (tn != this.treeView.SelectedNode)) //���ؼ��ƶ����հ״�ʱ�����ò����á�   
            {
                if ((e.AllowedEffect & DragDropEffects.Move) != 0)
                {
                    e.Effect = DragDropEffects.Move;
                }
                if (((e.AllowedEffect & DragDropEffects.Copy) != 0) && ((e.KeyState & 0x08) != 0))//Ctrl key   
                {
                    e.Effect = DragDropEffects.Copy;
                }
                if (((e.AllowedEffect & DragDropEffects.Link) != 0) && ((e.KeyState & 0x08) != 0) && ((e.KeyState & 0x04) != 0))//Ctrl hift key   
                {
                    e.Effect = DragDropEffects.Link;
                }
                if (e.Data.GetDataPresent(typeof(TreeNode)))//�϶�����TreeNode   
                {
                    TreeNode parND = tn;//�ж��Ƿ��ϵ�������   
                    bool isChildNode = false;
                    while (parND.Parent != null)
                    {
                        parND = parND.Parent;
                        if (parND == this.treeView1.SelectedNode)
                        {
                            isChildNode = true;
                            break;
                        }
                    }
                    if (isChildNode)
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else if (e.Data.GetDataPresent(typeof(ListViewItem)))//�϶�����ListViewItem   
                {
                    if (tn.Parent == null)
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
            //�����Ϸ�Ŀ��TreeNode�ı���ɫ   
            if (e.Effect == DragDropEffects.None)
            {
                if (this.dragDropTreeNode != null) //ȡ�������õĽڵ������ʾ   
                {
                    this.dragDropTreeNode.BackColor = SystemColors.Window;
                    this.dragDropTreeNode.ForeColor = SystemColors.WindowText;
                    this.dragDropTreeNode = null;
                }
            }
            else
            {
                if (tn != null)
                {
                    if (this.dragDropTreeNode != null)
                    {
                        if (this.dragDropTreeNode != tn)
                        {
                            this.dragDropTreeNode.BackColor = Color.Empty;//ȡ����һ�������õĽڵ������ʾ   
                            this.dragDropTreeNode.ForeColor = SystemColors.WindowText;
                            this.dragDropTreeNode = tn;//����Ϊ�µĽڵ�   
                            this.dragDropTreeNode.BackColor = SystemColors.Highlight;
                            this.dragDropTreeNode.ForeColor = SystemColors.HighlightText;
                        }
                    }
                    else
                    {
                        this.dragDropTreeNode = tn;//����Ϊ�µĽڵ�   
                        this.dragDropTreeNode.BackColor = SystemColors.Highlight;
                        this.dragDropTreeNode.ForeColor = SystemColors.HighlightText;
                    }
                }
            }
        }
        /// <summary>
        /// �ϷŲ������ʱ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode myNode = null;
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                myNode = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            }
            else
            {
                MessageBox.Show("error");
            }
            Position.X = e.X;
            Position.Y = e.Y;
            Position = treeView1.PointToClient(Position);
            TreeNode DropNode = this.treeView1.GetNodeAt(Position);
            // 1.Ŀ��ڵ㲻�ǿա�2.Ŀ��ڵ㲻�Ǳ���ק�ӵ���ӽڵ㡣3.Ŀ��ڵ㲻�Ǳ���ק�ڵ㱾��
            if (DropNode != null && DropNode.Parent != myNode && DropNode != myNode)
            {
                TreeNode DragNode = (TreeNode)myNode.Clone();
                if (myNode.Parent == DropNode.Parent)//�����ͬ���ڵ�
                {
                    TreeNodes mynode=TreeNodes.Find(Convert.ToInt32(myNode.Tag));
                    TreeNodes dropnode=TreeNodes.Find(Convert.ToInt32(DropNode.Tag));
                    if (myNode.NextNode == DropNode)
                    {
                        TreeNodes.UpdateParentIndex(Convert.ToInt32(DragNode.Tag), DropNode.Index);
                        TreeNodes.UpdateParentIndex(Convert.ToInt32(DropNode.Tag), DropNode.Index - 1);
                        DropNode.Parent.Nodes.Insert(DropNode.Index + 1, DragNode);
                    }
                    else
                    {
                        if (mynode.ParentIndex > dropnode.ParentIndex)//�����϶�
                        {
                            TreeNodes.UpdateParentIndexAdd(mynode.Parentid, mynode.ParentIndex, DropNode.Index);
                            TreeNodes.UpdateParentIndex(Convert.ToInt32(DragNode.Tag), DropNode.Index);
                        }
                        else//�����϶�
                        {
                            TreeNodes.UpdateParentIndexDel(mynode.Parentid, mynode.ParentIndex, DropNode.Index);
                            TreeNodes.UpdateParentIndex(Convert.ToInt32(DragNode.Tag), DropNode.Index - 1);
                        }
                        DropNode.Parent.Nodes.Insert(DropNode.Index , DragNode);
                    }
                    treeView1.SelectedNode = DragNode;
                    myNode.Remove();            
                }
                else
                {
                    // ������ק�ڵ��ԭ��λ��ɾ����
                    myNode.Remove();

                    // ��Ŀ��ڵ������ӱ���ק�ڵ�
                    DropNode.Nodes.Add(DragNode);
                    TreeNodes.UpdateParentAndParentIndex(Convert.ToInt32(DragNode.Tag), Convert.ToInt32(DropNode.Tag), DropNode.Nodes.Count - 1);
                }
                DropNode.BackColor = Color.Empty;//ȡ����һ�������õĽڵ������ʾ   
                DropNode.ForeColor = SystemColors.WindowText;
            }
            // ���Ŀ��ڵ㲻���ڣ�����ק��λ�ò����ڽڵ㣬��ô�ͽ�����ק�ڵ���ڸ��ڵ�֮��
            if (DropNode == null)
            {
                MessageBox.Show("Ŀ��ڵ㲻����!");
                //TreeNode DragNode = myNode;
                //myNode.Remove();
                //treeView1.Nodes.Add(DragNode);
            }
        }
        /// <summary>
        /// ����꽫ĳ���϶����ÿؼ��Ĺ�����ʱ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }
        #endregion
        #region ���ɾ���޸ĵ������ڵ�
        private void ���ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void �޸�ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void �޸�ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
        #endregion
        private void ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IntroForm introform = new IntroForm();
            introform.ShowDialog();

        }

        private void ��ѡ��PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolsOptionForm toform = new ToolsOptionForm();
            toform.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Activate();
            this.Refresh();
        }

        /// <summary>
        /// ������ǰ�Ӵ��嵽excel��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExcelStripButton_Click(object sender, EventArgs e)
        {
            string frmtext = MDIForm.pMainWin.ActiveMdiChild.Name.ToString();
            DataGridView dgv = null;
            int count = 0;
            string tabpage = string.Empty;
            switch (frmtext)
            {
                case "SpoolGeneralViewForm":
                    dgv = (DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;

                case "ProgressStatistics":
                    dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["panel2"].Controls["dataGridView1"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;
                    
                case "BlockConstructionPlanForm":
                    dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["dgvInfo"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;

                case "DataMaintenanceForm":
                    dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["EditDgv"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;

                case "PROJECTDRAWINGINFO":
                    dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["splitContainer1"].Controls[1].Controls["groupBox2"].Controls["DrawingsDgv"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;

                case "DetailsForm":
                    tabpage = ((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).SelectedTab.Text.ToString();
                    if (tabpage == "������Ϣ")
                    {
                        dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage1"].Controls[0]);
                        count = dgv.Rows.Count;
                        if (count == 0)
                        {
                            return;
                        }
                        User.ExportToExcel(dgv, toolStripProgressBar2);
                    }
                    else if (tabpage == "���Ӽ���Ϣ")
                    {
                        dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage2"].Controls[0]);
                        count = dgv.Rows.Count;
                        if (count == 0)
                        {
                            return;
                        }
                        User.ExportToExcel(dgv, toolStripProgressBar2);
                    }
                    else if (tabpage == "�ӹ���Ϣ")
                    {
                        dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage3"].Controls[0]);
                        count = dgv.Rows.Count;
                        if (count == 0)
                        {
                            return;
                        }
                        User.ExportToExcel(dgv, toolStripProgressBar2);
                    }
                    else if (tabpage == "������Ϣ")
                    {
                        dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage4"].Controls[0]);
                        count = dgv.Rows.Count;
                        if (count == 0)
                        {
                            return;
                        }
                        User.ExportToExcel(dgv, toolStripProgressBar2);
                    }
                    else if (tabpage == "���������Ϣ")
                    {
                        dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage5"].Controls[0]);
                        count = dgv.Rows.Count;
                        if (count == 0)
                        {
                            return;
                        }
                        User.ExportToExcel(dgv, toolStripProgressBar2);
                    }

                    else if (tabpage == "СƱ��־")
                    {
                        dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage10"].Controls[0]);
                        count = dgv.Rows.Count;
                        if (count == 0)
                        {
                            return;
                        }
                        User.ExportToExcel(dgv, toolStripProgressBar2);
                    }
                    break;
                case "DrawingForm":
                    tabpage = ((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).SelectedTab.Text.ToString();
                    if (tabpage == "СƱ��Ϣ")
                    {
                        dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage1"].Controls["Appdgv"]);
                        count = dgv.Rows.Count;
                        if (count == 0)
                        {
                            return;
                        }
                        User.ExportToExcel(dgv, toolStripProgressBar2);
                    }
                    else if (tabpage == "������Ϣ")
                    {
                        dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage2"].Controls["Materialdgv"]);
                        count = dgv.Rows.Count;
                        if (count == 0)
                        {
                            return;
                        }
                        User.ExportToExcel(dgv, toolStripProgressBar2);
                    }
                    else if (tabpage == "���Ӽ���Ϣ")
                    {
                        dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage12"].Controls["Connectordgv"]);
                        count = dgv.Rows.Count;
                        if (count == 0)
                        {
                            return;
                        }
                        User.ExportToExcel(dgv, toolStripProgressBar2);
                    }
                    else if (tabpage == "������Ϣ")
                    {
                        dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage3"].Controls["Partdgv"]);
                        count = dgv.Rows.Count;
                        if (count == 0)
                        {
                            return;
                        }
                        User.ExportToExcel(dgv, toolStripProgressBar2);
                    }
                    else if (tabpage == "�������ϱ�")
                    {
                        dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage6"].Controls["dataGridView1"]);
                        count = dgv.Rows.Count;
                        if (count == 0)
                        {
                            return;
                        }
                        User.ExportToExcel(dgv, toolStripProgressBar2);
                    }
                    else if (tabpage == "���������豸�����")
                    {
                        dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage7"].Controls["dataGridView2"]);
                        count = dgv.Rows.Count;
                        if (count == 0)
                        {
                            return;
                        }
                        User.ExportToExcel(dgv, toolStripProgressBar2);
                    }

                    else if (tabpage == "��ϵ���̱�")
                    {
                        dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage8"].Controls["dataGridView3"]);
                        count = dgv.Rows.Count;
                        if (count == 0)
                        {
                            return;
                        }
                        User.ExportToExcel(dgv, toolStripProgressBar2);
                    }

                    else if (tabpage == "��ϵ���ϱ�")
                    {
                        dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage9"].Controls["dataGridView4"]);
                        count = dgv.Rows.Count;
                        if (count == 0)
                        {
                            return;
                        }
                        User.ExportToExcel(dgv, toolStripProgressBar2);
                    }

                    else if (tabpage == "��ϵ�����豸�����")
                    {
                        dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage10"].Controls["dataGridView5"]);
                        count = dgv.Rows.Count;
                        if (count == 0)
                        {
                            return;
                        }
                        User.ExportToExcel(dgv, toolStripProgressBar2);
                    }
                    break;
                case "HourNormForm":
                    dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["dgvhournorm"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;

                case "WorkShopPlan":
                    dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["splitContainer1"].Controls[1].Controls["PlanDGV"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;
                case "WShopSPlTrace":
                    dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["splitContainer1"].Controls[1].Controls["SPTraceDgv"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;
                case "WShopManHour":
                    dgv = (DataGridView)((MDIForm.pMainWin.ActiveMdiChild.Controls["splitContainer1"].Controls[1].Controls[0]));
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;

                case "NormalPipeWorkingTime":
                    dgv = (DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["dataGridView1"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;
                case "SpoolReportFrm":
                    dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["splitContainer1"].Controls[1].Controls["groupBox2"].Controls["dataGridView1"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;
                case "MaterialReportsFrm":
                    dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["splitContainer1"].Controls[1].Controls["groupBox5"].Controls["dataGridView1"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;
                case "MaterialTraceabilityFrm":
                    dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["splitContainer1"].Controls[1].Controls["dataGridView1"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;
                case "ModifyStatisticsFrm":
                    dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["splitContainer1"].Controls[1].Controls["groupBox2"].Controls["dataGridView1"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;
                case "ModifyDelInfoFrm":
                    dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["splitContainer1"].Controls[1].Controls["dataGridView1"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;
                case "ECDrawingReportFrm":
                    dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["splitContainer1"].Controls[1].Controls["dataGridView1"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;
                case "MaterialRationForm":
                    dgv = (DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["groupBox2"].Controls["dgv1"]);
                    count = dgv.Rows.Count;
                    if (count == 0)
                    {
                        return;
                    }
                    User.ExportToExcel(dgv, toolStripProgressBar2);
                    break;
                default:
                    break;
            }
        }

        private void ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BatchSheet bsform = new BatchSheet();
            bsform.MdiParent = this;
            bsform.Show();
        }

        private void ����ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ToolsOptionForm tofprm = new ToolsOptionForm();
            tofprm.MdiParent = this;
            tofprm.Show();
        }

        #region ��������
        /// <summary>
        /// ������ǰѡ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyStripButton_Click(object sender, EventArgs e)
        {
            int count = this.MdiChildren.Length;
            if (count == 0)
            {
                return;
            }
            else
            {
                DataGridView dgv = null;
                string frmtext = MDIForm.pMainWin.ActiveMdiChild.Name.ToString();
                string tabpage = string.Empty;
                switch (frmtext)
                {
                    case "SpoolGeneralViewForm":
                        dgv = (DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"]);
                        if (dgv.Rows.Count == 0)
                        {
                            return;
                        }
                        DataGridViewEnableCopy(dgv);
                        break;
                    case "ProgressStatistics":
                        dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["panel2"].Controls["dataGridView1"]);
                        if (dgv.Rows.Count == 0)
                        {
                            return;
                        }
                        DataGridViewEnableCopy(dgv);
                        break;

                    case "BlockConstructionPlanForm":
                        dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["dgvInfo"]);
                        if (dgv.Rows.Count == 0)
                        {
                            return;
                        }
                        DataGridViewEnableCopy(dgv);
                        break;

                    case "DataMaintenanceForm":
                        dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["EditDgv"]);
                        if (dgv.Rows.Count == 0)
                        {
                            return;
                        }
                        DataGridViewEnableCopy(dgv);
                        break;

                    case "PROJECTDRAWINGINFO":
                        dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["splitContainer1"].Controls[1].Controls["groupBox2"].Controls["DrawingsDgv"]);
                        if (dgv.Rows.Count == 0)
                        {
                            return;
                        }
                        DataGridViewEnableCopy(dgv);
                        break;

                    case "DetailsForm":
                        tabpage = ((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).SelectedTab.Text.ToString();
                        if (tabpage == "������Ϣ")
                        {
                            dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage1"].Controls[0]);
                            if (dgv.Rows.Count == 0)
                            {
                                return;
                            }
                            DataGridViewEnableCopy(dgv);
                        }
                        else if (tabpage == "���Ӽ���Ϣ")
                        {
                            dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage2"].Controls[0]);
                            if (dgv.Rows.Count == 0)
                            {
                                return;
                            }
                            DataGridViewEnableCopy(dgv);
                        }
                        else if (tabpage == "�ӹ���Ϣ")
                        {
                            dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage3"].Controls[0]);
                            if (dgv.Rows.Count == 0)
                            {
                                return;
                            }
                            DataGridViewEnableCopy(dgv);
                        }
                        else if (tabpage == "������Ϣ")
                        {
                            dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage4"].Controls[0]);
                            if (dgv.Rows.Count == 0)
                            {
                                return;
                            }
                            DataGridViewEnableCopy(dgv);
                        }
                        else if (tabpage == "���������Ϣ")
                        {
                            dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage5"].Controls[0]);
                            if (dgv.Rows.Count == 0)
                            {
                                return;
                            }
                            DataGridViewEnableCopy(dgv);
                        }

                        else if (tabpage == "СƱ��־")
                        {
                            dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage10"].Controls[0]);
                            if (dgv.Rows.Count == 0)
                            {
                                return;
                            }
                            DataGridViewEnableCopy(dgv);
                        }
                        break;

                    case "DrawingForm":
                        tabpage = ((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).SelectedTab.Text.ToString();
                        if (tabpage == "СƱ��Ϣ")
                        {
                            dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage1"].Controls["Appdgv"]);
                            if (dgv.Rows.Count == 0)
                            {
                                return;
                            }
                            DataGridViewEnableCopy(dgv);
                        }
                        else if (tabpage == "������Ϣ")
                        {
                            dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage2"].Controls["Materialdgv"]);
                            if (dgv.Rows.Count == 0)
                            {
                                return;
                            }
                            DataGridViewEnableCopy(dgv);
                        }
                        else if (tabpage == "������Ϣ")
                        {
                            dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage3"].Controls["Partdgv"]);
                            if (dgv.Rows.Count == 0)
                            {
                                return;
                            }
                            DataGridViewEnableCopy(dgv);
                        }
                        else if (tabpage == "�������ϱ�")
                        {
                            dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage6"].Controls["dataGridView1"]);
                            if (dgv.Rows.Count == 0)
                            {
                                return;
                            }
                            DataGridViewEnableCopy(dgv);
                        }
                        else if (tabpage == "���������豸�����")
                        {
                            dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage7"].Controls["dataGridView2"]);
                            if (dgv.Rows.Count == 0)
                            {
                                return;
                            }
                            DataGridViewEnableCopy(dgv);
                        }

                        else if (tabpage == "��ϵ���̱�")
                        {
                            dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage8"].Controls["dataGridView3"]);
                            if (dgv.Rows.Count == 0)
                            {
                                return;
                            }
                            DataGridViewEnableCopy(dgv);
                        }

                        else if (tabpage == "��ϵ���ϱ�")
                        {
                            dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage9"].Controls["dataGridView4"]);
                            if (dgv.Rows.Count == 0)
                            {
                                return;
                            }
                            DataGridViewEnableCopy(dgv);
                        }

                        else if (tabpage == "��ϵ�����豸�����")
                        {
                            dgv = (DataGridView)(((TabControl)(MDIForm.ActiveForm.ActiveMdiChild.Controls["tabControl1"])).Controls["tabPage10"].Controls["dataGridView5"]);
                            if (dgv.Rows.Count == 0)
                            {
                                return;
                            }
                            DataGridViewEnableCopy(dgv);
                        }
                        break;
                    case "HourNormForm":
                        dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls["dgvhournorm"]);
                        if (dgv.Rows.Count == 0)
                        {
                            return;
                        }
                        DataGridViewEnableCopy(dgv);
                        break;
                    case "MachinningInfoFrm":
                        dgv = (DataGridView)(MDIForm.ActiveForm.ActiveMdiChild.Controls[0].Controls["dataGridView1"]);
                        if (dgv.Rows.Count == 0)
                        {
                            return;
                        }
                        DataGridViewEnableCopy(dgv);
                        break;

                    default:
                        break;
                }
            }
        }

        private void DataGridViewEnableCopy(DataGridView dgv)
        {
            Clipboard.SetData(DataFormats.Text,dgv.GetClipboardContent().GetData(DataFormats.Text.ToString()));
        }
        #endregion

        private void �ŷ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkShopWorkerData wwdform = new WorkShopWorkerData();
            wwdform.MdiParent = this;
            wwdform.Show();
        }


        private void ����ƻ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkShopPlan planfrm = new WorkShopPlan();
            planfrm.MdiParent = this;
            planfrm.Show();
        }

        private void test1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolsOptionForm tpform = new ToolsOptionForm();
            tpform.MdiParent = this;
            tpform.Show();
        }

        private void nnnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WaitingForm wform = new WaitingForm();
            wform.MdiParent = this;
            wform.Show();
        }
    }
}