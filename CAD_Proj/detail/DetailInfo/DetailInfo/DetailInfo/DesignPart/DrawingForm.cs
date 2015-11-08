using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OracleClient;
using System.Collections;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DetailInfo
{
    public partial class DrawingForm : Form
    {
        public DrawingForm()
        {
            InitializeComponent();
            this.toolStripStatusLabel2.Text = null;
            for (int i = 0; i < DetailMStrip.Items.Count; i++)
            {
                DetailMStrip.Items[i].Visible = false;
            }
        }

        private string projectid;

        public string Projectid
        {
            get { return projectid; }
            set { projectid = value; }
        }

        private string drawing;

        public string Drawing
        {
            get { return drawing; }
            set { drawing = value; }
        }

        private int indicator;

        public int Indicator
        {
            get { return indicator; }
            set { indicator = value; }
        }

        private void DrawingForm_Load(object sender, EventArgs e)
        {
            this.pidtb.Text = projectid;
            if (drawing != string.Empty)
            {
                string[] drawingno = drawing.Split(new char[] {','});

                for (int i = 0; i < drawingno.Length - 1; i++)
                {
                    this.DRAWINGNOcomboBox.Items.Add(drawingno[i].ToString());
                }
            }
            foreach (TabPage tp in this.tabControl1.TabPages)
            {
                if (tp.Text == "�������ϱ�" || tp.Text == "���������豸�����" || tp.Text == "��ϵ���̱�" || tp.Text == "��ϵ���ϱ�" || tp.Text == "��ϵ�����豸�����")
                {
                    this.tabControl1.TabPages.Remove(tp);
                }
            }


            if (indicator == 1)
            {
                foreach ( TabPage tp in  this.tabControl1.TabPages )
                {
                    if (tp.Text == "ͼֽ����")
                    {
                        this.tabControl1.TabPages.Remove(tp);
                    }
                }
            }
            SetStatus();
            this.DRAWINGNOcomboBox.SelectedIndex = 0;
        }

        private void SetStatus()
        {
            int count = 0;

            if (this.tabControl1.SelectedTab.Text == "СƱ��Ϣ")
            {
                count = this.Appdgv.Rows.Count;
            }
            else if (this.tabControl1.SelectedTab.Text == "������Ϣ")
            {
                count = this.Materialdgv.Rows.Count;
            }
            else if (this.tabControl1.SelectedTab.Text == "������Ϣ")
            {
                count = this.Partdgv.Rows.Count;
            }

            else if (this.tabControl1.SelectedTab.Text == "�������ϱ�")
            {
                count = this.dataGridView1.Rows.Count;
            }

            else if (this.tabControl1.SelectedTab.Text == "���������豸�����")
            {
                count = this.dataGridView2.Rows.Count;
            }

            else if (this.tabControl1.SelectedTab.Text == "��ϵ���̱�")
            {
                count = this.dataGridView3.Rows.Count;
            }

            else if (this.tabControl1.SelectedTab.Text == "��ϵ���ϱ�")
            {
                count = this.dataGridView4.Rows.Count;
            }

            this.toolStripStatusLabel1.Text = string.Format(" ��ǰ�ܼ�¼����{0}��", count);
            
        }

        private void GetSelectionRowCount(DataGridView dgv)
        {
            int count = dgv.SelectedRows.Count;
            this.toolStripStatusLabel2.Text = string.Format("��ǰѡ��{0}��", count);
        }

        #region ѡ��������ʾ�仯
        private void Appdgv_SelectionChanged(object sender, EventArgs e)
        {
            GetSelectionRowCount(this.Appdgv);
            int j = this.Appdgv.SelectedRows.Count;
            if (j > 0)
            {
                for (int i = 0; i < this.DetailMStrip.Items.Count; i++)
                {
                    DetailMStrip.Items[i].Visible = true;
                }
            }
            else
            {
                for (int i = 0; i < DetailMStrip.Items.Count; i++)
                {
                    DetailMStrip.Items[i].Visible = false;
                }
            }
        }

        private void Materialdgv_SelectionChanged(object sender, EventArgs e)
        {
            GetSelectionRowCount(this.Materialdgv);
        }

        private void Partdgv_SelectionChanged(object sender, EventArgs e)
        {
            GetSelectionRowCount(this.Partdgv);
        }

        #endregion

        private void Appdgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            SpoolCellFormat.FormatCell(Appdgv);
        }

        /// <summary>
        /// ����ͼֽ���沢���뵽���ݿ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void requestbtn_Click(object sender, EventArgs e)
        {
            string comText = string.Empty;
            string btntext = this.requestbtn.Text.ToString();
            string pid = this.pidtb.Text.ToString();
            object drawno = this.DRAWINGNOcomboBox.SelectedItem;
            if (drawno == null)
            {
                return;
            }
            else
            {
                switch (btntext)
                {
                    case "���ɷ���":
                        string sqlstr = "SELECT count(*) FROM SP_CREATEPDFDRAWING T WHERE T.PROJECTID = '"+pid+"' AND T.DRAWINGNO = '"+drawno+"'";
                        object count = User.GetScalar1(sqlstr,DataAccess.OIDSConnStr);
                        if ( Convert.ToInt16(count) == 0)
                        {
                            comText = "INSERT INTO SP_CREATEPDFDRAWING (PROJECTID, DRAWINGNO, FRONTPAGE) VALUES ('" + pid + "', '" + drawno + "', :dfd)";
                            InsertFrontPage.GenerateFrontPage(comText);

                        }
                        else
                        {
                            DialogResult result = MessageBox.Show("��ͼֽ�����Ѵ��ڣ�ȷ��Ҫ�������ɣ�", "WARNNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                            if (result == DialogResult.OK)
                            {
                                comText = "UPDATE SP_CREATEPDFDRAWING SET FRONTPAGE = :dfd WHERE PROJECTID = '" + pid + "' AND DRAWINGNO = '" + drawno + "'";
                                InsertFrontPage.GenerateFrontPage(comText);
                            }
                        }

                        break;
                    default:
                        break;
                }
            }
        }

        private void DRAWINGNOcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RightClick.ProjectDrawingDetails_TabHop(indicator, this.pidtb, this.DRAWINGNOcomboBox, this.tabControl1, Appdgv, Materialdgv, Partdgv, axAcroPDF1, axAcroPDF2, dataGridView1, dataGridView2, dataGridView3, dataGridView4, dataGridView5, axAcroPDF3, Connectordgv);
            SetStatus();
        }

        private void DrawingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDIForm.tool_strip.Items[1].Enabled = false;
            MDIForm.treeview.Refresh();
            try
            {
                string root = User.rootpath + "\\" + "temp";
                DirectoryInfo dir = new DirectoryInfo(root);
                if (dir.Exists)
                {
                    System.IO.Directory.Delete(root, true);
                }
                else
                {
                    return;
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RightClick.ProjectDrawingDetails_TabHop(indicator, this.pidtb, this.DRAWINGNOcomboBox, this.tabControl1, Appdgv, Materialdgv, Partdgv, axAcroPDF1, axAcroPDF2, dataGridView1, dataGridView2, dataGridView3, dataGridView4, dataGridView5, axAcroPDF3, Connectordgv);
            SetStatus();
            if (this.tabControl1.SelectedIndex >= 3)
            {
                this.toolStripStatusLabel2.Text = string.Empty;
            }
        }

        private void ��·������ϢToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowcount = this.Appdgv.SelectedRows.Count;
            if (rowcount > 0)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Appdgv.Rows.Count; i++)
                {
                    if (Appdgv.Rows[i].Selected == true)
                    {
                        string spoolname = Appdgv.Rows[i].Cells["SPOOLNAME"].Value.ToString();
                        sb.Append("'" + spoolname + "'" + ",");
                    }
                }
                string str = sb.Remove(sb.Length - 1, 1).ToString();
                DataSet ds = new DataSet();
                string sql = "select t.projectid ��Ŀ��, t.spoolname СƱ��, t.erpcode ERP�������, t.materialname �����ͺ�, t.logname ¼����, t.logdate ¼������ from SP_SPOOLMATERIAL_TAB t where t.materialname like '%��%' and t.flag = 'Y' and t.spoolname in (" + str + ")";
                foreach (Form form in MDIForm.pMainWin.MdiChildren)
                {
                    if (form.Text == "��·������Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).DataSource = null;
                        User.DataBaseConnect(sql, ds);
                        ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).DataSource = ds.Tables[0].DefaultView;
                        int i = ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).Rows.Count;
                        ((ToolStrip)(MDIForm.pMainWin.ActiveMdiChild.Controls["statusStrip1"])).Items["toolStripStatusLabel1"].Text = string.Format(" ��ǰ������������{0}��", i);
                        ds.Dispose();
                        return;
                    }
                }
                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "��·������Ϣ";
                sgvf.MdiParent = MDIForm.pMainWin;
                sgvf.Show();
                User.DataBaseConnect(sql, ds);
                ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).DataSource = ds.Tables[0].DefaultView;
                ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                int count = ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).Rows.Count;
                ((ToolStrip)(MDIForm.pMainWin.ActiveMdiChild.Controls["statusStrip1"])).Items["toolStripStatusLabel1"].Text = string.Format(" ��ǰ������������{0}��", count);
                ds.Dispose();
            }
            else
            {
                return;
            }
        }

        private void ��·������ϢToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowcount = this.Appdgv.SelectedRows.Count;

            if (rowcount > 0)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Appdgv.Rows.Count; i++)
                {
                    if (Appdgv.Rows[i].Selected == true)
                    {
                        string spoolname = Appdgv.Rows[i].Cells["SPOOLNAME"].Value.ToString();
                        sb.Append("'" + spoolname + "'" + ",");
                    }
                }
                string str = sb.Remove(sb.Length - 1, 1).ToString();
                DataSet ds = new DataSet();
                string sql = "select t.projectid ��Ŀ��, t.spoolname СƱ��, t.erpcode ERP�������, t.materialname �����ͺ�, t.logname ¼����, t.logdate ¼������ from SP_SPOOLMATERIAL_TAB t where t.materialname not like '%��%' and t.flag = 'Y' and t.spoolname in (" + str + ")";
                foreach (Form form in MDIForm.pMainWin.MdiChildren)
                {
                    if (form.Text == "��·������Ϣ")
                    {
                        if (form.WindowState == FormWindowState.Minimized)
                        {
                            form.WindowState = FormWindowState.Normal;
                        }
                        form.Activate();
                        ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).DataSource = null;
                        User.DataBaseConnect(sql, ds);
                        ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).DataSource = ds.Tables[0].DefaultView;
                        int i = ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).Rows.Count;
                        ((ToolStrip)(MDIForm.pMainWin.ActiveMdiChild.Controls["statusStrip1"])).Items["toolStripStatusLabel1"].Text = string.Format(" ��ǰ������������{0}��", i);
                        ds.Dispose();
                        return;
                    }
                }
                SpoolGeneralViewForm sgvf = new SpoolGeneralViewForm();
                sgvf.Text = "��·������Ϣ";
                sgvf.MdiParent = MDIForm.pMainWin;
                sgvf.Show();
                User.DataBaseConnect(sql, ds);
                ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).DataSource = ds.Tables[0].DefaultView;
                ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                int count = ((DataGridView)(MDIForm.pMainWin.ActiveMdiChild.Controls["OverViewdgv"])).Rows.Count;
                ((ToolStrip)(MDIForm.pMainWin.ActiveMdiChild.Controls["statusStrip1"])).Items["toolStripStatusLabel1"].Text = string.Format(" ��ǰ������������{0}��", count);
                ds.Dispose();
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// ֱ�Ӵ򿪹�·СƱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void �鿴СƱtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            string spoolstr = this.Appdgv.CurrentRow.Cells["SPOOLNAME"].Value.ToString();
            DataSet ds = new DataSet();
            string sqlstr = @"select t.pdfpath from sp_pdf_tab t where t.spoolname = '"+spoolstr+"' and t.flag = 'Y' ";
            User.DataBaseConnect(sqlstr, ds);
            if (ds.Tables[0].Rows.Count<1)
            {
                MessageBox.Show("Ŀ��洢��ַû�д�СƱ�ļ���Ϣ,�������Ա��ϵ��", "WARNNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                ds.Dispose();
                return;
            }
            string pathstr = ds.Tables[0].Rows[0][0].ToString();
            System.Diagnostics.Process.Start(pathstr);
            ds.Dispose();

        }

        private void ����Excel��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User.ExportToTxt(this.Appdgv, progressBar1);
        }

        private void DrawingForm_Activated(object sender, EventArgs e)
        {
            MDIForm.tool_strip.Items[0].Enabled = false;
            MDIForm.tool_strip.Items[1].Enabled = true;
            MDIForm.tool_strip.Items[2].Enabled = true;
        }

        [System.Runtime.InteropServices.DllImport("ole32.dll")]
        static extern void CoFreeUnusedLibraries();

        private void DrawingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.axAcroPDF1 != null)
            {
                axAcroPDF1.Dispose();
                System.Windows.Forms.Application.DoEvents();
                CoFreeUnusedLibraries();
            }
            if (this.axAcroPDF2 != null)
            {
                axAcroPDF2.Dispose();
                System.Windows.Forms.Application.DoEvents();
                CoFreeUnusedLibraries();
            }
            if (this.axAcroPDF3 != null)
            {
                axAcroPDF3.Dispose();
                System.Windows.Forms.Application.DoEvents();
                CoFreeUnusedLibraries();
            }
        }

        private void Appdgv_MouseUp(object sender, MouseEventArgs e)
        {
            int selectcount = this.Appdgv.SelectedRows.Count;
            if (selectcount > 1)
            {
                this.DetailMStrip.Items["�鿴СƱtoolStripMenuItem"].Enabled = false;
            }
            else
            {
                this.DetailMStrip.Items["�鿴СƱtoolStripMenuItem"].Enabled = true;
            }
        }





    }
}