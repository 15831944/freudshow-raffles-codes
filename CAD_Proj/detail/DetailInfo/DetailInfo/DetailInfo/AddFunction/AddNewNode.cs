using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using DetailInfo.Categery;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;

namespace DetailInfo
{
    public partial class AddNewNode : Form
    {
        private TreeNode parentnode=new TreeNode();
        string flag = "N";
        private formrefresh add_setreload;
        private TreeNodes tnTmp = null;
        private List<TreeNodes> nodelist = TreeNodes.FindAll();
        public AddNewNode(formrefresh setreload)
        {
            InitializeComponent();
            this.add_setreload = setreload;
        }
        /// <summary>
        /// ��ø��ڵ�
        /// </summary>
        /// <param name="node"></param>
        public void GetNode(TreeNode node)
        {
            parentnode = node;
        }
        /// <summary>
        /// ��ø���������imagelist
        /// </summary>
        /// <param name="imglist"></param>
        public  void GetImagelist(System.Windows.Forms.ImageList imglist)
        {
            imageList1 = imglist;
        }
        /// <summary>
        /// ���ݰ�
        /// </summary>
        /// <param name="sqlstr"></param>
        private void DataBind(string sqlstr)
        {
            try
            {
                OracleConnection con = new OracleConnection(DataAccess.OIDSConnStr);
                con.Open();
                OracleDataAdapter oda = new OracleDataAdapter(sqlstr, con);
                OracleCommandBuilder builder = new OracleCommandBuilder(oda);
                DataSet ds = new DataSet();
                oda.Fill(ds);
                checkedListBox1.DataSource = ds.Tables[0];
                checkedListBox1.DisplayMember = ds.Tables[0].Columns[1].ToString(); //Ҫ��ʾ��������
                checkedListBox1.ValueMember = ds.Tables[0].Columns[0].ToString(); //�洢��������
                con.Close();
                ds.Dispose();
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message.ToString());
               return;
            }
        } 
        private void AddNewNode_Load(object sender, EventArgs e)
        {
            label7.Text = parentnode.Text;
            checkedListBox1.Visible = false;
            DataSet ds = TreeNodes.GetSubNodes(Convert.ToInt32(parentnode.Tag));
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                comboBox3.Items.Add(ds.Tables[0].Rows[i][1]);
            }
            comboBox3.Items.Add("���ޡ�");
            #region ��ʼ������չ��������ؼ�
            comboBox1.ImageList = imageList1;
            comboBox2.ImageList = imageList1;
            for (int i = 0; i < imageList1.Images.Count; i++)
            {
                comboBox1.Items.Add(new ComboBoxExItem(i.ToString(),i));
                comboBox2.Items.Add(new ComboBoxExItem(i.ToString(),i));
            }
            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox2.Text.Trim();
            if (text == string.Empty)
            {
                MessageBox.Show("�ڵ��ʶ����Ϊ�գ�");
                return;
            }
            string name = textBox3.Text.Trim();
            if (name == string.Empty)
            {
                MessageBox.Show("�ڵ����Ʋ���Ϊ�գ�");
                return;
            }
            int imageindex = Convert.ToInt32(comboBox1.SelectedIndex);
            int selectedimageindex = Convert.ToInt32(comboBox2.SelectedIndex);
            int parentid =  Convert.ToInt32(parentnode.Tag);
            int afternode = Convert.ToInt32(comboBox3.SelectedIndex);
            if (afternode == -1)
            {
                MessageBox.Show("��ѡ�������ڵ�ĺ�ڵ㣡");
                return;
            }
            if (Convert.ToInt32(comboBox3.SelectedIndex) != (comboBox3.Items.Count - 1))//����ýڵ㲻����ӵ����
                TreeNodes.UpdateParentIndexAdd(parentid, afternode);
            string sql = "insert into treenodes_tab t (t.name,t.text,t.imageindex,t.selectedimageindex,t.parent_id,t.flag,t.parent_index) values ('" + name + "','" + text + "'," + imageindex + "," + selectedimageindex + "," + parentid + ",'" + flag + "'," + afternode + ")";
            User.UpdateCon(sql, DataAccess.OIDSConnStr);
            if (flag == "Y")
            {
                int n = 0;
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                
                        n++;
                        checkedListBox1.SetSelected(i,true);
                        int privilegeid = Convert.ToInt32(checkedListBox1.SelectedValue);
                        string sqlstr = "insert into privilege_node_tab t ( t.privilege_id,t.node_id) values ("+privilegeid+",(select max(d.id) from treenodes_tab d))";//��Ӹýڵ��Ȩ�����ù�ϵ
                        User.UpdateCon(sqlstr, DataAccess.OIDSConnStr);
                        SetPrivilegetoParentnode(privilegeid,parentid);
                    }
                }
                if (n == 0)
                {
                    MessageBox.Show("��ѡ��Ȩ�ޣ�","��ʾ��Ϣ",MessageBoxButtons.OK);
                    return;
                }

                MessageBox.Show("��ӽڵ�ɹ���");
            }
            else
                MessageBox.Show("��ӽڵ�ɹ���");
            this.add_setreload();
        }

        private void radioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
            {
                return;
            }
            switch (((RadioButton)sender).Text.ToString())
            {
                case "N":
                    checkedListBox1.Visible = false;
                    flag = "N";
                    break;
                case "Y":
                    checkedListBox1.Visible = true;
                    flag = "Y";
                    checkedListBox1.Focus();
                    string sql = "select t.privilege_id,t.privilege_flag from privilege_tab t where t.privilege_cata='Pipe'";
                    DataBind(sql);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// ���ӽڵ�Ȩ�޸ı�ʱ�����ڵ�������ͬȨ��
        /// </summary>
        /// <param name="nodeid"></param>
        private void SetPrivilegetoParentnode(int privilegeid,int nodeid)
        {
            int parentid = TreeNodes.GetParentid(nodeid);
            tnTmp = nodelist.Find(delegate(TreeNodes tn) { return tn.Id == parentid; });
            if (tnTmp == null)//û�и��ڵ�
                return;
            else
            {
                if (TreeNodes.GetNodeFlag(parentid) == "N")//���ڵ�û��Ȩ������
                    return;
                else
                {
                    if (PrivilegeNode.ExistPrivilege(privilegeid, parentid))//������ڵ��и�Ȩ������
                        return;
                    else
                    {
                        PrivilegeNode pnTmp = new PrivilegeNode();//���ڵ�Ȩ������
                        pnTmp.PrivilegeId = privilegeid;
                        pnTmp.NodeId = parentid;
                        int n = pnTmp.Add();
                        if (n == 0)
                            MessageBox.Show("Error!");
                        SetPrivilegetoParentnode(privilegeid,parentid);
                    }
                }
            }
        }
    }
}