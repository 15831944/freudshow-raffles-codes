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
    public class PartType
    {
        private int _id;
        /// <summary>
        /// ���
        /// </summary>
        [BindingField]
        public int TYPEID
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _pid;
        /// <summary>
        /// ��ID
        /// </summary>
        [BindingField]
        public int PARENTID
        {
            get { return _pid; }
            set { _pid = value; }
        }
        private string _disc;
        /// <summary>
        /// ����
        /// </summary>
        [BindingField]
        public string TYPE_DESC
        {
            get { return _disc; }
            set { _disc = value; }
        }
        private string _typeno;
        /// <summary>
        /// ���ͱ�� 
        /// </summary>
        [BindingField]
        public string TYPE_NO
        {
            get { return _typeno; }
            set { _typeno = value; }
        }
        private string _creator;
        /// <summary>
        /// ������
        /// </summary>
        [BindingField]
        public string CREATOR
        {
            get { return _creator; }
            set { _creator = value; }
        }
        public static DataSet PartTypeDs()
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            //Database db = DatabaseFactory.CreateDatabase("ifsConnection");
            string sql = "SELECT TYPEID, TYPE_DESC FROM plm.MM_PART_TYPE_TAB";
            DbCommand cmd = db.GetSqlStringCommand(sql);         
            return db.ExecuteDataSet(cmd);
        }
        /// <summary>
        /// ȡ�����No�б�
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        public static DataSet PartNoData(string pid, string contract)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            //Database db = DatabaseFactory.CreateDatabase("ifsConnection");
            string sql = "SELECT part_no FROM plm.MM_PART_TAB where parentid=" + pid + " and contract='" + contract + "'";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return db.ExecuteDataSet(cmd);
        }
        /// <summary>
        /// ȡ�����Spec�б�
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        public static DataSet PartSpecData(string pid, string contract)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            //Database db = DatabaseFactory.CreateDatabase("ifsConnection");
            string sql = "SELECT part_no,part_spec FROM plm.MM_PART_TAB where parentid=" + pid + " and contract='" + contract + "'";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return db.ExecuteDataSet(cmd);
        }
        /// <summary>
        /// �������NOȡ�����Spec�б�
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        public static DataSet PartSpecDs(string pno, string contract)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            //Database db = DatabaseFactory.CreateDatabase("ifsConnection");
            string sql = "SELECT part_no,part_spec FROM plm.MM_PART_TAB where part_no like '%" + pno + "%' and contract='" + contract + "'";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return db.ExecuteDataSet(cmd);
        }
        public static string FindPartTypeDesc(int typeid)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT TYPE_DESC FROM plm.MM_PART_TYPE_TAB WHERE TYPEID=:typeid";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "typeid", DbType.Int32, typeid);
            return Convert.ToString(db.ExecuteScalar(cmd));
        }
       
       
        /// <summary>
        /// ȡ��һ����������
        /// </summary>
        /// <returns></returns>
        public static PartType Find(string id)
        {
            // Database db = DatabaseFactory.CreateDatabase();
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT * FROM MM_PART_TAB WHERE project_id=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, id);
            return Populate(db.ExecuteReader(cmd));
        }
        
        /// <summary>
        /// �������е�һ������б�
        /// </summary>
        /// <returns></returns>
        public static List<PartType> Find1STPartType()
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT * FROM plm.MM_PART_TYPE_TAB WHERE PARENT_ID=0";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            
            return EntityBase<PartType>.DReaderToEntityList(db.ExecuteReader(cmd));
        }
        public static PartType Populate(IDataReader dr)
        {
            return EntityBase<PartType>.DReaderToEntity(dr);
        }
        /// <summary>
        /// �������е�2������б�
        /// </summary>
        /// <returns></returns>
        public static List<PartType> Find2STPartType(int typeid)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT * FROM plm.MM_PART_TYPE_TAB WHERE PARENT_ID=:typeid";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "typeid", DbType.Int32, typeid);
            return EntityBase<PartType>.DReaderToEntityList(db.ExecuteReader(cmd));
        }
        /// <summary>
        /// ��������Project�б�
        /// </summary>
        /// <returns></returns>
        public static DataSet FindProDataset()
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            //Database db = DatabaseFactory.CreateDatabase("ifsConnection");
            string sql = "SELECT * FROM IFSAPP.PROJECT";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return db.ExecuteDataSet(cmd);
        }
    }

}
