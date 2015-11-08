using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DetailInfo.Application_Code;
using System.Threading;
using System.Diagnostics;
using System.IO;
using UpdateSoft;
namespace DetailInfo
{
    public partial class LoginForm : Form
    {
        public string LoginUserName = string.Empty;

        public LoginForm()
        {
            InitializeComponent();

            this.tbUserName.Text = XmlOper.getXMLContent("Name");

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            string userName = tbUserName.Text.ToLower();
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("��¼������Ϊ�գ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbUserName.Focus();
                return ;
            }

            string passWord = tbPassword.Text;
            if (string.IsNullOrEmpty(passWord))
            {
                MessageBox.Show("��¼���벻��Ϊ�գ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbPassword.Focus();
                return ;
            }

            string dbsever = string.Empty;
            if (this.comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("�������ݿ����Ա��ϵ�������ݿ�����", "���ݿ���������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (this.comboBox1.SelectedItem.ToString().Trim()=="��ʽ��")
                {
                    dbsever = "OIDS";
                }
                else if (this.comboBox1.SelectedItem.ToString().Trim()=="���Կ�")
                {
                    dbsever = "OIDSNEW";
                }
                DataAccess.GetSeverName(dbsever);
                Framework.DataAccess.GetSeverName(dbsever);
            }

            bool loginState = User.Verify(userName, passWord);

            if (loginState)
            {
                LoginUserName = userName;
                XmlOper.setXML("Name", userName);
                this.DialogResult = DialogResult.OK;        
            }
            else
            {
                MessageBox.Show("��¼�����������","СƱ��ѯ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            string[] OTNS;
            OTNS = GetOracleTnsNames.GetOTNames();
            if (OTNS.Length == 0)
            {
                MessageBox.Show("�������ݿ����Ա��ϵ�������ݿ�����", "���ݿ���������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                for (int i = 0; i < OTNS.Length; i++)
                {
                    if (OTNS[i].Trim() == "OIDS")
                    {
                        comboBox1.Items.Add("��ʽ��");
                    }
                    else if (OTNS[i].Trim() == "OIDSNEW")
                    {
                        comboBox1.Items.Add("���Կ�");
                    }
                }
                if (this.comboBox1.Items.Contains("��ʽ��"))
                {
                    this.comboBox1.SelectedItem = "��ʽ��";
                }
            }
        }


    }
}