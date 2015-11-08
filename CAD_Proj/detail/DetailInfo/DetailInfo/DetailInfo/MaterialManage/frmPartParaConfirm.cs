using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Framework;
using System.Data.OleDb;
namespace DetailInfo
{
    public partial class frmPartParaConfirm : Form
    {
        string ProjectId, mmtypeid="", systemid="0",mSite;
        int ecprojectid;
        DataSet ds;
        string SubPro, YuguType, activity, LoginUser;
        public frmPartParaConfirm()
        {
            InitializeComponent();
            ProjectId = string.Empty;
            YuguType = "S";
            //string sysname= ProjectSystem.GetName(int.Parse(activity));
            
            //this.Text = this.Text + "-----" + sysname;
            if (ProjectId == string.Empty)
            {
                //ProjectId = NTypeTreeView.Str_Project;
                //SubPro = NTypeTreeView.ParentSubProjectId;
                //YuguType = NTypeTreeView.SubProjectId;
                //activity = NTypeTreeView.ActivityId;
            }
            LoginUser = User.cur_user;
            
        }
        
        private void frmPartParaList_Load(object sender, EventArgs e)
        {
            //ProjectCmbBind();
            //ȡ������������
            
            ecprojectid = ProjectSystem.FindProjectid(ProjectId);
            string sqlstr = "select typeid,type_desc from mm_part_type_tab where parent_id<>0";
            ProjectCmbItem.ProjectCmbBind(cmb_project);
            ProjectCmbItem.BindDiscipline(cmb_dpname);
            ProjectCmbItem.SiteCmbBind(cmb_site); 
            
            //QuerydataBindpara();

        }
        
