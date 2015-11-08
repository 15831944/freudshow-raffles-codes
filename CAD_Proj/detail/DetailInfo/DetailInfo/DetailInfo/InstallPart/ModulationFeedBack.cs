using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.OracleClient;

namespace DetailInfo
{
    public partial class ModulationFeedBack : Form
    {
        public ModulationFeedBack()
        {
            InitializeComponent();
            this.ModulationFeedBackContextMenuStrip.Enabled = false;
        }

        private void ModulationFeedBack_Load(object sender, EventArgs e)
        {
            //DataSet ds = new DataSet();
            //string sql = "select PROJECTID ��Ŀ��, SPOOLNAME СƱ��, SYSTEMID  ϵͳ��, SYSTEMNAME  ϵͳ��, PIPEGRADE  ��·�ȼ�, SURFACETREATMENT  ���洦��, WORKINGPRESSURE  ����ѹ��, PRESSURETESTFIELD  ѹ�����Գ���, PIPECHECKFIELD  У�ܳ��� , SPOOLWEIGHT as  \"СƱ����(kg)\", PAINTCOLOR  ������ɫ, CABINTYPE  ��������, REVISION  СƱ�汾, SPOOLMODIFYTYPE  СƱ�޸�����,ELBOWTYPE  ��ͷ��ʽ, WELDTYPE  �㺸��, DRAWINGNO  ͼ��, BLOCKNO  �ֶκ�, MODIFYDRAWINGNO  �޸�֪ͨ����, REMARK  ��ע, FLAG  �汾��ʶ, LOGNAME  ¼����, LOGDATE  ¼������, LINENAME  �ߺ�, (SELECT NAME FROM spflowstatus_tab s WHERE s.ID=FLOWSTATUS)   ����״̬��ʶ, FLOWSTATUSREMARK ����״̬��ע from SP_SPOOL_TAB where flag = 'Y' and  spoolname in (SELECT s.SPOOLNAME as СƱ�� from spflowlog_tab S where s.spoolname in (select DISTINCT t.spoolname  from spflowlog_tab t where t.username = '" + User.cur_user + "' and t.flowstatus = " + (int)FlowState.������ + ") and s.flowstatus = " + (int)FlowState.����ʧ�� + ")";
            //User.DataBaseConnect(sql, ds);
            //dgv_modulationfeedback.DataSource = ds.Tables[0].DefaultView;
            //if (dgv_modulationfeedback.Rows.Count != 0)
            //{
            //    this.ModulationFeedBackContextMenuStrip.Enabled = true;
            //}
            //ds.Dispose();
        }

        private void ���ܴ���ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ArrayList selectlist = new ArrayList();
            //ArrayList select_array = new ArrayList();
            //if (dgv_modulationfeedback.RowCount == 0)
            //{
            //    MessageBox.Show("��ʱû��Ҫ��������ݣ�");
            //    return;
            //}
            //if (dgv_modulationfeedback.RowCount != 0)
            //{
            //    for (int i = 0; i < dgv_modulationfeedback.RowCount; i++)
            //    {
            //        if (dgv_modulationfeedback.Rows[i].Selected == true)
            //        {
            //            selectlist.Add(i);
            //        }
            //    }
            //    if (selectlist.Count != 0)
            //    {
            //        DialogResult result;
            //        result = MessageBox.Show("ȷ����ѡ�����ݽ��д���", "����СƱ����", MessageBoxButtons.OKCancel);
            //        if (result == DialogResult.OK)
            //        {
            //            foreach (DataGridViewRow dr in dgv_modulationfeedback.Rows)
            //            {
            //                if (dr.Selected == true)
            //                {
            //                    int id = Convert.ToInt32(dr.Index);
            //                    select_array.Add(id);
            //                }
            //            }
            //            if (select_array.Count != 0)
            //            {
            //                for (int j = 0; j < select_array.Count; j++)
            //                {
            //                    int index = Convert.ToInt32(select_array[j]);
            //                    string spname = dgv_modulationfeedback.Rows[index].Cells["СƱ��"].Value.ToString();
            //                    string prid = dgv_modulationfeedback.Rows[index].Cells["��Ŀ��"].Value.ToString();

            //                    //string sql3 = "update SP_SPOOL_TAB set FLOWSTATUS =(select ID from SPFLOWSTATUS_TAB where NAME = '������') where SPOOLNAME = '" + spname + "' and PROJECTID = '"+prid+"'  and FLAG = 'Y'";
            //                    DBConnection.UpDateState((int)FlowState.������, spname, prid, "Y");

            //                    //string sql4 = "insert into SPFLOWLOG_TAB (SPOOLNAME,USERNAME,FLOWSTATUS,PROJECTID) VALUES( '" + spname + "', '" + User.cur_user + "',  (select ID from SPFLOWSTATUS_TAB where NAME = '������') ,'" + prid + "')";
            //                    DBConnection.InsertFlowLog(spname, User.cur_user, (int)FlowState.������, prid);
            //                }
            //                ModulationFeedBack_Load(sender, e);
            //            }
            //        }
            //        else
            //        {
            //            return;
            //        }
            //    }
            //}
        }

        private void ����Excel��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User.ExportToTxt(dgv_modulationfeedback, progressBar1);
        }

        private void dgv_modulationfeedback_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            SpoolCellFormat.FormatCell(dgv_modulationfeedback);
        }

    }
}