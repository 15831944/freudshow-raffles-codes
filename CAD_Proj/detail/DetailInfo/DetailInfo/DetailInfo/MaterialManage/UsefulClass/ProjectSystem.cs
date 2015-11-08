using System;
using System.Data;
using System.IO;
using System.Data.Common;
using System.Data.OracleClient;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Common;
using DreamStu.Common.Log;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Framework;
namespace DetailInfo
{
    /// <summary>
    /// ϵͳʵ��
    /// </summary>
   
    public class ProjectSystem
    {
        private  string  _project;
        /// <summary>
        /// ��Ŀ���
        /// </summary>
        [BindingField]
        public  string  Project_Id
        {
            get { return _project; }
            set { _project = value; }
        }

        private int _system;
        /// <summary>
        /// ��ǰϵͳ���
        /// </summary>
        [BindingField]
        public int System_Id
        {
            get { return _system; }
            set { _system = value; }
        }

        private string  _description;
        /// <summary>
        /// ϵͳ����
        /// </summary>
        [BindingField]
        public string  Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _newFlag;
        /// <summary>
        /// �Ƿ�Ϊ��ϵͳ�ı�ʶ(N:�� Y:��)
        /// </summary>
        [BindingField]
        public string NewFlag
        {
            get { return _newFlag; }
            set { _newFlag = value; }
        }


        private int _parent;
        /// <summary>
        /// ϵͳparent���
        /// </summary>
        [BindingField]
        public int Parent_Id
        {
            get { return _parent; }
            set { _parent = value; }
        }

