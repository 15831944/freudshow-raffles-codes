using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Bolt
{
    public partial class Form_Result : Form
    {
        public Form_Result()
        {
            InitializeComponent();
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btn_CopyTo_Click(object sender, EventArgs e)
        {
            string FinalText = string.Empty;
            FinalText += @"��˨��" + this.textBox_Bolt.Text + "\r\n" +
                         "��ĸ��" + this.textBox_Nut.Text + "\r\n" +
                         "ƽ�棺" + this.textBox_PWasher.Text ;
            if (!(this.textBox_SWasher.Text==string.Empty))
            {
                FinalText += "\r\n" + "���棺" + this.textBox_SWasher.Text;
            }

            Clipboard.SetDataObject(FinalText);
        }
    }
}