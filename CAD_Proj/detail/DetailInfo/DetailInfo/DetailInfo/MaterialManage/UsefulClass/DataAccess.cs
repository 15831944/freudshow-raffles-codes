using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using DetailInfo;
namespace Framework
{
    public class DataAccess
    {
        public static string severstr = string.Empty;
        /// <summary>
        /// ERP�����ַ���
        /// </summary>
        public const string IFSConnStr = "Data Source=prod;User ID=project_check;Password=TYBH#4768;Unicode=True";
        /// <summary>
        /// ECDM�����ַ���
        /// </summary>
        public static string OIDSConnStr = string.Empty;
        public static void GetSeverName(string sql)
        {
            severstr = sql;
            OIDSConnStr = "Data Source=" + severstr + ";User ID=plm;Password=123!feed;Unicode=True";
        }
    }
}
