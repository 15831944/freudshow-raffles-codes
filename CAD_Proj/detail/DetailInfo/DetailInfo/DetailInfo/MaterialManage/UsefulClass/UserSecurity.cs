using System;
using System.Web;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Web.Security;
using System.Security.Principal;
using DreamStu.Common;
using DreamStu.Common.Log;

namespace Framework
{
    public class UserSecurity
    {
        /// <summary>
        /// �������û���ɫ��
        /// </summary>
        public readonly static string CommonRole = "CommonUser";
        /// <summary>
        /// �������û���ɫ��
        /// </summary>
        public readonly static string LiabilityRole = "LiabilityUser";

        #region Login,Logout,Auth
        /// <summary>
        /// �û�����������֤
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int UserVerifyId(string userName, string password)
        {
            return User.VerifyID(userName, password);
        }

        /// <summary>
        /// �û�����������֤
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool UserVerify(string userName, string password)
        {
            return User.Verify(userName, password);
        }

        public static bool UserExist(string userName)
        {
            return User.Exist(userName.Trim());
        }

        
        #endregion

        #region Properties
        

        

        
        #endregion

       

        #region Privilege
        /// <summary>
        /// ����Ȩ�ޱ�ʶ���жϵ�ǰ��¼�û��Ƿ���и�Ȩ��
        /// </summary>
        /// <param name="privilegeFlag"></param>
        /// <returns></returns>
        public static bool HavingPrivilege(string UserName, string privilegeFlag)
        {
            //string[] roleArray = RoleName.Split('|');
            //string[] roleArray = User.FindRoleName(UserName).ToArray();
            //if (roleArray.Length == 0) return false;
            //privilegeFlag = privilegeFlag.Trim();
            //foreach (string rname in roleArray)
            //{
            //    if (Role.HavingPrivilege(rname, privilegeFlag))
            //        return true;
            //}
            //return false;
            return User.HavingPrivilege(UserName, privilegeFlag);
        }
        


        #endregion
    }
}
