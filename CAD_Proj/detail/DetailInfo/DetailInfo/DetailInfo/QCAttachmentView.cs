using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Collections;
using System.IO;

namespace DetailInfo
{
    public partial class QCAttachmentView : Form
    {
        public QCAttachmentView()
        {
            InitializeComponent();
            for (int i = 0; i < DeleteContextMenuStrip.Items.Count; i++)
            {
                DeleteContextMenuStrip.Items[i].Visible = false;
            }
        }
        public string namestr;

        public string Namestr
        {
            get { return namestr; }
            set { namestr = value; }
        }

        private void QCAttachmentView_Load(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string sql = "select FILENAME as �ļ��� from SPLATTACHMENT_TAB where id in (select id  from SPLINATT_TAB where spoolname = '"+namestr+"')";
            User.DataBaseConnect(sql,ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                FileListBox.Items.Add(dr["�ļ���"]);
            }
            if (FileListBox.Items.Count != 0)
            {
                this.DeleteContextMenuStrip.Items[0].Visible = true;
                this.DeleteContextMenuStrip.Items[1].Visible = true;
                this.DeleteContextMenuStrip.Items[2].Visible = true;
            }

        }

        /// <summary>
        /// �򿪸���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileListBox_DoubleClick(object sender, EventArgs e)
        {
            if (FileListBox.Items.Count != 0)
            {
                string filestr = FileListBox.SelectedItem.ToString();
                OracleDataReader dr = null;
                OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select uploadfile from SPLATTACHMENT_TAB where filename = '" + filestr + "'";
                dr = cmd.ExecuteReader();
                byte[] File = null;
                try
                {
                    if (dr.Read())
                    {
                        File = (byte[])dr[0];
                    }

                    string str = System.Environment.CurrentDirectory;
                    FileStream fs = new FileStream(filestr, FileMode.OpenOrCreate);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(File, 0, File.Length);
                    System.Diagnostics.Process.Start(str + "\\" + filestr);
                    bw.Close();
                    fs.Close();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show( ex.Message.ToString() + "ϵͳû�в�ѯ������ļ�");
                    return;
                }
            }
            else
            {
                //this.DeleteContextMenuStrip.Visible = false;
                return;
            }

        }

 
        /// <summary>
        /// �رո����б�ɾ����ȡ�����ص��ļ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QCAttachmentView_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (int i = 0; i < FileListBox.Items.Count; i++)
            {
                string filepath = User.rootpath + "\\" + FileListBox.Items[i].ToString();
                if(System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
            }

        }

        /// <summary>
        /// ɾ�����ϴ��ĸ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int total = 0;
            string filestr = this.FileListBox.SelectedItem.ToString();
            string user_name = User.cur_user.ToString();
            total = DBConnection.GetSpoolAttechmentCount(filestr, user_name);
            if (total == 0)
            {
                MessageBox.Show("��û��Ȩ��ɾ�����ĵ���", "WARNNING", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            else
            {
                DialogResult result;
                result = MessageBox.Show("ȷ��Ҫɾ���ø�����", "����ɾ��", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    
                    DBConnection.DeletSpoolAttachment(user_name, filestr);
                    FileListBox.Items.Remove(filestr);

                }
            }
        }

        /// <summary>
        /// �����ݿⱣ�渽��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filestr = FileListBox.SelectedItem.ToString();
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = filestr;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                OracleDataReader dr = null;
                OracleConnection conn = new OracleConnection(DataAccess.OIDSConnStr);
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select uploadfile from SPLATTACHMENT_TAB where filename = '" + filestr + "'";
                dr = cmd.ExecuteReader();
                byte[] File = null;
                if (dr.Read())
                {
                    File = (byte[])dr[0];
                }

                FileStream fs = new FileStream(filestr, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(File, 0, File.Length);
                bw.Close();
                fs.Close();
                conn.Close();
                MessageBox.Show(this.FileListBox.SelectedItem.ToString() + "  �ļ��������!!");
                
                
            }
        }


    }
}