        private void cmb_mtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void BindPartNobyAct()
        {
            cmb_partno.Items.Clear();
            cmb_partno.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_partno.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            if (activity == string.Empty) return;
            DataSet PartDS = PartType.PartNoData(activity,"07");
            DataRow row = PartDS.Tables[0].NewRow();
            row[0] = "";
            PartDS.Tables[0].Rows.InsertAt(row, 0);
            cmb_partno.DataSource = PartDS.Tables[0].DefaultView;
            cmb_partno.DisplayMember = "part_no";
            cmb_partno.ValueMember = "part_no";
            cmb_dpname.Items.Clear();
            cmb_dpname.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_dpname.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            DataSet PartNameDS = PartType.PartSpecData(activity,"07");
            DataRow rowName = PartNameDS.Tables[0].NewRow();
            rowName[0] = "";
            PartNameDS.Tables[0].Rows.InsertAt(rowName, 0);
            cmb_dpname.DataSource = PartNameDS.Tables[0].DefaultView;
            cmb_dpname.DisplayMember = "part_spec";
            cmb_dpname.ValueMember = "part_no";
        }

                
        public void QuerydataBind()
        {
            //string ProjectId = cmb_project.SelectedValue.ToString();
            if (YuguType == "M") mmtypeid = activity;
            if (YuguType == "S") systemid = activity;
            string site=cmb_site.SelectedValue.ToString();
            string partno=cmb_partno.Text.Trim().ToString().ToLower();
            string PartName = cmb_dpname.Text.Trim().ToString().ToLower();
            string parttype = tb_type.Text.Trim().ToString().ToLower();
            
            StringBuilder sb = new StringBuilder();
            //if (ProjectId != string.Empty) sb.Append(" AND PROJECTID = '" + ProjectId + "'");
            if (site != string.Empty) sb.Append(" AND CONTRACT = '" + site + "'");
            if (partno != string.Empty) sb.Append(" AND lower(part_no) like '%" + partno + "%'");
            if (PartName != string.Empty) sb.Append(" AND lower(part_spec) like '%" + PartName + "%'");
            if (parttype != string.Empty) sb.Append(" AND lower(part_type) like '%" + parttype + "%'");
            
            if (YuguType == "S")
            {
                if (mmtypeid!="0")
                sb.Append(" and parentid=" + mmtypeid);
                //sb.Append(" and systemid=" + activity);
            }
            if (YuguType == "M")
            {
                sb.Append(" and parentid=" + mmtypeid);
                //sb.Append(" and systemid=" + systemid);
            }
            //if(checkBox1.)
            //string sqlSelect = "SELECT '','',pp.*,'' FROM  PLM.MM_PART_TAB pp WHERE 1=1 and parentid= " + activity;
            string sqlSelect = "select t.ID ���,t.part_no �����,t.part_type ������,t.part_spec ������,t.part_mat ����,t.part_cert ֤��,t.part_unit ��λ,t.part_unitdensity ��λ�ܶ�,t.part_densityunit �ܶȵ�λ,t.part_level �ȼ�,t.parentid,t.contract ��" +
",'' as Ԥ����,'' as Ԥ��ϵ��,'' ���Ӽ� from mm_part_tab t WHERE 1=1 ";
            string wheresql = sb.ToString();
            sqlSelect = sqlSelect + wheresql + " order by t.parentid,t.part_type";
            XmlOper.setXML("Type", mmtypeid);
            listviewBind(sqlSelect);
        }
        public void listviewBind(string sql)
        {
            //���Ĳ������������Լ�����
            
            ds = PartParameter.QueryPartPara(sql);
            dgv1.DataSource = ds.Tables[0].DefaultView;
            DataGridViewComboBoxColumn dgvcom = new DataGridViewComboBoxColumn();
            DataSet blockds = PartParameter.QueryPartPara("select block_id,description from project_block_tab where project_id=" + ecprojectid.ToString()+" order by description");
            DataRow rowdim = blockds.Tables[0].NewRow();
            rowdim[0]=1;
            blockds.Tables[0].Rows.InsertAt(rowdim, 0);
            dgvcom.DataSource = blockds.Tables[0].DefaultView;
            dgvcom.DisplayMember = "description";
            dgvcom.ValueMember = "description";
            dgvcom.HeaderText="�ֶ�";
            dgvcom.Name = "�ֶ�";
            dgvcom.ReadOnly= false;
            CalendarColumn dgvcal = new CalendarColumn();
            dgvcal.HeaderText = "Ԥ������";
            dgvcal.Name = "Ԥ������";
            
            //DateColumn dgvcaln = new DateColumn();
            //dgvcaln.HeaderText = "������";
            if (!dgv1.Columns.Contains("Ԥ������"))
            {
                //dgv1.Columns.Add(dgvcom);
                //dgv1.Columns.Add(dgvcal);
                //dgv1.Columns.Add(dgvcaln);
                //dgv1.Columns["Ԥ������"].Width = 200;
            }
            dgv1.Columns["Ԥ����"].Width = 100;
            //dgv1.Columns["�ֶ�"].Width = 100;
            dgv1.Columns["Ԥ����"].ValueType = typeof(double);
            dgv1.Columns["��"].ReadOnly = true;
            dgv1.Columns["���"].ReadOnly = true;
            dgv1.Columns["�����"].ReadOnly = true;
            dgv1.Columns["������"].ReadOnly = true;
            dgv1.Columns["������"].ReadOnly = true;
            dgv1.Columns["����"].ReadOnly = true;
            dgv1.Columns["֤��"].ReadOnly = true;
            dgv1.Columns["��λ"].ReadOnly = true;
            dgv1.Columns["�ܶȵ�λ"].ReadOnly = true;
            dgv1.Columns["�ȼ�"].ReadOnly = true;
            dgv1.Columns["��λ�ܶ�"].ReadOnly = true;
            dgv1.Columns["parentid"].Visible = false;
            #region �����е�ֻ����
            //int specnum = PartParameter.GetSpecCou(activity);
            //if (specnum != 0)
            //{
            //    if (specnum > 0)
            //    {
            //        string colstr = PartParameter.GetSpecName(activity, "1").Trim();
            //        dgv1.Columns[colstr].ReadOnly = true;
            //    }
            //    if (specnum > 1)
            //    {
            //        string colstr = PartParameter.GetSpecName(activity, "2").Trim();
            //        dgv1.Columns[colstr].ReadOnly = true;
            //    }
            //    if (specnum > 2)
            //    {
            //        string colstr = PartParameter.GetSpecName(activity, "3").Trim();
            //        dgv1.Columns[colstr].ReadOnly = true;
            //    }
            //    if (specnum > 3)
            //    {
            //        string colstr = PartParameter.GetSpecName(activity, "4").Trim();
            //        dgv1.Columns[colstr].ReadOnly = true;
            //    }
            //}
            #endregion
           
            #region �������ù���Ԥ��ֵ������
            //if (dgv1.RowCount != 0)
            //{
            //    List<PartParameter> pp = PartParameter.FindPartList(systemid,mmtypeid, ProjectId,LoginUser,1);
            //    //if (pp.Count != 0)
            //    // {
            //    for (int i = 0; i < dgv1.Rows.Count; i++)
            //    {

            //        int partid = int.Parse(dgv1.Rows[i].Cells["���"].Value.ToString());
            //        PartParameter pone = pp.Find(delegate(PartParameter bb) { return bb.ID == partid; });
            //        if (pone != null)
            //        {
            //            dgv1.Rows[i].Cells["Ԥ����"].Value = pone.PREDICTION_QTY;
            //            //DataGridViewComboBoxCell combo = new DataGridViewComboBoxCell();

            //            dgv1.Rows[i].Cells["Ԥ��ϵ��"].Value = pone.PREDICTION_ALERT;
            //            if (pone.BLOCKID != "0")
            //            {
            //                dgv1.Rows[i].Cells["����"].Value = pone.CREATEDATE;
            //                dgv1.Rows[i].Cells["�ֶ�"].Value = pone.BLOCKID;
            //                //DataGridViewComboBoxCell combo = dgv1.Rows[i].Cells["�ֶ�"] as DataGridViewComboBoxCell;
            //                //combo.DataSource = blockds.Tables[0];
            //                //combo.DisplayMember = "description";
            //                //combo.ValueMember = "block_id";
            //                //combo.Value = pone.BLOCKID;
            //            }
            //        }
            //        else
            //        {
            //            dgv1.Rows[i].Cells["Ԥ����"].Value = 0;
            //            dgv1.Rows[i].Cells["Ԥ��ϵ��"].Value = 0;
            //        }

            //    }
            //    //}
            //}
            #endregion



        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            
           // string projectid = cmb_project.SelectedValue.ToString();
            string site = cmb_site.SelectedValue.ToString();
            
            decimal singleWeight = 0;
            //decimal fpreAlert = 0;
            decimal fcoe = 0;
            //decimal.TryParse(sigleWeight, out singleWeight);
            //decimal.TryParse(preAlert, out fpreAlert);
            //decimal.TryParse(coe, out fcoe);
            if (ProjectId == string.Empty)
            {
                MessageBox.Show("��ѡ����Ŀ��");
                return;
            }
            if (dgv1.RowCount == 0)
            {
                MessageBox.Show("��ѡ�����ݣ�");
                return;
            }
            int kqrow;
            kqrow = dgv1.RowCount - 1;
            try
            {

                //DataSet unitds=PartParameter.QueryPartPara("select name from mm_unit_tab"); 
                #region ѭ����,��������
                for (int i = 0; i <= kqrow; i++)
                {

                    string partno = dgv1.Rows[i].Cells["�����"].Value.ToString().Trim();
                    PartParameter pp = PartParameter.Find(int.Parse(activity), ProjectId, partno, site, LoginUser);
                    PartParameter ppn = new PartParameter();
                    ppn.PART_NO = partno;
                    ppn.CONTRACT = site;
                    ppn.PROJECTID = ProjectId;
                    ppn.WEIGHT_SINGLE = singleWeight;
                    string preQty = dgv1.Rows[i].Cells["Ԥ����"].Value.ToString().Trim();
                    decimal fpreQty = string.IsNullOrEmpty(preQty)== true ? 0 : decimal.Parse(preQty);
                    ppn.PREDICTION_QTY = decimal.Round(fpreQty,2);
                    string preAlert = dgv1.Rows[i].Cells["Ԥ��ϵ��"].Value.ToString().Trim();
                    //decimal fpreAlert = decimal.Parse(preAlert);
                    //ppn.PREDICTION_ALERT = decimal.Round(fpreAlert, 2);
                    //ppn.PREDICTION_ALERT = fpreAlert;
                    //ppn.COEFFICIENT_ERP = fcoe;
                    ppn.DESCRIPTION = "";
                    ppn.PREDICT_CREATOR = LoginUser;
                    ppn.ECPROJECTID = ecprojectid;
                    ppn.SYSTEMID = int.Parse(activity);
                    //if (dgv1.Rows[i].Cells["�ֶ�"].Value != null)
                    //    ppn.BLOCKID = dgv1.Rows[i].Cells["�ֶ�"].Value.ToString();
                    ppn.PARTID =int.Parse( dgv1.Rows[i].Cells["���"].Value.ToString());
                    #region �ж�Unit�Ƿ��Ǻϸ��ʽ
                    //if (dgv1.Rows[i].Cells[2].Value != null)
                    //{
                    //    string punit = dgv1.Rows[i].Cells[2].Value.ToString().Trim().ToLower();
                    //    DataRow[] pone = unitds.Tables[0].Select("name ='"+punit+"'");
                    //    if (pone.Length != 0)
                    //    { 
                    //        ppn.UNIT = punit; 
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("��"+(i+1)+"�е�λ���Ʋ��淶,���飡","������ʾ");
                    //        dgv1.Rows[i].Selected=true;
                    //        return;
                    //    }
                    //}
                    #endregion
                    if (pp != null)
                    {
                        if (dgv1.Rows[i].Cells["checkbox"].Value != null)
                        {
                            if (fpreQty > 0 || dgv1.Rows[i].Cells["checkbox"].Value.ToString() == "1")
                            {
                                DateTime cdate = pp.CREATEDATE;
                                int count = ppn.Update();
                                ppn.LAST_FLAG = 0;
                                ppn.PREDICTION_QTY = pp.PREDICTION_QTY;
                                ppn.PREDICTION_ALERT = pp.PREDICTION_ALERT;
                                ppn.CREATEDATE = cdate;
                                int countnew = 0;
                                if (pp.PREDICTION_QTY != fpreQty)
                                    countnew = ppn.Add();
                                if (count == 0)
                                {
                                    MessageBox.Show("����ʧ��");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (dgv1.Rows[i].Cells["checkbox"].Value != null)
                        {
                            if (fpreQty > 0 || dgv1.Rows[i].Cells["checkbox"].Value.ToString() == "1")
                            {
                                ppn.CREATEDATE = DateTime.Today;
                                ppn.LAST_FLAG = 1;
                                int count = ppn.Add();
                                if (count == 0)
                                {
                                    MessageBox.Show("���ʧ��");
                                }
                            }
                        }
                    }

                }
                #endregion

                MessageBox.Show("�������Ԥ�����ɹ�!! ", "��ܰ��ʾ!");
            }
            catch (Exception err)
            {

                MessageBox.Show("����ԭ��" + err.Message, "������ʾ��Ϣ",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            QuerydataBindpara();
        }

        private void btn_query_Click(object sender, EventArgs e)
        {
            
        }
        private void dgv1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgv1.RowCount == 0) return;
            //string partid = dgv1.CurrentRow.Cells["���"].Value.ToString();
            //if (PartParameter.GetPartParaCou(partid, LoginUser, ProjectId) == 0)
            //{
            //    MessageBox.Show("�޴˲��ϵ�Ԥ����¼��");
            //    return;
            //}
            //frmParaLog frm = new frmParaLog(LoginUser, ProjectId, int.Parse(partid), "", activity);
            
            ////frm.MdiParent = this.ParentForm;
            ////frm.Show();

            ////frm.TopLevel = false;
            ////sc.Panel2.Controls.Add(frm);
            ////frm.Dock = DockStyle.Fill;
            //frm.Show();

        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            int dgrow = dgv1.RowCount;
            string checkstr="";
            for (int i = 0;  i < dgrow; i++)
            {
                if (dgv1.Rows[i].Cells[0].Value.ToString() == "1")
                {
                    string partid = dgv1.Rows[i].Cells[3].Value.ToString();
                    checkstr = "checked";
                }

            }

                if (checkstr == "")
                {
                    MessageBox.Show("����û��ѡ�����ݣ�");
                    return;
                }
            int seqNo = Convert.ToInt32(lbl_seqno.Text.ToString());
            if (PartParameter.Delete(seqNo) > 0)
            {
                lbl_seqno.Text = string.Empty;
                MessageBox.Show("ɾ���ɹ���");
            }
            else
            {
                MessageBox.Show("ɾ��ʧ�ܣ�");
            }
            QuerydataBind();
        }

        private void cmb_partno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //string projectid = cmb_project.SelectedValue.ToString();
                string site = cmb_site.SelectedValue.ToString();
                string PartNo = cmb_partno.Text.Trim().ToString();
                if (PartNo == string.Empty)
                {
                    MessageBox.Show("��д��������");
                    return;
                }
                DataSet PartNameDS = InventoryPart.FindInvPartDataset(PartNo, site);
                DataRow row = PartNameDS.Tables[0].NewRow();
                row[0] = "";
                PartNameDS.Tables[0].Rows.InsertAt(row, 0);
                cmb_partno.DataSource = PartNameDS.Tables[0].DefaultView;
                cmb_partno.DisplayMember = "PART_NO";
                cmb_partno.ValueMember = "PART_NO";

                cmb_dpname.DataSource = PartNameDS.Tables[0].DefaultView;
                
                if (PartNameDS.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("�����ڴ������ţ�");
                    return;
                }
                cmb_dpname.DisplayMember = "DESCRIPTION";
                cmb_dpname.ValueMember = "PART_NO";


                
                //InventoryPart pp = InventoryPart.FindInvInfor(partno, site);
                //if (pp != null)
                //{
                //    lbl_name.Text = pp.description;
                //    txt_unit.Text = pp.unit_meas;
                //}
                //else
                //    MessageBox.Show("�����ڴ������ţ�");
            }
        }

        private void cmb_partno_SelectedIndexChanged(object sender, EventArgs e)
        {
            string site = cmb_site.SelectedValue.ToString();
            if (activity == string.Empty) return;
            
            DataRowView drvPartno = cmb_partno.SelectedItem as DataRowView;
            DataRow drno = drvPartno.Row;
            String PartNo = drno["part_no"].ToString();

            if (PartNo != string.Empty)
            {

                DataSet PartNameDS = PartType.PartSpecDs(PartNo, site);
                cmb_dpname.AutoCompleteSource = AutoCompleteSource.ListItems;
                cmb_dpname.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmb_dpname.DataSource = PartNameDS.Tables[0].DefaultView;
                if (PartNameDS.Tables[0].Rows.Count == 0)
                {
                    //MessageBox.Show("�����ڴ������ţ�");
                    return;
                }
                cmb_dpname.DisplayMember = "part_spec";
                cmb_dpname.ValueMember = "part_no";
                //    //txt_unit.Text = InventoryPart.FindInvInfor(PartNo, site).unit_meas;

            }
            
        }

        private void cmb_partname_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            lbl_seqno.Text = string.Empty;
            cmb_partno.Text = string.Empty;
            cmb_dpname.Text = string.Empty;
            
        }

