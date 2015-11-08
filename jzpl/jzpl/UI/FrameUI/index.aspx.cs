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
using jzpl.UI.JP;
using jzpl.Lib;
using System.Text;

namespace jzpl.UI.FrameUI
{
    public partial class index : Page
    {
        Authentication auth = null;
        private string m_permission;        
        StringBuilder html_;

        protected void Page_Load(object sender, EventArgs e)
        {
            string errmsg = "";
            int seq = 1;
            auth = new Authentication(this);
            auth.RemoveSession();
            if (auth.CheckUser(Lib.Misc.GetHtmlRequestValue(Request, "userid"), Lib.Misc.GetHtmlRequestValue(Request, "pwd"), ref errmsg) == false)
            {
                if (errmsg.Length > 0)
                {
                    Response.Write("\n<HTML>");
                    Response.Write("\n<HEAD></HEAD>");
                    Response.Write("\n<BODY>");
                    Response.Write("\n<SCRIPT LANGUAGE='Javascript'>");
                    Response.Write(string.Format("\nalert(\"{0}\");", errmsg));
                    Response.Write(string.Format("\ndocument.location='../../UI/FrameUI/login.htm'"));
                    Response.Write("\n</SCRIPT>");
                    Response.Write("\n</BODY>");
                    Response.Write("\n</HTML>");
                }
                else
                {
                    Response.Redirect("../../UI/FrameUI/login.htm");
                    Response.End();
                }
            }
            else
            {
                m_permission = ((Authentication.LOGININFO)Session["USERINFO"]).Permission;
                html_ = new StringBuilder("<ul id=\"sddm\">\n");
                html_.Append(GenMenuHtml("��������", ref seq, new SubMenuInfo[] {
                    new SubMenuInfo("��˾","../../UI/admin/company.aspx",Authentication.PERMDEFINE.BS_COMPANY),
                    new SubMenuInfo("��Ŀ","../../UI/admin/project.aspx",Authentication.PERMDEFINE.BS_PROJECT),
                    new SubMenuInfo("��Ŀ������Ա","../../UI/admin/project_acc_per.aspx",Authentication.PERMDEFINE.BS_PRJ_ACC),
                    new SubMenuInfo("����","../../UI/admin/currency.aspx",Authentication.PERMDEFINE.BS_CURRENCY),
                    new SubMenuInfo("ȱ��ԭ��","../../UI/admin/lack_reason.aspx",Authentication.PERMDEFINE.BS_LACK),
                    new SubMenuInfo("���ͽ��ղ���","../../UI/admin/receipt_dept.aspx",Authentication.PERMDEFINE.BS_RCPT_DEPT),
                    new SubMenuInfo("���ͽ��ճ���","../../UI/admin/receipt_place.aspx",Authentication.PERMDEFINE.BS_RCPT_PLACE),
                    new SubMenuInfo("���ͽ�����","../../UI/admin/receipt_person.aspx",Authentication.PERMDEFINE.BS_RCPT_PER),
                    new SubMenuInfo("������","../../UI/admin/req_group.aspx",Authentication.PERMDEFINE.BS_REQ_GROUP),
                    new SubMenuInfo("������λ","../../UI/admin/part_unit.aspx",Authentication.PERMDEFINE.BS_UNIT),
                    new SubMenuInfo("�ֿ�����","../../UI/admin/wh_area.aspx",Authentication.PERMDEFINE.BS_WH_AREA),
                    new SubMenuInfo("�����λ","../../UI/admin/wh_location.aspx",Authentication.PERMDEFINE.BS_WH_LOC),
                    new SubMenuInfo("����Ա","../../UI/admin/check_person.aspx",Authentication.PERMDEFINE.BS_CHK_PER)
                }));
                html_.Append(GenMenuHtml("�����Ϣ", ref seq, new SubMenuInfo[] {
                    new SubMenuInfo("�������-���","../../UI/Package/package.aspx",Authentication.PERMDEFINE.PKG),
                    new SubMenuInfo("�������-������","../../UI/Package/pkg_part.aspx",Authentication.PERMDEFINE.PKG_PART),
                    new SubMenuInfo("��������Ǽ�","../../UI/Package/pkg_arrival.aspx",Authentication.PERMDEFINE.PKG_ARR),
                    new SubMenuInfo("���������ѯ","../../UI/Package/pkg_arrival_query.aspx",Authentication.PERMDEFINE.PKG_ARR_Q),
                    new SubMenuInfo("�������","../../UI/Package/pkg_check.aspx",Authentication.PERMDEFINE.PKG_CHK),
                    new SubMenuInfo("��������ѯ","../../UI/Package/pkg_check_query.aspx",Authentication.PERMDEFINE.PKG_CHK_Q),
                    new SubMenuInfo("����ƿ�","../../UI/Package/pkg_loc_move.aspx",Authentication.PERMDEFINE.PKG_MOV),
                    new SubMenuInfo("����ƿ��ѯ","../../UI/Package/pkg_loc_move_query.aspx",Authentication.PERMDEFINE.PKG_MOV_Q),
                    new SubMenuInfo("�����ѯ","../../UI/Package/pkg_query.aspx",Authentication.PERMDEFINE.PKG_Q),
                    new SubMenuInfo("��������ʲ�ѯ","../../UI/Package/pkg_part_query.aspx",Authentication.PERMDEFINE.PKG_PART_Q),
                    new SubMenuInfo("������ʿ��","../../UI/Package/pkg_loc_query.aspx",Authentication.PERMDEFINE.PKG_LOC_Q),
                    new SubMenuInfo("���������Ŀת��","../../UI/Package/pkg_change_project.aspx",Authentication.PERMDEFINE.PKG_CHG_PRJ),
                    new SubMenuInfo("������ʿ�����","../../UI/Package/pkg_part_receive.aspx",Authentication.PERMDEFINE.PKG_STORE_IN),
                    new SubMenuInfo("������ʿ���·�","../../UI/Package/pkg_part_issue.aspx",Authentication.PERMDEFINE.PKG_STORE_ISS),
                    new SubMenuInfo("�����ֵ","../../UI/Package/pkg_value_query.aspx",Authentication.PERMDEFINE.PKG_VALUE_Q),
                    new SubMenuInfo("���������񵵰�","../../UI/Package/pkg_loc_history.aspx",Authentication.PERMDEFINE.PKG_LOCHIS_Q)
                }));
                html_.Append(GenMenuHtml("�������", ref seq , new SubMenuInfo[] { 
                    new SubMenuInfo("�����������","../../UI/Package/jp_pkg_add.aspx",Authentication.PERMDEFINE.PKG_JP_ADD) ,
                    new SubMenuInfo("�������������","../../UI/Package/jp_pkg_query.aspx",Authentication.PERMDEFINE.PKG_JP_QUERY),
                    new SubMenuInfo("�����������ȷ�ϣ����ܣ�","../../UI/Package/jp_pkg_confirm.aspx",Authentication.PERMDEFINE.PKG_JP_CONFIRM),
                    new SubMenuInfo("�����������ȷ�ϣ�������","../../UI/Package/jp_pkg_confirm1.aspx",Authentication.PERMDEFINE.PKG_JP_CONFIRM1),
                    new SubMenuInfo("������������ѯ","../../UI/Package/jp_pkg_query_ext.aspx",Authentication.PERMDEFINE.PKG_JP_QUERY1),
                    new SubMenuInfo("��������·�","../../UI/Package/jp_pkg_issue.aspx",Authentication.PERMDEFINE.PKG_JP_ISS),
                    new SubMenuInfo("������ʽ��ӵ�","../../UI/Package/jp_pkg_jjd_new.aspx",Authentication.PERMDEFINE.PKG_JP_JJD_NEW),                    
                    new SubMenuInfo("������ʽ��ӵ����","../../UI/Package/jp_pkg_jjd_finish.aspx",Authentication.PERMDEFINE.PKG_JP_JJD_FINISH),
                    new SubMenuInfo("������ʽ��ӵ���ѯ","../../UI/Package/jp_pkg_jjd.aspx",Authentication.PERMDEFINE.PKG_JP_JJD)
                    //,new SubMenuInfo("����������ϴ�ӡ","../../UI/Package/pkg_issue_voucher.aspx",Authentication.PERMDEFINE.PKG_JP_ISS_VCHR)
                }));
                html_.Append(GenMenuHtml("��ͨ����", ref seq, new SubMenuInfo[] { 
                    new SubMenuInfo("��ͨ������������","../../UI/JP/wzxqjh_add.aspx",Authentication.PERMDEFINE.PART_JP_ADD),
                    new SubMenuInfo("��ͨ��������������","../../UI/JP/wzxqjh_query.aspx",Authentication.PERMDEFINE.PART_JP_QUERY),
                    new SubMenuInfo("��ͨ����ȱ���´�(ȱƷ)","../../UI/JP/wzxqjh_lack.aspx",Authentication.PERMDEFINE.PART_JP_CONFIRM),
                    new SubMenuInfo("��ͨ���������´�","../../UI/JP/wzxqjh_confirm.aspx",Authentication.PERMDEFINE.PART_JP_CONFIRM),
                    new SubMenuInfo("��ͨ��������ȷ�ϣ�������","../../UI/JP/wzxqjh_confirm1.aspx",Authentication.PERMDEFINE.PART_JP_CONFIRM1),
                    new SubMenuInfo("��ͨ�������Ͳ�ѯ","../../UI/JP/wzxqjh_query_ext.aspx",Authentication.PERMDEFINE.PART_JP_QUERY1),
                    new SubMenuInfo("��ͨ�������ͽ��ӵ�","../../UI/JP/wzxqjh_jjd_new_.aspx",Authentication.PERMDEFINE.PART_JP_JJD_NEW),
                    new SubMenuInfo("��ͨ�������ͽ��ӵ����","../../UI/JP/wzxqjh_jjd_finish.aspx",Authentication.PERMDEFINE.PART_JP_JJD_FINISH),
                    new SubMenuInfo("��ͨ�������ͽ��ӵ���ѯ","../../UI/JP/wzxqjh_jjd.aspx",Authentication.PERMDEFINE.PART_JP_JJD),
                    new SubMenuInfo("ERP-DMS�·�����","../../UI/JP/wzxqjh_iss_compare.aspx",Authentication.PERMDEFINE.PART_ISS_COMPARE)              
                
                }));
                if (((Authentication.LOGININFO)Session["USERINFO"]).Admin == "1")
                {
                    html_.Append(string.Format("<li><a href=\"#\" onclick=\"mopen('m{0}')\" onmouseout=\"mclosetime()\">{1}</a>\n", 5, "ϵͳ����"));
                    html_.Append(string.Format("<div id=\"m{0}\" onmouseover=\"mcancelclosetime()\" onmouseout=\"mclosetime()\">", 5));
                    //������ϵͳ������
                    html_.Append(string.Format("<a href=\"{0}\" target=\"jzpl_page\" onclick=\"set_currsite(this);\">{1}</a>\n", "../../UI/ADMIN/user_manage_base.aspx", "ϵͳ�ʺ�"));
                    html_.Append(string.Format("<a href=\"{0}\" target=\"jzpl_page\" onclick=\"set_currsite(this);\">{1}</a>\n", "../../UI/ADMIN/permission.aspx", "Ȩ������"));

                    html_.Append("</div></li>\n");
                }
                html_.Append("</ul><div style='clear:both'></div>\n");
                Panel1.Controls.Add(new LiteralControl(html_.ToString()));
                UserID.Text = auth.UserID;
                LblServer.Text = auth.ServerName;
            }
        }

