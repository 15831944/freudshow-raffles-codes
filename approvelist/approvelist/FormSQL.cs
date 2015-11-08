using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.HSSF.Util;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;

namespace approvelist
{
    public partial class FormSQL : Form
    {
        public FormSQL()
        {
            InitializeComponent();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private DataSet GetDataSet(string Sql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmd = db.GetSqlStringCommand(Sql);
            return db.ExecuteDataSet(cmd);
        }

        private string GetSQLs(string ProjName, string AppObj, string DwgType, string StartTime, string EndTime, bool IsDiscipline)
        {
            string SQLHead = string.Empty;
            string ObjSubDate = string.Empty;
            string ObjAppDate = string.Empty;
            string ObjAppStatus = string.Empty;

            //�������
            switch (AppObj)
            {
                case "����":
                    ObjSubDate = "OWNER_SUBMISSION_DATE";
                    ObjAppDate = "OWNER_APPROVAL_DATE";
                    ObjAppStatus = "OWNER_APPROVAL_STATUS";
                    break;
                case "����":
                    ObjSubDate = "CLASS_SUBMISSION_DATE";
                    ObjAppDate = "CLASS_APPROVAL_DATE";
                    ObjAppStatus = "CLASS_APPROVAL_STATUS";
                    break;
                default:
                    MessageBox.Show(@"����������ѡ�� �������� �� �����족 ֮һ");
                    return string.Empty;
            }

            //ͼֽ����
            if (IsDiscipline)
            {
                SQLHead = @"SELECT DISTAB.NAME רҵ, COUNT(*) ����
                            FROM PROJECT_DRAWING_TAB PDT LEFT JOIN DISCIPLINE_TAB DISTAB ON PDT.DISCIPLINE_ID = DISTAB.ID
                            WHERE
                                PDT.project_id = (
                                                     SELECT PRJTAB.ID
                                                     FROM PROJECT_TAB PRJTAB
                                                     WHERE
                                                         PRJTAB.NAME='" + ProjName + @"'
                                                 )
                            AND
                                PDT.DELETE_FLAG='N'
                            AND
                                PDT.DOCTYPE_ID IN (
                                                    SELECT DTT.DOCUMENT_TYPE_ID
                                                    FROM DOCUMENT_TYPE_TAB DTT
                                                    WHERE
                                                        DTT.DESCRIPTION  NOT LIKE '%TP%'
                                                  )
                              ";
            }
            else
            {
                SQLHead = @"SELECT DISTAB.NAME רҵ, COUNT(*) ����
                            FROM PROJECT_DRAWING_TAB PDT LEFT JOIN DISCIPLINE_TAB DISTAB ON PDT.DISCIPLINE_ID = DISTAB.ID
                            WHERE
                                PDT.project_id = (
                                                     SELECT PRJTAB.ID
                                                     FROM PROJECT_TAB PRJTAB
                                                     WHERE
                                                         PRJTAB.NAME='" + ProjName + @"'
                                                 )
                            AND
                                PDT.DELETE_FLAG='N'
                            AND
                                PDT.DOCTYPE_ID IN (
                                                    SELECT DTT.DOCUMENT_TYPE_ID
                                                    FROM DOCUMENT_TYPE_TAB DTT
                                                    WHERE
                                                        DTT.DESCRIPTION  LIKE '%TP%'
                                                    OR
                                                        DTT.DESCRIPTION  LIKE '%�꾮%'
                                                  )
                          ";
            }

            //��������
            string SQLCondition = string.Empty;
            switch (DwgType)
            {
                case "ͼֽ����":
                    break;
                case "�ƻ�����":
                    SQLCondition = @"AND
                                     PDT.PLANNED_FINISH_DATE IS NOT NULL
                                     AND
                                     TO_DATE(PDT.PLANNED_FINISH_DATE,'YYYY-MM-DD') BETWEEN TO_DATE('" + StartTime + "','YYYY-MM-DD') AND TO_DATE('" + EndTime + "','YYYY-MM-DD')";
                    break;
                case "ʵ������":
                    SQLCondition = @"AND
                                     PDT.PLANNED_FINISH_DATE IS NOT NULL
                                     AND
                                     PDT." + ObjSubDate + @" IS NOT NULL
                                     AND
                                     TO_DATE(PDT." + ObjSubDate + @",'YYYY-MM-DD'
) BETWEEN TO_DATE('" + StartTime + @"','YYYY-MM-DD') AND TO_DATE('" + EndTime + @"','YYYY-MM-DD')
                                     ";
                    break;
                case "�ۼ���������":
                    SQLCondition = @"AND
                                     PDT.PLANNED_FINISH_DATE IS NOT NULL
                                     AND
                                     TO_DATE(PDT.PLANNED_FINISH_DATE,'YYYY-MM-DD') < SYSDATE
                                     AND
                                     PDT." + ObjSubDate + @" IS NOT NULL
                                     AND
                                     TO_DATE(PDT.PLANNED_FINISH_DATE,'YYYY-MM-DD') < TO_DATE(PDT." + ObjSubDate + @",'YYYY-MM-DD')
                                     ";
                    break;
                case "�ۼ�����δ����":
                    SQLCondition = @"AND
                                     TO_DATE(PDT.PLANNED_FINISH_DATE,'YYYY-MM-DD') < SYSDATE
                                     AND
                                     PDT." + ObjSubDate + " IS NULL";
                    break;
                case "���ܷ���":
                    SQLCondition = @"AND
                                     TO_DATE(PDT.PLANNED_FINISH_DATE,'YYYY-MM-DD') < SYSDATE
                                     AND
                                     PDT." + ObjSubDate + @" IS NOT NULL
                                     AND
                                     PDT." + ObjAppDate + @" IS NOT NULL
                                     AND
                                     PDT." + ObjAppStatus + @" = 'reject'
                                     AND
                                     TO_DATE(PDT.PLANNED_FINISH_DATE,'YYYY-MM-DD') BETWEEN TO_DATE('" + StartTime + @"','YYYY-MM-DD') AND TO_DATE('" + EndTime + @"','YYYY-MM-DD')
                                     ";
                    break;
                case "������Ͽ�":
                    SQLCondition = @"AND
                                     TO_DATE(PDT.PLANNED_FINISH_DATE,'YYYY-MM-DD') < SYSDATE
                                     AND
                                     PDT." + ObjSubDate + @" IS NOT NULL
                                     AND
                                     PDT." + ObjAppDate + @" IS NOT NULL
                                     AND
                                     PDT." + ObjAppStatus + @" = 'approvedC'
                                     AND
                                     TO_DATE(PDT.PLANNED_FINISH_DATE,'YYYY-MM-DD') BETWEEN TO_DATE('" + StartTime + @"','YYYY-MM-DD') AND TO_DATE('" + EndTime + @"','YYYY-MM-DD')
                                     ";
                    break;
                case "�ƻ���ȫ�Ͽ�":
                    SQLCondition = @"AND
                                     TO_DATE(PDT.PLANNED_FINISH_DATE,'YYYY-MM-DD') < SYSDATE
                                     AND
                                     PDT." + ObjSubDate + @" IS NOT NULL
                                     AND
                                     ( 
                                       ( TO_DATE(PDT." + ObjSubDate + @", 'YYYY-MM-DD') + 20 ) 
                                       BETWEEN 
                                         TO_DATE('" + StartTime + @"','YYYY-MM-DD') 
                                       AND 
                                         TO_DATE('" + EndTime + @"','YYYY-MM-DD') 
                                     )";
                    break;
                case "��ȫ�Ͽ�":
                    SQLCondition = @"AND
                                     TO_DATE(PDT.PLANNED_FINISH_DATE,'YYYY-MM-DD') < SYSDATE
                                     AND
                                     PDT." + ObjSubDate + @" IS NOT NULL
                                     AND
                                     PDT." + ObjAppDate + @" IS NOT NULL
                                     AND
                                     PDT." + ObjAppStatus + @" = 'approved'
                                     AND
                                     TO_DATE(PDT.PLANNED_FINISH_DATE,'YYYY-MM-DD') BETWEEN TO_DATE('" + StartTime + @"','YYYY-MM-DD') AND TO_DATE('" + EndTime + @"','YYYY-MM-DD')
                                     ";
                    break;
                default:
                    return string.Empty;
            }
            return SQLHead + SQLCondition + "\nGROUP BY DISTAB.NAME";
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            FileStream xls = new FileStream(@"Template.xls", FileMode.Open);
            HSSFWorkbook wb = new HSSFWorkbook(xls);

            #region sheet
            ISheet sheet1 = wb.GetSheet("Sheet1");
            string start = this.dateTimePicker1.Value.Date.ToShortDateString();
            string end = this.dateTimePicker2.Value.Date.ToShortDateString();
            wb.SetSheetName(0, start + "~" + end + @" ͼֽ������(��רҵ)");

            string Obj = this.comboBox1.Text;//�������
            if (Obj == string.Empty)
            {
                MessageBox.Show("��ѡ����������");
                return;
            }

            string proj = this.comboBox2.Text;//��Ŀ
            if (proj == string.Empty)
            {
                MessageBox.Show("��ѡ����Ŀ");
                return;
            }
            DataSet mDSTotalDwg = GetDataSet(GetSQLs(proj, Obj, "ͼֽ����", start, end, true));
            DataSet mDSPlanApp = GetDataSet(GetSQLs(proj, Obj, "�ƻ�����", start, end, true));
            DataSet mDSActualApp = GetDataSet(GetSQLs(proj, Obj, "ʵ������", start, end, true));
            DataSet mDSDelayApp = GetDataSet(GetSQLs(proj, Obj, "�ۼ���������", start, end, true));
            DataSet mDSDelayNotApp = GetDataSet(GetSQLs(proj, Obj, "�ۼ�����δ����", start, end, true));
            DataSet mDSRejectedDwg = GetDataSet(GetSQLs(proj, Obj, "���ܷ���", start, end, true));
            DataSet mDSRejectedCDwg = GetDataSet(GetSQLs(proj, Obj, "������Ͽ�", start, end, true));
            DataSet mDSPlannedAppDwg = GetDataSet(GetSQLs(proj, Obj, "�ƻ���ȫ�Ͽ�", start, end, true));
            DataSet mDSApprovedDwg = GetDataSet(GetSQLs(proj, Obj, "��ȫ�Ͽ�", start, end, true));

            IRow HeadRow = sheet1.GetRow(0);
            IRow TotalDwg = sheet1.GetRow(1);
            IRow PlanApp = sheet1.GetRow(2);
            IRow ActualApp = sheet1.GetRow(3);
            IRow DelayApp = sheet1.GetRow(4);
            IRow DelayNotApp = sheet1.GetRow(5);
            IRow RejectedDwg = sheet1.GetRow(6);
            IRow RejectedCDwg = sheet1.GetRow(7);
            IRow PlannedDwg = sheet1.GetRow(8);
            IRow ApprovedDwg = sheet1.GetRow(9);

            for (int i = 1; i < HeadRow.Cells.Count - 3; i++)
            {
                string discipline = HeadRow.Cells[i].ToString();
                //ͼֽ����
                for (int j = 0; j < mDSTotalDwg.Tables[0].Rows.Count; j++)
                {
                    if (mDSTotalDwg.Tables[0].Rows[j][0].ToString() == discipline)
                        TotalDwg.GetCell(i).SetCellValue(Convert.ToInt32(mDSTotalDwg.Tables[0].Rows[j][1].ToString()));
                }
                //�ƻ�����
                for (int j = 0; j < mDSPlanApp.Tables[0].Rows.Count; j++)
                {
                    if (mDSPlanApp.Tables[0].Rows[j][0].ToString() == discipline)
                        PlanApp.GetCell(i).SetCellValue(Convert.ToInt32(mDSPlanApp.Tables[0].Rows[j][1].ToString()));
                }
                //ʵ������
                for (int j = 0; j < mDSActualApp.Tables[0].Rows.Count; j++)
                {
                    if (mDSActualApp.Tables[0].Rows[j][0].ToString() == discipline)
                        ActualApp.GetCell(i).SetCellValue(Convert.ToInt32(mDSActualApp.Tables[0].Rows[j][1].ToString()));
                }
                //�ۼ���������
                for (int j = 0; j < mDSDelayApp.Tables[0].Rows.Count; j++)
                {
                    if (mDSDelayApp.Tables[0].Rows[j][0].ToString() == discipline)
                        DelayApp.GetCell(i).SetCellValue(Convert.ToInt32(mDSDelayApp.Tables[0].Rows[j][1].ToString()));
                }
                //�ۼ�����δ����
                for (int j = 0; j < mDSDelayNotApp.Tables[0].Rows.Count; j++)
                {
                    if (mDSDelayNotApp.Tables[0].Rows[j][0].ToString() == discipline)
                        DelayNotApp.GetCell(i).SetCellValue(Convert.ToInt32(mDSDelayNotApp.Tables[0].Rows[j][1].ToString()));
                }
                //���ܷ���
                for (int j = 0; j < mDSRejectedDwg.Tables[0].Rows.Count; j++)
                {
                    if (mDSRejectedDwg.Tables[0].Rows[j][0].ToString() == discipline)
                        RejectedDwg.GetCell(i).SetCellValue(Convert.ToInt32(mDSRejectedDwg.Tables[0].Rows[j][1].ToString()));
                }
                //������Ͽ�
                for (int j = 0; j < mDSRejectedCDwg.Tables[0].Rows.Count; j++)
                {
                    if (mDSRejectedCDwg.Tables[0].Rows[j][0].ToString() == discipline)
                        RejectedCDwg.GetCell(i).SetCellValue(Convert.ToInt32(mDSRejectedCDwg.Tables[0].Rows[j][1].ToString()));
                }
                //�ƻ���ȫ�Ͽ�
                for (int j = 0; j < mDSPlannedAppDwg.Tables[0].Rows.Count; j++)
                {
                    if (mDSPlannedAppDwg.Tables[0].Rows[j][0].ToString() == discipline)
                        PlannedDwg.GetCell(i).SetCellValue(Convert.ToInt32(mDSPlannedAppDwg.Tables[0].Rows[j][1].ToString()));
                }
                //��ȫ�Ͽ�
                for (int j = 0; j < mDSApprovedDwg.Tables[0].Rows.Count; j++)
                {
                    if (mDSApprovedDwg.Tables[0].Rows[j][0].ToString() == discipline)
                        ApprovedDwg.GetCell(i).SetCellValue(Convert.ToInt32(mDSApprovedDwg.Tables[0].Rows[j][1].ToString()));
                }
            }
            #endregion
            
            try
            {
                FileStream file = new FileStream(@"Report.xls", FileMode.Create);
                wb.Write(file);
                file.Close();
                MessageBox.Show("�������!");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Report mRept = new Report();
            /*
             * DataSet mDSTotalDwg = GetDataSet(GetSQLs(proj, Obj, "ͼֽ����", start, end, true));
            DataSet mDSPlanApp = GetDataSet(GetSQLs(proj, Obj, "�ƻ�����", start, end, true));
            DataSet mDSActualApp = GetDataSet(GetSQLs(proj, Obj, "ʵ������", start, end, true));
            DataSet mDSDelayApp = GetDataSet(GetSQLs(proj, Obj, "�ۼ���������", start, end, true));
            DataSet mDSDelayNotApp = GetDataSet(GetSQLs(proj, Obj, "�ۼ�����δ����", start, end, true));
            DataSet mDSRejectedDwg = GetDataSet(GetSQLs(proj, Obj, "���ܷ���", start, end, true));
            DataSet mDSRejectedCDwg = GetDataSet(GetSQLs(proj, Obj, "������Ͽ�", start, end, true));
            DataSet mDSPlannedAppDwg = GetDataSet(GetSQLs(proj, Obj, "�ƻ���ȫ�Ͽ�", start, end, true));
            DataSet mDSApprovedDwg = GetDataSet(GetSQLs(proj, Obj, "��ȫ�Ͽ�", start, end, true));
             */
            mRept.dataGridView1.DataSource = mDSTotalDwg.Tables[0];
            mRept.dataGridView2.DataSource = mDSPlanApp.Tables[0];
            mRept.dataGridView3.DataSource = mDSActualApp.Tables[0];
            mRept.dataGridView4.DataSource = mDSDelayApp.Tables[0];
            mRept.dataGridView5.DataSource = mDSDelayNotApp.Tables[0];
            mRept.dataGridView6.DataSource = mDSRejectedDwg.Tables[0];
            mRept.dataGridView7.DataSource = mDSRejectedCDwg.Tables[0];
            mRept.dataGridView8.DataSource = mDSPlannedAppDwg.Tables[0];
            mRept.dataGridView9.DataSource = mDSApprovedDwg.Tables[0];
            mRept.Show();
        }

        private void comboBox2_Click(object sender, EventArgs e)
        {
            string sql = @"SELECT PDT.ID, PDT.NAME FROM PLM.PROJECT_TAB PDT";
            DataSet ds = GetDataSet(sql);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                this.comboBox2.Items.Add(ds.Tables[0].Rows[i][1].ToString());
            }
        }

    }
}