using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Common;
using DreamStu.Common;
using System.Data.OleDb;
namespace Framework
{
    /// <summary>
    /// �û�ʵ����
    /// </summary>
    public partial class User
    {
        private int _id;
        /// <summary>
        /// �û����
        /// </summary>
        [BindingField]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _userName;
        /// <summary>
        /// �û�����
        /// </summary>
        [BindingField]
        public string Name
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _userpass;
        /// <summary>
        /// �û�����
        /// </summary>
        [BindingField]
        public string Pass
        {
            get { return _userpass; }
            set { _userpass = value; }
        }

        private int _typeid;
        /// <summary>
        /// �û����ͱ��
        /// </summary>
        [BindingField]
        public int TypeID
        {
            get { return _typeid; }
            set { _typeid = value; }
        }

        private string _ext;
        /// <summary>
        /// ���ߵ绰
        /// </summary>
        [BindingField]
        public string Ext
        {
            get { return _ext; }
            set { _ext = value; }
        }

        private string _email;
        /// <summary>
        /// Email
        /// </summary>
        [BindingField]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private int _departmentId;
        /// <summary>
        /// ���ű��
        /// </summary>
        [BindingField]
        public int DepartmentID
        {
            get { return _departmentId; }
            set { _departmentId = value; }
        }

        private int _projectID;
        /// <summary>
        /// ��Ŀ���
        /// </summary>
        [BindingField]
        public int ProjectID
        {
            get { return _projectID; }
            set { _projectID = value; }
        }

        private string _icard;
        /// <summary>
        /// IC����
        /// </summary>
        [BindingField]
        public string ICard
        {
            get { return _icard; }
            set { _icard = value; }
        }

        private int _positionId;
        /// <summary>
        /// ְ����
        /// </summary>
        [BindingField]
        public int PositionID
        {
            get { return _positionId; }
            set { _positionId = value; }
        }


        private DateTime _createDate;
        /// <summary>
        /// �ʺŴ�������
        /// </summary>
        [BindingField]
        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        private string _lastIP;
        /// <summary>
        /// ����¼IP
        /// </summary>
        [BindingField]
        public string Last_IP
        {
            get { return _lastIP; }
            set { _lastIP = value; }
        }

        private DateTime _lastTime;
        /// <summary>
        /// ����¼ʱ��
        /// </summary>
        [BindingField]
        public DateTime Last_Time
        {
            get { return _lastTime; }
            set { _lastTime = value; }
        }


        private string _state;
        /// <summary>
        /// �û��ʺŵ�״̬
        /// </summary>
        [BindingField]
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }
        private string _cnname;
        /// <summary>
        /// �û�������
        /// </summary>
        [BindingField]
        public string NAME_CN
        {
            get { return _cnname; }
            set { _cnname = value; }
        }

        private string _setstr;
        /// <summary>
        /// �û��Ա�
        /// </summary>
        [BindingField]
        public string SEX
        {
            get { return _setstr; }
            set { _setstr = value; }
        }

        private string _xl;
        /// <summary>
        /// �û�ѧ��
        /// </summary>
        [BindingField]
        public string DIPLOMA
        {
            get { return _xl; }
            set { _xl = value; }
        }

        private string _spec;
        /// <summary>
        /// �û�רҵ
        /// </summary>
        [BindingField]
        public string SPECIALITY
        {
            get { return _spec; }
            set { _spec = value; }
        }
        private string _gschool;
        /// <summary>
        /// �û���ҵԺУ
        /// </summary>
        [BindingField]
        public string GRADUATE_COLLEGE
        {
            get { return _gschool; }
            set { _gschool = value; }
        }
        private string _np;
        /// <summary>
        /// �û�����
        /// </summary>
        [BindingField]
        public string NATIVE_PLACE
        {
            get { return _np; }
            set { _np = value; }
        }
        private DateTime _birth;
        /// <summary>
        /// �û�����
        /// </summary>
        [BindingField]
        public DateTime BIRTHDATE
        {
            get { return _birth; }
            set { _birth = value; }
        }
        private DateTime _jd;
        /// <summary>
        /// �û��볧ʱ��
        /// </summary>
        [BindingField]
        public DateTime JOIN_DATE
        {
            get { return _jd; }
            set { _jd = value; }
        }
        private DateTime _gd;
        /// <summary>
        /// �û���ҵʱ��
        /// </summary>
        [BindingField]
        public DateTime GRADUATE_DATE
        {
            get { return _gd; }
            set { _gd = value; }
        }
        private string _married;
        /// <summary>
        /// ���
        /// </summary>
        [BindingField]
        public string MARRIED
        {
            get { return _married; }
            set { _married = value; }
        }