        private string _code;
        /// <summary>
        /// ϵͳCode
        /// </summary>
        [BindingField]
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        
        /// <summary>
        /// ����System��Ų���ɾ����Ϣ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static  int  Findid(int id)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT system_id FROM PLM.PROJECT_System_TAB WHERE parent_Id=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.Int32, id);
            return Convert.ToInt32(db.ExecuteScalar(cmd));
        }
        /// <summary>
        /// �����Ŀ����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetProName(string id)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT name FROM PLM.PROJECT_TAB WHERE Id=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, id);
            return Convert.ToString(db.ExecuteScalar(cmd));
        }
        /// <summary>
        /// ����system id����ϵͳ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetSubCount(string id)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT count(*) FROM PLM.PROJECT_System_TAB WHERE parent_Id=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, id);
            return Convert.ToInt32(db.ExecuteScalar(cmd));
        }
        /// <summary>
        /// ����ϵͳID����ϵͳ����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetDes(int id)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT description FROM PLM.PROJECT_System_TAB WHERE System_Id=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.Int32 , id);
            return Convert.ToString(db.ExecuteScalar(cmd));
        }
        public static string GetCode(int id)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT code FROM PLM.PROJECT_System_TAB WHERE System_Id=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.Int32, id);
            return Convert.ToString(db.ExecuteScalar(cmd));
        }
        /// <summary>
        /// ����ERP����ĿID���Ҷ�ӦECDMS����ĿID
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        public static int FindProjectid(string proid)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT ecdmsid FROM PLM.PROJECT_RELATION_TAB WHERE ERPId=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, proid);
            return Convert.ToInt32(db.ExecuteScalar(cmd));
        }


        public  static int FindParentid(int id)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr); 
            string sql = "SELECT parent_id FROM PLM.PROJECT_System_TAB WHERE System_Id=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.Int32, id);
            return Convert.ToInt32(db.ExecuteScalar(cmd));
        }

        public static int FindPid(int id)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr); 
            string sql = "SELECT parent_id FROM PLM.PROJECT_System_TAB WHERE System_Id=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.Int32, id);
            return Convert.ToInt32(db.ExecuteScalar(cmd));
        }

        /// <summary>
        /// ���� ����system_id,��Ŀ���ڵ�fid �ҵ�·��
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        public static string FindPath(int id, int fid)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr); 
            
            //string sql = "select sys_connect_by_path(description,'/')  from plm.project_system_tab where parent_id<>0 and system_id=:id start with system_id =:fid connect by   parent_id   =   prior system_id";
            string sql = "select sys_connect_by_path(description,'/')  from plm.project_system_tab  where  system_id=:id  start with system_id =:fid connect by   parent_id   =   prior system_id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.Int32, id);
            db.AddInParameter(cmd, "fid", DbType.Int32, fid);
            return db.ExecuteScalar(cmd).ToString();
        }

        public static string GetSystemTree(int systemId)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "select PLM.PROJECT_SYSTEM_API.GetSystemTree(" + systemId + ") from dual";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return db.ExecuteScalar(cmd).ToString();
        }

        /// <summary>
        /// �鿴��PROJECT_System_TAB������û�� ѡ�����Ŀ
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public  static DataSet  TreeByPid(string id)
        {
            return TreeByPid(id, string.Empty, "N");
        }

        public static DataSet TreeByPidNewFlag(string projectId)
        {
            return TreeByPid(projectId, string.Empty, "Y");
        }

        /// <summary>
        /// �鿴��PROJECT_System_TAB������û�� ѡ�����Ŀ
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public  static DataSet TreeByPid(string id, string keyword, string newFlag)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT * FROM PLM.PROJECT_System_TAB WHERE project_id=:id and NEWFLAG ='" + newFlag + "'";
            if (keyword != string.Empty) sql += " AND DESCRIPTION LIKE '%" + keyword + "%'";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, id);
            return db.ExecuteDataSet((cmd));
        }


        public static ProjectSystem Populate(IDataReader dr)
        {
            return EntityBase<ProjectSystem>.DReaderToEntity(dr);
        }



        /// <summary>
        /// ��������ϵͳ�б�
        /// </summary>
        /// <returns></returns>
        public static List<ProjectSystem> FindAll()
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT * FROM PLM.PROJECT_SYSTEM_TAB";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return EntityBase<ProjectSystem>.DReaderToEntityList(db.ExecuteReader(cmd));
        }

        /// <summary>
        /// ������Ŀ������ϵͳ
        /// </summary>
        /// <returns></returns>
        public static List<ProjectSystem> FindAll(int projectid)
        {
            return FindAll(projectid, "N");
        }

        /// <summary>
        /// ������Ŀ������ϵͳ
        /// </summary>
        /// <returns></returns>
        public static List<ProjectSystem> FindAll(int projectid, string newFlag)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT * FROM PLM.PROJECT_SYSTEM_TAB WHERE PROJECT_ID=:projectid AND NEWFLAG=:newflag";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "projectid", DbType.Int32, projectid);
            db.AddInParameter(cmd, "newflag", DbType.String, newFlag);
            return EntityBase<ProjectSystem>.DReaderToEntityList(db.ExecuteReader(cmd));
        }

        /// <summary>
        /// ������Ŀ������ϵͳ
        /// </summary>
        /// <returns></returns>
        public static List<ProjectSystem> FindAll(int projectid, string parentId, string newFlag)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT * FROM PLM.PROJECT_SYSTEM_TAB WHERE PROJECT_ID=:projectid AND NEWFLAG=:newflag";
            if (parentId != string.Empty) sql += " AND PARENT_ID=:parentId";
            sql += " ORDER BY SYSTEM_ID";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "projectid", DbType.Int32, projectid);
            db.AddInParameter(cmd, "newflag", DbType.String, newFlag);
            if (parentId != string.Empty) db.AddInParameter(cmd, "parentId", DbType.Int32, parentId);
            return EntityBase<ProjectSystem>.DReaderToEntityList(db.ExecuteReader(cmd));
        }

        public static List<ProjectSystem> FindSub(int projectid, string newFlag)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT * FROM PLM.PROJECT_SYSTEM_TAB WHERE PROJECT_ID=:projectid AND NEWFLAG=:newflag AND PARENT_ID<>0";
            sql += " ORDER BY SYSTEM_ID";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "projectid", DbType.Int32, projectid);
            db.AddInParameter(cmd, "newflag", DbType.String, newFlag);
            return EntityBase<ProjectSystem>.DReaderToEntityList(db.ExecuteReader(cmd));
        }

        /// <summary>
        /// ���ϵͳ������
        /// </summary>
        /// <param name="id">ϵͳ ID</param>
        /// <returns></returns>
        public static string GetName(int id)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT DESCRIPTION FROM PLM.PROJECT_SYSTEM_TAB WHERE SYSTEM_ID=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.String, id);
            object name = db.ExecuteScalar(cmd); if (name == null) return string.Empty;
            return Convert.ToString(name);
        }

        

        /// <summary>
        /// ����ϵͳID�������ϵͳ�µ�ID����(ֻ��һ��)
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public static List<string> FindAllChild(int systemId)
        {
            return FindAllChild(systemId, false);
        }

        /// <summary>
        /// ����ϵͳID�������ϵͳ�µ�����ID����
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="recursiveFlag">�Ƿ�ݹ��ѯ</param>
        /// <returns></returns>
        public static List<string> FindAllChild(int systemId, bool recursiveFlag)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT SYSTEM_ID FROM PLM.PROJECT_SYSTEM_TAB WHERE PARENT_ID=:pid";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "pid", DbType.Int32, systemId);
            List<string> idsList = new List<string>();
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    idsList.Add(dr[0].ToString());
                    if (recursiveFlag)
                        idsList.AddRange(FindAllChild(Convert.ToInt32(dr[0]), recursiveFlag));
                }
                dr.Close();
            }
            return idsList;
        }
        /// <summary>
        /// ��ȡ��ϵͳ
        /// </summary>
        /// <param name="SysId"></param>
        /// <returns></returns>
        public static List<int> GetChildSystems(int SysId)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT SYSTEM_ID FROM PLM.PROJECT_SYSTEM_TAB WHERE PARENT_ID=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.Int32, SysId);
            List<int> SysIdList = new List<int>();
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                    SysIdList.Add(Convert.ToInt32(dr[0].ToString()));
                dr.Close();
            }
            return SysIdList;
        }
        //��ȡ��ϵͳ�б�
        public static List<int> GetChildSysId(List<int> SysList, int SysId)
        {
            SysList.Add(SysId);
            List<int> RootSys= new List<int>();
            RootSys = GetChildSystems(SysId);
            //DeptList.AddRange(RootDept);
            foreach (int id in RootSys)
            {
                if (Findid(id) != 0)
                {
                    // CreateChildDeptId(id);
                    GetChildSysId(SysList, id);
                }
                else
                {
                    SysList.Add(id);
                }
            }
            return SysList;
        }


        public override string ToString()
        {
            return string.Format("[{0},\"{1}\"]", System_Id, string.Format("{0}{1}", Description, string.IsNullOrEmpty(Code) ? "" : ("(" + Code + ")")));
        }


        public static int FindDelDrawing(int id)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT DRAWING_ID FROM PLM.project_drawing_tab WHERE SYSTEM_ID=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.Int32, id);
            return Convert.ToInt32(db.ExecuteScalar(cmd));
        }
        public static int FindDelDevice(int id)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT DEVICE_ID FROM PLM.device_tab WHERE SYSTEM_ID=:id";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "id", DbType.Int32, id);
            return Convert.ToInt32(db.ExecuteScalar(cmd));
        }
        


    }
}
