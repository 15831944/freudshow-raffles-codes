using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using jzpl.WebReference1;
using jzpl.UI.JP;

namespace jzpl.Lib
{
    public class Authentication
    { 
        public static readonly int  MaxPermisonLength = 100;
       
        public enum PERMDEFINE
        {
            PKG_JP_ADD = 1,             //���������������/ҳ��jp_pkg_add.aspx/��ҳ��jp_pkg_mod.aspx/��ҳ��pkg_part_lov1.aspx
            PKG_JP_QUERY = 2,           //�������������/ҳ��jp_pkg_query.aspx/��ҳ��jp_pkg_mod.aspx
            PKG_JP_CONFIRM = 3,         //�������ȷ��/ҳ��jp_pkg_confirm.aspx/
            PKG_JP_CONFIRM1 = 4,        //�������ȷ��/ҳ��jp_pkg_confirm1.aspx/
            PKG_JP_QUERY1 = 5,          //������������ѯ/ҳ��jp_pkg_query_ext.aspx/
            PKG_JP_ISS=6,               //��������·�/ҳ��jp_pkg_issue.aspx/��ҳ��jp_pkg_issue_a.aspx
            PKG_JP_JJD_NEW = 7,         //����������ӵ�/jp_pkg_jjd_new.aspx
            PKG_JP_JJD_FINISH = 8,      //������ӵ����/jp_pkg_jjd_finish.aspx
            PKG_JP_JJD = 9,             //������ӵ���ѯ
            PKG_JP_ISS_VCHR=10,         //����������ϵ���ӡ
                     

            PART_JP_ADD = 21,           //��ͨ���ʴ�����������/����ҳ��wzxqjh_add.aspx/�̳�ҳ��wzxqjh_ration_lov.aspx
            PART_JP_QUERY = 22,         //��������ѯ/wzxqjh_query.aspx/wzxqjh_mod.aspx
            PART_JP_LACK = 30,       //ȱ��/wzxqjh_lack.aspx
            PART_JP_CONFIRM = 23,       //����ȷ��/wzxqjh_confirm.aspx
            PART_JP_CONFIRM1 = 24,      //����ȷ��/wzxqjh_confirm1.aspx
            PART_JP_QUERY1 = 25,        //�����ѯ/wzxqjh_query_ext.aspx           
            PART_JP_JJD_NEW = 26,       //�������ӵ�/wzxqjh_jjd_new_.aspx
            PART_JP_JJD_FINISH = 27,    //���ӵ����/wzxqjh_jjd_finish.aspx
            PART_JP_JJD = 28,           //���ӵ���ѯ/
            PART_ISS_COMPARE = 29,      //ERP-DMS�·�����/


            PKG = 41,                  //�����Ϣά��/package.aspx/��ҳ��pkg_erp_lov.aspx/��ҳ��pkg_mod.aspx
            PKG_PART = 42,             //���С����Ϣά��/pkg_part.aspx/pkg_lov.aspx/pkg_part_mod.aspx
            PKG_ARR = 43,              //��������Ǽ�/pkg_arrival.aspx
            PKG_CHK = 44,              //�������/pkg_check.aspx
            PKG_MOV = 45,              //���λ��ת��/pkg_loc_move.aspx
            PKG_Q = 46,                //�����ѯ/pkg_query.aspx
            PKG_PART_Q = 47,           //���С����ѯ/pkg_part_query.aspx  
            PKG_ARR_Q = 48,            //���������ѯ/pkg_arrival_query.aspx
            PKG_CHK_Q = 49,            //��������ѯ/pkg_check_query.aspx
            PKG_MOV_Q = 50,            //���λ��ת�Ʋ�ѯ/pkg_loc_move_query.aspx
            PKG_LOC_Q = 51,             //�������ѯ/pkg_loc_query.aspx
            PKG_CHG_PRJ=52,             //�����Ŀת��/pkg_change_project.aspx   
            PKG_STORE_IN=53,
            PKG_STORE_ISS=54,
            PKG_VALUE_Q=55,
            PKG_LOCHIS_Q=56,


            BS_COMPANY = 61,            //��˾
            BS_PROJECT = 62,            //��Ŀ
            BS_CURRENCY = 63,           //����
            BS_UNIT = 64,               //��λ
            BS_LACK = 65,               //ȱ��ԭ��
            BS_RCPT_PLACE = 66,         //���յ�
            BS_RCPT_DEPT = 67,          //���ղ���
            BS_WH_AREA = 68,            //����
            BS_WH_LOC = 69,             //λ��
            BS_REQ_GROUP = 70,          //������
            BS_PRJ_ACC=71,              //��Ŀ������Աά��
            BS_RCPT_PER=72,             //������ά��
            BS_CHK_PER=73,              //������ά��
            ADMIN=100
        }

        public struct LOGININFO
        {
            public string UserID;
            public string UserPwd;
            public string Permission;
            public string Admin;
            public string GroupID;
            public string ServerName;
        }
        
        private System.Web.HttpApplicationState Application;
        private System.Web.SessionState.HttpSessionState Session;        
        //public string[] PERMISSION_PAGE;
        public LOGININFO logininfo;        