        /// <summary>
        /// �����û����ж��û��Ƿ����
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool Exist(string userName)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            //Database db = DatabaseFactory.CreateDatabase("oidsConnection");
            string sql = "SELECT NAME FROM PLM.USER_TAB WHERE LOWER(NAME)=:name";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "name", DbType.String, userName.ToLower());
            object rname = db.ExecuteScalar(cmd);
            return (rname == null || rname == DBNull.Value) ? false : true;
        }

        /// <summary>
        /// ��֤�û�
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Verify(string userName, string password)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            //Database db = DatabaseFactory.CreateDatabase("oidsConnection");
            string sql = "SELECT COUNT(*) FROM PLM.USER_TAB WHERE TRIM(LOWER(NAME))=:username AND PASS=:userpass";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "username", DbType.String, userName.ToLower());
            db.AddInParameter(cmd, "userpass", DbType.String, Security.HashCryptString(password));
            return Convert.ToInt32(db.ExecuteScalar(cmd)) >= 1;
        }

        /// <summary>
        /// ��֤�û�������û���ID(��û���û��򷵻�0)
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int VerifyID(string userName, string password)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            //Database db = DatabaseFactory.CreateDatabase("oidsConnection");
            string sql = "SELECT ID FROM PLM.USER_TAB WHERE LOWER(NAME)=:username AND PASS=:userpass";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "username", DbType.String, userName.ToLower());
            db.AddInParameter(cmd, "userpass", DbType.String, Security.HashCryptString(password));
            object ret = db.ExecuteScalar(cmd); if (ret == null) return 0;
            return Convert.ToInt32(ret);
        }

        /// <summary>
        /// ����û��ʺ�״̬
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static UserState GetState(string userName)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            //Database db = DatabaseFactory.CreateDatabase("oidsConnection");
            string sql = "SELECT STATE FROM PLM.USER_TAB WHERE TRIM(LOWER(NAME))=:name";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "name", DbType.String, userName.ToLower());
            object s = db.ExecuteScalar(cmd);
            if (s == null || s == DBNull.Value) return UserState.LOCKED;
            if (string.IsNullOrEmpty(s.ToString())) return UserState.LOCKED;
            return (UserState)Enum.Parse(typeof(UserState), s.ToString());
        }

        /// <summary>
        /// �����û�������û�
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static User Find(string userName)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            //Database db = DatabaseFactory.CreateDatabase("oidsConnection");
            string sql = "SELECT * FROM PLM.USER_TAB WHERE LOWER(NAME)=:username";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, ":username", DbType.String, userName.ToLower());
            return Populate(db.ExecuteReader(cmd));
        }

        /// <summary>
        /// ��DataReader������û�ʵ����
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static User Populate(IDataReader dr)
        {
            return EntityBase<User>.DReaderToEntity(dr);
        }
        /// <summary>
        /// �����û����Ƽ�Ȩ�ޱ�ʶ���ж��Ƿ���и�Ȩ��
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="privilegeFlag"></param>
        /// <returns></returns>
        public static bool HavingPrivilege(string userName, string privilegeFlag)
        {
            int privlegeId = Privilege.FindIdByFlag(privilegeFlag);
            if (privlegeId == 0) return false;

            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT PRIVILEGE_ID FROM PLM.USERINPRIVILEGE_TAB WHERE USERNAME=:username AND PRIVILEGE_ID=:privilegeid";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "username", DbType.String, userName);
            db.AddInParameter(cmd, "privilegeid", DbType.Int32, privlegeId);
            object isExist = db.ExecuteScalar(cmd);
            if (isExist != null && isExist != DBNull.Value) return true;

            List<string> roleNameList = FindRoleName(userName);
            if (roleNameList.Count == 0) return false;
            bool ret = false;
            foreach (string roleName in roleNameList)
            {
                if (Role.HavingPrivilege(roleName, privlegeId))
                {
                    ret = true; break;
                }
            }
            return ret;
        }

        /// <summary>
        /// �����û�����������н�ɫ��
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static List<string> FindRoleName(string userName)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT r.ROLENAME FROM PLM.ROLE_TAB r, PLM.USERINROLE_TAB u WHERE r.ROLENAME=u.ROLENAME AND LOWER(u.USERNAME)=:username";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, ":username", DbType.String, userName.ToLower());
            List<string> rnameList = new List<string>();
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                    rnameList.Add(dr[0].ToString());
                dr.Close();
            }
            return rnameList;
        }

        /// <summary>
        /// �����û����Ƽ�Ȩ�ޱ�ʶ���ж��Ƿ���и�Ȩ��
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="privilegeFlag"></param>
        /// <returns></returns>
        public static bool HavingPrivilege(string userName, string privilegeFlag, string andSql)
        {
            int privlegeId = Privilege.FindIdByFlag(privilegeFlag);
            if (privlegeId == 0) return false;

            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = "SELECT ASQL FROM PLM.USERINPRIVILEGE_TAB WHERE USERNAME=:username AND PRIVILEGE_ID=:privilegeid";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "username", DbType.String, userName);
            db.AddInParameter(cmd, "privilegeid", DbType.Int32, privlegeId);
            object asql = db.ExecuteScalar(cmd);

            if (asql != null)
            {
                if (asql.ToString() == "ALL") return true;

                string finalSql = string.Format("{0}{1}", asql, string.IsNullOrEmpty(andSql) ? string.Empty : (" AND " + andSql));
                DbCommand finalCmd = db.GetSqlStringCommand(finalSql);
                object c = db.ExecuteScalar(finalCmd);
                if (Convert.ToInt32(c) > 0) return true;
            }


            List<string> roleNameList = FindRoleName(userName);
            if (roleNameList.Count == 0) return false;
            bool ret = false;
            foreach (string roleName in roleNameList)
            {
                if (Role.HavingPrivilege(roleName, privlegeId, andSql))
                {
                    ret = true; break;
                }
            }
            return ret;
        }
    }

    /// <summary>
    /// �û��ʺŵ�״̬
    /// </summary>
    public enum UserState
    {
        /// <summary>
        /// ����
        /// </summary>
        NORMAL,
        /// <summary>
        /// ����
        /// </summary>
        LOCKED
    }

    /// <summary>
    /// ��ɫ��״̬
    /// </summary>
    public enum RoleState
    {
        /// <summary>
        /// ����
        /// </summary>
        NORMAL,
        /// <summary>
        /// ����
        /// </summary>
        LOCKED
    }
}