        private void cmb_partname_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //string projectid = cmb_project.SelectedValue.ToString();
            //    string site = cmb_site.SelectedValue.ToString();
            //    string PartName =cmb_partname.Text.Trim().ToString();
            //    if (PartName == string.Empty)
            //    {
            //        MessageBox.Show("��д��������");
            //        return;
            //    }
            //    DataSet PartNameDS = InventoryPart.FindInvPartInfByPartNameDataset(PartName, site);
            //    DataRow row = PartNameDS.Tables[0].NewRow();
            //    row[0] = "";
            //    PartNameDS.Tables[0].Rows.InsertAt(row, 0);

            //    cmb_partname.DataSource = PartNameDS.Tables[0].DefaultView;

            //    cmb_partname.DisplayMember = "DESCRIPTION";
            //    cmb_partname.ValueMember = "DESCRIPTION";

            //    cmb_partno.DataSource = PartNameDS.Tables[0].DefaultView;

            //    if (PartNameDS.Tables[0].Rows.Count == 0)
            //    {
            //        MessageBox.Show("�����ڴ������ţ�");
            //        return;
            //    }
            //    cmb_partno.DisplayMember = "PART_NO";
            //    cmb_partno.ValueMember = "PART_NO";

            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            //dgv1.DataSource = DataSet.Talbe[0];//���³�ʼ��DataGridView