        public Authentication(System.Web.UI.Page Parent)
        {
            this.Application = Parent.Application;
            this.Session = Parent.Session;
            //this.PERMISSION_PAGE = Enum.GetNames(typeof(FUN_INTERFACE));          
        }

        public Authentication(System.Web.UI.UserControl Parent)
        {
            this.Application = Parent.Application;
            this.Session = Parent.Session;
            //this.PERMISSION_PAGE = Enum.GetNames(typeof(FUN_INTERFACE));
        }

        public bool CheckUser(string id, string pwd, ref string ermsg)
        {
            authenticateService auth = new authenticateService();
            const string DOMAIN = "@raffles.local";
            DataSet ds = Lib.DBHelper.createDataset("select * from jp_user where user_id='" + id + "'" );
            if (ds.Tables[0].Rows.Count <= 0)
            {
                ermsg = id + "�����µ�¼��";
                return false;
            }
            if (false) //ming.li
            //if(auth.get_authenticate(string.Format("{0}{1}",id,DOMAIN),pwd)!="1")
            {
                ermsg = "��֤����";
                return false;
            }
            logininfo.UserID = id;
            logininfo.UserPwd = pwd;
            logininfo.Permission = ds.Tables[0].Rows[0]["role"].ToString();
            logininfo.Admin = ds.Tables[0].Rows[0]["admin"].ToString();
            logininfo.GroupID = ds.Tables[0].Rows[0]["group_id"].ToString();
            logininfo.ServerName = DBHelper.dbStr; 
            Session["USERINFO"] = this.logininfo;
            return true;
        }

        //public bool CheckAccessAble(int index)
        //{
            
