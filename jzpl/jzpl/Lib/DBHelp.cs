using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OracleClient;

namespace jzpl.Lib
{
    public class DBHelper
    {
        /// <summary>
        /// ��ȡWeb.config�е����ݿ������ַ���
        /// </summary>
        /// 
        public static readonly string dbStr = "didev";
        public static readonly string oledbStr = "oledidev";
        //public static readonly string dbStr = "prod";
        //public static readonly string oledbStr = "oleprod";
        public static string ConnectionString
        {
            get
            {
                string _connectionString = ConfigurationManager.ConnectionStrings[dbStr].ConnectionString;               
                return _connectionString;
            }
        }
        /// <summary>
        /// ��ȡWeb.config�е����ݿ������ַ���
        /// </summary>
        public static string OleConnectionString
        {
            get
            {    
                string _connectionString = ConfigurationManager.ConnectionStrings[oledbStr].ConnectionString;  
                return _connectionString;
            }
        }
        #region ��������ת����dataset
        /// <summary>
        /// �����ֵȼ���ʾ�����ݱ�
        /// </summary>
        /// <param name="sqlStr">sqlstr����Ϊ���ڵ㡢�ڵ����������ڵ�</param>
        /// <returns></returns>
        public static DataTable createDataTable(string sqlStr)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SysOrganizationID", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Name", System.Type.GetType("System.String"));
            dt.Columns.Add("FatherSysOrganizationID", System.Type.GetType("System.Int32"));
            //yroframe.ManPower.MpClass mp = new yroframe.ManPower.MpClass();
            //DBHelper db = new DBHelper();
            DataView view = createGridView(sqlStr);
            for (int i = 0; i < view.Count; i++)
            {
                DataRow NewRow = dt.NewRow();
                NewRow[0] = Convert.ToInt32(view[i][0].ToString());
                NewRow[1] = view[i][1].ToString();
                NewRow[2] = Convert.ToInt32(view[i][2].ToString());
                dt.Rows.Add(NewRow);
            }
            return dt;
        }
        #endregion
        //�󶨸��ڵ�
        /// <summary>
        /// �ּ���DropDownList
        /// </summary>
        /// <param name="drplist">DropDownList</param>
        /// <param name="sqlstr">SQL���</param>
        public void BindFather(DropDownList drplist, string sqlstr)
        {
            //drplist.Items.Add(new ListItem("��ѡ��", "0"));
            DataTable dt = createDataTable(sqlstr);

            DataRow[] drs = dt.Select("FatherSysOrganizationID= " + 0);//���ڵ��һ����Ӧ�ĸ��ڵ�Ϊ0
            foreach (DataRow dr in drs)
            {
                string classid = dr["SysOrganizationID"].ToString();
                string classname = dr["Name"].ToString();
                //����������ʾ��ʽ
                classname = "" + classname;

                drplist.Items.Add(new ListItem(classname, classid));
                int sonparentid = int.Parse(classid);
                string blank = "��---";
                //�ݹ��ӷ��෽��
                BindNode(sonparentid, dt, blank, drplist);
            }
            drplist.DataTextField = "Name";
            drplist.DataValueField = "SysOrganizationID";
            drplist.DataBind();

        }
        //���ӷ���
        /// <summary>
        /// �ּ����Ӽ�����
        /// </summary>
        /// <param name="parentid">��ID</param>
        /// <param name="dt">���ݱ�</param>
        /// <param name="blank">�ָ���</param>
        /// <param name="drplist">DropDownList</param>
        public void BindNode(int parentid, DataTable dt, string blank, DropDownList drplist)
        {
            DataRow[] drs = dt.Select("FatherSysOrganizationID= " + parentid);

            foreach (DataRow dr in drs)
            {
                string classid = dr["SysOrganizationID"].ToString();
                string classname = dr["Name"].ToString();

                classname = blank + classname;
                drplist.Items.Add(new ListItem(classname, classid));

                int sonparentid = int.Parse(classid);
                string blank2 = blank + "��";

                BindNode(sonparentid, dt, blank2, drplist);//�ݹ����
            }
        }
        /// <summary>
        /// ����Sql��䷵�����ݱ�2�ֶ� ������ѡ��...��
        /// </summary>
        /// <param name="sqlStr">Sql</param>
        /// <returns>DataTable</returns>
        public static DataView createDDLView(string sqlStr)
        {
            OracleConnection conn = new OracleConnection(DBHelper.ConnectionString);
            conn.Open();
            OracleDataAdapter od = new OracleDataAdapter();
            DataSet ds = new DataSet();
            DataView seleView = new DataView();
            od.SelectCommand = new OracleCommand(sqlStr, conn);
            od.Fill(ds, "Table1");
            DataRow NewRow = ds.Tables["Table1"].NewRow();
            String Title = ds.Tables["Table1"].Columns[1].Caption;
            String TitleName = ds.Tables["Table1"].Columns[1].Caption;
            NewRow[0] = "0";
            NewRow[1] = " ��ѡ��....";
            //ds.Tables["Table1"].Rows.Add(NewRow);
            ds.Tables["Table1"].Rows.InsertAt(NewRow, 0);
            seleView = ds.Tables["Table1"].DefaultView;
            //seleView.Sort = "" + Title + "," + Title + " ASC";
            conn.Close();
            conn.Dispose();
            return seleView;
        }
        /// <summary>
        /// ����Sql��䷵�����ݱ�1�ֶ� ������ѡ��...��
        /// </summary>
        /// <param name="sqlstr">SQL</param>
        /// <returns></returns>
        public DataView createDDlDataView(string sqlstr)
        {
            OracleConnection conn = new OracleConnection(DBHelper.ConnectionString);
            conn.Open();
            OracleDataAdapter od = new OracleDataAdapter();
            DataSet ds = new DataSet();
            DataView seleView = new DataView();
            od.SelectCommand = new OracleCommand(sqlstr, conn);
            od.Fill(ds, "Table1");
            DataRow NewRow = ds.Tables["Table1"].NewRow();
            String Title = ds.Tables["Table1"].Columns[0].Caption;
            NewRow[0] = " ��ѡ��....";
            ds.Tables["Table1"].Rows.Add(NewRow);
            seleView = ds.Tables["Table1"].DefaultView;
            seleView.Sort = "" + Title + "," + Title + " ASC";
            conn.Close();
            conn.Dispose();
            return seleView;
        }
        /// <summary>
        /// ����GridView������Դ
        /// </summary>
        /// <param name="sqlStr">SQL ���</param>
        /// <returns></returns>
        public static DataView createGridView(string sqlStr)
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConnectionString);
                conn.Open();
                OracleDataAdapter od = new OracleDataAdapter();
                DataSet ds = new DataSet();
                DataView seleView = new DataView();
                od.SelectCommand = new OracleCommand(sqlStr, conn);
                od.Fill(ds, "Table1");
                seleView = ds.Tables["Table1"].DefaultView;
                conn.Close();
                conn.Dispose();
                return seleView;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// �������ݿ��ѯ���������н������Object����ʽ����
        /// </summary>
        /// <param name="sqlStr">SQL ���</param>
        /// <returns></returns>
        public static Object getObject(string sqlStr)
        {
            Object obj = null;
            OracleConnection conn = new OracleConnection(ConnectionString);
            conn.Open();
            OracleCommand comm = new OracleCommand(sqlStr, conn);
            obj = comm.ExecuteScalar();
            conn.Close();
            conn.Dispose();
            return obj;
        }
        /// <summary>
        /// �������ݼ�
        /// </summary>
        /// <param name="sqlStr">sql ���</param>
        /// <returns>�������ݼ�</returns>
        public static DataSet createDataset(string sqlStr)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(ConnectionString))
                {                    
                    OracleDataAdapter od = new OracleDataAdapter();
                    DataSet ds = new DataSet();
                    od.SelectCommand = new OracleCommand(sqlStr, conn);
                    od.Fill(ds, "Table1");                    
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// �������ݼ�
        /// </summary>
        /// <param name="sqlStr">sql ���</param>
        /// <returns>�������ݼ�</returns>
        public static DataSet GetDataset(string sqlStr)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(ConnectionString))
                {
                    OracleDataAdapter od = new OracleDataAdapter();
                    DataSet ds = new DataSet();
                    od.SelectCommand = new OracleCommand(sqlStr, conn);
                    od.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// �������ݿ�������
        /// </summary>
        /// <param name="sqlStr">����</param>
        /// <returns></returns>
        public static int getCount(string sqlStr)
        {
            OracleConnection conn = new OracleConnection(ConnectionString);
            conn.Open();
            OracleCommand comm = new OracleCommand(sqlStr, conn);
            int cnt = Convert.ToInt32(comm.ExecuteScalar());
            conn.Close();
            conn.Dispose();
            return cnt;
        }
    }

}