        private string GenMenuHtml(string topMenuText, ref int seq, SubMenuInfo[] subMenu)
        {
            StringBuilder ret_=new StringBuilder();
            ret_.Append(string.Format("<li><a href=\"#\" onclick=\"mopen('m{0}')\" onmouseout=\"mclosetime()\">{1}</a>\n", seq, topMenuText)); 
            ret_.Append(string.Format("<div id=\"m{0}\"  onmouseover=\"mcancelclosetime()\" onmouseout=\"mclosetime()\">", seq));
            Boolean hasSubMenu = false;
            for (int i = 0; i < subMenu.Length; i++)
            {
                if (m_permission[(int)subMenu[i].permDefine] == '1')
                {
                    ret_.Append(string.Format("<a href=\"{0}\" target=\"jzpl_page\" onclick=\"set_currsite(this);\">{1}</a>\n", subMenu[i].subMenuUrl, subMenu[i].subMenuText));
                    hasSubMenu = true;
                }
            }
            ret_.Append("</div></li>\n");
            if (hasSubMenu)
            {
                seq++;
                return ret_.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        
        private struct SubMenuInfo
        {
            public string subMenuText;
            public string subMenuUrl;
            public Authentication.PERMDEFINE permDefine;

            public SubMenuInfo(string text, string url,Authentication.PERMDEFINE perm)
            {
                this.subMenuText = text;
                this.subMenuUrl = url;
                this.permDefine = perm;
            }
        }        
    }
}