        //}
        public bool LoadSession()
        {
            try
            {
                LOGININFO logininfo = (LOGININFO)Session["USERINFO"];
                if (logininfo.UserID.Trim().Length <= 0 || logininfo.UserPwd.Trim().Length <= 0) return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public void RemoveSession()
        {
            LOGININFO loginInfo;
            loginInfo.UserID = string.Empty;
            loginInfo.UserPwd = string.Empty;
            loginInfo.Permission = string.Empty;
            logininfo.Admin = string.Empty;
            loginInfo.GroupID = string.Empty;
            logininfo.ServerName = string.Empty;

            Session["USERINFO"] = logininfo;
        }

        //public string[] InitPermissionArray()
        //{
        //    int range_;
        //    range_ = Enum.GetValues(typeof(FUN_INTERFACE)).Length;
        //    string[] m_permission_array = new string[range_];
        //    for (int i = 0; i < range_; i++)
        //    {
        //        m_permission_array[i] = "0";//accessλ��ʼֵ
        //        System.Type t = System.Type.GetType("jzpl." + PERMISSION_PAGE[i]);
        //        IBasePage p = (IBasePage)Activator.CreateInstance(t);
        //        if (p.GetPermissionType() == PERMISSION_TYPE.page_.ToString()) continue;
        //        for (int j = 0; j < p.GetFun().Length; j++)
        //        {
        //            m_permission_array[i] += "0";
        //        }
        //    }
            
        //    return m_permission_array;
        //}

        public static DataTable GetPermissionTable()
        {            
            DataTable per = new DataTable();
            per.Columns.Add("code", typeof(string));
            per.Columns.Add("page", typeof(string));
            per.Columns.Add("description", typeof(string));
            per.Columns.Add("permission", typeof(string));

            DataRow row  = per.NewRow();

            #region ������� 
            row = per.NewRow(); row["code"] = "PKG_JP_ADD"; row["page"] = "jp_pkg_add"; row["description"] = "����������봴��"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_JP_QUERY"; row["page"] = "jp_pkg_query"; row["description"] = "�������������"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_JP_CONFIRM"; row["page"] = "jp_pkg_confirm"; row["description"] = "�����������ȷ�ϣ����ܣ�"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_JP_CONFIRM1"; row["page"] = "jp_pkg_confirm1"; row["description"] = "����������봴����������"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_JP_QUERY1"; row["page"] = "jp_pkg_query_ext"; row["description"] = "������������ѯ"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_JP_ISS"; row["page"] = "jp_pkg_issue"; row["description"] = "������������·�"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_JP_JJD_NEW"; row["page"] = "jp_pkg_jjd_new"; row["description"] = "������ͽ��ӵ�����"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_JP_JJD_FINISH"; row["page"] = "jp_pkg_jjd_finish"; row["description"] = "������ͽ��ӵ����"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_JP_JJD"; row["page"] = "jp_pkg_jjd"; row["description"] = "������ͽ��ӵ���ѯ"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_JP_ISS_VCHR"; row["page"] = "pkg_issue_voucher"; row["description"] = "����������ϴ�ӡ"; row["permission"] = "0"; per.Rows.Add(row);
            #endregion
            #region ��ͨ�������� 
            row = per.NewRow(); row["code"] = "PART_JP_ADD"; row["page"] = "wzxqjh_add"; row["description"] = "��ͨ�����������봴��"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PART_JP_QUERY"; row["page"] = "wzxqjh_query"; row["description"] = "��ͨ��������������"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PART_JP_LACK"; row["page"] = "wzxqjh_lack"; row["description"] = "��ͨ����ȱ���´ȱƷ��"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PART_JP_CONFIRM"; row["page"] = "wzxqjh_confirm"; row["description"] = "��ͨ������������ȷ�ϣ����ܣ�"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PART_JP_CONFIRM1"; row["page"] = "wzxqjh_confirm1"; row["description"] = "��ͨ�����������루������"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PART_JP_QUERY1"; row["page"] = "wzxqjh_query_ext"; row["description"] = "��ͨ�������������ѯ"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PART_JP_JJD_NEW"; row["page"] = "wzxqjh_jjd_new_"; row["description"] = "��ͨ�������ͽ��ӵ�����"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PART_JP_JJD_FINISH"; row["page"] = "wzxqjh_jjd_finish"; row["description"] = "��ͨ�������ͽ��ӵ����"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PART_JP_JJD"; row["page"] = "wzxqjh_jjd"; row["description"] = "��ͨ�������ͽ��ӵ���ѯ"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PART_ISS_COMPARE"; row["page"] = "wzxqjh_iss_compare"; row["description"] = "ERP-DMS���϶���"; row["permission"] = "0"; per.Rows.Add(row);

            #endregion
            #region �����Ϣά��
            row = per.NewRow(); row["code"] = "PKG"; row["page"] = "package"; row["description"] = "�����Ϣά��"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_PART"; row["page"] = "pkg_part"; row["description"] = "���С����Ϣά��"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_ARR"; row["page"] = "pkg_arrival"; row["description"] = "��������Ǽ�"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_CHK"; row["page"] = "pkg_check"; row["description"] = "�������"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_MOV"; row["page"] = "pkg_loc_move"; row["description"] = "���λ��ת��"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_Q"; row["page"] = "pkg_query"; row["description"] = "�����ѯ"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_PART_Q"; row["page"] = "pkg_part_query"; row["description"] = "���С����ѯ"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_ARR_Q"; row["page"] = "pkg_arrival_query"; row["description"] = "���������ѯ"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_CHK_Q"; row["page"] = "pkg_check_query"; row["description"] = "��������ѯ"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_MOV_Q"; row["page"] = "pkg_loc_move_query"; row["description"] = "���λ��ת�Ʋ�ѯ"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_LOC_Q"; row["page"] = "pkg_loc_query"; row["description"] = "�������ѯ"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_CHG_PRJ"; row["page"] = "pkg_change_project"; row["description"] = "�����Ŀת��"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_STORE_IN"; row["page"] = "pkg_part_receive"; row["description"] = "������ʿ�����"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_STORE_ISS"; row["page"] = "pkg_part_issue"; row["description"] = "������ʿ���·�"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_VALUE_Q"; row["page"] = "pkg_value_query"; row["description"] = "������ʼ�ֵ"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "PKG_LOCHIS_Q"; row["page"] = "pkg_loc_history"; row["description"] = "������ʿ�����񵵰�"; row["permission"] = "0"; per.Rows.Add(row);
            #endregion
            #region ��������
            row = per.NewRow(); row["code"] = "BS_COMPANY"; row["page"] = "company"; row["description"] = "��˾"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "BS_PROJECT"; row["page"] = "project"; row["description"] = "��Ŀ"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "BS_CURRENCY"; row["page"] = "currency"; row["description"] = "����"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "BS_UNIT"; row["page"] = "part_unit"; row["description"] = "��λ"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "BS_LACK"; row["page"] = "lack_reason"; row["description"] = "ȱ��ԭ��"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "BS_RCPT_PLACE"; row["page"] = "receipt_place"; row["description"] = "���յ�"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "BS_RCPT_DEPT"; row["page"] = "receipt_dept"; row["description"] = "���ղ���"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "BS_WH_AREA"; row["page"] = "wh_area"; row["description"] = "����"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "BS_WH_LOC"; row["page"] = "wh_location"; row["description"] = "λ��"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "BS_REQ_GROUP"; row["page"] = "req_group"; row["description"] = "������"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "BS_PRJ_ACC"; row["page"] = "project_acc_per"; row["description"] = "��Ŀ������Աά��"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "BS_RCPT_PER"; row["page"] = "receipt_person"; row["description"] = "������"; row["permission"] = "0"; per.Rows.Add(row);
            row = per.NewRow(); row["code"] = "BS_CHK_PER"; row["page"] = "check_person"; row["description"] = "����Ա"; row["permission"] = "0"; per.Rows.Add(row);
            
            #endregion

            return per;
        }
  
        public string UserID
        {
            get { return this.logininfo.UserID; }
        }
               
        public string UserPassword
        {
            get { return this.logininfo.UserPwd; }
        }
        
        public string Permission
        {
            get { return this.logininfo.Permission; }
        }

        public string Admin
        {
            get { return this.logininfo.Admin; }
        }
        public string GroupID
        {
            get { return this.logininfo.GroupID; }
        }
        public string ServerName
        {
            get { return this.logininfo.ServerName; }
        }
    }
}
