using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DetailInfo
{
    public partial class ApproveForm : Form
    {
        public ApproveForm()
        {
            InitializeComponent();
            this.toolStripStatusLabel2.Text = null;
            requestbtn.Visible = false;
            disbtn.Visible = false; 

        }

        private void ApproveForm_Load(object sender, EventArgs e)
        {
            SetStatus();
            string sqlstr = string.Empty;
            string formtext = this.Text.ToString();
            switch (formtext)
            {
                case "�������":
                    sqlstr = "SELECT DISTINCT PROJECTID FROM SPOOL_TAB WHERE FLAG = 'Y'";
                    DetailInfo.Application_Code.FillComboBox.GetFlowStatus(this.PIDcomboBox, sqlstr);
                    this.PIDcomboBox.SelectedIndex = 0;
                    requestbtn.Visible = true;
                    this.requestbtn.Text = "�������";
                    break;

                case "��˴���":
                    sqlstr = "SELECT DISTINCT PROJECTID FROM PIPEAPPROVE_TAB WHERE ASSESOR = '"+User.cur_user+"' AND STATE = 0 AND APPROVENEEDFLAG = 'Y'";
                    DetailInfo.Application_Code.FillComboBox.GetFlowStatus(this.PIDcomboBox, sqlstr);
                    this.PIDcomboBox.SelectedIndex = 0;
                    this.requestbtn.Visible = true;  this.requestbtn.Text = "ͬ��";
                    this.disbtn.Visible = true; disbtn.Enabled = false;
                    break;

                case "��������˵�СƱ":
                    this.groupBox1.Text = string.Format("{0}", "��ѯ����");
                    sqlstr = "SELECT DISTINCT T.PROJECTID FROM SPFLOWLOG_TAB T WHERE T.FLOWSTATUS = 1 AND T.USERNAME = '"+User.cur_user+"'";
                    DetailInfo.Application_Code.FillComboBox.GetFlowStatus(this.PIDcomboBox, sqlstr);
                    this.PIDcomboBox.SelectedIndex = 0;
                    
                    break;

                case "��ͨ����˵�СƱ":
                    this.groupBox1.Text = string.Format("{0}", "��ѯ����");
                    sqlstr = "SELECT DISTINCT T.PROJECTID FROM SPOOL_TAB T WHERE T.FLOWSTATUS = 2 AND T.PROJECTID IN (SELECT DISTINCT S.PROJECTID FROM SPFLOWLOG_TAB S WHERE S.USERNAME = '"+User.cur_user+"')";
                    DetailInfo.Application_Code.FillComboBox.GetFlowStatus(this.PIDcomboBox, sqlstr);
                    this.requestbtn.Visible = true; this.requestbtn.Text = "��������";
                    //this.PIDcomboBox.SelectedIndex = 0;
                    break;

                case "���ͨ��СƱ":
                    this.groupBox1.Text = string.Format("{0}", "��ѯ����");
                    sqlstr = "SELECT DISTINCT T.PROJECTID FROM PIPEAPPROVE_TAB T WHERE T.ASSESOR = '"+User.cur_user+"' AND T.STATE = 1 AND T.APPROVENEEDFLAG = 'N'";
                    DetailInfo.Application_Code.FillComboBox.GetFlowStatus(this.PIDcomboBox, sqlstr);
                    this.PIDcomboBox.SelectedIndex = 0;
                    break;

                case "���δͨ��СƱ":
                    this.groupBox1.Text = string.Format("{0}", "��ѯ����");
                    sqlstr = "SELECT DISTINCT T.PROJECTID FROM SPOOL_TAB T WHERE T.FLOWSTATUS = 5 AND T.FLAG = 'Y' AND T.PROJECTID IN (SELECT DISTINCT S.PROJECTID FROM SPFLOWLOG_TAB S WHERE S.FLOWSTATUS = 5 AND S.USERNAME = '"+User.cur_user+"')";
                    DetailInfo.Application_Code.FillComboBox.GetFlowStatus(this.PIDcomboBox, sqlstr);
                    this.PIDcomboBox.SelectedIndex = 0;
                    break;

                case "��˷���СƱ":
                    this.groupBox1.Text = string.Format("{0}", "��ѯ����");
                    this.requestbtn.Visible = true; this.requestbtn.Text = "����";
                    sqlstr = "SELECT DISTINCT T.PROJECTID FROM SPOOL_TAB T WHERE T.FLOWSTATUS = 5 AND T.FLAG = 'Y' AND  T.PROJECTID IN (SELECT S.PROJECTID from spflowlog_tab S where S.PROJECTID in (select DISTINCT A.Projectid from spflowlog_tab A where A.username = '" + User.cur_user + "' and A.flowstatus = 1) and S.flowstatus = 5)";
                    DetailInfo.Application_Code.FillComboBox.GetFlowStatus(this.PIDcomboBox, sqlstr);
                    this.PIDcomboBox.SelectedIndex = 0;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// ����״̬��
        /// </summary>
        private void SetStatus()
        {
            int count = 0;

            if (this.tabControl1.SelectedTab.Text == "СƱ��Ϣ")
            {
                count = this.Appdgv.Rows.Count;
            }
            else if (this.tabControl1.SelectedTab.Text == "������Ϣ")
            {
                count = this.Materialdgv.Rows.Count;
            }
            else if (this.tabControl1.SelectedTab.Text == "������Ϣ")
            {
                count = this.Partdgv.Rows.Count;
            }

            this.toolStripStatusLabel1.Text = string.Format(" ��ǰ�ܼ�¼����{0}��", count);
        }

        /// <summary>
        /// СƱ��Ϣѡ���б仯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Appdgv_SelectionChanged(object sender, EventArgs e)
        {
            GetSelectionRowCount(this.Appdgv);
        }

        /// <summary>
        /// ѡ����Ŀ�Ż�ȡͼֽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PIDcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Appdgv.DataSource = this.Materialdgv.DataSource = this.Partdgv.DataSource = null;
            this.DRAWINGNOcomboBox.Items.Clear(); this.DRAWINGNOcomboBox.Text = null;
            string formtext = this.Text.ToString();
            string sqlstr = string.Empty;
            switch (formtext)
            {
                case "�������":
                    sqlstr = "SELECT DISTINCT T.DRAWINGNO FROM SPOOL_TAB T WHERE T.PROJECTID = '" + this.PIDcomboBox.SelectedItem + "' AND  T.FLAG = 'Y' AND T.DRAWINGNO IS NOT NULL AND T.DRAWINGNO NOT IN ( SELECT S.DRAWINGNO FROM PIPEAPPROVE_TAB S WHERE S.PROJECTID = '" + this.PIDcomboBox.SelectedItem + "') ";
                    DetailInfo.Application_Code.FillComboBox.GetFlowStatus(this.DRAWINGNOcomboBox, sqlstr);
                    break;

                case "��˴���":
                    sqlstr = "SELECT DISTINCT T.DRAWINGNO FROM PIPEAPPROVE_TAB T WHERE T.PROJECTID = '" + this.PIDcomboBox.SelectedItem + "' AND T.ASSESOR = '" + User.cur_user + "' AND T.STATE = 0 AND T.APPROVENEEDFLAG = 'Y'";
                    DetailInfo.Application_Code.FillComboBox.GetFlowStatus(this.DRAWINGNOcomboBox, sqlstr);
                    break;

                case "��������˵�СƱ":
                    sqlstr = "SELECT DISTINCT T.DRAWINGNO FROM SPOOL_TAB T WHERE T.FLAG = 'Y' AND  T.FLOWSTATUS = 1 AND T.FLOWSTATUS IN ( SELECT DISTINCT S.FLOWSTATUS FROM SPFLOWLOG_TAB S WHERE S.USERNAME = '" + User.cur_user + "' AND S.PROJECTID = '" + this.PIDcomboBox.SelectedItem + "' )";
                    DetailInfo.Application_Code.FillComboBox.GetFlowStatus(this.DRAWINGNOcomboBox, sqlstr);
                    break;

                case "��ͨ����˵�СƱ":
                    sqlstr = "SELECT DISTINCT T.DRAWINGNO FROM SPOOL_TAB T WHERE T.FLAG = 'Y' AND T.FLOWSTATUS = 2 AND T.DRAWINGNO IN (SELECT DISTINCT S.DRAWINGNO FROM SPFLOWLOG_TAB S WHERE S.USERNAME = '" + User.cur_user + "' AND S.PROJECTID = '" + this.PIDcomboBox.SelectedItem + "')";
                    DetailInfo.Application_Code.FillComboBox.GetFlowStatus(this.DRAWINGNOcomboBox, sqlstr);
                    break;

                case "���ͨ��СƱ":
                    sqlstr = "SELECT DISTINCT T.DRAWINGNO FROM PIPEAPPROVE_TAB T WHERE T.ASSESOR = '"+User.cur_user+"' AND T.STATE = 1 AND T.APPROVENEEDFLAG = 'N' AND T.PROJECTID = '"+this.PIDcomboBox.SelectedItem+"'";
                    DetailInfo.Application_Code.FillComboBox.GetFlowStatus(this.DRAWINGNOcomboBox, sqlstr);
                    break;

                case "���δͨ��СƱ":
                    sqlstr = "SELECT DISTINCT T.DRAWINGNO FROM SPOOL_TAB T WHERE T.FLOWSTATUS = 5 AND T.FLAG = 'Y' AND T.DRAWINGNO IN (SELECT DISTINCT S.DRAWINGNO FROM SPFLOWLOG_TAB S WHERE S.FLOWSTATUS = 5 AND  S.USERNAME = '"+User.cur_user+"' AND S.PROJECTID = '"+this.PIDcomboBox.SelectedItem+"')";
                    DetailInfo.Application_Code.FillComboBox.GetFlowStatus(this.DRAWINGNOcomboBox, sqlstr);
                    break;

                case "��˷���СƱ":
                    sqlstr = "SELECT DISTINCT T.DRAWINGNO FROM SPOOL_TAB T WHERE T.FLOWSTATUS = 5 AND T.FLAG = 'Y' AND T.PROJECTID = '" + this.PIDcomboBox.SelectedItem + "' AND  T.DRAWINGNO IN (SELECT S.DRAWINGNO from spflowlog_tab S where S.FLOWSTATUS in (select DISTINCT A.FLOWSTATUS from spflowlog_tab A where A.username = '"+User.cur_user+"' and A.flowstatus = 1) )";
                    DetailInfo.Application_Code.FillComboBox.GetFlowStatus(this.DRAWINGNOcomboBox, sqlstr);
                    break;
            }
        }

        /// <summary>
        /// ѡȡͼֽ�Ż�ȡСƱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DRAWINGNOcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            object drawnostr = this.DRAWINGNOcomboBox.SelectedItem;
            if (drawnostr !=null )
            {
                this.disbtn.Enabled = true;
            }
            if (this.tabControl1.SelectedTab.Text == "СƱ��Ϣ")
            {
                string sqlstr = "SELECT T.PROJECTID  ��Ŀ��, T.SPOOLNAME  СƱ��, T.SYSTEMID  ϵͳ��, T.SYSTEMNAME  ϵͳ��, T.PIPEGRADE  ��·�ȼ�, T.SURFACETREATMENT  ���洦��, T.WORKINGPRESSURE  ����ѹ��, T.PRESSURETESTFIELD  ѹ�����Գ���, T.PIPECHECKFIELD  У�ܳ��� , T.SPOOLWEIGHT  \"СƱ����(kg)\", T.PAINTCOLOR  ������ɫ, T.CABINTYPE  ��������, T.REVISION  СƱ�汾, T.SPOOLMODIFYTYPE  СƱ�޸�����,T.ELBOWTYPE  ��ͷ��ʽ, T.WELDTYPE  �㺸��, T.DRAWINGNO  ͼ��, T.BLOCKNO  �ֶκ�, T.MODIFYDRAWINGNO  �޸�֪ͨ����, T.REMARK  ��ע,  T.LOGNAME  ¼����, T.LOGDATE  ¼������, T.LINENAME  �ߺ� FROM SPOOL_TAB T WHERE T.FLAG = 'Y' AND T.PROJECTID = '" + this.PIDcomboBox.SelectedItem + "' AND T.DRAWINGNO = '" + this.DRAWINGNOcomboBox.SelectedItem + "' ";
                GetData(sqlstr, this.Appdgv);
            }

            else if (this.tabControl1.SelectedTab.Text == "������Ϣ")
            {
                string sqlstr = "select t.projectid ��Ŀ��, t.spoolname СƱ��, t.erpcode ERP����, t.materialname �����ͺ�, t.logname ¼����, t.logdate ¼������ from spoolmaterial_tab t where t.materialname like '%��%' and t.flag = 'Y' and t.projectid = '" + this.PIDcomboBox.SelectedItem + "' and t.spoolname in (select s.spoolname from spool_tab s where s.drawingno = '" + this.DRAWINGNOcomboBox.SelectedItem + "' and s.projectid = '" + this.PIDcomboBox.SelectedItem + "' and s.flag = 'Y') ";
                GetData(sqlstr, this.Materialdgv);
            }

            else if (this.tabControl1.SelectedTab.Text == "������Ϣ")
            {
                string sqlstr = "select t.projectid ��Ŀ��, t.spoolname СƱ��, t.erpcode ERP����, t.materialname �����ͺ�, t.logname ¼����, t.logdate ¼������ from spoolmaterial_tab t where t.materialname not like '%��%' and t.flag = 'Y' and t.projectid = '" + this.PIDcomboBox.SelectedItem + "' and t.spoolname in (select s.spoolname from spool_tab s where s.drawingno = '" + this.DRAWINGNOcomboBox.SelectedItem + "' and s.projectid = '" + this.PIDcomboBox.SelectedItem + "' and s.flag = 'Y') ";
                GetData(sqlstr, this.Partdgv);
            }

        }

        /// <summary>
        /// ��Ԫ���ʽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Appdgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            SpoolCellFormat.FormatCell(Appdgv);
        }

        /// <summary>
        /// ��ǩ��ת
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab.Text == "СƱ��Ϣ")
            {
                if (this.PIDcomboBox.Text.ToString() == string.Empty)
                {
                    return;
                }
                else
                {
                    if (this.DRAWINGNOcomboBox.Text.ToString() == string.Empty)
                    {
                        return;
                    }
                    else
                    {
                        string sqlstr = "SELECT T.PROJECTID  ��Ŀ��, T.SPOOLNAME  СƱ��, T.SYSTEMID  ϵͳ��, T.SYSTEMNAME  ϵͳ��, T.PIPEGRADE  ��·�ȼ�, T.SURFACETREATMENT  ���洦��, T.WORKINGPRESSURE  ����ѹ��, T.PRESSURETESTFIELD  ѹ�����Գ���, T.PIPECHECKFIELD  У�ܳ��� , T.SPOOLWEIGHT  \"СƱ����(kg)\", T.PAINTCOLOR  ������ɫ, T.CABINTYPE  ��������, T.REVISION  СƱ�汾, T.SPOOLMODIFYTYPE  СƱ�޸�����,T.ELBOWTYPE  ��ͷ��ʽ, T.WELDTYPE  �㺸��, T.DRAWINGNO  ͼ��, T.BLOCKNO  �ֶκ�, T.MODIFYDRAWINGNO  �޸�֪ͨ����, T.REMARK  ��ע,  T.LOGNAME  ¼����, T.LOGDATE  ¼������, T.LINENAME  �ߺ� FROM SPOOL_TAB T WHERE T.FLAG = 'Y' AND T.PROJECTID = '" + this.PIDcomboBox.SelectedItem + "' AND T.DRAWINGNO = '" + this.DRAWINGNOcomboBox.SelectedItem + "' ";
                        GetInfo(sqlstr, this.Appdgv);
                    }
                }
            }

            else if (this.tabControl1.SelectedTab.Text == "������Ϣ")
            {
                string sqlstr = "select t.projectid ��Ŀ��, t.spoolname СƱ��, t.erpcode ERP����, t.materialname �����ͺ�, t.logname ¼����, t.logdate ¼������ from spoolmaterial_tab t where t.materialname like '%��%' and t.flag = 'Y' and t.projectid = '" + this.PIDcomboBox.SelectedItem + "' and t.spoolname in (select s.spoolname from spool_tab s where s.drawingno = '" + this.DRAWINGNOcomboBox.SelectedItem + "' and s.projectid = '" + this.PIDcomboBox.SelectedItem + "' and s.flag = 'Y') ";
                GetInfo(sqlstr, this.Materialdgv);
            }

            else if (this.tabControl1.SelectedTab.Text == "������Ϣ")
            {
                string sqlstr = "select t.projectid ��Ŀ��, t.spoolname СƱ��, t.erpcode ERP����, t.materialname �����ͺ�, t.logname ¼����, t.logdate ¼������ from spoolmaterial_tab t where t.materialname not like '%��%' and t.flag = 'Y' and t.projectid = '" + this.PIDcomboBox.SelectedItem + "' and t.spoolname in (select s.spoolname from spool_tab s where s.drawingno = '" + this.DRAWINGNOcomboBox.SelectedItem + "' and s.projectid = '" + this.PIDcomboBox.SelectedItem + "' and s.flag = 'Y') ";
                GetInfo(sqlstr, this.Partdgv);
            }
        }

        /// <summary>
        /// ���ݰ�
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dgv"></param>
        private void GetInfo(string sql, DataGridView dgv)
        {
            DataSet ds = new DataSet();
            User.DataBaseConnect(sql, ds);
            dgv.DataSource = ds.Tables[0].DefaultView;
            ds.Dispose();
            SetStatus();
        }

        private void GetData(string sql, DataGridView dgv)
        {

            if (this.PIDcomboBox.SelectedItem.ToString() == string.Empty)
            {
                return;
            }
            else
            {
                if (this.DRAWINGNOcomboBox.SelectedItem.ToString() == string.Empty)
                {
                    return;
                }
                else
                {
                    GetInfo(sql, dgv);
                }
            }

        }

        /// <summary>
        /// ������Ϣѡ���б仯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Materialdgv_SelectionChanged(object sender, EventArgs e)
        {
            GetSelectionRowCount(this.Materialdgv);
        }

        /// <summary>
        /// ������Ϣѡ���б仯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Partdgv_SelectionChanged(object sender, EventArgs e)
        {
            GetSelectionRowCount(this.Partdgv);
        }

        /// <summary>
        /// ��ȡѡ������
        /// </summary>
        /// <param name="dgv"></param>
        private void GetSelectionRowCount(DataGridView dgv )
        {
            int count = dgv.SelectedRows.Count;
            this.toolStripStatusLabel2.Text = string.Format("��ǰѡ��{0}��", count);
        }

        /// <summary>
        /// ������˻���ˣ���ͼֽ�ţ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void requestbtn_Click(object sender, EventArgs e)
        {
            object pid = this.PIDcomboBox.SelectedItem;
            object drawno = this.DRAWINGNOcomboBox.SelectedItem;
            string btntext = this.requestbtn.Text.ToString();
            switch (btntext)
            {
                case "�������":
                    if (pid == null || drawno == null)
                    {
                        return;
                    }
                    else
                    {
                        string sql = "select count(*) from projectapprove where PROJECTID = '" + pid.ToString() + "'";
                        object count = User.GetScalar1(sql, DataAccess.OIDSConnStr);
                        if (Convert.ToInt16(count) == 0)
                        {
                            MessageBox.Show("����ϵͳ����ά����Ա��ϵ�����Ŀ����ˣ�");
                            return;
                        }
                        else
                        {
                            AssessorForm asform = new AssessorForm();
                            asform.pid = this.PIDcomboBox.SelectedItem.ToString();
                            asform.ShowDialog();
                            if (asform.DialogResult == DialogResult.OK)
                            {
                                string[] person = asform.personstr.Split(new char[] { ',' });
                                int num = person.Length - 1;
                                if (num < 0)
                                {
                                    MessageBox.Show("����ϵͳά����Ա��ϵ�����Ŀ����ˣ�");
                                    return;
                                }
                                string[] assesor = new string[num];

                                for (int n = 0; n < num; n++)
                                {
                                    assesor[n] = person[n];
                                }

                                char[] flag = new char[num];

                                flag[0] = 'Y';

                                for (int m = 1; m < num; m++)
                                {
                                    flag[m] = 'N';
                                }

                                for (int k = 1; k <= num; k++)
                                {
                                    DBConnection.InsertPipeApproveTab(pid.ToString(), drawno.ToString(), k, assesor[k - 1], 0, flag[k - 1]);
                                    if (k == num)
                                    {
                                        DBConnection.UpdateSpoolTabWithDrawingNo((int)FlowState.����, pid.ToString(), drawno.ToString());
                                        DBConnection.InsertApproveIntoFlowLog(User.cur_user, (int)FlowState.����, pid.ToString(), drawno.ToString());
                                        ClearControls();
                                        MessageBox.Show("�ȴ����ͨ��");
                                        return;
                                    }
                                }

                            }
                        }
                    }
                    break;

                case "ͬ��":

                    if (pid == null || drawno == null)
                    {
                        return;
                    }
                    else
                    {
                        string sql1 = "UPDATE PIPEAPPROVE_TAB SET STATE = 1,  APPROVENEEDFLAG = 'N' WHERE  INDEX_ID =(SELECT INDEX_ID FROM PIPEAPPROVE_TAB WHERE ASSESOR ='" + User.cur_user + "' AND APPROVENEEDFLAG = 'Y' AND PROJECTID = '" + this.PIDcomboBox.SelectedItem + "'  AND DRAWINGNO = '" + this.DRAWINGNOcomboBox.SelectedItem + "') AND PROJECTID = '" + this.PIDcomboBox.SelectedItem + "' AND DRAWINGNO = '" + this.DRAWINGNOcomboBox.SelectedItem + "'";
                        User.UpdateCon(sql1, DataAccess.OIDSConnStr);
                        string sql2 = "UPDATE PIPEAPPROVE_TAB SET   APPROVENEEDFLAG = 'Y' WHERE STATE = 0 AND INDEX_ID =((SELECT INDEX_ID FROM PIPEAPPROVE_TAB WHERE ASSESOR ='" + User.cur_user + "' AND APPROVENEEDFLAG = 'N' AND PROJECTID = '" + this.PIDcomboBox.SelectedItem + "' AND DRAWINGNO = '" + this.DRAWINGNOcomboBox.SelectedItem + "')+1) AND PROJECTID = '" + this.PIDcomboBox.SelectedItem + "' AND DRAWINGNO = '" + this.DRAWINGNOcomboBox.SelectedItem + "' ";
                        User.UpdateCon(sql2, DataAccess.OIDSConnStr);

                        DataSet ds = new DataSet();
                        string sqlcount = "SELECT COUNT(*) FROM PROJECTAPPROVE WHERE PROJECTID = '" + this.PIDcomboBox.SelectedItem + "'";
                        User.DataBaseConnect(sqlcount, ds);
                        int countvalue = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                        ds.Dispose();

                        DataSet ds1 = new DataSet();
                        string sql3 = "SELECT DISTINCT INDEX_ID FROM PIPEAPPROVE_TAB WHERE ASSESOR = '" + User.cur_user + "' AND PROJECTID = '" + this.PIDcomboBox.SelectedItem + "' AND DRAWINGNO = '" + this.DRAWINGNOcomboBox.SelectedItem + "' ";
                        User.DataBaseConnect(sql3, ds1);
                        if (Convert.ToInt32(ds1.Tables[0].Rows[0][0]) == countvalue)
                        {
                            DBConnection.UpdateSpoolTabWithDrawingNo((int)FlowState.���ͨ��, pid.ToString(), drawno.ToString());

                            DBConnection.InsertApproveIntoFlowLog(User.cur_user, (int)FlowState.���ͨ��, pid.ToString(), drawno.ToString());

                            ClearControls();

                        }

                        else
                        {
                            DBConnection.InsertApproveIntoFlowLog(User.cur_user, (int)FlowState.����, pid.ToString(), drawno.ToString());
                            ClearControls();
                        }
                        ds1.Dispose();
                    }

                    break;

                case "��������":
                    if (pid == null || drawno == null)
                    {
                        return;
                    }
                    else
                    {
                        DBConnection.UpdateSpoolTabWithDrawingNo((int)FlowState.������, pid.ToString(), drawno.ToString());
                        DBConnection.InsertApproveIntoFlowLog(User.cur_user, (int)FlowState.������, pid.ToString(), drawno.ToString());
                        ClearControls();
                    }
                    break;

                case "����":
                    if (pid == null || drawno == null)
                    {
                        return;
                    }
                    else
                    {
                        DBConnection.UpdateSpoolTabWithDrawingNo((int)FlowState.������˷���, pid.ToString(), drawno.ToString());
                        DBConnection.InsertApproveIntoFlowLog(User.cur_user, (int)FlowState.������˷���, pid.ToString(), drawno.ToString());
                        ClearControls();
                    }
                    break;

                default:
                    break;
            }

        }

        /// <summary>
        /// ��˲�ͨ�����������Ա
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void disbtn_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("ȷ��Ҫ������", "WARNNING", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            if (result == DialogResult.OK)
            {
                Add_Remark remarkform = new Add_Remark();
                remarkform.ShowDialog();
                if (remarkform.DialogResult == DialogResult.OK)
                {
                    string marktext = remarkform.GetRemark();
                    string sqlstr = "UPDATE PIPEAPPROVE_TAB SET APPROVENEEDFLAG = 'N', STATE = 1 WHERE PROJECTID = '" + this.PIDcomboBox.SelectedItem + "' AND DRAWINGNO = '" + this.DRAWINGNOcomboBox.SelectedItem + "'";
                    User.UpdateCon(sqlstr, DataAccess.OIDSConnStr);
                    DBConnection.UpdateSpoolTabWithDrawingNoRemark((int)FlowState.��˷���, marktext, this.PIDcomboBox.SelectedItem.ToString(), this.DRAWINGNOcomboBox.SelectedItem.ToString());
                    DBConnection.InsertApproveFeedBackFlowLog(User.cur_user, (int)FlowState.��˷���, marktext, this.PIDcomboBox.SelectedItem.ToString(), this.DRAWINGNOcomboBox.SelectedItem.ToString());
                    ClearControls();
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// ���ǰ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApproveForm_Activated(object sender, EventArgs e)
        {
            MDIForm.tool_strip.Items[0].Enabled = false;
        }

        /// <summary>
        /// ��տؼ�
        /// </summary>
        private void ClearControls()
        {
            this.DRAWINGNOcomboBox.Items.Remove(this.DRAWINGNOcomboBox.SelectedItem);
            this.Appdgv.DataSource = this.Materialdgv.DataSource = this.Partdgv.DataSource = null;
            SetStatus();
        }




    }

   
}