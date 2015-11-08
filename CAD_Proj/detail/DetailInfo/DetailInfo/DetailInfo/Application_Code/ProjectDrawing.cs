using System;
using System.Data;
using System.Web;
using System.Data.Common;
using System.Data.OracleClient;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Common;
using DreamStu.Common.Log;

namespace DetailInfo
{
    public enum DrawingFlowStatus
    {
        /// <summary>
        /// ��ʼ״̬
        /// </summary>
        INITIAL = 0,
        /// <summary>
        /// �Ѽƻ�
        /// </summary>
        PLANNED = 1,
        /// <summary>
        /// ����ɵ���δ�ύ���
        /// </summary>
        FINISHED = 2,
        /// <summary>
        /// ���δͨ�����˻ء�
        /// </summary>
        REJECTED = 3,
        /// <summary>
        /// ���ͨ��
        /// </summary>
        APPROVED = 4,
        /// <summary>
        /// ���·�
        /// </summary>
        ISSUED = 5,
        /// <summary>
        /// �����
        /// </summary>
        APPROVING = 6,
        /// <summary>
        /// ���󴬶���
        /// </summary>
        OWNERAPPROVING = 7,
        /// <summary>
        /// �������ͨ��
        /// </summary>
        OWNERAPPROVED = 8,
        /// <summary>
        /// ��������˻�
        /// </summary>
        OWNERREJECTED = 9,
        /// <summary>
        /// ���󴬼�����
        /// </summary>
        CLASSAPPROVING = 10,
        /// <summary>
        /// ���������ͨ��
        /// </summary>
        CLASSAPPROVED = 11,
        /// <summary>
        /// �������˻�
        /// </summary>
        CLASSREJECTED = 12,
        /// <summary>
        /// �ɹ�ȷ��
        /// </summary>
        PURCHASECONFIRM = 111,
        /// <summary>
        /// �ɹ�����
        /// </summary>
        PURCHASEREJECT = 112,
        /// <summary>
        /// �·��ɹ�
        /// </summary>
        SENDTOPURCHASE = 211,
        /// <summary>
        /// ��������
        /// </summary>
        TAKEBACKPURCHASE = 212
    }
    public partial class ProjectDrawing
    {

