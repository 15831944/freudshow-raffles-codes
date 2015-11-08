using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Data.OracleClient;

namespace DetailInfo
{
    public partial class SpoolGeneralViewForm : Form
    {
        public ContextMenuStrip displayMenueStrip = null;
        public System.DateTime currentTime = System.DateTime.Now;
        public SpoolGeneralViewForm()
        {
            InitializeComponent();
            displayMenueStrip = this.DisplaycontextMenuStrip;
            //DisplaycontextMenuStrip.Enabled = false;
            for (int i = 0; i < DisplaycontextMenuStrip.Items.Count; i++)
            {
                DisplaycontextMenuStrip.Items[i].Visible = false;
            }
        }
        
        /// <summary>
        /// �رմ���ʱsearch��ť����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpoolGeneralViewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDIForm.tool_strip.Items[0].Enabled = false;
            MDIForm.tool_strip.Items[1].Enabled = false;
        }

        #region ��Ʋ��Ų���
        /// <summary>
        /// ����СƱ��صĲ��ϣ����Ӽ����ӹ��������Լ����������Ϣչʾ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetailstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            int count = this.OverViewdgv.SelectedRows.Count;
            if (count > 0)
            {
                for (int i = 0; i < OverViewdgv.Rows.Count; i++)
                {
                    if (OverViewdgv.Rows[i].Selected == true)
                    {
                        string spoolname = OverViewdgv.Rows[i].Cells["СƱ��"].Value.ToString();
                        sb.Append(spoolname + ",");
                    }
                }

                DetailsForm detailform = new DetailsForm();
                detailform.Text = "СƱ��ϸ��Ϣ";
                detailform.MdiParent = this.MdiParent;
                detailform.spoolstr = sb.ToString();
                detailform.Show();

            }
            else
            {
                return;
            }
            
            
        }

        /// <summary>
        /// �����Ա��ת���ܼӹ�֮ǰ�����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RequireApproveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ArrayList req_arry = new ArrayList(); ArrayList st_arry = new ArrayList(); ArrayList pidlist = new ArrayList();
            //if (OverViewdgv.RowCount==0)
            //{
            //    return;
            //}
            //if (OverViewdgv.RowCount != 0)
            //{
            //    int rowcount = this.OverViewdgv.SelectedRows.Count;
            //    if (rowcount > 0)
            //    {
            //        for (int i = 0; i < OverViewdgv.RowCount; i++)
            //        {
            //            if (OverViewdgv.Rows[i].Selected == true)
            //            {
            //                string sname = OverViewdgv.Rows[i].Cells["СƱ��"].Value.ToString();
            //                string status = OverViewdgv.Rows[i].Cells["����״̬��ʶ"].Value.ToString();
            //                st_arry.Add(status);
            //                foreach (string item in st_arry)
            //                {
            //                    if (item != "��ʼ")
            //                    {
            //                        MessageBox.Show("�����ظ����");
            //                        return;
            //                    }
            //                }
            //            }
            //        }

            //        AssessorForm aform = new AssessorForm();
            //        for (int k = 0; k < OverViewdgv.Rows.Count; k++)
            //        {
            //            if (OverViewdgv.Rows[k].Selected == true)
            //            {
            //                string pidstr = OverViewdgv.Rows[k].Cells["��Ŀ��"].Value.ToString();
            //                pidlist.Add(pidstr);
            //            }
            //        }
            //        if (pidlist.Count == 1)
            //        {
            //            aform.pid = pidlist[0].ToString();
            //            string sql1 = "select count(PROJECTID) from projectapprove where PROJECTID = '" + aform.pid + "'";
            //            object count1 = User.GetScalar1(sql1, DataAccess.OIDSConnStr);
            //            if (Convert.ToInt16(count1) == 0)
            //            {
            //                MessageBox.Show("����ϵͳ����ά����Ա��ϵ�����Ŀ����ˣ�");
            //                return;
            //            }
            //        }
            //        else if (pidlist.Count > 1)
            //        {
            //            for (int h = 0; h < pidlist.Count - 1; h++)
            //            {
            //                if (pidlist[h].ToString() == pidlist[h + 1].ToString())
            //                {
            //                    aform.pid = pidlist[0].ToString();
            //                    string sql2 = "select count(PROJECTID) from projectapprove where PROJECTID = '" + aform.pid + "'";
            //                    object count2 = User.GetScalar1(sql2, DataAccess.OIDSConnStr);
            //                    if (Convert.ToInt16(count2) == 0)
            //                    {
            //                        MessageBox.Show("����ϵͳ����ά����Ա��ϵ�����Ŀ����ˣ�");
            //                        return;
            //                    }
            //                }
            //                else
            //                {
            //                    MessageBox.Show("��ȷ����ѡ����ͬһ��Ŀ");
            //                    return;
            //                }
            //            }
            //        }

            //        aform.ShowDialog();
            //        try
            //        {
            //            if (aform.DialogResult == DialogResult.OK)
            //            {
            //                string[] person = aform.personstr.Split(new char[] { ',' });
            //                int num = person.Length - 1;
            //                if (num < 0)
            //                {
            //                    MessageBox.Show("����ϵͳά����Ա��ϵ�����Ŀ����ˣ�");
            //                    return;
            //                }
            //                string[] assesor = new string[num];
            //                for (int n = 0; n < num; n++)
            //                {
            //                    assesor[n] = person[n];
            //                }

            //                char[] flag = new char[num];
            //                flag[0] = 'Y';
            //                for (int m = 1; m < num; m++)
            //                {
            //                    flag[m] = 'N';
            //                }

            //                foreach (DataGridViewRow dr in OverViewdgv.Rows)
            //                {
            //                    if (dr.Selected == true)
            //                    {
            //                        int id = Convert.ToInt32(dr.Index);
            //                        req_arry.Add(id);
            //                    }
            //                }
            //                if (req_arry.Count != 0)
            //                {
            //                    for (int j = 0; j < req_arry.Count; j++)
            //                    {
            //                        int index = Convert.ToInt32(req_arry[j]);
            //                        OverViewdgv.Rows[index].Cells["����״̬��ʶ"].Value = "����";
            //                        string prid = OverViewdgv.Rows[index].Cells["��Ŀ��"].Value.ToString();
            //                        string namestr = OverViewdgv.Rows[index].Cells["СƱ��"].Value.ToString().Trim();

            //                        DBConnection.UpDateState((int)FlowState.����, namestr, prid, "Y");

            //                        for (int k = 1; k <= num; k++)
            //                        {
            //                            //string app_sql = "INSERT INTO SPOOL_APPROVE_TAB (SPOOLNAME, INDEX_ID, ASSESOR, STATE, APPROVENEEDFLAG) VALUES ('" + namestr + "', " + k + ", '" + assesor[k - 1] + "', 0, '" + flag[k - 1] + "')";
            //                            //User.UpdateCon(app_sql, DataAccess.OIDSConnStr);
            //                            DBConnection.InsertSpoolApproveTab(namestr, k, assesor[k - 1], 0, flag[k - 1]);
            //                        }

            //                        DBConnection.InsertFlowLog(namestr, User.cur_user, (int)FlowState.����, prid);

            //                        if (j == req_arry.Count - 1)
            //                        {
            //                            MessageBox.Show("�ȴ����ͨ����");
            //                            return;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message, "������Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
        }

        /// <summary>
        /// ���ͨ�����������Աת�����ܼӹ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToMachineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ArrayList tran_array = new ArrayList();
            //ArrayList status_arrray = new ArrayList();
            //if (OverViewdgv.RowCount != 0)
            //{
            //    int rowcount = this.OverViewdgv.SelectedRows.Count;
            //    if (rowcount > 0)
            //    {
            //        for (int i = 0; i < OverViewdgv.Rows.Count; i++)
            //        {
            //            if (OverViewdgv.Rows[i].Selected == true)
            //            {
            //                string status = OverViewdgv.Rows[i].Cells["����״̬��ʶ"].Value.ToString();
            //                status_arrray.Add(status);
            //            }
            //        }
            //        foreach (string item in status_arrray)
            //        {
            //            if (item != "���ͨ��")
            //            {
            //                MessageBox.Show("��ȷ��������ͨ�����");
            //                return;
            //            }

            //        }
            //        DialogResult result;
            //        result = MessageBox.Show("ȷ��ת���ܼӹ���", "ת�ܼӹ�", MessageBoxButtons.OKCancel);
            //        if (result == DialogResult.OK)
            //        {
            //            foreach (DataGridViewRow dr in OverViewdgv.Rows)
            //            {
            //                if (dr.Selected == true)
            //                {
            //                    int id = Convert.ToInt32(dr.Index);
            //                    tran_array.Add(id);
            //                }
            //            }
            //            if (tran_array.Count != 0)
            //            {
            //                for (int j = 0; j < tran_array.Count; j++)
            //                {
            //                    int index = Convert.ToInt32(tran_array[j]);
            //                    string name = OverViewdgv.Rows[index].Cells["СƱ��"].Value.ToString();
            //                    string pidname = OverViewdgv.Rows[index].Cells["��Ŀ��"].Value.ToString();
            //                    OverViewdgv.Rows[index].Cells["����״̬��ʶ"].Value = "������";

            //                    DBConnection.UpDateState((int)FlowState.������, name, pidname, "Y");

            //                    DBConnection.InsertFlowLog(name, User.cur_user, (int)FlowState.������, pidname);

            //                    if (j == tran_array.Count - 1)
            //                    {
            //                        MessageBox.Show("���ݼ�¼�ɹ���");
            //                    }
            //                }

            //            }
            //        }
            //        if (result == DialogResult.Cancel)
            //        {
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
        }

        /// <summary>
        /// ������·������Ϣ��ˮ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ExportPipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report.SPLPIPEReprot pipreport = new DetailInfo.Report.SPLPIPEReprot();
            pipreport.MdiParent = MDIForm.pMainWin;
            pipreport.Show();
        }

        /// <summary>
        /// ������·��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportMaterialtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report.MaterAtt partreport = new DetailInfo.Report.MaterAtt();
            partreport.MdiParent = MDIForm.pMainWin;
            partreport.Show();
        }

        #endregion

        
        #region �ܼӹ����Ų���
        /// <summary>
        /// �ɹܼӹ������˷�������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void AllocateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ArrayList status_array = new ArrayList();
            //ArrayList allocation_list = new ArrayList();
            //string charger = string.Empty;
            //if (OverViewdgv.RowCount != 0)
            //{
            //    int rowcount = this.OverViewdgv.SelectedRows.Count;
            //    if (rowcount > 0)
            //    {
            //        for (int k = 0; k < OverViewdgv.RowCount; k++)
            //        {
            //            if (OverViewdgv.Rows[k].Selected == true)
            //            {
            //                string status = OverViewdgv.Rows[k].Cells["����״̬��ʶ"].Value.ToString();
            //                status_array.Add(status);
            //            }
            //            foreach (string item in status_array)
            //            {
            //                if (item != "������")
            //                {
            //                    MessageBox.Show("��ȷ���������Ѵ��ڴ�����״̬��");
            //                    return;
            //                }
            //            }

            //        }
            //        //WorkersInfo wform = new WorkersInfo();
            //        //wform.ShowDialog();
            //        //Allocation af = new Allocation();
            //        //af.ShowDialog();
            //        AssignTask atform = new AssignTask();
            //        atform.ShowDialog();
            //        try
            //        {
            //            if (atform.DialogResult == DialogResult.OK)
            //            {
            //                //object obj = wform.GetChargerName();
            //                string objstr = atform.Personnames.ToString();
            //                //string charger = af.GetChargerName();
            //                //if (obj == null)
            //                if (objstr == string.Empty)
            //                {
            //                    MessageBox.Show("�����˲���Ϊ�գ������·���");
            //                    return;
            //                }
            //                else
            //                {
            //                    charger = objstr.ToString();
            //                }
            //                foreach (DataGridViewRow dr in OverViewdgv.Rows)
            //                {
            //                    if (dr.Selected == true)
            //                    {
            //                        int id = Convert.ToInt32(dr.Index);
            //                        allocation_list.Add(id);
            //                    }
            //                }
            //                if (allocation_list.Count != 0)
            //                {
            //                    for (int m = 0; m < allocation_list.Count; m++)
            //                    {
            //                        int index = Convert.ToInt32(allocation_list[m]);
            //                        string pid = OverViewdgv.Rows[index].Cells["��Ŀ��"].Value.ToString();
            //                        string name = OverViewdgv.Rows[index].Cells["СƱ��"].Value.ToString();
            //                        OverViewdgv.Rows[index].Cells["������"].Value = charger;
            //                        OverViewdgv.Rows[index].Cells["����״̬��ʶ"].Value = "�ӹ���";

            //                        OverViewdgv.Rows[index].Cells["�����������"].Value = currentTime.Date;

            //                        DBConnection.InsertTaskCarrier(charger, currentTime.Date, (int)FlowState.�ӹ���, name, pid, "Y");
            //                        DBConnection.InsertFlowLog(name, User.cur_user, (int)FlowState.�ӹ���, pid);

            //                    }
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message, "������Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("û�й����������");
            //    return;
            //}
        }

        /// <summary>
        /// �ӹ���ɺ�ת���ʼ첿�Ŵ���
        /// </summary>
        public string QCremark = "";
        private void ToQCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArrayList qclist = new ArrayList();
            ArrayList status_array = new ArrayList();
            if (OverViewdgv.RowCount != 0)
            {
                int rowcount = this.OverViewdgv.SelectedRows.Count;
                if (rowcount > 0)
                {
                    for (int j = 0; j < OverViewdgv.RowCount; j++)
                    {
                        if (OverViewdgv.Rows[j].Selected == true)
                        {
                            string status = OverViewdgv.Rows[j].Cells["����״̬��ʶ"].Value.ToString();
                            status_array.Add(status);
                        }
                        foreach (string item in status_array)
                        {
                            if (item != "�ӹ���")
                            {
                                MessageBox.Show("��ȷ���������Ѽӹ���ɣ�");
                                return;
                            }
                        }
                    }
                    AddQCRemark addQCR = new AddQCRemark();
                    addQCR.ShowDialog();
                    try
                    {
                        if (addQCR.DialogResult == DialogResult.OK)
                        {
                            QCremark = addQCR.GetQCRemark();
                            foreach (DataGridViewRow dr in OverViewdgv.Rows)
                            {
                                if (dr.Selected == true)
                                {
                                    int id = Convert.ToInt32(dr.Index);
                                    qclist.Add(id);
                                }
                            }
                            if (qclist.Count != 0)
                            {
                                for (int i = 0; i < qclist.Count; i++)
                                {
                                    int index = Convert.ToInt32(qclist[i]);
                                    string name = OverViewdgv.Rows[index].Cells["СƱ��"].Value.ToString();
                                    string proid = OverViewdgv.Rows[index].Cells["��Ŀ��"].Value.ToString();
                                    OverViewdgv.Rows[index].Cells["����״̬��ʶ"].Value = "����";

                                    DBConnection.UpDateState((int)FlowState.����, name, proid, "Y");
                                    DBConnection.InsertFlowLog(name, User.cur_user, (int)FlowState.����, proid);
                                }

                            }
                            if (qclist.Count == 0)
                            {
                                MessageBox.Show("��ѡ��Ҫת���ʼ��СƱ��");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "������Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("û��Ҫת������������");
                return;
            }
        }

        /// <summary>
        /// �ܼӹ����������СƱ�����������Ա
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FeedBackToDesignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ArrayList feedback = new ArrayList(); ArrayList select_feedback = new ArrayList(); ArrayList status_array = new ArrayList();
            //if (OverViewdgv.RowCount == 0)
            //{
            //    return;
            //}
            //if (OverViewdgv.RowCount != 0)
            //{
            //    int rowcount = this.OverViewdgv.SelectedRows.Count;
            //    if (rowcount > 0)
            //    {
            //        for (int i = 0; i < OverViewdgv.RowCount; i++)
            //        {
            //            if (OverViewdgv.Rows[i].Selected == true)
            //            {
            //                feedback.Add(i);
            //                string status = OverViewdgv.Rows[i].Cells["����״̬��ʶ"].Value.ToString();
            //                status_array.Add(status);
            //            }
            //            foreach (string item in status_array)
            //            {
            //                if (item != "�ӹ���")
            //                {
            //                    MessageBox.Show("��ѡ��ܷ��ص���ƣ�");
            //                    return;
            //                }
            //            }

            //        }
            //        if (feedback.Count != 0)
            //        {
            //            DialogResult result;
            //            result = MessageBox.Show("ȷ�������������ƣ�", "���ⷴ��", MessageBoxButtons.OKCancel);
            //            if (result == DialogResult.OK)
            //            {
            //                Add_Remark FeedBackRemark = new Add_Remark();
            //                FeedBackRemark.ShowDialog();
            //                try
            //                {
            //                    if (FeedBackRemark.DialogResult == DialogResult.OK)
            //                    {
            //                        foreach (DataGridViewRow dr in OverViewdgv.Rows)
            //                        {
            //                            if (dr.Selected == true)
            //                            {
            //                                int id = Convert.ToInt32(dr.Index);
            //                                select_feedback.Add(id);
            //                            }
            //                        }
            //                        if (select_feedback.Count != 0)
            //                        {
            //                            for (int j = 0; j < select_feedback.Count; j++)
            //                            {
            //                                int index = Convert.ToInt32(select_feedback[j]);

            //                                string remark = FeedBackRemark.GetRemark();
            //                                string name = OverViewdgv.Rows[index].Cells["СƱ��"].Value.ToString();
            //                                string prid = OverViewdgv.Rows[index].Cells["��Ŀ��"].Value.ToString();
            //                                OverViewdgv.Rows[index].Cells["����״̬��ʶ"].Value = "�������";

            //                                DBConnection.InsertFlowLogWithRemark(name, User.cur_user, (int)FlowState.�������, remark, prid);

            //                                DBConnection.UpdateSpoolTabWithRemark((int)FlowState.�������, remark, name, prid, "Y");
            //                            }
            //                        }
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    MessageBox.Show(ex.Message, "������Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                }

            //            }
            //            if (result == DialogResult.Cancel)
            //            {
            //                return;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
        }
        #endregion

        #region �ʼಿ�Ų���
        /// <summary>
        /// �ʼಿ�ż���ͨ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PassQCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ArrayList status_array = new ArrayList();
            //ArrayList selectlist = new ArrayList();
            //if (OverViewdgv.RowCount != 0)
            //{
            //    int rowcount = this.OverViewdgv.SelectedRows.Count;
            //    if (rowcount > 0)
            //    {
            //        for (int i = 0; i < OverViewdgv.RowCount; i++)
            //        {
            //            if (OverViewdgv.Rows[i].Selected == true)
            //            {
            //                string status = OverViewdgv.Rows[i].Cells["����״̬��ʶ"].Value.ToString();
            //                status_array.Add(status);
            //            }
            //            foreach (string item in status_array)
            //            {
            //                if (item != "����")
            //                {
            //                    MessageBox.Show("��ȷ��������Ϊ����״̬��");
            //                    return;
            //                }
            //            }
            //        }

            //        DialogResult result;
            //        result = MessageBox.Show("ȷ���ʼ�ͨ����", "�ʼ�ͨ��", MessageBoxButtons.OKCancel);
            //        if (result == DialogResult.OK)
            //        {
            //            foreach (DataGridViewRow dr in OverViewdgv.Rows)
            //            {

            //                if (dr.Selected == true)
            //                {
            //                    if (dr.Cells["����״̬"].Value.ToString() == "��������")
            //                    {
            //                        MessageBox.Show("��ѡ������ݲ��ܲ���", "����", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //                        return;
            //                    }
            //                    int id = Convert.ToInt32(dr.Index);
            //                    selectlist.Add(id);
            //                }
            //            }
            //            if (selectlist.Count != 0)
            //            {
            //                for (int j = 0; j < selectlist.Count; j++)
            //                {
            //                    int index = Convert.ToInt32(selectlist[j]);

            //                    string name = OverViewdgv.Rows[index].Cells["СƱ��"].Value.ToString();
            //                    string prid = OverViewdgv.Rows[index].Cells["��Ŀ��"].Value.ToString();
            //                    OverViewdgv.Rows[index].Cells["����״̬��ʶ"].Value = "����ͨ������װ";

            //                    DBConnection.InsertFlowLog(name, User.cur_user, (int)FlowState.����ͨ������װ, prid);
            //                    DBConnection.UpDateState((int)FlowState.����ͨ������װ, name, prid, "Y");
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("��ʱû�д���СƱ");
            //    return;
            //}

        }

        /// <summary>
        /// �ʼಿ�ŷ������ܼӹ�������СƱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FeedBackToMachineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ArrayList status_array = new ArrayList();
            //ArrayList select_list = new ArrayList();
            //if (OverViewdgv.RowCount != 0)
            //{
            //    int rowcount = this.OverViewdgv.SelectedRows.Count;
            //    if (rowcount > 0)
            //    {
            //        for (int i = 0; i < OverViewdgv.RowCount; i++)
            //        {
            //            if (OverViewdgv.Rows[i].Selected == true)
            //            {
            //                string status = OverViewdgv.Rows[i].Cells["����״̬��ʶ"].Value.ToString();
            //                status_array.Add(status);
            //            }
            //            foreach (string item in status_array)
            //            {
            //                if (item != "����")
            //                {
            //                    MessageBox.Show("��ȷ��������Ϊ����״̬��");
            //                    return;
            //                }
            //            }
            //        }

            //        DialogResult result;
            //        result = MessageBox.Show("ȷ������������ӹ���", "���ⷴ��", MessageBoxButtons.OKCancel);
            //        if (result == DialogResult.OK)
            //        {
            //            Add_Remark qcremark = new Add_Remark();
            //            qcremark.ShowDialog();
            //            try
            //            {
            //                if (qcremark.DialogResult == DialogResult.OK)
            //                {
            //                    foreach (DataGridViewRow dr in OverViewdgv.Rows)
            //                    {
            //                        if (dr.Selected == true)
            //                        {
            //                            int id = Convert.ToInt32(dr.Index);
            //                            select_list.Add(id);
            //                        }
            //                    }
            //                    if (select_list.Count != 0)
            //                    {
            //                        for (int k = 0; k < select_list.Count; k++)
            //                        {
            //                            int index = Convert.ToInt32(select_list[k]);

            //                            string remark = qcremark.GetRemark();
            //                            string name = OverViewdgv.Rows[index].Cells["СƱ��"].Value.ToString();
            //                            string prid = OverViewdgv.Rows[index].Cells["��Ŀ��"].Value.ToString();
            //                            OverViewdgv.Rows[index].Cells["����״̬��ʶ"].Value = "���ϸ�";

            //                            DBConnection.InsertFlowLogWithRemark(name, User.cur_user, (int)FlowState.���ϸ�, remark, prid);

            //                            DBConnection.UpdateSpoolTabWithRemark((int)FlowState.���ϸ�, remark, name, prid, "Y");
            //                        }

            //                    }
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show(ex.Message, "������Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("��ʱû�д���СƱ��");
            //    return;
            //}
        }
        #endregion

        #region �������Ų���
        /// <summary>
        /// ��ʼ��װ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ArrayList selectlist = new ArrayList();
            //ArrayList prolist = new ArrayList();
            //if (OverViewdgv.RowCount != 0)
            //{
            //    int rowcount = this.OverViewdgv.SelectedRows.Count;
            //    if (rowcount > 0)
            //    {
            //        for (int i = 0; i < OverViewdgv.Rows.Count; i++)
            //        {
            //            if (OverViewdgv.Rows[i].Selected == true)
            //            {
            //                string status = OverViewdgv.Rows[i].Cells["����״̬��ʶ"].Value.ToString();
            //                selectlist.Add(status);
            //            }
            //            foreach (string item in selectlist)
            //            {
            //                if (item != "����ͨ������װ")
            //                {
            //                    MessageBox.Show("��ȷ����ѡ��Ϊ����װ״̬��");
            //                    return;
            //                }
            //            }
            //        }

            //        DialogResult result;
            //        result = MessageBox.Show("ȷ��ʵʩ��װ��", "ʵʩ��װ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            //        if (result == DialogResult.OK)
            //        {
            //            foreach (DataGridViewRow dr in OverViewdgv.Rows)
            //            {
            //                if (dr.Selected == true)
            //                {
            //                    int id = Convert.ToInt32(dr.Index);
            //                    prolist.Add(id);
            //                }
            //            }
            //            if (prolist.Count != 0)
            //            {
            //                for (int j = 0; j < prolist.Count; j++)
            //                {
            //                    int index = Convert.ToInt32(prolist[j]);
            //                    string name = OverViewdgv.Rows[index].Cells["СƱ��"].Value.ToString();
            //                    string proid = OverViewdgv.Rows[index].Cells["��Ŀ��"].Value.ToString();
            //                    OverViewdgv.Rows[index].Cells["����״̬��ʶ"].Value = "��װ��";

            //                    DBConnection.UpDateState((int)FlowState.��װ��, name, proid, "Y");
            //                    DBConnection.InsertFlowLog(name, User.cur_user, (int)FlowState.��װ��, proid);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("��ʱû�в�ѯ����Ҫ�����ݣ�");
            //    return;
            //}
        }

        /// <summary>
        /// ��װ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompleteInstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ArrayList selectlist = new ArrayList();
            //ArrayList prolist = new ArrayList();
            //if (OverViewdgv.RowCount != 0)
            //{
            //    int rowcount = this.OverViewdgv.SelectedRows.Count;
            //    if (rowcount > 0)
            //    {
            //        for (int i = 0; i < OverViewdgv.RowCount; i++)
            //        {
            //            if (OverViewdgv.Rows[i].Selected == true)
            //            {
            //                string status = OverViewdgv.Rows[i].Cells["����״̬��ʶ"].Value.ToString();
            //                selectlist.Add(status);
            //            }
            //            foreach (string item in selectlist)
            //            {
            //                if (item != "��װ��")
            //                {
            //                    MessageBox.Show("��ȷ����ѡ��ڴ���װ״̬��");
            //                    return;
            //                }
            //            }
            //        }

            //        DialogResult result;
            //        result = MessageBox.Show("ȷ����װ��ϣ�", "��װ���", MessageBoxButtons.OKCancel);
            //        if (result == DialogResult.OK)
            //        {
            //            foreach (DataGridViewRow dr in OverViewdgv.Rows)
            //            {
            //                if (dr.Selected == true)
            //                {
            //                    int id = Convert.ToInt32(dr.Index);
            //                    prolist.Add(id);
            //                }
            //            }
            //            if (prolist.Count != 0)
            //            {
            //                for (int j = 0; j < prolist.Count; j++)
            //                {
            //                    int index = Convert.ToInt32(prolist[j]);
            //                    string name = OverViewdgv.Rows[index].Cells["СƱ��"].Value.ToString();
            //                    string proid = OverViewdgv.Rows[index].Cells["��Ŀ��"].Value.ToString();
            //                    OverViewdgv.Rows[index].Cells["����״̬��ʶ"].Value = "������";

            //                    DBConnection.UpDateState((int)FlowState.������, name, proid, "Y");
            //                    DBConnection.InsertFlowLog(name, User.cur_user, (int)FlowState.������, proid);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("��ʱû�в�ѯ����Ҫ�����ݣ�");
            //    return;
            //}
        }
        #endregion 

        #region ���Բ��Ų���
        private void CompleteModulatetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ArrayList selectlist = new ArrayList();
            //ArrayList prolist = new ArrayList();
            //if (OverViewdgv.RowCount != 0)
            //{
            //    int rowcount = this.OverViewdgv.SelectedRows.Count;
            //    if (rowcount > 0)
            //    {
            //        for (int i = 0; i < OverViewdgv.RowCount; i++)
            //        {
            //            if (OverViewdgv.Rows[i].Selected == true)
            //            {
            //                string status = OverViewdgv.Rows[i].Cells["����״̬��ʶ"].Value.ToString();
            //                selectlist.Add(status);
            //            }
            //            foreach (string item in selectlist)
            //            {
            //                if (item != "������")
            //                {
            //                    MessageBox.Show("��ȷ����ѡ��ڴ�����״̬��");
            //                    return;
            //                }
            //            }
            //        }

            //        DialogResult result;
            //        result = MessageBox.Show("ȷ��������ɣ�", "�������", MessageBoxButtons.OKCancel);
            //        if (result == DialogResult.OK)
            //        {
            //            foreach (DataGridViewRow dr in OverViewdgv.Rows)
            //            {
            //                if (dr.Selected == true)
            //                {
            //                    int id = Convert.ToInt32(dr.Index);
            //                    prolist.Add(id);
            //                }
            //            }
            //            if (prolist.Count != 0)
            //            {
            //                for (int j = 0; j < prolist.Count; j++)
            //                {
            //                    int index = Convert.ToInt32(prolist[j]);
            //                    string name = OverViewdgv.Rows[index].Cells["СƱ��"].Value.ToString();
            //                    string proid = OverViewdgv.Rows[index].Cells["��Ŀ��"].Value.ToString();
            //                    OverViewdgv.Rows[index].Cells["����״̬��ʶ"].Value = "�������";

            //                    DBConnection.UpDateState((int)FlowState.�������, name, proid, "Y");
            //                    DBConnection.InsertFlowLog(name, User.cur_user, (int)FlowState.�������, proid);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("��ǰû��ѡ���κ��У�", "WARNNING", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //        return;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("��ʱû�в�ѯ����Ҫ�����ݣ�");
            //    return;
            //}
        }

        private void FeedBackToInstalltoolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ArrayList select_list = new ArrayList();
            //ArrayList prolist = new ArrayList();
            //if (OverViewdgv.RowCount != 0)
            //{
            //    int rowcount = this.OverViewdgv.SelectedRows.Count;
            //    if (rowcount > 0)
            //    {
            //        for (int i = 0; i < OverViewdgv.RowCount; i++)
            //        {
            //            if (OverViewdgv.Rows[i].Selected == true)
            //            {
            //                string status = OverViewdgv.Rows[i].Cells["����״̬��ʶ"].Value.ToString();
            //                prolist.Add(status);
            //            }
            //            foreach (string item in prolist)
            //            {
            //                if (item != "������")
            //                {
            //                    MessageBox.Show("��ȷ����ѡ��ڴ�����״̬��");
            //                    return;
            //                }
            //            }
            //        }
            //        DialogResult result;
            //        result = MessageBox.Show("ȷ�������������װ��", "���ⷴ��", MessageBoxButtons.OKCancel);
            //        if (result == DialogResult.OK)
            //        {
            //            Add_Remark qcremark = new Add_Remark();
            //            qcremark.ShowDialog();
            //            try
            //            {
            //                if (qcremark.DialogResult == DialogResult.OK)
            //                {
            //                    foreach (DataGridViewRow dr in OverViewdgv.Rows)
            //                    {
            //                        if (dr.Selected == true)
            //                        {
            //                            int id = Convert.ToInt32(dr.Index);
            //                            select_list.Add(id);
            //                        }
            //                    }
            //                    if (select_list.Count != 0)
            //                    {
            //                        for (int i = 0; i < select_list.Count; i++)
            //                        {
            //                            int index = Convert.ToInt32(select_list[i]);
            //                            string remark = qcremark.GetRemark();
            //                            string name = OverViewdgv.Rows[index].Cells["СƱ��"].Value.ToString();
            //                            string prid = OverViewdgv.Rows[index].Cells["��Ŀ��"].Value.ToString();
            //                            OverViewdgv.Rows[index].Cells["����״̬��ʶ"].Value = "����ʧ��";
            //                            //string sql1 = "INSERT INTO SPFLOWLOG_TAB (SPOOLNAME,USERNAME,FLOWSTATUS, REMARK,PROJECTID) VALUES ( '" + name + "', '" + User.cur_user + "', (SELECT ID FROM SPFLOWSTATUS_TAB WHERE NAME = '����ʧ��'), '" + remark + "','" + prid + "')";
            //                            //User.UpdateCon(sql1, DataAccess.OIDSConnStr);
            //                            DBConnection.InsertFlowLogWithRemark(name, User.cur_user, (int)FlowState.����ʧ��, remark, prid);

            //                            //string sql2 = "UPDATE SPOOL_tAB SET FLOWSTATUS = (SELECT ID FROM SPFLOWSTATUS_TAB WHERE NAME = '����ʧ��'), FLOWSTATUSREMARK = '" + remark + "' WHERE FLAG = 'Y' AND SPOOLNAME = '" + name + "' AND PROJECTID = '"+prid+"'";
            //                            //User.UpdateCon(sql2, DataAccess.OIDSConnStr);
            //                            DBConnection.UpdateSpoolTabWithRemark((int)FlowState.����ʧ��, remark, name, prid, "Y");
            //                        }
            //                    }
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show(ex.Message, "������Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("��ǰû��ѡ���κ��У�", "WARNNING", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //        return;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("��ʱû�е���СƱ��");
            //    return;
            //}
            
        }

        #endregion


        /// <summary>
        /// ��Ӹ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAttachmenttoolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowcount = this.OverViewdgv.SelectedRows.Count;
            if (rowcount > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < OverViewdgv.Rows.Count; i++)
                {
                    if (OverViewdgv.Rows[i].Selected == true)
                    {
                        string spoolname = OverViewdgv.Rows[i].Cells["СƱ��"].Value.ToString();
                        sb.Append(spoolname + ",");
                    }
                }
                QCAttachment attachform = new QCAttachment();
                attachform.namestr = sb.ToString();
                attachform.ShowDialog();
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// ˫������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverViewdgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.Text == "�ӹ�СƱ����" || this.Text == "�ʼ�СƱ����" || this.Text =="��װСƱ����")
            {
                string spoolname = OverViewdgv.CurrentRow.Cells["СƱ��"].Value.ToString();
                QCAttachmentView qcav = new QCAttachmentView();
                qcav.namestr = spoolname;
                qcav.ShowDialog();
            }

            else
            { return; }
        }

        /// <summary>
        /// �Ҽ��˵�ѡ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpoolGeneralViewForm_Load(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = string.Format(" ");
            this.toolStripStatusLabel2.Text = string.Format(" ");
        }

        /// <summary>
        /// ���¯����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddHeattoolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowcount = this.OverViewdgv.SelectedRows.Count;
            if (rowcount > 0)
            {
                AddHeatNumber heatnumform = new AddHeatNumber();
                heatnumform.ShowDialog();
                object obj = heatnumform.heatnumber;
                if (obj == null)
                {
                    return;
                }
                string heatno = heatnumform.heatnumber.ToUpper().ToString();
                try
                {
                    if (heatnumform.DialogResult == DialogResult.OK)
                    {
                        for (int i = 0; i < this.OverViewdgv.Rows.Count; i++)
                        {
                            if (this.OverViewdgv.Rows[i].Selected == true)
                            {
                                string pid = OverViewdgv.Rows[i].Cells["��Ŀ��"].Value.ToString();
                                string name = OverViewdgv.Rows[i].Cells["СƱ��"].Value.ToString();
                                string norm = OverViewdgv.Rows[i].Cells["�����ͺ�"].Value.ToString();
                                this.OverViewdgv.Rows[i].Cells["¯����"].Value = heatnumform.heatnumber.ToUpper();
                                object num = OverViewdgv.Rows[i].Cells["���"].Value;
                                DBConnection.AddHeatNo(heatno,pid,name,norm,Convert.ToInt16(num));

                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "������Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// ���֤���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCertitoolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowcount = this.OverViewdgv.SelectedRows.Count;
            if (rowcount > 0)
            {
                AddCertificateNumber certificatenumform = new AddCertificateNumber();
                certificatenumform.ShowDialog();
                object obj = certificatenumform.certificatenumber;
                if (obj == null)
                {
                    return;
                }
                string certi = certificatenumform.certificatenumber.ToUpper().ToString();
                try
                {
                    if (certificatenumform.DialogResult == DialogResult.OK)
                    {
                        for (int i = 0; i < this.OverViewdgv.Rows.Count; i++)
                        {
                            if (this.OverViewdgv.Rows[i].Selected == true)
                            {
                                string pid = OverViewdgv.Rows[i].Cells["��Ŀ��"].Value.ToString();
                                string name = OverViewdgv.Rows[i].Cells["СƱ��"].Value.ToString();
                                string norm = OverViewdgv.Rows[i].Cells["�����ͺ�"].Value.ToString();
                                this.OverViewdgv.Rows[i].Cells["֤���"].Value = certificatenumform.certificatenumber.ToUpper();
                                object num = OverViewdgv.Rows[i].Cells["���"].Value;
                                DBConnection.AddCertificate(certi, pid, name, norm, Convert.ToInt16(num));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "������Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// ��ӹ�ʱ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddHourNormtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowcount = this.OverViewdgv.SelectedRows.Count;
            if (rowcount > 0)
            {
                AddHourNorm hournormform = new AddHourNorm();
                hournormform.ShowDialog();
                try
                {
                    if (hournormform.DialogResult == DialogResult.OK)
                    {
                        for (int i = 0; i < this.OverViewdgv.Rows.Count; i++)
                        {
                            if (this.OverViewdgv.Rows[i].Selected == true)
                            {
                                string pid = OverViewdgv.Rows[i].Cells["��Ŀ��"].Value.ToString();
                                string name = OverViewdgv.Rows[i].Cells["СƱ��"].Value.ToString();
                                this.OverViewdgv.Rows[i].Cells["��ʱ����(Сʱ)"].Value = Math.Round(Convert.ToDouble(hournormform.hour), 2);
                                string sql = "update HOURNORM_TAB SET HOURNORM = " + Math.Round(Convert.ToDouble(hournormform.hour), 2) + " WHERE PROJECTID = '" + pid + "' AND SPOOLNAME = '" + name + "' ";
                                User.UpdateCon(sql, DataAccess.OIDSConnStr);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "������Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// ���ʵ�Ĺ�ʱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddActualHourtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowcount = this.OverViewdgv.SelectedRows.Count;
            if (rowcount > 0)
            {
                AddActualHour actualhourform = new AddActualHour();
                actualhourform.ShowDialog();
                try
                {
                    if (actualhourform.DialogResult == DialogResult.OK)
                    {
                        for (int i = 0; i < this.OverViewdgv.Rows.Count; i++)
                        {
                            if (this.OverViewdgv.Rows[i].Selected == true)
                            {
                                string pid = OverViewdgv.Rows[i].Cells["��Ŀ��"].Value.ToString();
                                string name = OverViewdgv.Rows[i].Cells["СƱ��"].Value.ToString();
                                this.OverViewdgv.Rows[i].Cells["ʵ�Ĺ�ʱ(Сʱ)"].Value = Math.Round(Convert.ToDouble(actualhourform.time), 2);
                                string sql = "update HOURNORM_TAB SET ACTUALHOUR = " + Math.Round(Convert.ToDouble(actualhourform.time), 2) + " WHERE PROJECTID = '" + pid + "' AND SPOOLNAME = '" + name + "' ";
                                User.UpdateCon(sql, DataAccess.OIDSConnStr);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "������Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                return;
            }

        }

        /// <summary>
        /// ������Ӳ��϶����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMSSNOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowcount = this.OverViewdgv.SelectedRows.Count;
            if (rowcount > 0)
            {
                AddMSS_NO mssnoform = new AddMSS_NO();
                mssnoform.ShowDialog();
                string mss = mssnoform.mssno.ToUpper().ToString();
                try
                {
                    if (mssnoform.DialogResult == DialogResult.OK)
                    {
                        for (int i = 0; i < this.OverViewdgv.Rows.Count; i++)
                        {
                            if (this.OverViewdgv.Rows[i].Selected == true)
                            {
                                string pid = OverViewdgv.Rows[i].Cells["��Ŀ��"].Value.ToString();
                                string name = OverViewdgv.Rows[i].Cells["СƱ��"].Value.ToString();
                                string norm = OverViewdgv.Rows[i].Cells["�����ͺ�"].Value.ToString();
                                this.OverViewdgv.Rows[i].Cells["���϶����"].Value = mssnoform.mssno.ToUpper().ToString();
                                object num = OverViewdgv.Rows[i].Cells["���"].Value;
                                DBConnection.AddMSSNO(mss,pid,name,norm, Convert.ToInt16(num));

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "������Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// ����Excel����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User.ExportToTxt(OverViewdgv, progressBar1);
        }

        /// <summary>
        /// ֱ������¯���Ż���֤�����ӻ��޸����ݿ��ԭʼ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverViewdgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int index = 0;
            try
            {
                index = OverViewdgv.CurrentRow.Index;
            }
            catch(SystemException ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }

            string pid = OverViewdgv.Rows[index].Cells["��Ŀ��"].Value.ToString();
            string name = OverViewdgv.Rows[index].Cells["СƱ��"].Value.ToString();
            if (this.Text == "������Ϣ����")
            {
                object num = OverViewdgv.Rows[index].Cells["���"].Value;
                string norm = OverViewdgv.Rows[index].Cells["�����ͺ�"].Value.ToString();
                string erpstr = OverViewdgv.Rows[index].Cells["ERP�����"].Value.ToString();
                //if (UserSecurity.HavingPrivilege(User.cur_user, "SPOOLDESIGNUSERS"))
                //{
                //    string mssno = this.OverViewdgv.Rows[index].Cells["���϶����"].Value.ToString();
                //    if (mssno != string.Empty)
                //    {
                //        DBConnection.AddMSSNO(mssno, pid, name, norm, Convert.ToInt16(num));
                //    }
                //    else
                //    {
                //        return;
                //    }
                //}
                if (UserSecurity.HavingPrivilege(User.cur_user, "SPOOLMACHINEUSERS"))
                {

                    //string trayno = this.OverViewdgv.Rows[index].Cells["���̺�"].Value.ToString();
                    string heat = this.OverViewdgv.Rows[index].Cells["¯����"].Value.ToString();
                    string certificate = this.OverViewdgv.Rows[index].Cells["֤���"].Value.ToString();
                    string wpsnostr = this.OverViewdgv.Rows[index].Cells["���ӹ��պ�"].Value.ToString();
                    //if (trayno != string.Empty)
                    //{
                    //    try
                    //    {
                    //        OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����   
                    //        conn.Open();
                    //        OracleCommand cmd = conn.CreateCommand();
                    //        cmd.CommandText = "update SP_SPOOLMATERIAL_TAB set TRAYNAME = :te where PROJECTID = :pd and  SPOOLNAME = :se AND MATERIALNAME = :me  AND AMOUNT = :at  AND FLAG = :fg";
                    //        cmd.Parameters.Add("te", OracleType.VarChar).Value = trayno;
                    //        cmd.Parameters.Add("pd", OracleType.VarChar).Value = pid;
                    //        cmd.Parameters.Add("se", OracleType.VarChar).Value = name;
                    //        cmd.Parameters.Add("me", OracleType.VarChar).Value = norm;
                    //        cmd.Parameters.Add("at", OracleType.Number).Value = num;
                    //        cmd.Parameters.Add("fg", OracleType.VarChar).Value = "Y";

                    //        cmd.ExecuteNonQuery();
                    //        conn.Close();
                    //    }
                    //    catch (OracleException ex)
                    //    {
                    //        MessageBox.Show(ex.Message.ToString());
                    //    }
                    //}
                    //else
                    //{
                    //    return;
                    //}
                    string colname = this.OverViewdgv.CurrentCell.OwningColumn.Name.ToString();

                    //if (heat != string.Empty)
                    if (colname == "¯����")
                    {
                        DBConnection.AddHeatNo(heat,pid,name,norm,Convert.ToInt16(num));
                    }


                    //else if (certificate != string.Empty)
                    else if (colname == "֤���")
                    {
                        DBConnection.AddCertificate(certificate, pid, name, norm, Convert.ToInt16(num));
                    }

                    else if (colname == "���ӹ��պ�")
                    {
                        try
                        {
                            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����   
                            conn.Open();
                            OracleCommand cmd = conn.CreateCommand();
                            cmd.CommandText = "update SP_SPOOLMATERIAL_TAB set WPSNO = :te where PROJECTID = :pd and  SPOOLNAME = :se AND MATERIALNAME = :me  AND AMOUNT = :at  AND FLAG = :fg";
                            cmd.Parameters.Add("te", OracleType.VarChar).Value = wpsnostr;
                            cmd.Parameters.Add("pd", OracleType.VarChar).Value = pid;
                            cmd.Parameters.Add("se", OracleType.VarChar).Value = name;
                            cmd.Parameters.Add("me", OracleType.VarChar).Value = norm;
                            cmd.Parameters.Add("at", OracleType.Number).Value = num;
                            cmd.Parameters.Add("fg", OracleType.VarChar).Value = "Y";

                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        catch (OracleException ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }

                    else
                    {
                        return;
                    }


                }
            }

            else if (this.Text == "������Ϣ����")
            {
                object wobj = OverViewdgv.Rows[index].Cells["�������"].Value;
                object wcount = OverViewdgv.Rows[index].Cells["�������"].Value;
                if (UserSecurity.HavingPrivilege(User.cur_user, "SPOOLMACHINEUSERS"))
                {
                    object weldperson = OverViewdgv.Rows[index].Cells["������"].Value;

                    if (weldperson != System.DBNull.Value)
                    {
                        try
                        {
                            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����   
                            conn.Open();
                            OracleCommand cmd = conn.CreateCommand();
                            cmd.CommandText = "update SP_WELD_TAB set WELDBY = :wb where PROJECTID = :pd and  SPOOLNAME = :se and WELDNAME = :wd and WELDCOUNT = :wc and FLAG = :fg ";
                            cmd.Parameters.Add("wb", OracleType.VarChar).Value = weldperson;
                            cmd.Parameters.Add("pd", OracleType.VarChar).Value = pid;
                            cmd.Parameters.Add("se", OracleType.VarChar).Value = name;
                            cmd.Parameters.Add("wd", OracleType.VarChar).Value = wobj;
                            cmd.Parameters.Add("wc", OracleType.Number).Value = wcount;
                            cmd.Parameters.Add("fg", OracleType.VarChar).Value = "Y";

                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        catch (OracleException ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }
                }

                else
                {
                    return;
                }
            }

        }

        private void SpoolGeneralViewForm_Activated(object sender, EventArgs e)
        {
            MDIForm.tool_strip.Items[1].Enabled = true;
            MDIForm.tool_strip.Items[2].Enabled = true;
            if(this.Text =="��·������Ϣ" || this.Text == "��·������Ϣ")
                MDIForm.tool_strip.Items[0].Enabled = false;
            else
                MDIForm.tool_strip.Items[0].Enabled = true;

        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverViewdgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(this.OverViewdgv.Columns[e.ColumnIndex].HeaderText + " is \" " + this.OverViewdgv.Columns[e.ColumnIndex].ValueType + "\". Data error. ����.");
            return;
        }

        private void AddWeldBytoolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddWeldBy weldby = new AddWeldBy();
            weldby.ShowDialog();
            try
            {
                if (weldby.DialogResult == DialogResult.OK)
                {
                    for (int i = 0; i < this.OverViewdgv.Rows.Count; i++)
                    {
                        if (this.OverViewdgv.Rows[i].Selected == true)
                        {
                            string pid = OverViewdgv.Rows[i].Cells["��Ŀ��"].Value.ToString();
                            string name = OverViewdgv.Rows[i].Cells["СƱ��"].Value.ToString();
                            string wname = OverViewdgv.Rows[i].Cells["�������"].Value.ToString();
                            string wcount = OverViewdgv.Rows[i].Cells["�������"].Value.ToString();
                            this.OverViewdgv.Rows[i].Cells["������"].Value = weldby.nameby.ToUpper();
                            string sql = "update SP_WELD_TAB SET WELDBY = '" + weldby.nameby + "' WHERE PROJECTID = '" + pid + "' AND SPOOLNAME = '" + name + "' AND WELDNAME = '" + wname + "' AND WELDCOUNT = '"+wcount+"' AND FLAG = 'Y'";
                            User.UpdateCon(sql, DataAccess.OIDSConnStr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "������Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// �����ʽ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverViewdgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewColumn dgvc in  OverViewdgv.Columns)
            {
                if (dgvc.ValueType == typeof(DateTime))
                {
                    dgvc.DefaultCellStyle.Format = "yyyy-MM-dd";
                }
                if (dgvc.ValueType == typeof(decimal) )
                {
                    dgvc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    if (dgvc.Name != "�к�" && dgvc.Name != "���" && dgvc.Name != "�������")
                    {
                        dgvc.DefaultCellStyle.Format = "N2";
                    }
                }
            }
        }

        /// <summary>
        /// �༭�����ʾ��д��ʽ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverViewdgv_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.DesiredType == typeof(string))
            {
                e.Value = e.Value.ToString().ToUpper();
                e.ParsingApplied = true;
            }
        }

        private void OverViewdgv_SelectionChanged(object sender, EventArgs e)
        {
            int count = this.OverViewdgv.SelectedRows.Count;
            this.toolStripStatusLabel2.Text = string.Format("��ǰѡ��{0}��",count);
            if (this.OverViewdgv.SelectedRows.Count > 0)
            {
                GetContextMenu();
            }
            else
            {
                for (int i = 0; i < DisplaycontextMenuStrip.Items.Count; i++)
                {
                    DisplaycontextMenuStrip.Items[i].Visible = false;
                }
            }

        }

        /// <summary>
        /// ��ѯ��ѡ�еĵ�������СƱ�Ĺ�·������Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PipeMatialtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowcount = this.OverViewdgv.SelectedRows.Count;
            if (rowcount > 0)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < OverViewdgv.Rows.Count; i++)
                {
                    if (OverViewdgv.Rows[i].Selected == true)
                    {
                        string spoolname = OverViewdgv.Rows[i].Cells["СƱ��"].Value.ToString();
                        sb.Append("'" + spoolname + "'" + ",");
                    }
                }
                string str = sb.Remove(sb.Length - 1, 1).ToString();
                DataSet ds = new DataSet();
                string sql = "select t.projectid ��Ŀ��, t.spoolname СƱ��, t.erpcode ERP����, t.materialname �����ͺ�, t.logname ¼����, t.logdate ¼������ from SP_SPOOLMATERIAL_TAB t where (T.MATERIALNAME LIKE '%����%' OR T.MATERIALNAME like '%֧��%') and t.flag = 'Y' and t.spoolname in (select spoolname from sp_spool_tab where  spoolname in (" + str + ") and flag = 'Y')";
                foreach (Form form in MDIForm.pMainWin.MdiChildren)
                {
                    if (form.Text == "��·������Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).DataSource = null;
                        User.DataBaseConnect(sql, ds);
                        ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).DataSource = ds.Tables[0].DefaultView;
                        int i = ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).Rows.Count;
                        ((ToolStrip)(MDIForm.pMainWin.ActiveMdiChild.Controls["statusStrip1"])).Items["toolStripStatusLabel1"].Text = string.Format(" ��ǰ������������{0}��", i);
                        ds.Dispose();
                        return;
                    }
                }
                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "��·������Ϣ";
                sgvf.MdiParent = MDIForm.pMainWin;
                sgvf.Show();
                User.DataBaseConnect(sql, ds);
                ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).DataSource = ds.Tables[0].DefaultView;
                ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                int count = ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).Rows.Count;
                ((ToolStrip)(MDIForm.pMainWin.ActiveMdiChild.Controls["statusStrip1"])).Items["toolStripStatusLabel1"].Text = string.Format(" ��ǰ������������{0}��", count);
                ds.Dispose();
            }
            else
            {
                return;
            }
 
        }

        /// <summary>
        /// ��ѯ��ѡ�еĵ�������СƱ�Ĺ�·������Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PipeSparetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowcount = this.OverViewdgv.SelectedRows.Count;

            if (rowcount > 0)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < OverViewdgv.Rows.Count; i++)
                {
                    if (OverViewdgv.Rows[i].Selected == true)
                    {
                        string spoolname = OverViewdgv.Rows[i].Cells["СƱ��"].Value.ToString();
                        sb.Append("'" + spoolname + "'" + ",");
                    }
                }
                string str = sb.Remove(sb.Length - 1, 1).ToString();
                DataSet ds = new DataSet();
                string sql = "select t.projectid ��Ŀ��, t.spoolname СƱ��, t.erpcode ERP����, t.materialname �����ͺ�, t.logname ¼����, t.logdate ¼������ from SP_SPOOLMATERIAL_TAB t  where t.materialname not like '%��%' and t.flag = 'Y' and t.spoolname in (select spoolname from sp_spool_tab where  spoolname in (" + str + ") and flag = 'Y')";
                foreach (Form form in MDIForm.pMainWin.MdiChildren)
                {
                    if (form.Text == "��·������Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).DataSource = null;
                        User.DataBaseConnect(sql, ds);
                        ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).DataSource = ds.Tables[0].DefaultView;
                        int i = ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).Rows.Count;
                        ((ToolStrip)(MDIForm.pMainWin.ActiveMdiChild.Controls["statusStrip1"])).Items["toolStripStatusLabel1"].Text = string.Format(" ��ǰ������������{0}��", i);
                        ds.Dispose();
                        return;
                    }
                }
                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "��·������Ϣ";
                sgvf.MdiParent = MDIForm.pMainWin;
                sgvf.Show();
                User.DataBaseConnect(sql, ds);
                ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).DataSource = ds.Tables[0].DefaultView;
                ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                int count = ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).Rows.Count;
                ((ToolStrip)(MDIForm.pMainWin.ActiveMdiChild.Controls["statusStrip1"])).Items["toolStripStatusLabel1"].Text = string.Format(" ��ǰ������������{0}��", count);
                ds.Dispose();
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverViewdgv_Scroll(object sender, ScrollEventArgs e)
        {
            this.WeightToolTip.Active = false;
            return;
        }

        /// <summary>
        /// �Ҽ��˵�����
        /// </summary>
        public void GetContextMenu()
        {
            #region �����û��Ҽ�����
            if (this.Text == "������Ϣ����")
            {
                if (UserSecurity.HavingPrivilege(User.cur_user, "SPOOLMACHINEUSERS"))
                {
                    DisplaycontextMenuStrip.Items["DetailstoolStripMenuItem"].Visible = DisplaycontextMenuStrip.Items["toolStripSeparator6"].Visible = DisplaycontextMenuStrip.Items["AddHeattoolStripMenuItem"].Visible = DisplaycontextMenuStrip.Items["AddCertitoolStripMenuItem"].Visible = true;
                }

                else if (UserSecurity.HavingPrivilege(User.cur_user, "SPOOLDESIGNUSERS"))
                {
                    DisplaycontextMenuStrip.Items["DetailstoolStripMenuItem"].Visible = DisplaycontextMenuStrip.Items["toolStripSeparator11"].Visible = DisplaycontextMenuStrip.Items["AddMSSNOToolStripMenuItem"].Visible = true;
                }
                else
                {
                    DisplaycontextMenuStrip.Items["DetailstoolStripMenuItem"].Visible = true;
                }
            }
            else if (this.Text == "���Ӽ���Ϣ����" || this.Text == "�ӹ���Ϣ����" || this.Text == "��·������Ϣ" || this.Text =="��·������Ϣ")
            {
                DisplaycontextMenuStrip.Items["DetailstoolStripMenuItem"].Visible =true;
            }

            else if (this.Text == "������Ϣ����")
            {
                if (UserSecurity.HavingPrivilege(User.cur_user, "SPOOLMACHINEUSERS"))
                {
                    DisplaycontextMenuStrip.Items["DetailstoolStripMenuItem"].Visible = DisplaycontextMenuStrip.Items["toolStripSeparator6"].Visible = DisplaycontextMenuStrip.Items["AddWeldBytoolStripMenuItem"].Visible = true;
                }
                else
                {
                    DisplaycontextMenuStrip.Items["DetailstoolStripMenuItem"].Visible =true;
                }
            }

            else if (this.Text == "��·ͳ����Ϣ")
            {
                DisplaycontextMenuStrip.Items["DetailstoolStripMenuItem"].Visible = DisplaycontextMenuStrip.Items["toolStripSeparator6"].Visible = DisplaycontextMenuStrip.Items["ExportPipeToolStripMenuItem"].Visible = true;
            }
            else if (this.Text == "��·����ͳ����Ϣ")
            {
                DisplaycontextMenuStrip.Items["DetailstoolStripMenuItem"].Visible = DisplaycontextMenuStrip.Items["toolStripSeparator6"].Visible = DisplaycontextMenuStrip.Items["ExportMaterialtoolStripMenuItem"].Visible = true;
            }

            #endregion

            #region ����û��Ҽ�����
            else if (this.Text == "���СƱ����")
            {
                if (UserSecurity.HavingPrivilege(User.cur_user, "SPOOLDESIGNUSERS"))
                {
                    DisplaycontextMenuStrip.Items["DetailstoolStripMenuItem"].Visible = DisplaycontextMenuStrip.Items["toolStripSeparator6"].Visible = DisplaycontextMenuStrip.Items["PipeSparetoolStripMenuItem"].Visible = DisplaycontextMenuStrip.Items["PipeMatialtoolStripMenuItem"].Visible = DisplaycontextMenuStrip.Items["toolStripSeparator12"].Visible = DisplaycontextMenuStrip.Items["ɾ����¼ToolStripMenuItem"].Visible = true;
                    
                }
            }
            #endregion

            #region  �ܼӹ��û��Ҽ�����
            else if (this.Text == "�ӹ�СƱ����")
            {
                if (UserSecurity.HavingPrivilege(User.cur_user, "SPOOLMACHINEVIEW"))
                {
                    DisplaycontextMenuStrip.Items["DetailstoolStripMenuItem"].Visible  = true;
                }
                else
                {
                    DisplaycontextMenuStrip.Items["DetailstoolStripMenuItem"].Visible = DisplaycontextMenuStrip.Items["toolStripSeparator6"].Visible = DisplaycontextMenuStrip.Items["AddAttachmenttoolStripMenuItem"].Visible  = true;
                }
            }

            #endregion

            #region �ʼ��û��Ҽ�����
            else if (this.Text == "�ʼ�СƱ����")
            {
                DisplaycontextMenuStrip.Items["DetailstoolStripMenuItem"].Visible = DisplaycontextMenuStrip.Items["toolStripSeparator6"].Visible =  DisplaycontextMenuStrip.Items["AddAttachmenttoolStripMenuItem"].Visible = true;
            }
            else if (this.Text == "����ͨ��СƱ����")
            {
                DisplaycontextMenuStrip.Items["DetailstoolStripMenuItem"].Visible =  true;
            }
            #endregion

            #region ��װ�û��Ҽ�����
            else if (this.Text == "��װСƱ����")
            {
                DisplaycontextMenuStrip.Items["DetailstoolStripMenuItem"].Visible = DisplaycontextMenuStrip.Items["toolStripSeparator6"].Visible = DisplaycontextMenuStrip.Items["InstallToolStripMenuItem"].Visible = DisplaycontextMenuStrip.Items["CompleteInstallToolStripMenuItem"].Visible  = true;
            }
            #endregion

            #region �����û��Ҽ�����
            else if (this.Text == "����СƱ����")
            {
                DisplaycontextMenuStrip.Items["DetailstoolStripMenuItem"].Visible = DisplaycontextMenuStrip.Items["toolStripSeparator6"].Visible = DisplaycontextMenuStrip.Items["CompleteModulatetoolStripMenuItem"].Visible = DisplaycontextMenuStrip.Items["FeedBackToInstalltoolStripMenuItem"].Visible =  true;
            }
            #endregion
        }
        /// <summary>
        /// ɾ����ѡ��¼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ɾ����¼ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowcount = this.OverViewdgv.SelectedRows.Count;
            if (rowcount > 0)
            {
                DialogResult result;
                result = MessageBox.Show("ȷ��Ҫɾ����ѡ�е�СƱ�Լ������Ϣ��","��Ϣ��ʾ��",MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    foreach (DataGridViewRow drow in this.OverViewdgv.SelectedRows)
                    {
                        int rowindex = drow.Index;
                        string project = this.OverViewdgv.Rows[rowindex].Cells["��Ŀ��"].Value.ToString();
                        string spool = this.OverViewdgv.Rows[rowindex].Cells["СƱ��"].Value.ToString();
                        string drawno = this.OverViewdgv.Rows[rowindex].Cells["ͼ��"].Value.ToString();
                        string sqStr = "select count(*)from project_drawing_tab t where t.project_id = (select id from project_tab where name = '" + project + "') and t.drawing_no = '"+drawno+"' and t.responsible_user = '"+User.cur_user+"'";

                        object count = User.GetScalar(sqStr, DataAccess.OIDSConnStr);
                        if (count == null || Convert.ToInt16(count.ToString()) == 0 )
                        {
                            MessageBox.Show("��û��Ȩ��ɾ����ѡ�������У�");
                            return;
                        }
                        DelSpoolRecord.MarkDeletedSpoolRecord(project, spool, drawno, User.cur_user);
                    }

                    MessageBox.Show("-����ɾ����ϣ�-�����²�ѯ����֤��");

                }
            }
            else
            {
                return;
            }
        }
        
    }
}