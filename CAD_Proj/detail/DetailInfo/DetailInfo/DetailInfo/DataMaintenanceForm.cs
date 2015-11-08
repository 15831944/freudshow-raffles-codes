using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.OracleClient;
using System.IO;
using System.Data.OleDb;
using Microsoft.Office.Interop.Excel;


namespace DetailInfo
{
    public partial class DataMaintenanceForm : Form
    {
        private OracleDataAdapter oda = new OracleDataAdapter();
        private BindingSource bs = new BindingSource();  
        public DataMaintenanceForm()
        {
            InitializeComponent();
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
                oda = new OracleDataAdapter(sqlstr, con);
                OracleCommandBuilder builder = new OracleCommandBuilder(oda);
                DataSet ds = new DataSet();
                oda.Fill(ds);
                bs.DataSource = ds.Tables[0];
                this.EditDgv.DataSource = bs;
                this.bindingNavigator1.BindingSource = bs;
                con.Close();
                ds.Dispose();
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
        } 

        private void DataMaintenanceForm_Load(object sender, EventArgs e)
        {
            if (User.cur_user == "fulong.wu")
            {
                this.bindingNavigatorDeleteItem.Visible = false;
            }
            
            string title = this.Text;
            switch (title)
            {
                case "��ͷ�б�":
                    string sqlSP_BEND = "select ID ���, PROJECT_ID  ��Ŀ��, M_NO ���ϴ���, DN ͨ��,ONE_DXW  \"1��������ͷ(mm)\",ONEHALF_DXW  \"1���붨����ͷ(mm)\", ONE_JCDXW  \"3��45�ȶ�����ͷ(mm)\", ONEHALF_JCDXW  \"3��90�ȶ�����ͷ(mm)\"  FROM  SP_BEND ORDER BY ID ASC";
                    DataBind(sqlSP_BEND);
                    this.EditDgv.Columns["���"].Visible = false;

                    break;

                case "�����б�":
                    string sqlSP_CABIN = "select ID ���, PROJECT_ID ��Ŀ��, EN_CABIN ����Ӣ������, CH_CABIN ������������ FROM SP_CABIN  ORDER BY ID ASC";
                    DataBind(sqlSP_CABIN);
                    this.EditDgv.Columns["���"].Visible = false;
                    break;

                case "���Ӽ��б�":
                    string sqlSP_CONNECTOR = "select ID ���,PROJECT_ID ��Ŀ��,NAME ��������, PARTCODE �������,  OUTDIAMETER \"�⾶(mm)\",  NUTWEIGHT \"��ĸ��(kg)\", BOLTWEIGHT \"��˨��(kg)\" FROM SP_CONNECTOR  ORDER BY ID ASC";
                    DataBind(sqlSP_CONNECTOR);
                    this.EditDgv.Columns["���"].Visible = false;

                    break;

                case "��ͷ���϶����б�":
                    string sqlSP_ELBOWMATERIAL = "select ID ���,PROJECT_ID ��Ŀ��, PIPEMATERIAL �ܲ���, EMATERIAL ��ͷ����, FLAGE  \"��ͷ��ʶ\" FROM SP_ELBOWMATERIAL  ORDER BY ID ASC";
                    DataBind(sqlSP_ELBOWMATERIAL);
                    this.EditDgv.Columns["���"].Visible = false;
                    break;

                case "��ģ�б�":
                    string sqlSP_PSTAD = "select ID ���,PROJECT_ID ��Ŀ��, OUTSIDEDIAMETER \"�ܲĵ��⾶(mm)\", WAMO \"��ģ(mm)\", QIANJA \"ǰ�г���(mm)\", HOUJA \"��г���(mm)\",  SECMACHINE \"��ܻ���ģ��С(mm)\" FROM SP_PSTAD  ORDER BY ID ASC";
                    DataBind(sqlSP_PSTAD);
                    this.EditDgv.Columns["���"].Visible = false;

                    break;

                case "�н���ͷ�б�":
                    string sqlSocketElow = "select ID ���,PROJECT_ID ��Ŀ��, DN \"ͨ��(mm)\", ELBOWONE \"45�ȳв���ͷ\", ELBOWTWO \"90�ȳв���ͷ\" FROM SP_SOCKETELBOW  ORDER BY ID ASC";
                    DataBind(sqlSocketElow);
                    this.EditDgv.Columns["���"].Visible = false;
                    break;

                case "���洦���б�":
                    string sqlSP_SURFACE = "select ID ���,PROJECT_ID ��Ŀ��,CODE ����, DESCRIPTION ���� FROM SP_SURFACE";
                    DataBind(sqlSP_SURFACE);
                    this.EditDgv.Columns["���"].Visible = false;
                    break;

                case "ϵͳ�б�":
                    string sqlSystem = "select ID ���,PROJECT_ID ��Ŀ��, SYSID ϵͳ����, SYSCODE ϵͳ����, SYSNAME ϵͳ��, GASKET ���ӵ�Ƭ, BENDMACHINE ��ܻ��ͺ� FROM SP_SYSTEM  ORDER BY ID ASC";
                    DataBind(sqlSystem);
                    this.EditDgv.Columns["���"].Visible = false;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("ȷ��Ҫ������", "��������", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                this.Validate();
                bs.EndEdit();
                try
                {
                    if (((System.Data.DataTable)bs.DataSource).GetChanges() != null)
                    {
                        oda.Update(((System.Data.DataTable)bs.DataSource).GetChanges());
                        DataMaintenanceForm_Load(sender,e);
                        MessageBox.Show("�������ݳɹ�");
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        /// <summary>
        /// �������뵽���ݿ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //bool ret = false;
            DataSet ds;
            DataSet sheetds = new DataSet();
            int dsLength;
            string formtext = this.Text;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Excel�ļ�";
            ofd.FileName = "";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);//Ϊ�˻�ȡ�ض���ϵͳ�ļ��У�����ʹ��System.Environment��ľ�̬����GetFolderPath()���÷�������һ��Environment.SpecialFolderö�٣����п��Զ���Ҫ����·�����ĸ�ϵͳĿ¼
            ofd.Filter = "Excel�ļ�(*.xls)|*.xls";
            ofd.ValidateNames = true;     //�ļ���Ч����֤ValidateNames����֤�û������Ƿ���һ����Ч��Windows�ļ���
            ofd.CheckFileExists = true;  //��֤·����Ч��
            ofd.CheckPathExists = true; //��֤�ļ���Ч��
            
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string sql = "SELECT EXCELTABLE FROM DATATABLE_TAB WHERE DESCRIPTION = '" + formtext + "'";
                User.DataBaseConnect(sql,sheetds);
                string excelsheet = sheetds.Tables[0].Rows[0][0].ToString();
                ds = ImportExcel(ofd.FileName,excelsheet);//���Excel   
                if (ds == null)
                {
                    return;
                }

            }
            else
            {
                return;
            }

            int odr = 0;

            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = conn.CreateCommand();
            cmd.Transaction = trans;
            try
            {
                switch (formtext)
                {
                    case "��ͷ�б�":
                        cmd.CommandText = "INSERT INTO SP_BEND (M_NO, DN, ONE_DXW, ONEHALF_DXW, ONE_JCDXW, ONEHALF_JCDXW, PROJECT_ID) VALUES (:xh,:hpzl,:hphm,:bz,:larq,:fdjh,:clpp) ";
                        cmd.Parameters.Add("xh", OracleType.Number);
                        cmd.Parameters.Add("hpzl", OracleType.VarChar);
                        cmd.Parameters.Add("hphm", OracleType.Number);
                        cmd.Parameters.Add("bz", OracleType.Number);
                        cmd.Parameters.Add("larq", OracleType.Number);
                        cmd.Parameters.Add("fdjh", OracleType.Number);
                        cmd.Parameters.Add("clpp", OracleType.VarChar);
                        
                        dsLength = ds.Tables[0].Rows.Count;
                        for (int i = 1; i < dsLength; i++)
                        {
                            cmd.Parameters["xh"].Value = ds.Tables[0].Rows[i][0];
                            cmd.Parameters["hpzl"].Value = ds.Tables[0].Rows[i][1];
                            cmd.Parameters["hphm"].Value = ds.Tables[0].Rows[i][2];
                            cmd.Parameters["bz"].Value = ds.Tables[0].Rows[i][3];
                            cmd.Parameters["larq"].Value = ds.Tables[0].Rows[i][4];
                            cmd.Parameters["fdjh"].Value = ds.Tables[0].Rows[i][5];
                            cmd.Parameters["clpp"].Value = ds.Tables[0].Rows[i][6];

                            odr = cmd.ExecuteNonQuery();//�ύ  
                        }
                        break;

                    case "�����б�":
                        cmd.CommandText = "INSERT INTO SP_CABIN (PROJECT_ID, EN_CABIN, CH_CABIN) VALUES(:xh,:hpzl,:hphm) ";
                        cmd.Parameters.Add("xh", OracleType.VarChar);
                        cmd.Parameters.Add("hpzl", OracleType.VarChar);
                        cmd.Parameters.Add("hphm", OracleType.VarChar);

                        dsLength = ds.Tables[0].Rows.Count;
                        for (int i = 1; i < dsLength; i++)
                        {
                            cmd.Parameters["xh"].Value = ds.Tables[0].Rows[i][0];
                            cmd.Parameters["hpzl"].Value = ds.Tables[0].Rows[i][1];
                            cmd.Parameters["hphm"].Value = ds.Tables[0].Rows[i][2];

                            odr = cmd.ExecuteNonQuery();//�ύ   
                        }

                        break;

                    case "���Ӽ��б�":
                        cmd.CommandText = "INSERT INTO SP_CONNECTOR (NAME, PARTCODE, OUTDIAMETER, NUTWEIGHT, BOLTWEIGHT,PROJECT_ID) VALUES(:xh,:hpzl,:cjh,:jdcsyr,:cllx,:csys) ";
                        cmd.Parameters.Add("xh", OracleType.VarChar);
                        cmd.Parameters.Add("hpzl", OracleType.VarChar);
                        cmd.Parameters.Add("cjh", OracleType.Number);
                        cmd.Parameters.Add("jdcsyr", OracleType.Number);
                        cmd.Parameters.Add("cllx", OracleType.Number);
                        cmd.Parameters.Add("csys", OracleType.VarChar);

                        dsLength = ds.Tables[0].Rows.Count;//���Excel�����ݳ���   
                        for (int i = 1; i < dsLength; i++)
                        {
                            cmd.Parameters["xh"].Value = ds.Tables[0].Rows[i][0];
                            cmd.Parameters["hpzl"].Value = ds.Tables[0].Rows[i][1];
                            cmd.Parameters["cjh"].Value = ds.Tables[0].Rows[i][2];
                            cmd.Parameters["jdcsyr"].Value = ds.Tables[0].Rows[i][3];
                            cmd.Parameters["cllx"].Value = ds.Tables[0].Rows[i][4];
                            cmd.Parameters["csys"].Value = ds.Tables[0].Rows[i][5];

                            odr = cmd.ExecuteNonQuery();
                        }

                        break;

                    case "��ͷ���϶����б�":
                        cmd.CommandText = "INSERT INTO SP_ELBOWMATERIAL (PROJECT_ID, PIPEMATERIAL, EMATERIAL, FLAGE) VALUES(:xh,:hpzl,:hphm,:bz) ";
                        cmd.Parameters.Add("xh", OracleType.VarChar);
                        cmd.Parameters.Add("hpzl", OracleType.VarChar);
                        cmd.Parameters.Add("hphm", OracleType.VarChar);
                        cmd.Parameters.Add("bz", OracleType.VarChar);

                        dsLength = ds.Tables[0].Rows.Count;
                        for (int i = 1; i < dsLength; i++)
                        {
                            cmd.Parameters["xh"].Value = ds.Tables[0].Rows[i][0];
                            cmd.Parameters["hpzl"].Value = ds.Tables[0].Rows[i][1];
                            cmd.Parameters["hphm"].Value = ds.Tables[0].Rows[i][2];
                            cmd.Parameters["bz"].Value = ds.Tables[0].Rows[i][3];

                            odr = cmd.ExecuteNonQuery();//�ύ   
                        }
                        break;

                    case "��ģ�б�":
                        cmd.CommandText = "INSERT INTO SP_PSTAD (OUTSIDEDIAMETER, WAMO, QIANJA, HOUJA, PROJECT_ID, SECMACHINE) VALUES(:xh,:hpzl,:hphm,:bz,:larq,:fdjh) ";
                        cmd.Parameters.Add("xh", OracleType.Number);
                        cmd.Parameters.Add("hpzl", OracleType.VarChar);
                        cmd.Parameters.Add("hphm", OracleType.Number);
                        cmd.Parameters.Add("bz", OracleType.Number);
                        cmd.Parameters.Add("larq", OracleType.VarChar);
                        cmd.Parameters.Add("fdjh", OracleType.VarChar);
                        
                        dsLength = ds.Tables[0].Rows.Count;//���Excel�����ݳ���   
                        for (int i = 1; i < dsLength; i++)
                        {
                            cmd.Parameters["xh"].Value = ds.Tables[0].Rows[i][0];
                            cmd.Parameters["hpzl"].Value = ds.Tables[0].Rows[i][1];
                            cmd.Parameters["hphm"].Value = ds.Tables[0].Rows[i][2];
                            cmd.Parameters["bz"].Value = ds.Tables[0].Rows[i][3];
                            cmd.Parameters["larq"].Value = ds.Tables[0].Rows[i][4];
                            cmd.Parameters["fdjh"].Value = ds.Tables[0].Rows[i][5];

                            odr = cmd.ExecuteNonQuery();//�ύ   
                        }
                        break;

                    case "�н���ͷ�б�":
                        cmd.CommandText = "INSERT INTO SP_SOCKETELBOW (PROJECT_ID, DN, ELBOWONE, ELBOWTWO) VALUES(:xh,:hpzl,:hphm,:bz) ";
                        cmd.Parameters.Add("xh", OracleType.VarChar);
                        cmd.Parameters.Add("hpzl", OracleType.VarChar);
                        cmd.Parameters.Add("hphm", OracleType.Number);
                        cmd.Parameters.Add("bz", OracleType.Number);

                        dsLength = ds.Tables[0].Rows.Count;
                        for (int i = 1; i < dsLength; i++)
                        {
                            cmd.Parameters["xh"].Value = ds.Tables[0].Rows[i][0];
                            cmd.Parameters["hpzl"].Value = ds.Tables[0].Rows[i][1];
                            cmd.Parameters["hphm"].Value = ds.Tables[0].Rows[i][2];
                            cmd.Parameters["bz"].Value = ds.Tables[0].Rows[i][3];

                            odr = cmd.ExecuteNonQuery();//�ύ   
                        }
                        break;

                    case "���洦���б�":
                        cmd.CommandText = "INSERT INTO SP_SURFACE (CODE, DESCRIPTION, PROJECT_ID) VALUES(:xh,:hpzl,:hphm) ";
                        cmd.Parameters.Add("xh", OracleType.VarChar);
                        cmd.Parameters.Add("hpzl", OracleType.VarChar);
                        cmd.Parameters.Add("hphm", OracleType.VarChar);

                        dsLength = ds.Tables[0].Rows.Count;
                        for (int i = 1; i < dsLength; i++)
                        {
                            cmd.Parameters["xh"].Value = ds.Tables[0].Rows[i][0];
                            cmd.Parameters["hpzl"].Value = ds.Tables[0].Rows[i][1];
                            cmd.Parameters["hphm"].Value = ds.Tables[0].Rows[i][2];

                            odr = cmd.ExecuteNonQuery();//�ύ   
                        }
                        break;

                    case "ϵͳ�б�":
                        cmd.CommandText = "INSERT INTO SP_SYSTEM (PROJECT_ID, SYSID, SYSCODE, SYSNAME, GASKET, BENDMACHINE) VALUES(:xh,:hpzl,:hphm,:bz,:larq,:fdjh) ";
                        cmd.Parameters.Add("xh", OracleType.VarChar); 
                        cmd.Parameters.Add("hpzl", OracleType.VarChar);
                        cmd.Parameters.Add("hphm", OracleType.VarChar);
                        cmd.Parameters.Add("bz", OracleType.VarChar);
                        cmd.Parameters.Add("larq", OracleType.VarChar);
                        cmd.Parameters.Add("fdjh", OracleType.VarChar);

                        dsLength = ds.Tables[0].Rows.Count;
                        for (int i = 1; i < dsLength; i++)
                        {
                            cmd.Parameters["xh"].Value = ds.Tables[0].Rows[i][0];
                            cmd.Parameters["hpzl"].Value = ds.Tables[0].Rows[i][1];
                            cmd.Parameters["hphm"].Value = ds.Tables[0].Rows[i][2];
                            cmd.Parameters["bz"].Value = ds.Tables[0].Rows[i][3];
                            cmd.Parameters["larq"].Value = ds.Tables[0].Rows[i][4];
                            cmd.Parameters["fdjh"].Value = ds.Tables[0].Rows[i][5];

                            odr = cmd.ExecuteNonQuery();//�ύ   
                        }
                        break;

                    case "������б�":
                        cmd.CommandText = "INSERT INTO PROJECTAPPROVE (PROJECTID, ASSESOR, INDEX_ID) VALUES(:xh,:hpzl,:hphm) ";
                        cmd.Parameters.Add("xh", OracleType.VarChar);
                        cmd.Parameters.Add("hpzl", OracleType.VarChar);
                        cmd.Parameters.Add("hphm", OracleType.Number);

                        dsLength = ds.Tables[0].Rows.Count;
                        for (int i = 1; i < dsLength; i++)
                        {
                            cmd.Parameters["xh"].Value = ds.Tables[0].Rows[i][0];
                            cmd.Parameters["hpzl"].Value = ds.Tables[0].Rows[i][1];
                            cmd.Parameters["hphm"].Value = ds.Tables[0].Rows[i][2];

                            odr = cmd.ExecuteNonQuery();//�ύ   
                        }
                        break;

                    case "������������ʱ�䶨���б�":
                        cmd.CommandText = "INSERT INTO BAITINGNORM_METALPIPE_TAB (CODE, NORM, PIPEMACHINING,EQUIPMENTOPERATION,UNPRODUCTIVETIME) VALUES(:xh,:hpzl,:hphm:bz,:larq) ";
                        cmd.Parameters.Add("xh", OracleType.VarChar);
                        cmd.Parameters.Add("hpzl", OracleType.Number);
                        cmd.Parameters.Add("hphm", OracleType.Number);
                        cmd.Parameters.Add("bz", OracleType.Number);
                        cmd.Parameters.Add("larq", OracleType.Number);

                        dsLength = ds.Tables[0].Rows.Count;
                        for (int i = 1; i < dsLength; i++)
                        {
                            cmd.Parameters["xh"].Value = ds.Tables[0].Rows[i][0];
                            cmd.Parameters["hpzl"].Value = ds.Tables[0].Rows[i][1];
                            cmd.Parameters["hphm"].Value = ds.Tables[0].Rows[i][2];
                            cmd.Parameters["bz"].Value = ds.Tables[0].Rows[i][3];
                            cmd.Parameters["larq"].Value = ds.Tables[0].Rows[i][4];

                            odr = cmd.ExecuteNonQuery();//�ύ   
                        }
                        break;

                    case "�¿ڼӹ�ʱ�䶨���б�":
                        cmd.CommandText = "INSERT INTO BEVEL_HOUR_NORM_TAB (CODE, NORM, EQUIPMENTOPERATION,UNPRODUCTIVETIME) VALUES(:xh,:hpzl,:hphm:bz) ";
                        cmd.Parameters.Add("xh", OracleType.VarChar);
                        cmd.Parameters.Add("hpzl", OracleType.Number);
                        cmd.Parameters.Add("hphm", OracleType.Number);
                        cmd.Parameters.Add("bz", OracleType.Number);

                        dsLength = ds.Tables[0].Rows.Count;
                        for (int i = 1; i < dsLength; i++)
                        {
                            cmd.Parameters["xh"].Value = ds.Tables[0].Rows[i][0];
                            cmd.Parameters["hpzl"].Value = ds.Tables[0].Rows[i][1];
                            cmd.Parameters["hphm"].Value = ds.Tables[0].Rows[i][2];
                            cmd.Parameters["bz"].Value = ds.Tables[0].Rows[i][3];

                            odr = cmd.ExecuteNonQuery();//�ύ   
                        }
                        break;

                    case "���ʱ�䶨���б�":
                        cmd.CommandText = "INSERT INTO ELBOW_HOUR_NORM_TAB (CODE, NORM, PIPEMACHINING,EQUIPMENTOPERATION,UNPRODUCTIVETIME) VALUES(:xh,:hpzl,:hphm:bz,:larq) ";
                        cmd.Parameters.Add("xh", OracleType.VarChar);
                        cmd.Parameters.Add("hpzl", OracleType.Number);
                        cmd.Parameters.Add("hphm", OracleType.Number);
                        cmd.Parameters.Add("bz", OracleType.Number);
                        cmd.Parameters.Add("larq", OracleType.Number);

                        dsLength = ds.Tables[0].Rows.Count;
                        for (int i = 1; i < dsLength; i++)
                        {
                            cmd.Parameters["xh"].Value = ds.Tables[0].Rows[i][0];
                            cmd.Parameters["hpzl"].Value = ds.Tables[0].Rows[i][1];
                            cmd.Parameters["hphm"].Value = ds.Tables[0].Rows[i][2];
                            cmd.Parameters["bz"].Value = ds.Tables[0].Rows[i][3];
                            cmd.Parameters["larq"].Value = ds.Tables[0].Rows[i][4];

                            odr = cmd.ExecuteNonQuery();//�ύ   
                        }
                        break;

                    case "У��ʱ�䶨���б�":
                        cmd.CommandText = "INSERT INTO PIPECHECKING_HOUR_NORM_TAB (CODE, NORM, PIPEMACHINING,EQUIPMENTOPERATION,WELD_WORKING,UNPRODUCTIVETIME) VALUES(:xh,:hpzl,:hphm:bz,:larq,:fdjh) ";
                        cmd.Parameters.Add("xh", OracleType.VarChar);
                        cmd.Parameters.Add("hpzl", OracleType.Number);
                        cmd.Parameters.Add("hphm", OracleType.Number);
                        cmd.Parameters.Add("bz", OracleType.Number);
                        cmd.Parameters.Add("larq", OracleType.Number);
                        cmd.Parameters.Add("fdjh", OracleType.Number);

                        dsLength = ds.Tables[0].Rows.Count;
                        for (int i = 1; i < dsLength; i++)
                        {
                            cmd.Parameters["xh"].Value = ds.Tables[0].Rows[i][0];
                            cmd.Parameters["hpzl"].Value = ds.Tables[0].Rows[i][1];
                            cmd.Parameters["hphm"].Value = ds.Tables[0].Rows[i][2];
                            cmd.Parameters["bz"].Value = ds.Tables[0].Rows[i][3];
                            cmd.Parameters["larq"].Value = ds.Tables[0].Rows[i][4];
                            cmd.Parameters["fdjh"].Value = ds.Tables[0].Rows[i][5];

                            odr = cmd.ExecuteNonQuery();//�ύ   
                        }
                        break;

                    default:
                        break;
                }

                trans.Commit();
                MessageBox.Show("����ɹ�");
            }
            catch (OracleException ee)
            {
                trans.Rollback();
                MessageBox.Show(ee.Message.ToString(), "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

        }

        /// <summary>
        /// ���Excel�����ݼ�
        /// </summary>
        /// <param name="file"></param>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        private  DataSet ImportExcel(string file,string worksheet)
        {
            FileInfo fileInfo = new FileInfo(file);
            if (!fileInfo.Exists)
                return null;

            string strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + file + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1'";
            OleDbConnection objConn = new OleDbConnection(strConn);
            DataSet dsExcel = new DataSet();
            try
            {
                objConn.Open();
                string strSql = "select * from  "+worksheet+"";
                OleDbDataAdapter odbcExcelDataAdapter = new OleDbDataAdapter(strSql, objConn);
                odbcExcelDataAdapter.Fill(dsExcel);
                return dsExcel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "����ʧ�ܣ���鿴Ҫ�����excel�ĵ���ʽ");
                return null;
                //throw ex;
            }
        }  

        /// <summary>
        /// ˢ��ҳ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            DataMaintenanceForm_Load(sender,e);
        }

        /// <summary>
        /// ����ģ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "����Excel (*.xls)|*.xls";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "�����ļ�����·��";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog.FileName.Trim().Length > 0)
                {
                    byte[] template = Properties.Resources.���ݵ���ģ��;
                    FileStream stream = new FileStream(saveFileDialog.FileName,FileMode.OpenOrCreate);
                    stream.Write(template,0,template.Length);
                    stream.Close();
                    stream.Dispose();
                    MessageBox.Show("����ģ��ɹ���");
                }
            }
        }

        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataMaintenanceForm_Activated(object sender, EventArgs e)
        {
            MDIForm.tool_strip.Items[0].Enabled = false;
            MDIForm.tool_strip.Items[1].Enabled = true;
            MDIForm.tool_strip.Items[2].Enabled = true;
        }

        private void EditDgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(this.EditDgv.Columns[e.ColumnIndex].HeaderText + " is \" " + this.EditDgv.Columns[e.ColumnIndex].ValueType + "\". Data error. ����.");
            return;
        }

        private void EditDgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewColumn dgvc in this.EditDgv.Columns)
            {
                if (dgvc.ValueType == typeof(decimal))
                {
                    dgvc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    if (dgvc.Name != "���" && dgvc.Name != "���ϴ���" && dgvc.Name != "��˼���" && dgvc.Name != "���(mm)")
                    {
                        dgvc.DefaultCellStyle.Format = "N2";
                    }
                }
            }
        }

        private void DataMaintenanceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDIForm.tool_strip.Items[0].Enabled = false;
            MDIForm.tool_strip.Items[1].Enabled = false;
        }
    }
}