        /// <summary>
        /// ����ͼֽ������ʱ��
        /// </summary>
        /// <param name="drawingid"></param>
        /// <param name="timeField"></param>
        /// <returns></returns>
        public static bool UpdateTime(int drawingid, string timeField)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = "UPDATE PLM.PROJECT_DRAWING_TAB SET " + timeField + "=sysdate WHERE DRAWING_ID=:drawingid";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "drawingid", DbType.Int32, drawingid);
            return db.ExecuteNonQuery(cmd) > 0;
        }
       
        /// <summary>
        /// �ύͼֽ
        /// </summary>
        /// <returns></returns>
        public int Submit(string Attachment_Name, string Attachment_Path,int Drawing_ID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmd = db.GetSqlStringCommand("UPDATE PLM.PROJECT_DRAWING_TAB SET ATTACHMENT_PATH = :attachment_path,ATTACHMENT_NAME=:attachment_name, PROGRESS=100, act_progress=100, FLOWSTATUS=6 WHERE DRAWING_ID=:drawing_id");
            db.AddInParameter(cmd, "attachment_path", DbType.String, Attachment_Path);
            db.AddInParameter(cmd, "attachment_name", DbType.String, Attachment_Name);
            db.AddInParameter(cmd, "drawing_id", DbType.Int32, Drawing_ID);
            return db.ExecuteNonQuery(cmd);
        }
        
        /// <summary>
        /// ����ͼֽ�Ÿ���ͼֽ������״̬
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dfs"></param>
        /// <returns></returns>
        public static int UpdateFlowStatus(int id, DrawingFlowStatus dfs)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = "UPDATE PLM.PROJECT_DRAWING_TAB SET FLOWSTATUS=:status WHERE DRAWING_ID=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "status", DbType.Int32, Convert.ToInt32(dfs));
            db.AddInParameter(cmd, "id", DbType.Int32, id);
            return db.ExecuteNonQuery(cmd);
        }
        
        public static int UpdateDrawingPercent(int id, double percent)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = "UPDATE PLM.PROJECT_DRAWING_TAB SET percent=:percent WHERE DRAWING_ID=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "percent", DbType.Double, percent);
            db.AddInParameter(cmd, "id", DbType.Int32, id);
            return db.ExecuteNonQuery(cmd);
        }
        /// <summary>
        /// ����ʵ�ʽ���
        /// </summary>
        /// <param name="pro"></param>
        /// <param name="drawingid"></param>
        /// <returns></returns>
        public static int UpdateActProgress(string pro, int drawingid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = "UPDATE PLM.PROJECT_DRAWING_TAB SET act_progress=:act_progress WHERE DRAWING_ID=:drawingid";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "act_progress", DbType.String, pro);
            db.AddInParameter(cmd, "drawingid", DbType.Int32, drawingid);
            return db.ExecuteNonQuery(cmd);
        }
       
       

        /// <summary>
        /// ����ͼֽ����ͼ��ʾ
        /// </summary>
        /// <param name="drawingid"></param>
        /// <returns></returns>
        public static bool UpdatePrintFlag(int drawingid, string uname, int result)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql;
            if(result == 1)
                sql = "UPDATE PLM.PROJECT_DRAWING_TAB SET PRINTDATE=sysdate ,  PRINTFLAG=" + result + " WHERE DRAWING_ID=:drawingid";
            else
                sql = "UPDATE PLM.PROJECT_DRAWING_TAB SET CANCELDATE=sysdate ,  PRINTFLAG=" + result + " WHERE DRAWING_ID=:drawingid";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "drawingid", DbType.Int32, drawingid);

            return db.ExecuteNonQuery(cmd) > 0;
        }

        public static int IsApproving(string drawingNo, string projectId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = "SELECT count(*) FROM PLM.PROJECT_DRAWING_TAB t WHERE TRIM(LOWER(DRAWING_NO))=:drawingno AND PROJECT_ID=:projectid AND LASTFLAG='Y' and DELETE_FLAG='N' AND t.flowstatus =6 ";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "drawingno", DbType.String, drawingNo.Trim().ToLower());
            db.AddInParameter(cmd, "projectid", DbType.String, projectId);
            object ret = db.ExecuteScalar(cmd);
            if (ret == null || ret == DBNull.Value) return 0;
            return Convert.ToInt32(ret);
        }
        
        public static int GetDrawingIdByNo(string drawingNo, string projectId, string Rev)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = "SELECT DRAWING_ID FROM PLM.PROJECT_DRAWING_TAB t WHERE TRIM(LOWER(DRAWING_NO))=:drawingno AND PROJECT_ID=:projectid  and DELETE_FLAG='N' AND t.REVISION =:rev";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "drawingno", DbType.String, drawingNo.Trim().ToLower());
            db.AddInParameter(cmd, "projectid", DbType.String, projectId);
            db.AddInParameter(cmd, "rev", DbType.String, Rev);
            object ret = db.ExecuteScalar(cmd);
            if (ret == null || ret == DBNull.Value) return 0;
            return Convert.ToInt32(ret);
        }
        /// <summary>
        /// ����ͼֽID���ж��Ƿ�Ϊ���һ��
        /// </summary>
        /// <param name="drawingid"></param>
        /// <returns></returns>
        public static bool LastRev(int drawingid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = "SELECT LASTFLAG FROM PLM.PROJECT_DRAWING_TAB WHERE DRAWING_ID=:drawingid";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "drawingid", DbType.Int32, drawingid);
            object ret = db.ExecuteScalar(cmd);
            if (ret == null || ret == DBNull.Value) return false;
            return ret.ToString() == "Y";
        }



        /// <summary>
        /// ����ͼֽ�Ż��ͼֽ������״̬
        /// </summary>
        /// <param name="drawingid"></param>
        /// <returns></returns>
        public static DrawingFlowStatus GetStatus(int drawingid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = "SELECT FLOWSTATUS FROM PLM.PROJECT_DRAWING_TAB WHERE DRAWING_ID=:drawingid";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "drawingid", DbType.Int32, drawingid);
            object s = db.ExecuteScalar(cmd);
            if (s == null || s == DBNull.Value) return DrawingFlowStatus.INITIAL;
            return (DrawingFlowStatus)Enum.Parse(typeof(DrawingFlowStatus), s.ToString());
        }
        
        /// <summary>
        /// ���ָ��ID��ͼֽ����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetAttachment(int id)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = "SELECT ATTACHMENT_PATH FROM PLM.PROJECT_DRAWING_TAB WHERE DRAWING_ID=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, id);
            object attach = db.ExecuteScalar(cmd);
            if (attach == null) return string.Empty;
            return attach.ToString();
        }

        /// <summary>
        /// ���ָ��ID��ͼֽ����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<string> GetAttachmentandName(int id)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = "SELECT ATTACHMENT_PATH,ATTACHMENT_NAME FROM PLM.PROJECT_DRAWING_TAB WHERE DRAWING_ID=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, id);
            List<string> att = new List<string>(2);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    att.Add(dr[0].ToString()); att.Add(dr[1].ToString());
                }
                dr.Close();
            }
            return att;
        }
        /// <summary>
        /// ��ȡPDF����������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<string> GetAttPDFName(int id)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = "SELECT ATT_PDF_PATH,ATT_PDF_NAME FROM PLM.PROJECT_DRAWING_TAB WHERE DRAWING_ID=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, id);
            List<string> att = new List<string>(2);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    att.Add(dr[0].ToString()); att.Add(dr[1].ToString());
                }
                dr.Close();
            }
            return att;
        }
        /// <summary>
        /// ��ȡͼֽ�ļ�����������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<string> GetAttDRAWName(int id)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = "SELECT ATT_DRAWING_PATH,ATT_DRAWING_NAME FROM PLM.PROJECT_DRAWING_TAB WHERE DRAWING_ID=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, id);
            List<string> att = new List<string>(2);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    att.Add(dr[0].ToString()); att.Add(dr[1].ToString());
                }
                dr.Close();
            }
            return att;
        }
        

        /// <summary>
        /// �Ƴ�ָ��ID��ͼֽ����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool RemoveAttachment(int id)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = "UPDATE PLM.PROJECT_DRAWING_TAB SET ATTACHMENT_PATH='' WHERE DRAWING_ID=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, id);
            return db.ExecuteNonQuery(cmd) > 0;
        }

       
        /// <summary>
        /// ����Drawing ID�ж�ͼֽ�Ƿ�ɾ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsDeL(int id)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = "SELECT DELETE_FLAG FROM PLM.PROJECT_DRAWING_TAB WHERE DRAWING_ID=:id ";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.Int32, id);
            bool delflag = true;
            if (db.ExecuteScalar(cmd).ToString() == "N")
                delflag = false;
            return delflag;
        }
        /// <summary>
        /// ����Drawing No�ж�ͼֽ�Ƿ�ɾ��
        /// </summary>
        /// <param name="drawingno"></param>
        /// <returns></returns>
        public static bool IsDeL(string drawingno)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = "SELECT DELETE_FLAG FROM PLM.PROJECT_DRAWING_TAB WHERE DRAWING_NO=:drawingno and lastflag='Y' ";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "drawingno", DbType.String, drawingno);
            bool delflag = true;
            if (db.ExecuteScalar(cmd).ToString() == "N")
                delflag = false;
            return delflag;
        }
        
       
       

       
       
        


    }
}
