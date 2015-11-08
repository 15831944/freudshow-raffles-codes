using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using System.Windows.Forms;
using System.Data;

namespace DetailInfo
{
    class DBConnection
    {
        /// <summary>
        /// ����СƱ����״̬���ޱ�ע��
        /// </summary>
        /// <param name="status"></param>
        /// <param name="spoolname"></param>
        /// <param name="projectid"></param>
        /// <param name="flag"></param>
        public static void UpDateState(int status,string spoolname,string projectid,string flag )
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            string queryString = "SP_UpDateSpoolStatus";
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("state", OracleType.Number).Value = status;
            cmd.Parameters.Add("spname", OracleType.VarChar).Value = spoolname;
            cmd.Parameters.Add("pid", OracleType.VarChar).Value = projectid;
            cmd.Parameters.Add("flag", OracleType.VarChar).Value = flag;
            cmd.Transaction = trans;
            try
            {
                
                cmd.ExecuteNonQuery();
                trans.Commit(); 
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
        /// ����������־
        /// </summary>
        /// <param name="spoolname"></param>
        /// <param name="username"></param>
        /// <param name="status"></param>
        /// <param name="projectid"></param>
        public static void InsertFlowLog(string spoolname, string username, int status,string projectid)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            string queryString = "SP_InsertLogRecord";
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString,conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("spname", OracleType.VarChar).Value = spoolname;
            cmd.Parameters.Add("person", OracleType.VarChar).Value = username;
            cmd.Parameters.Add("state", OracleType.Number).Value = status;
            cmd.Parameters.Add("pid", OracleType.VarChar).Value = projectid;

            cmd.Transaction = trans;
            try
            {
                //conn.Open();
                cmd.ExecuteNonQuery();
                trans.Commit(); 
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
        /// �ܼӹ���������
        /// </summary>
        /// <param name="carrier"></param>
        /// <param name="dt"></param>
        /// <param name="status"></param>
        /// <param name="spoolname"></param>
        /// <param name="projectid"></param>
        /// <param name="flag"></param>
        public static void InsertTaskCarrier(string carrier, DateTime dt, int status, string spoolname, string projectid, string flag)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����   
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = conn.CreateCommand();
            cmd.Transaction = trans;
            try
            {
                cmd.CommandText = "UPDATE SP_SPOOL_TAB SET ALLOCATION = :cr, ALLOCATIONTIME = :ate, FLOWSTATUS = :fs  WHERE SPOOLNAME = :se  AND PROJECTID = :pd  AND FLAG =:fg";
                cmd.Parameters.Add("cr", OracleType.VarChar).Value = carrier;
                cmd.Parameters.Add("ate", OracleType.DateTime).Value = dt;
                cmd.Parameters.Add("fs", OracleType.Number).Value = status;
                cmd.Parameters.Add("se", OracleType.VarChar).Value = spoolname;
                cmd.Parameters.Add("pd", OracleType.VarChar).Value = projectid;
                cmd.Parameters.Add("fg", OracleType.VarChar).Value = flag;
                cmd.ExecuteNonQuery();
                trans.Commit(); 
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
        /// ���뵽������־�����б�ע�ģ�
        /// </summary>
        /// <param name="spoolname"></param>
        /// <param name="username"></param>
        /// <param name="status"></param>
        /// <param name="mark"></param>
        /// <param name="projectid"></param>
        public static void InsertFlowLogWithRemark(string spoolname, string username, int status, string mark, string projectid)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����   
            conn.Open();
            string queryString = "SP_InsertLogWithRemark";
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("spname", OracleType.VarChar).Value = spoolname;
            cmd.Parameters.Add("username", OracleType.VarChar).Value = username;
            cmd.Parameters.Add("state", OracleType.Number).Value = status;
            cmd.Parameters.Add("mark", OracleType.VarChar).Value = mark;
            cmd.Parameters.Add("pid", OracleType.VarChar).Value = projectid;

            cmd.Transaction = trans;
            try
            {
                cmd.ExecuteNonQuery();
                trans.Commit(); 
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
        ///��������� 
        /// </summary>
        /// <param name="spoolname"></param>
        /// <param name="count"></param>
        /// <param name="assessby"></param>
        /// <param name="id"></param>
        /// <param name="fg"></param>
        public static void InsertSpoolApproveTab(string spoolname, int count, string assessby, int id, char fg)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����   
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = conn.CreateCommand();
            cmd.Transaction = trans;
            try
            {
                cmd.CommandText = "INSERT INTO SPOOL_APPROVE_TAB (SPOOLNAME, INDEX_ID, ASSESOR, STATE, APPROVENEEDFLAG) VALUES (:se, :pram, :ass, :sat, :fg)";
                cmd.Parameters.Add("se", OracleType.VarChar).Value = spoolname;
                cmd.Parameters.Add("pram", OracleType.Number).Value = count;
                cmd.Parameters.Add("ass", OracleType.VarChar).Value = assessby;
                cmd.Parameters.Add("sat", OracleType.Number).Value = id;
                cmd.Parameters.Add("fg", OracleType.VarChar).Value = fg;
                cmd.ExecuteNonQuery();
                trans.Commit(); 
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
        /// ����СƱ���״̬�����б�ע��
        /// </summary>
        /// <param name="status"></param>
        /// <param name="mark"></param>
        /// <param name="spoolname"></param>
        /// <param name="projectid"></param>
        /// <param name="flag"></param>
        public static void UpdateSpoolTabWithRemark(int status, string mark, string spoolname, string projectid, string flag )
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����   
            conn.Open();
            string queryString = "SP_UpdateSpoolTabWithRemark";
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("state", OracleType.Number).Value = status;
            cmd.Parameters.Add("mark", OracleType.VarChar).Value = mark;
            cmd.Parameters.Add("spname", OracleType.VarChar).Value = spoolname;
            cmd.Parameters.Add("pid", OracleType.VarChar).Value = projectid;
            cmd.Parameters.Add("flag", OracleType.VarChar).Value = flag;
            cmd.Transaction = trans;
            try
            {
                cmd.ExecuteNonQuery();
                trans.Commit(); 
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
        /// ��ͼ����˲���������б�
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="drawno"></param>
        /// <param name="count"></param>
        /// <param name="assessby"></param>
        /// <param name="id"></param>
        /// <param name="fg"></param>
        public static void InsertPipeApproveTab(string pid, string drawno, int count, string assessby, int id, char fg)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����   
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = conn.CreateCommand();
            cmd.Transaction = trans;
            try
            {
                cmd.CommandText = "INSERT INTO PIPEAPPROVE_TAB (PROJECTID,DRAWINGNO, INDEX_ID, ASSESOR, STATE, APPROVENEEDFLAG) VALUES (:pd, :do,:pram, :ass, :sat, :fg)";
                cmd.Parameters.Add("pd", OracleType.VarChar).Value = pid;
                cmd.Parameters.Add("do", OracleType.VarChar).Value = drawno;
                cmd.Parameters.Add("pram", OracleType.Number).Value = count;
                cmd.Parameters.Add("ass", OracleType.VarChar).Value = assessby;
                cmd.Parameters.Add("sat", OracleType.Number).Value = id;
                cmd.Parameters.Add("fg", OracleType.VarChar).Value = fg;
                cmd.ExecuteNonQuery();
                trans.Commit(); 
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
        /// �������ͨ����СƱ״̬��������Ŀ�ź�ͼ�ţ�
        /// </summary>
        /// <param name="status"></param>
        /// <param name="projectid"></param>
        /// <param name="drawno"></param>
        public static void UpdateSpoolTabWithDrawingNo(int status, string projectid, string drawno)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����   
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = conn.CreateCommand();
            cmd.Transaction = trans;
            try
            {
                cmd.CommandText = "UPDATE SP_SPOOL_TAB SET FLOWSTATUS = :fs WHERE FLAG = 'Y' AND PROJECTID = :pid AND DRAWINGNO = :do";
                cmd.Parameters.Add("fs", OracleType.Number).Value = status;
                cmd.Parameters.Add("pid", OracleType.VarChar).Value = projectid;
                cmd.Parameters.Add("do", OracleType.VarChar).Value = drawno;
                cmd.ExecuteNonQuery();
                trans.Commit();
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
        /// ���ͬ�������־
        /// </summary>
        /// <param name="username"></param>
        /// <param name="status"></param>
        /// <param name="projectid"></param>
        /// <param name="drawno"></param>
        public static void InsertApproveIntoFlowLog(string username, int status, string projectid, string drawno)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����   
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = conn.CreateCommand();
            cmd.Transaction = trans;
            try
            {
                cmd.CommandText = "INSERT INTO SPFLOWLOG_TAB (USERNAME,FLOWSTATUS,PROJECTID, DRAWINGNO) VALUES ( :ur, :fs, :pid, :do)";
                cmd.Parameters.Add("ur", OracleType.VarChar).Value = username;
                cmd.Parameters.Add("fs", OracleType.Number).Value = status;
                cmd.Parameters.Add("pid", OracleType.VarChar).Value = projectid;
                cmd.Parameters.Add("do", OracleType.VarChar).Value = drawno;
                cmd.ExecuteNonQuery();
                trans.Commit();
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
        /// ��˹���δͨ������������־
        /// </summary>
        public static void InsertApproveFeedBackFlowLog(string username, int status,string mark, string projectid, string drawno)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����   
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = conn.CreateCommand();
            cmd.Transaction = trans;
            try
            {
                cmd.CommandText = "INSERT INTO SPFLOWLOG_TAB (USERNAME,FLOWSTATUS,REMARK, PROJECTID,DRAWINGNO) VALUES ( :ur, :fs, :rk, :pd, :do)";
                cmd.Parameters.Add("ur", OracleType.VarChar).Value = username;
                cmd.Parameters.Add("fs", OracleType.Number).Value = status;
                cmd.Parameters.Add("rk", OracleType.VarChar).Value = mark;
                cmd.Parameters.Add("pd", OracleType.VarChar).Value = projectid;
                cmd.Parameters.Add("do", OracleType.VarChar).Value = drawno;
                cmd.ExecuteNonQuery();
                trans.Commit();
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
        /// ���δͨ���ı�СƱ״̬
        /// </summary>
        public static void UpdateSpoolTabWithDrawingNoRemark(int status, string mark, string projectid, string drawno)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����   
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = conn.CreateCommand();
            cmd.Transaction = trans;
            try
            {
                cmd.CommandText = "UPDATE SP_SPOOL_TAB SET FLOWSTATUS = :fs, FLOWSTATUSREMARK = :fk  WHERE PROJECTID = :pd AND DRAWINGNO = :do AND FLAG = 'Y'";
                cmd.Parameters.Add("fs", OracleType.Number).Value = status;
                cmd.Parameters.Add("fk", OracleType.VarChar).Value = mark;
                cmd.Parameters.Add("pd", OracleType.VarChar).Value = projectid;
                cmd.Parameters.Add("do", OracleType.VarChar).Value = drawno;
                cmd.ExecuteNonQuery();
                trans.Commit();
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
        /// ����ͼֽ������СƱ״̬
        /// </summary>
        public static void UpdateDrawingStatus(int status, string drawingno)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����   
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = conn.CreateCommand();
            cmd.Transaction = trans;
            try
            {
                cmd.CommandText = "UPDATE SP_SPOOL_TAB SET FLOWSTATUS = :fs  WHERE  DRAWINGNO = :do AND FLAG = 'Y'";
                cmd.Parameters.Add("fs", OracleType.Number).Value = status;
                cmd.Parameters.Add("do", OracleType.VarChar).Value = drawingno;
                cmd.ExecuteNonQuery();
                trans.Commit();
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
        /// �����޸�֪ͨ��������СƱ״̬
        /// </summary>
        /// <param name="status"></param>
        /// <param name="drawingno"></param>
        public static void UpdateModifyDrawingStatus(int status, string drawingno)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����   
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = conn.CreateCommand();
            cmd.Transaction = trans;
            try
            {
                cmd.CommandText = "UPDATE SP_SPOOL_TAB SET FLOWSTATUS = :fs  WHERE  MODIFYDRAWINGNO = :do AND FLAG = 'Y' ";
                cmd.Parameters.Add("fs", OracleType.Number).Value = status;
                cmd.Parameters.Add("do", OracleType.VarChar).Value = drawingno;
                cmd.ExecuteNonQuery();
                trans.Commit();
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
        /// ���²��϶����
        /// </summary>
        /// <param name="materialno"></param>
        /// <param name="projectid"></param>
        /// <param name="spoolname"></param>
        /// <param name="materialname"></param>
        /// <param name="seqnum"></param>
        public static void AddMSSNO(string materialno, string projectid, string spoolname, string materialname, int seqnum)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            string queryString = "SP_AddMSSNO";
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("mssno", OracleType.VarChar).Value = materialno;
            cmd.Parameters.Add("pid", OracleType.VarChar).Value = projectid;
            cmd.Parameters.Add("spname", OracleType.VarChar).Value = spoolname;
            cmd.Parameters.Add("material", OracleType.VarChar).Value = materialname;
            cmd.Parameters.Add("seqnumber",OracleType.Number).Value = seqnum;

            cmd.Transaction = trans;
            try
            {
                //conn.Open();
                cmd.ExecuteNonQuery();
                trans.Commit();
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
        /// ������̺�
        /// </summary>
        /// <param name="heatno"></param>
        /// <param name="projectid"></param>
        /// <param name="spoolname"></param>
        /// <param name="materialname"></param>
        /// <param name="seqnum"></param>
        public static void AddHeatNo(string heatno, string projectid, string spoolname, string materialname, int seqnum)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            string queryString = "SP_AddHeatNo";
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("heatno", OracleType.VarChar).Value = heatno;
            cmd.Parameters.Add("pid", OracleType.VarChar).Value = projectid;
            cmd.Parameters.Add("spname", OracleType.VarChar).Value = spoolname;
            cmd.Parameters.Add("material", OracleType.VarChar).Value = materialname;
            cmd.Parameters.Add("seqnumber", OracleType.Number).Value = seqnum;

            cmd.Transaction = trans;
            try
            {
                //conn.Open();
                cmd.ExecuteNonQuery();
                trans.Commit();
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
        /// 
        /// </summary>
        /// <param name="certificate"></param>
        /// <param name="projectid"></param>
        /// <param name="spoolname"></param>
        /// <param name="materialname"></param>
        /// <param name="seqnum"></param>
        public static void AddCertificate(string certificate, string projectid, string spoolname, string materialname, object seqnum)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            string queryString = "SP_AddCertificateNo";
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("certificateno", OracleType.VarChar).Value = certificate;
            cmd.Parameters.Add("pid", OracleType.VarChar).Value = projectid;
            cmd.Parameters.Add("spname", OracleType.VarChar).Value = spoolname;
            cmd.Parameters.Add("material", OracleType.VarChar).Value = materialname;
            cmd.Parameters.Add("seqnumber", OracleType.Number).Value = seqnum;

            cmd.Transaction = trans;
            try
            {
                //conn.Open();
                cmd.ExecuteNonQuery();
                trans.Commit();
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
        /// ��ȡСƱ״̬������
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetSpoolStatus(string status)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            string queryString = "SP_GetSpoolStatus";
            //OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("ID_out", OracleType.Number);
            cmd.Parameters["ID_out"].Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add("status_in",OracleType.VarChar).Value = status;

            //cmd.Transaction = trans;
            try
            {
                //conn.Open();
                cmd.ExecuteNonQuery();
                //trans.Commit();
            }
            catch (OracleException ee)
            {
                //trans.Rollback();
                MessageBox.Show(ee.Message.ToString(), "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
            return cmd.Parameters["ID_out"].Value.ToString();
        }

        /// <summary>
        /// �鿴СƱ��ϸ��Ϣ
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="tabtext"></param>
        /// <param name="spoolname"></param>
        /// <param name="dgv"></param>
        public static void GetSpoolMaterialDetail(string queryString, string tabtext, string spoolname,int version, int itemcount,DataGridView dgv)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            OracleParameter pram = new OracleParameter("V_CS", OracleType.Cursor);
            pram.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(pram);

            cmd.Parameters.Add("spool_in", OracleType.VarChar).Value = spoolname;
            cmd.Parameters["spool_in"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.Add("tabstr_in", OracleType.VarChar).Value = tabtext;
            cmd.Parameters["tabstr_in"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.Add("version_in", OracleType.Number).Value = version;
            cmd.Parameters["version_in"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.Add("itemcount_in", OracleType.Number).Value = itemcount;
            cmd.Parameters["itemcount_in"].Direction = System.Data.ParameterDirection.Input;

            OracleDataAdapter adapter = new OracleDataAdapter(cmd);
            DataSet myDS = new DataSet();
            adapter.Fill(myDS,"test");
            dgv.DataSource = myDS.Tables["test"];
            myDS.Dispose();
        }

        /// <summary>
        /// �鿴ͼֽ���޸�֪ͨ����ϸ��Ϣ
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="tabtext"></param>
        /// <param name="queryString"></param>
        /// <param name="drawing"></param>
        /// <param name="dgv"></param>
        public static void GetDrawingDetail(int flag, string tabtext, string queryString, string projectid,string drawing, DataGridView dgv)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            OracleParameter pram = new OracleParameter("V_CS", OracleType.Cursor);
            pram.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(pram);

            cmd.Parameters.Add("projectid_in", OracleType.VarChar).Value = projectid;
            cmd.Parameters["projectid_in"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.Add("drawing_in", OracleType.VarChar).Value = drawing;
            cmd.Parameters["drawing_in"].Direction = System.Data.ParameterDirection.Input;

            cmd.Parameters.Add("flag_in", OracleType.VarChar).Value = flag;
            cmd.Parameters["flag_in"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.Add("tabstr_in", OracleType.VarChar).Value = tabtext;
            cmd.Parameters["tabstr_in"].Direction = System.Data.ParameterDirection.Input;

            OracleDataAdapter adapter = new OracleDataAdapter(cmd);
            DataSet myDS = new DataSet();
            adapter.Fill(myDS, "test");
            dgv.DataSource = myDS.Tables["test"];
            myDS.Dispose();
        }

        /// <summary>
        /// ɾ��СƱ����
        /// </summary>
        /// <param name="username"></param>
        /// <param name="filename"></param>
        public static void DeletSpoolAttachment(string username, string filename)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            string queryString = "SP_DeleteSpoolAttachment";
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("username_in", OracleType.VarChar).Value = username;
            cmd.Parameters["username_in"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.Add("filename_in", OracleType.VarChar).Value = filename;
            cmd.Parameters["filename_in"].Direction = System.Data.ParameterDirection.Input;

            cmd.Transaction = trans;

            try
            {
                //conn.Open();
                cmd.ExecuteNonQuery();
                trans.Commit();
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
        /// ��ȡָ��СƱ����������
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="usernanme"></param>
        /// <returns></returns>
        public static int GetSpoolAttechmentCount(string filename, string usernanme)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            string queryString = "SP_GetSpoolAttechmentCount";
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("count_out", OracleType.Number);
            cmd.Parameters["count_out"].Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add("filename_in", OracleType.VarChar).Value = filename;
            cmd.Parameters["filename_in"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.Add("username_in", OracleType.VarChar).Value = usernanme;
            cmd.Parameters["username_in"].Direction = System.Data.ParameterDirection.Input;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException ee)
            {
                MessageBox.Show(ee.Message.ToString(), "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
            return Convert.ToInt16( cmd.Parameters["ID_out"].Value );
        }

        public static void GetProjectPlan(string queryString, string frmtext, OracleCommandBuilder builder, DataSet myDS, DataGridView dgv, OracleDataAdapter adapter, BindingSource bdsource, BindingNavigator bdn)
        {
            //OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            //conn.Open();
            //OracleCommand cmd = new OracleCommand(queryString, conn);
            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

            //OracleParameter pram = new OracleParameter("V_CS", OracleType.Cursor);
            //pram.Direction = System.Data.ParameterDirection.Output;
            //cmd.Parameters.Add(pram);

            //cmd.Parameters.Add("formtext_in", OracleType.VarChar).Value = frmtext;
            //cmd.Parameters["formtext_in"].Direction = System.Data.ParameterDirection.Input;

            ////OracleDataAdapter adapter = new OracleDataAdapter(cmd);
            //myDS = new DataSet();
            //adapter = new OracleDataAdapter(cmd);
            //builder = new OracleCommandBuilder(adapter); 
            ////adapter.FillSchema(
            //adapter.Fill(myDS);
            ////BindingSource bds = new BindingSource();
            //bdsource.DataSource = myDS.Tables[0];
            //dgv.DataSource = bdsource;
            //bdn.BindingSource = bdsource;
            ////dgv.DataSource = myDS.Tables["plantable"];
            //myDS.Dispose();
        }

        public static void GetProjectID(ToolStripComboBox comb)
        {
            comb.Items.Add("-����������-");
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            string queryString = "SP_GetProjectID";
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            OracleParameter pram = new OracleParameter("V_CS", OracleType.Cursor);
            pram.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(pram);

            try
            {
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    comb.ComboBox.Items.Add(dr[0]);
                }
                dr.Close();
                conn.Close();

            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }

        }

        /// <summary>
        /// �������ڵ�
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="nodetxt"></param>
        /// <param name="parent"></param>
        /// <param name="nodeindex"></param>
        public static void SaveTreeView(string projectid, string nodetxt,  int parent, int nodeindex)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            string queryString = "SP_SaveTreeView";
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("projectid_in", OracleType.VarChar).Value = projectid;
            cmd.Parameters.Add("nodetxt_in", OracleType.VarChar).Value = nodetxt;
            //cmd.Parameters.Add("parenttxt_in", OracleType.VarChar).Value = parenttxt;
            cmd.Parameters.Add("parentid_in", OracleType.Number).Value = parent;
            cmd.Parameters.Add("nodeindex_in", OracleType.Number).Value = nodeindex;

            cmd.Transaction = trans;
            try
            {
                cmd.ExecuteNonQuery();
                trans.Commit();
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

        public static void InsertRootNode(string projectid, string nodetxt, int parentid,int nodeid)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            string queryString = "SP_InsertRootNode";
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("projectid_in", OracleType.VarChar).Value = projectid;
            cmd.Parameters.Add("nodetxt_in", OracleType.VarChar).Value = nodetxt;
            //cmd.Parameters.Add("parenttxt_in", OracleType.VarChar).Value = parenttxt;
            cmd.Parameters.Add("parent_in", OracleType.Number).Value = parentid;
            cmd.Parameters.Add("nodeindex_in", OracleType.Number).Value = nodeid;

            cmd.Transaction = trans;
            try
            {
                cmd.ExecuteNonQuery();
                trans.Commit();
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

        public static int GetParentID(string projectid,string nodename)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            string queryString = "SP_GetParentID";
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("ID_out", OracleType.Number);
            cmd.Parameters["ID_out"].Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add("projectid_in", OracleType.VarChar).Value = projectid;
            cmd.Parameters.Add("nodename_in", OracleType.VarChar).Value = nodename;

            cmd.Transaction = trans;
            try
            {
                //conn.Open();
                cmd.ExecuteNonQuery();
                trans.Commit();
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
            return Convert.ToInt16( cmd.Parameters["ID_out"].Value.ToString() );
        }

        /// <summary>
        /// ���ƻ����������Ŀ
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="nodetxt"></param>
        public static void InsertNodesIntoPlanTab(string projectid, string nodetxt)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            string queryString = "SP_InsertNodesIntoPlanTab";
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("projectid_in", OracleType.VarChar).Value = projectid;
            cmd.Parameters.Add("nodetxt_in", OracleType.VarChar).Value = nodetxt;


            cmd.Transaction = trans;
            try
            {
                cmd.ExecuteNonQuery();
                trans.Commit();
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
        /// �༭�ڵ�
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="nodetxt"></param>
        /// <param name="nodeind"></param>
        /// <param name="nodetxt_new"></param>
        /// <param name="parid"></param>
        public static void EditTreeNode(string projectid, string nodetxt,  int nodeind, string nodetxt_new, int parid)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            string queryString = "SP_EditTreeNode";
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("projectid_in", OracleType.VarChar).Value = projectid;
            cmd.Parameters.Add("nodetxt_in", OracleType.VarChar).Value = nodetxt;
            //cmd.Parameters.Add("parentnodetxt_in", OracleType.VarChar).Value = parentnodetxt;
            cmd.Parameters.Add("nodeind_in", OracleType.Number).Value = nodeind;
            cmd.Parameters.Add("nodetxt_new_in", OracleType.VarChar).Value = nodetxt_new;
            cmd.Parameters.Add("parid_in", OracleType.Number).Value = parid;

            cmd.Transaction = trans;
            try
            {
                cmd.ExecuteNonQuery();
                trans.Commit();
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
        /// ���泵�乤�˻�����Ϣ�����ݿ�
        /// </summary>
        /// <param name="Cname"></param>
        /// <param name="Ename"></param>
        /// <param name="IDno"></param>
        /// <param name="TMname"></param>
        /// <param name="TLNO"></param>
        /// <param name="Eaddress"></param>
        public static void WorkShopNewWorker(string Cname, string Ename, string IDno, string TMname, string TLNO, string Eaddress)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            string queryString = "SP_WorkShopNewWorker";
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("Cname_in", OracleType.VarChar).Value = Cname;
            cmd.Parameters.Add("Ename_in", OracleType.VarChar).Value = Ename;
            cmd.Parameters.Add("IDno_in", OracleType.VarChar).Value = IDno;
            cmd.Parameters.Add("TMname_in",OracleType.VarChar).Value = TMname;
            cmd.Parameters.Add("TLNO_in", OracleType.VarChar).Value = TLNO;
            cmd.Parameters.Add("Eaddress_in",OracleType.VarChar).Value = Eaddress;

            cmd.Transaction = trans;
            try
            {
                cmd.ExecuteNonQuery();
                trans.Commit();
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
        /// ��ECDMS��֤ͼֽ״̬
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="drawing"></param>
        /// <returns></returns>
        public static string GetDrawingStatus(string queryString,string drawing,int version)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            ///string queryString = "SP_GetSpoolStatus";
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("status_out", OracleType.Number);
            cmd.Parameters["status_out"].Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add("drawing_in", OracleType.VarChar).Value = drawing;
            cmd.Parameters.Add("version_in", OracleType.Number).Value = version;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException ee)
            {
                MessageBox.Show(ee.Message.ToString(), "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
            return cmd.Parameters["status_out"].Value.ToString();
        }

        /// <summary>
        /// ΪСƱ����ҳ��
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="project"></param>
        /// <param name="drawing"></param>
        /// <param name="spool"></param>
        public static void InsertSpoolPageNo(string queryString, string project,string drawing,string spool,int pageno)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("project_in", OracleType.VarChar).Value = project;
            cmd.Parameters.Add("drawing_in", OracleType.VarChar).Value = drawing;
            cmd.Parameters.Add("spool_in", OracleType.VarChar).Value = spool;
            cmd.Parameters.Add("pageno_in", OracleType.Number).Value = pageno;
            cmd.Transaction = trans;
            try
            {
                cmd.ExecuteNonQuery();
                trans.Commit();
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

        public static DataSet QueryMaterialRation(string queryString, string projectid, string drawing)
        {
            OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);//���conn����  
            conn.Open();
            OracleCommand cmd = new OracleCommand(queryString, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            OracleParameter pram = new OracleParameter("V_CS", OracleType.Cursor);
            pram.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(pram);

            cmd.Parameters.Add("projectid_in", OracleType.VarChar).Value = projectid;
            cmd.Parameters["projectid_in"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.Add("drawing_in", OracleType.VarChar).Value = drawing;
            cmd.Parameters["drawing_in"].Direction = System.Data.ParameterDirection.Input;
            OracleDataAdapter adapter = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }
    }
}
