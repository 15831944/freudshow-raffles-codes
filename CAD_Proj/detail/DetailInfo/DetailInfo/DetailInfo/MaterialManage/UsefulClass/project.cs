using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Common;
using DreamStu.Common.Log;

namespace Framework
{
    public class project
    {
        private string _id;
        /// <summary>
        /// ���
        /// </summary>
        [BindingField]
        public string project_id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _disc;
        /// <summary>
        /// ���
        /// </summary>
        [BindingField]
        public string description
        {
            get { return _disc; }
            set { _disc = value; }
        }
        public static project Find(string id)
        {
           // Database db = DatabaseFactory.CreateDatabase();
            OracleDatabase db = new OracleDatabase(DataAccess.IFSConnStr);
            string sql = "SELECT * FROM IFSAPP.PROJECT WHERE project_id=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, id);
            return Populate(db.ExecuteReader(cmd));
        }
        public static string FindName(string id)
        {
            //Database db = DatabaseFactory.CreateDatabase();
            OracleDatabase db = new OracleDatabase(DataAccess.IFSConnStr);
            string sql = "SELECT description FROM IFSAPP.PROJECT WHERE project_id=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, id);
            return  Convert.ToString (db.ExecuteScalar(cmd));
        }
        /// <summary>
        /// ����ERPID��ȡ��Ӧ��ECDMSID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string FindECDMSID(string id)
        {
            //Database db = DatabaseFactory.CreateDatabase();
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT ECDMSID FROM plm.PROJECT_RELATION_TAB WHERE ERPid=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, id);
            return Convert.ToString(db.ExecuteScalar(cmd));
        }
        public static string FindSiteName(string id)
        {
            //Database db = DatabaseFactory.CreateDatabase();
            OracleDatabase db = new OracleDatabase(DataAccess.IFSConnStr);
            string sql = "SELECT CONTRACT_REF FROM IFSAPP.site_tab WHERE CONTRACT=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, id);
            return Convert.ToString(db.ExecuteScalar(cmd));
        }
        /// <summary>
        /// ��IDataReader�����Commentʵ��
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static project Populate(IDataReader dr)
        {
            return EntityBase<project>.DReaderToEntity(dr);
        }
        /// <summary>
        /// ��������Project�б�
        /// </summary>
        /// <returns></returns>
        public static List<project> FindAll()
        {
            OracleDatabase db = new OracleDatabase(DataAccess.IFSConnStr);
           // Database db = DatabaseFactory.CreateDatabase("ifsConnection");
            string sql = "SELECT * FROM IFSAPP.PROJECT";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return EntityBase<project>.DReaderToEntityList(db.ExecuteReader(cmd));
        }
        /// <summary>
        /// ��������Project�б�
        /// </summary>
        /// <returns></returns>
        public static DataSet FindProDataset()
        {
            OracleDatabase db = new OracleDatabase(DataAccess.IFSConnStr);
            //Database db = DatabaseFactory.CreateDatabase("ifsConnection");
            string sql = "SELECT * FROM IFSAPP.PROJECT where project_id in ('YRO-06-194','YRO-06-209','YRO-197-C', 'YRO-06-201','YRO-07-218','YRO-07-233','YRO-07-211', 'YRO-06-206','YRO-11-266','YCRO11-256','YRO-11MA20','YRO-06-195','YRO-07-212','YRO-11-267', 'YRO-06-196','YRO-11-264','YRO-11-265') order  by project_id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return db.ExecuteDataSet(cmd);
        }
        /// <summary>
        /// ��������Site�б�
        /// </summary>
        /// <returns></returns>
        public static DataSet FindSiteDataset()
        {
            OracleDatabase db = new OracleDatabase(DataAccess.IFSConnStr);
           // Database db = DatabaseFactory.CreateDatabase("ifsConnection");
            string sql = "SELECT * FROM IFSAPP.site_tab";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return db.ExecuteDataSet(cmd);
        }
        /// <summary>
        /// ����PROJECTNAME��ȡ��Ӧ��ERP����ĿID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string FindERPID(string id)
        {
            //Database db = DatabaseFactory.CreateDatabase();
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT ERPID FROM plm.project_relation_view WHERE projectname_db=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, id);
            return Convert.ToString(db.ExecuteScalar(cmd));
        }

    }
}