            if (dgv1.RowCount == 0) return;

            if (dgv1.RowCount != 0)
            {
                for (int i = 0; i < dgv1.RowCount; i++)
                {
                    //int vint = int.Parse(dgv1.Rows[i].Cells["checkbox"].Value.ToString());
                    if (SelectAll.Checked == true)
                        dgv1.Rows[i].Cells["confirmflag"].Value = 1;
                    else
                        dgv1.Rows[i].Cells["confirmflag"].Value = 0;
                }

            }
        }

        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgv1.RowCount == 0) return;
            pb1.Visible = true;
            bool restr = PartParameter.ExportToTxt(dgv1, pb1);
            pb1.Visible = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            mSite = cmb_site.SelectedValue.ToString();
            if (mSite == string.Empty)
            {
                MessageBox.Show("��ѡ����");
                return;
            }
            ProjectCmbItem item = (ProjectCmbItem)cmb_project.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("��ѡ����Ŀ", "��ʾ");
                return;
            }
            ProjectId = item.Value;
            #region ���ѡ�������רҵ ��ô���մ�רҵ����������л���Ԥ������
            QuerydataBindpara();
            #endregion
            #region ���û��ѡ�������רҵ ��ô���ղ����л���Ԥ������
            QuerydataBindpara();
            #endregion
            
        }

        public void QuerydataBindpara()
        {
            //string ProjectId = cmb_project.SelectedValue.ToString();
            //if (YuguType == "M") mmtypeid = activity;
            //if (YuguType == "S") systemid = activity;
            string parttype = cmb_partno.Text.Trim().ToString().ToLower();
            string disciplineid = cmb_dpname.SelectedValue.ToString();
            string projectzone = tb_type.Text.Trim().ToLower();
            StringBuilder sb = new StringBuilder();
           //if (ProjectId != string.Empty) sb.Append(" AND PROJECTID = '" + ProjectId + "'");
            if (mSite != string.Empty) sb.Append(" AND CONTRACT = '" + mSite + "'");
            if (projectzone != string.Empty) sb.Append(" AND  lower(project_zone) like '%" + projectzone + "%'");
            if (disciplineid != string.Empty) sb.Append(" AND discipline  = " + disciplineid );
            if (parttype != string.Empty) sb.Append(" AND lower(part_type) like '%" + parttype + "%'");
            
            //if (YuguType == "S")
            //{
            //    if (mmtypeid != "0")
            //        sb.Append(" and parentid=" + mmtypeid);
            //    //sb.Append(" and systemid=" + activity);
            //}
            //if (YuguType == "M")
            //{
            //    sb.Append(" and parentid=" + mmtypeid);
            //    //sb.Append(" and systemid=" + systemid);
            //}
            XmlOper.setXML("Type", mmtypeid);
            //if(checkBox1.)
            //string sqlSelect = "SELECT '','',pp.*,'' FROM  PLM.MM_PART_TAB pp WHERE 1=1 and parentid= " + activity;
            string sqlSelect = "select * from mm_part_para_view p where projectid='"+ProjectId+"' and p.operator='" + LoginUser + "'";
            string wheresql = sb.ToString();
            sqlSelect = sqlSelect + wheresql + " order by p.part_no";
            listviewBindpara(sqlSelect);
        }
        public void listviewBindpara(string sql)
        {
            //���Ĳ������������Լ�����
            dgv1.AutoGenerateColumns = false;
            ds = PartParameter.QueryPartPara(sql);
            dgv1.DataSource = ds.Tables[0].DefaultView;
            if (dgv1.RowCount == 0) return;
            int rowcount = dgv1.RowCount;
            //for (int i = 0; i < rowcount; i++)
            //{
            //    dgv1.Rows[i].Cells["confirmflag"].Value = 0;
            //}

            //DataGridViewComboBoxColumn dgvcom = new DataGridViewComboBoxColumn();
            //DataSet blockds = PartParameter.QueryPartPara("select block_id,description from project_block_tab where project_id=" + ecprojectid.ToString() + " order by description");
            //DataRow rowdim = blockds.Tables[0].NewRow();
            //rowdim[0] = 1;
            //blockds.Tables[0].Rows.InsertAt(rowdim, 0);
            //dgvcom.DataSource = blockds.Tables[0].DefaultView;
            //dgvcom.DisplayMember = "description";
            //dgvcom.ValueMember = "description";
            //dgvcom.HeaderText = "�ֶ�";
            //dgvcom.Name = "�ֶ�";
            //dgvcom.ReadOnly = false;
            //CalendarColumn dgvcal = new CalendarColumn();
            //dgvcal.HeaderText = "Ԥ������";
            //dgvcal.Name = "Ԥ������";

            ////DateColumn dgvcaln = new DateColumn();
            ////dgvcaln.HeaderText = "������";
            //if (!dgv1.Columns.Contains("Ԥ������"))
            //{
            //    //dgv1.Columns.Add(dgvcom);
            //    dgv1.Columns.Add(dgvcal);
            //    //dgv1.Columns.Add(dgvcaln);
            //    dgv1.Columns["Ԥ������"].Width = 200;
            //}
            



        }

        private void dgv1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int i = e.ColumnIndex;
                DataGridViewColumn column = dgv1.Columns[e.ColumnIndex];
                if (column.Name == "final_prediction_qty")
                {
                    if (dgv1.Rows[e.RowIndex].Cells[i].Value != null)
                    {
                        string rqty = dgv1.Rows[e.RowIndex].Cells[i].Value.ToString();
                        if (BaseClass.validateNum(rqty) == false)
                        {
                            MessageBox.Show("����������!!!", "��ʾ");
                            dgv1.Rows[e.RowIndex].Cells[i].Value = "";
                        }
                        else
                        {
                            string predicqty = dgv1.Rows[e.RowIndex].Cells[i -1].Value.ToString();
                            string finalqty = dgv1.Rows[e.RowIndex].Cells[i].Value.ToString().Trim();
                            if (predicqty != finalqty)
                            {
                                decimal modpercent = Math.Abs(decimal.Parse(finalqty) - decimal.Parse(predicqty)) / decimal.Parse(predicqty) * 100;
                                dgv1.Rows[e.RowIndex].Cells[i + 1].Value = decimal.Round(modpercent, 1);
                                if (modpercent > 10)
                                {
                                    MessageBox.Show("��������ԭ�򣡣���","������ʾ");
                                    dgv1.Rows[e.RowIndex].Selected = true;
                                }
                            }
                        }

                    }

                }
                

            }
        }

        private void dgv1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control&&e.KeyCode== Keys.V)
                MessageBox.Show("�㰴����ctrl+vճ����");
        }

        private void dgv1_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void dgv1_KeyDown_1(object sender, KeyEventArgs e)
        {

        }

        private void ȷ���ϴ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dgv1.RowCount==0) return;
            DialogResult dgresult = MessageBox.Show("ȷ��Ҫ��ѡ�е�Ԥ���������ȷ����", "����ȷ��", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dgresult == DialogResult.No) return;
            int confirmcount = dgv1.RowCount;
            string sequenceList = "";
            for (int i = 0; i < confirmcount; i++)
            {
                if (dgv1.Rows[i].Cells["confirmflag"].EditedFormattedValue != null)
                {
                    if (dgv1.Rows[i].Cells["confirmflag"].EditedFormattedValue.ToString() == "True")
                    {
                        string sequenceid = dgv1.Rows[i].Cells["sequence_id"].Value.ToString();
                        sequenceList += ","+sequenceid  ;
                    }
                }
            }
            PartParameter.DeleteEstimate(sequenceList.Substring(1));
            MessageBox.Show("��ѡ���ϵ�Ԥ�����ȷ�ϳɹ�","������ʾ");
        }

        

    }
}