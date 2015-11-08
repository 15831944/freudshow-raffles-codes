using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.OracleClient;

namespace CopyPhoto
{
    public partial class Form_copy : Form
    {
        public Form_copy()
        {
            InitializeComponent();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f1 = new FolderBrowserDialog();
            if (f1.ShowDialog() == DialogResult.OK)
            {
                this.textPath.Text = f1.SelectedPath;
            }
        }

        private void btn_CopyByIC_Click(object sender, EventArgs e)
        {
            string ICCard, Name;
            string mPath = this.textPath.Text;
            DirectoryInfo di;

            try
            {
                di = new DirectoryInfo(mPath);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            FileInfo[] filelist = di.GetFiles("*.jpg");
            foreach (FileInfo fi in filelist)
            {
                ICCard = fi.Name.Split('.')[0];
                DataTable ManListInfo;
                OracleHelper newhelp = new OracleHelper();
                string sql = @"SELECT T.NAME, T.USED_NAME, T.CREATE_DATE 
                                FROM cimc_lbr_idcard_read_trans_vie T 
                                WHERE T.USED_NAME='" + ICCard + "'";

                try
                {
                    ManListInfo = newhelp.GetDataTable(sql);
                }
                catch (System.Exception ex)
                {
                    continue;
                }
                Name = ManListInfo.Rows[0]["Name"].ToString();
                if (Name == string.Empty)
                {
                    continue;
                }

                if (!Directory.Exists(mPath + "\\NAME"))
                {
                    Directory.CreateDirectory(mPath + "\\NAME");
                }

                if (File.Exists(mPath + "NAME\\" + Name + ".jpg"))
                {
                    try
                    {
                        File.Copy(fi.Name, mPath + "NAME\\" + Name + "_" + ICCard + ".jpg", true);
                        continue;
                    }
                    catch (System.Exception err)
                    {
                        continue;
                    }
                }

                try
                {
                    File.Copy(fi.Name, mPath + "NAME\\" + Name + ".jpg", true);
                }
                catch (System.Exception ex)
                {
                    continue;
                }
            }

            MessageBox.Show("�������");
        }
    }

    public class OracleHelper
    {
        /// <summary>
        /// ORACLE�����ַ���
        /// </summary>
        public static string OraConnectString
        {
            get
            {
                return "Data Source=HDMS;User ID=hrdemo;Password=hrdemo";
            }
        }
        /// <summary>
        /// OLEDB�����ַ���
        /// </summary>
        public static string OleConnectString
        {
            get
            {
                return "Provider=MSDAORA;Data Source=HDMS;Password=hrdemo;User ID=hrdemo";
            }
        }
        /// <summary>
        /// �������ݼ���
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string SQL)
        {
            OracleConnection conn = new OracleConnection(OraConnectString);
            try
            {
                OracleDataAdapter Oda = new OracleDataAdapter();
                DataSet ds = new DataSet();
                Oda.SelectCommand = new OracleCommand(SQL, conn);
                Oda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// �������ݱ�
        /// </summary>
        /// <param name="SQL">SQL���</param>
        /// <returns></returns>
        public DataTable GetDataTable(string SQL)
        {
            return GetDataSet(SQL).Tables[0];
        }
        /// <summary>
        /// ִ��һ�������ѯ�����䣬���ز�ѯ�������������
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static int GetCount(string strSQL)
        {
            using (OleDbConnection connection = new OleDbConnection(OleConnectString))
            {
                OleDbCommand cmd = new OleDbCommand(strSQL, connection);
                try
                {
                    connection.Open();
                    OleDbDataReader result = cmd.ExecuteReader();
                    int i = 0;
                    while (result.Read())
                    {
                        i = result.GetInt32(0);
                    }
                    result.Close();
                    return i;
                }
                catch (OleDbException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// ִ��һ�������ѯ�����䣬���ز�ѯ�����object����
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static object GetSingle(string SQLString)
        {
            using (OleDbConnection connection = new OleDbConnection(OleConnectString))
            {
                OleDbCommand cmd = new OleDbCommand(SQLString, connection);
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (OleDbException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// ִ�в�ѯ��䣬����SqlDataReader
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static OleDbDataReader ExecuteReader(string strSQL)
        {
            using (OleDbConnection connection = new OleDbConnection(OleConnectString))
            {
                OleDbCommand cmd = new OleDbCommand(strSQL, connection);
                OleDbDataReader myReader;
                try
                {
                    connection.Open();
                    myReader = cmd.ExecuteReader();
                    return myReader;
                }
                catch (OleDbException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// ִ�в�ѯ��䣬����DataSet
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static DataSet Query(string SQLString)
        {
            using (OleDbConnection connection = new OleDbConnection(OleConnectString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OleDbDataAdapter command = new OleDbDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (OleDbException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }

        }
        /// <summary>
        /// ִ�в�ѯ��䣬����DataView
        /// </summary>
        /// <param name="SQLString">��ѯ���</param>
        /// <returns></returns>
        public static DataView QuerySQL(string SQLString)
        {
            using (OleDbConnection connection = new OleDbConnection(OleConnectString))
            {
                DataSet ds = new DataSet();
                DataView view = new DataView();
                try
                {
                    connection.Open();
                    OleDbDataAdapter command = new OleDbDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                    view = ds.Tables["ds"].DefaultView;
                }
                catch (OleDbException ex)
                {
                    throw new Exception(ex.Message);
                }
                return view;
            }
        }

        public static DataView Query_(string SQLString)
        {
            return Query(SQLString).Tables[0].DefaultView;
        }
    }
}