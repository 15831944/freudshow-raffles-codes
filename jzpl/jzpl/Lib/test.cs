using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Net;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
namespace jzpl.Lib
{
    public class TimeRun1
    {
        //#region ���ݿ����
        //public string dbType = "0";
        //public SqlConnection conn = new SqlConnection("server=.;uid=sa;pwd=123;database=DBCT_Dev");
      
        //public SqlCommand cmd;
        //public SqlDataAdapter da;
        //�ر�������Դ
        //public void Reset()
        //{
        //    cmd.Dispose();
        //    da.Dispose();
        //    conn.Dispose();
        //    conn.Close();
        //}
        // <summary>
        // ���ݴ������䣬�õ�DataSet
        // </summary>
        // <param name="strsql"></param>
        // <returns></returns>
        //public DataSet GetDataSet(string strsql)
        //{
        //    this.Reset();
        //    cmd = new SqlCommand("", conn);
        //    cmd.CommandText = strsql;
        //    DataSet ds = new DataSet();
        //    da = new SqlDataAdapter();
        //    da.SelectCommand = cmd;
        //    da.Fill(ds);
        //    return ds;
        //}
        // <summary>
        // ���ݴ������䣬�õ�DataTable
        // </summary>
        // <param name="strsql"></param>
        // <returns></returns>
        //public DataTable GetDataTable(string strsql)
        //{
        //    DataSet ds = GetDataSet(strsql);
        //    return ds.Tables[0];
        //}
        // <summary>
        // ���ִ��ɾ�������룬�޸Ĳ���.
        // </summary>
        // <param name="strsql"></param>
        // <returns></returns>
        //public bool ExecuteSql(string strsql)
        //{
        //    this.Reset();
        //    conn.Close();
        //    conn.Open();
        //    cmd = new SqlCommand("", conn);
        //    cmd.CommandText = strsql;
        //    return cmd.ExecuteNonQuery() != 0;
        //}
        // <summary>
        // ���ݴ�����䣬ȡ����Ӧ������һ�㴫��ֵΪselect count(*) from table
        // </summary>
        // <param name="strsql"></param>
        // <returns></returns>
        //public int ExecuteScalarSql(string strsql)
        //{
        //    conn.Open();
        //    cmd = new SqlCommand("", conn);
        //    cmd.CommandText = strsql;
        //    return Convert.ToInt32(cmd.ExecuteScalar());
        //    conn.Close();
        //}
        // <summary>
        // ���ݴ�������ֵ��ȡ��Ӧ���ֶ�
        // </summary>
        // <param name="GetTable">��Ҫ��ѯ�ı�</param>
        // <param name="GetValue">��Ҫ�õ����ֶ�ֵ</param>
        // <param name="Wherestr">�������</param>
        // <returns></returns>
        //public string GetSingleValue(string GetTable, string GetValue, string Wherestr)
        //{
        //    string strsql = "select " + GetValue + " from " + GetTable + " where " + Wherestr + "";
        //    DataTable dt = this.GetDataTable(strsql);
        //    if (dt.Rows.Count > 0)
        //        return dt.Rows[0][0].ToString();
        //    else
        //        return "����Ϣ";
        //}
        //#endregion
        //private static ManualResetEvent allDone = new ManualResetEvent(false);
        //public static string singleip;
        
        //public void getPost(string id)
        //{
        //    singleid ������û���ϢID
        //    singleip �����û���ϢID����ȡ����IP
        //    strAction post�ش�ֵ֮һ
        //    strCode�������û���Ϣ���е�codeֵ
        //    string singleid = id;
        //    ͨ��GetSingleValue�������õ���Ӧ��ֵ������GetSingleValue��������Ҫ�õ���ֵ��������䣩
        //    singleip = GetSingleValue("UserCode", "UserIP", "id=" + singleid).Trim();
        //    ����HttpWebRequest
        //    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://www2.ip138.com/ips8.asp");
        //    myRequest.Method = "POST";
        //    myRequest.ContentType = "application/x-www-form-urlencoded";
        //    ��ʼ�����첽����
        //    myRequest.BeginGetRequestStream(new AsyncCallback(ReadCallback), myRequest);
        //    allDone.WaitOne();
        //    �첽�ص�����ʹ�� EndGetResponse ��������ʵ�ʵ� WebResponse�� 
        //    HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();
        //    Stream streamResponse = response.GetResponseStream();
        //    StreamReader streamRead = new StreamReader(streamResponse, Encoding.Default);
        //    string content = streamRead.ReadToEnd();
        //    streamResponse.Close();
        //    streamRead.Close();
        //    response.Close();
        //    �ж��Ƿ�����ʡ�е���Ϣ�������Ϣ����ʡ���У�����д������ݿ⡣������Ϣ����Ϊ0035��������
        //    string singleip = "";
        //    string strCode = "";
        //    if (singleip == "127.0.0.1")
        //    {
        //        strCode = "0008";
        //    }
        //    else
        //    {
        //        if (content.IndexOf("ʡ") == -1 && content.IndexOf("��") == -1 && singleid != "127.0.0.1")
        //        {
        //            if (content.IndexOf("��ѯ̫Ƶ��") != -1)
        //            {
        //                strCode = "��ѯ̫Ƶ��";
        //            }
        //            else
        //            {
        //                strCode = "0035";
        //            }
        //        }
        //        else
        //        {
        //            if (content.IndexOf("ʡ") != -1)
        //            {
        //                string con = content.Substring(content.IndexOf("��վ������") + 6, content.IndexOf("</li><li>�ο�����һ") - content.IndexOf("��վ������") - 1);
        //                string strpro = con.Substring(0, con.IndexOf("ʡ") + 1);
        //                strCode = GetSingleValue("S_Province", "ProvinceCode", "ProvinceName='" + strpro + "'").Trim();
        //                strCode = strpro;
        //            }
        //            if (content.IndexOf("��") != -1)
        //            {
        //                string con = content.Substring(content.IndexOf("��վ������") + 6, content.IndexOf("</li><li>�ο�����һ") - content.IndexOf("��վ������") - 1);
        //                string strpro = con.Substring(con.IndexOf("ʡ") + 1, con.IndexOf("��") - con.IndexOf("ʡ"));
        //                strCode += GetSingleValue("S_City", "ZipCode", "CityName='" + strpro + "'").Trim(); ;
        //            }
        //        }
        //    }
        //    if (strCode == "")
        //    {
        //        strCode = "0035";
        //    }
        //    ����Ϣ����hashtable
        //    string strsql = "update UserCode set Code='" + strCode + "' where id=" + id + "";
        //    try
        //    {
        //        ExecuteSql(strsql);
        //    }
        //    catch
        //    {
        //    }
        //}
        //private void ReadCallback(IAsyncResult asynchronousResult)
        //{
        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
        //        Stream postStream = request.EndGetRequestStream(asynchronousResult);
        //        ASCIIEncoding encoding = new ASCIIEncoding();
        //        string postData = "ip=" + singleip;
        //        postData += "&action=2";
        //        byte[] data = encoding.GetBytes(postData);
        //        postStream.Write(data, 0, postData.Length);
        //        postStream.Close();
        //        allDone.Set();
        //    }
        //    catch
        //    { return; }
        //}
        
    }
}