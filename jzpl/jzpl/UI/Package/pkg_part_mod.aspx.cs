using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using jzpl.Lib;
using System.Data.OleDb;

namespace jzpl.UI.Package
{
    public partial class pkg_part_mod : System.Web.UI.Page
    {
        private string m_perimission;
        private string m_picpath;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.BufferOutput = true;
            Authentication auth = new Authentication(this);
            if (auth.LoadSession() == false)
            {
                auth.RemoveSession();
                Response.Redirect("../../UI/FrameUI/login.htm");
                Response.End();
            }
            else
            {
                m_perimission = ((Authentication.LOGININFO)Session["USERINFO"]).Permission;
                if (CheckAccessAble())
                {
                    if (!IsPostBack)
                    {
                        bindDDLData();
                        string partNo = Misc.GetHtmlRequestValue(Request, "part");
                        string packageNo = Misc.GetHtmlRequestValue(Request, "pkg");

                        if (partNo == "" || packageNo == "")
                        {
                            Misc.Message(this.GetType(), ClientScript, "ҳ�����ʧ�ܣ�δ��ñ�Ҫ������");
                            return;
                        }

                        LblPartNo.Text = partNo;
                        LblPackageNo.Text = packageNo;

                        GenPkgPartBase objPart = new GenPkgPartBase(packageNo, partNo);

                        LblPackageName.Text = objPart.PackageName;
                        TxtPartName.Text = objPart.PartName;
                        TxtPartNameE.Text = objPart.PartNameE;
                        TxtPartSpec.Text = objPart.PartSpec;
                        TxtDecNo.Text = objPart.DecNo;
                        TxtContractNo.Text = objPart.ContractNo;
                        DdlUnit.SelectedIndex = DdlUnit.Items.IndexOf(DdlUnit.Items.FindByValue(objPart.Unit));
                        TxtPO.Text = objPart.PO;
                        ChkPayFlag.Checked = objPart.PayFlag == "1" ? true : false;
                        m_picpath = objPart.PicPath;

                        HiddenObjId.Value = objPart.ObjId;
                        HiddenRowversion.Value = objPart.ObjVersion;
                        //TextBox1.Text = Request.Url.ToString();
                    }
                }
                else
                {
                    auth.RemoveSession();
                    Response.Redirect("../../UI/FrameUI/login.htm");
                    Response.End();
                }
            }
        }

        protected Boolean CheckAccessAble()
        {
            if (m_perimission[(int)Authentication.PERMDEFINE.PKG_PART] == '1') return true;
            return false;
        }

       

        protected void ButtSave_Click(object sender, EventArgs e)
        {
            string v_out;
           
            using (OleDbConnection conn = new OleDbConnection(DBHelper.OleConnectionString))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                OleDbCommand cmd = new OleDbCommand("gen_part_package_item_api.Modify_", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;

                cmd.Parameters.Clear();
                cmd.Parameters.Add("v_objid", OleDbType.VarChar, 20).Value = HiddenObjId.Value;
                cmd.Parameters.Add("v_rowversion", OleDbType.VarChar, 20).Value = HiddenRowversion.Value;
                cmd.Parameters.Add("v_Part_name", OleDbType.VarChar, 500).Value = TxtPartName.Text;
                cmd.Parameters.Add("v_Part_name_e", OleDbType.VarChar, 500).Value = TxtPartNameE.Text;
                cmd.Parameters.Add("v_Part_spec", OleDbType.VarChar, 100).Value = TxtPartSpec.Text;
                cmd.Parameters.Add("v_UNIT", OleDbType.VarChar, 20).Value = DdlUnit.SelectedValue =="0"?"":DdlUnit.SelectedValue;
                cmd.Parameters.Add("v_Dec_no", OleDbType.VarChar, 100).Value = TxtDecNo.Text;
                cmd.Parameters.Add("v_Contract_no", OleDbType.VarChar, 100).Value = TxtContractNo.Text;
                cmd.Parameters.Add("v_po_no", OleDbType.VarChar).Value = TxtPO.Text;
                cmd.Parameters.Add("v_pay_flag", OleDbType.VarChar).Value = ChkPayFlag.Checked ? "1" : "0";

                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    Misc.RegisterClientScript(this.GetType(),"part_refresh",ClientScript,
                        "<script type='text/javascript'>alert('���ݸ��³ɹ���');window.dialogArguments.refresh();window.close();</script>");  
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    if (Misc.CheckIsDBCustomException(ex))
                    {
                        Misc.Message(this.GetType(), ClientScript, Misc.GetDBCustomException(ex));
                    }
                    else
                    {
                        throw;
                    }
                    //Misc.Message(this.GetType(),ClientScript,string.Format("���ݸ���ʧ�ܣ�{0}",ex.Message));
                    //return;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void bindDDLData()
        {
            BaseInfoLoader baseInfoLoader = new BaseInfoLoader();
            baseInfoLoader.PartUnitDropDownListLoad(DdlUnit, false, true);
        }

        protected void LBtnUpload_Click(object sender, EventArgs e)
        {
            string tempDir;
            //������ʱ�ļ���
            string tempFileName;
            //������ʱ�ļ������ȴ��û�������水ť���ڴ�����ʽ�ļ�����
            string partDir;
            //�Դ��������Ϊ��һ��Ŀ¼
            //�ж��Ƿ�����Դ������������Ŀ¼
            //��ȡĿǰ�Ŀ¼
            //��ȡ�ɴ�������ļ���
            //�ж��ļ������ѵ���ֵ
            //�����µ��ļ��У�������Ϊ��ļ���
            string partFileName;

            string fileExtension;

            //if (FileUpload1.FileName.Trim() != string.Empty)
            //{
            //    //fileExtension = 
            //}



        }
    }
}
