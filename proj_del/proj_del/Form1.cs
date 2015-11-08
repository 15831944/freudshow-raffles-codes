using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.IO;
namespace proj_del
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            FileInfo finfo = new FileInfo("\\log.txt");
            FileStream fs = finfo.OpenWrite();
            for (int i = 0; i < 130; i++)
            {
                if (IsRefD(Convert.ToInt32(this.textBox1.Text)))
                {
                    
                    
                        //�������洴�����ļ�������д������  
                        StreamWriter w = new StreamWriter(fs);
                        //����д����������ʼλ��Ϊ�ļ�����ĩβ  
                        w.BaseStream.Seek(0, SeekOrigin.End);
                        //д�롰Log Entry : ��  
                        w.Write("\nLog Entry : ");
                        //д�뵱ǰϵͳʱ�䲢����  
                        w.Write(
                            "{0} {1} \r\n",
                            DateTime.Now.ToLongTimeString(),
                            DateTime.Now.ToLongDateString());
                        //д����־���ݲ�����  
                        w.Write("found ! " + i);
                        //д��----------------��������  
                        w.Write("------------------\n");
                        //��ջ��������ݣ����ѻ���������д�������  
                        w.Flush();
                        //�ر�д������  
                        w.Close();                 
                    return;
                }
                else
                    MessageBox.Show("Not found !" + i);
            }

        }

        /// <summary>
        /// ����PROJECT_TAB�е���Ŀ���Ƿ�����
        /// </summary>
        /// <param name="id"></param>
        public static bool IsRefD(int prj_id)
        {
            try
            {
                /*
                 * �õ����ù�PROJECT_ID�ֶεı�(����ÿ������ֶ�����������PROJECT_ID����)
                 */
                OracleConnection OraCon = new OracleConnection("Data Source=oidsnew;User ID=plm;Password=123!feed;Unicode=True");
                OraCon.Open();

                OracleDataAdapter OrclPrjAdapter = new OracleDataAdapter();
                OracleCommand OrclPrjCmd = OraCon.CreateCommand();
                OrclPrjAdapter.SelectCommand = OrclPrjCmd;
                OrclPrjCmd.CommandText = @"SELECT T.TABLE_NAME,T.COLUMN_NAME FROM USE_PROJECTID_TABLES_VIEW T";
                DataSet Mydata = new DataSet();
                OrclPrjAdapter.Fill(Mydata);
                /*
                 * ����ÿ����
                 * {
                 *      ���е�PROJECT_ID���������в����Ƿ������Ŀ��
                 *      ����ҵ�����Ŀ�ţ��򷵻� true
                 *      ���û�У������������һ����
                 * }
                 * Ĭ�Ϸ��� false
                 */
                DataSet tmpData = new DataSet();
                string QueryPrjIdCmdStr = string.Empty;
                for (int i = 0; i < Mydata.Tables[0].Rows.Count; i++)
                {
                    QueryPrjIdCmdStr = @"SELECT T." + Mydata.Tables[0].Rows[i][1] + " FROM " + Mydata.Tables[0].Rows[i][0] + " T WHERE TO_CHAR(T." + Mydata.Tables[0].Rows[i][1] + ")=TO_CHAR(" + prj_id+")";
                    OrclPrjCmd.CommandText = QueryPrjIdCmdStr;
                    OrclPrjAdapter.Fill(tmpData);
                    if (tmpData.Tables[0].Rows.Count > 0)
                        return true;
                }

                return false;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                return true;
            }
        }
    }
}