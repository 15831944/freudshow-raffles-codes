<%@ Page Language="C#" AutoEventWireup="true" Codebehind="jp_pkg_confirm.aspx.cs"
    Inherits="Package.jp_pkg_confirm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>�����������ȷ��</title>
    
    <style type="text/css">  
     #overLay{ width:100%; background:#E0E0E0;  position:absolute; top:0; left:0; display:none; opacity:0; filter:alpha(opacity=0);}
     #popuContent{ width:370px; line-height:20px;  display:none; position:absolute; z-index:100; background:#fff; border:2px solid #888888;}
    </style>
    <link href="../../UI/CSS/common.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../UI/script/DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="../../UI/script/common.js"></script>
    <script type="text/javascript">      
    window.onload=function(){init();}
    function init()
    {
        SetGVBoxHeight("UserSaveData","GVData");
    }
    function setOpacity(elem,current){
	//�����ie�����
	if(elem.filters){ 
	elem.style.filter = 'alpha(opacity=' + current + ')';
    }
    else{ //����w3c�����
	elem.style.opacity = current/100;
    }
	}
	
	function display_popudiv()
	{
	    alertEle = myf('popuContent'); 
	    obj = myf('overLay');
	    obj.style.display = 'block';
		//�ж�ҳ��ĸ߶��Ƿ񳬹�������������ĸ߶�
		if(document.body.offsetHeight>=document.documentElement.clientHeight){
			obj.style.height = document.body.offsetHeight + 'px'; 
		}else{
			obj.style.height = document.documentElement.clientHeight + 'px';
		}
		var currentOpacity = 0;
		//���ö�ʱ��timer1
		var timer1 = setInterval(
			function(){
				if(currentOpacity<=50){
					setOpacity(obj,currentOpacity);
					currentOpacity+=5;	
				}
				else{
					clearInterval(timer1);
				}
			}
		,50);
		alertEle.style.display = 'block';
		alertEle.style.left = (document.body.offsetWidth - alertEle.offsetWidth)/2 + 'px';
		alertEle.style.top = '100px';
	}
	
	function close_popudiv()
	{
	    alertEle = myf('popuContent'); 
	    obj = myf('overLay');
	    alertEle.style.display = 'none';
		var currentOpe = 50;
		//���ö�ʱ��timer2
		var timer2 = setInterval(
			function(){
				if(currentOpe>=0){
					setOpacity(obj,currentOpe);
					currentOpe-=5;
				}
				else{
					obj.style.display = 'none';
					clearInterval(timer2);
				}
			}
		,50);
	}
	
	function nextdo(req_qty,release_objid,objid,rowversion)
    {
        var reg = /^\d+(\.\d+)?$/;    
        
        obj=myf(release_objid);
        if(!obj)
        {
            alert("���������");
            return false;
        }
        else
        {	        
            if(!reg.test(obj.value))
            {
                alert("����������Ч��ֵ��");
                obj.value=0;
                obj.focus();
                return false;
            }
            released_qty = parseFloat(obj.value);
            if(released_qty==req_qty)
            {
                return confirm("ȷ��Ҫ�´�������룡");
            }
            if(released_qty>req_qty)
            {
                alert("�����´�ʧ�ܣ��´��������ܳ�������������");
                return false;
            }   
            if(released_qty<req_qty)
            {
                myf("HidObjId").value=objid;
                myf("HidRowversion").value=rowversion;
                myf("HidReleaseQty").value=released_qty;
                if(released_qty==0)
                {
                    myf("HidAction").value="cancel";
                }
                else
                {
                    myf("HidAction").value="release";
                }
                display_popudiv();
                return false;
            }            
        }     
    }

    </script>

</head>
<body style="font-size: 10pt;">
    <div id="overLay">
    </div>
    <form id="form1" runat="server">
    <div id="title">�����������ȷ��<hr /></div>
        <div>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 80px">
                        ����ID��</td>
                    <td style="width: 120px">
                        <asp:TextBox ID="TxtReqID" runat="server" Width="120px"></asp:TextBox></td>
                    <td width="20">
                    </td>
                    <td style="width: 80px">
                        ������룺</td>
                    <td style="width: 120px">
                        <asp:TextBox ID="TxtPartNo" runat="server" Width="120px"></asp:TextBox></td>
                    <td  width="20">
                       </td>
                    <td style="width: 80px">
                        �������</td>
                    <td style="width: 80px">
                        <asp:DropDownList ID="DdlPSType" runat="server" Width="120px">
                            <asp:ListItem Value="-1">��ѡ��....</asp:ListItem>
                            <asp:ListItem Value="0">ֱ������</asp:ListItem>
                            <asp:ListItem Value="1">����</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
            </table>
            <hr />
            <table>
                <tr>
                    <td width="120">
                        ��Ŀ��
                    </td>
                    <td>
                        <asp:DropDownList ID="DdlProject" runat="server" Width="245px">
                        </asp:DropDownList></td>
                    <td style="width: 20px">
                    </td>
                    <td width="120">
                        PO��
                    </td>
                    <td>
                        <asp:TextBox ID="TxtPO" runat="server" Width="240px"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td width="120">
                        ������룺</td>
                    <td>
                        <asp:TextBox ID="TxtPkgNo" runat="server" Width="240px"></asp:TextBox></td>
                    <td>
                    </td>
                    <td width="120">
                        ������ƣ�</td>
                    <td>
                        <asp:TextBox ID="TxtPkgName" runat="server" Width="240px"></asp:TextBox><span style="color: Red">%</span></td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td width="120">
                        ������ƣ�Ӣ�ģ���</td>
                    <td>
                        <asp:TextBox ID="TxtPartNameE" runat="server" Width="240px"></asp:TextBox><span style="color: Red">%</span></td>
                    <td>
                    </td>
                    <td width="120">
                        ������ƣ����ģ���</td>
                    <td>
                        <asp:TextBox ID="TxtPartName" runat="server" Width="240px"></asp:TextBox><span style="color: Red">%</span></td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td width="120">
                        ������</td>
                    <td>
                        <asp:TextBox ID="TxtPartSpec" runat="server" Width="240px"></asp:TextBox><span style="color: Red">%</span></td>
                    <td>
                    </td>
                    <td width="120">
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <hr />
            <table id="req_content" style="font-size: 10pt">
                <tr>
                    <td width="80">
                        <asp:Label ID="Label1" runat="server" Text="���ճ��أ�"></asp:Label></td>
                    <td width="180">
                        <asp:DropDownList ID="DdlProdSite" runat="server" Width="160px">
                        </asp:DropDownList><span style="color: #ff0000"></span></td>
                    <td width="20">
                    </td>
                    <td width="80">
                        <asp:Label ID="Label5" runat="server" Text="����ʱ�䣺"></asp:Label></td>
                    <td width="180">
                        <asp:TextBox ID="TxtDate" runat="server" Width="154px" onfocus="WdatePicker();"></asp:TextBox><span style="color: red"></span></td>
                    <td width="20">
                    </td>
                    <td width="80">
                        ���ղ��ţ�</td>
                    <td width="180">
                        <asp:DropDownList ID="DdlReceiptDept" runat="server" Width="160px">
                        </asp:DropDownList><span style="color: #ff0000"></span></td>
                    <td width="20">
                    </td>
                </tr>
                <tr>
                    <td width="80">
                        <asp:Label ID="Label6" runat="server" Text="�����ˣ�"></asp:Label></td>
                    <td width="180">
                        <asp:TextBox ID="TxtReceiver" runat="server" Width="154px"></asp:TextBox><span style="color: #ff0000"></span></td>
                    <td width="20">
                    </td>
                    <td width="80">
                        <asp:Label ID="Label7" runat="server" Text="IC��"></asp:Label></td>
                    <td width="180">
                        <asp:TextBox ID="TxtIC" runat="server" Width="154px"></asp:TextBox><span style="color: #ff0000"></span></td>
                    <td width="20">
                    </td>
                    <td width="80">
                        <asp:Label ID="Label8" runat="server" Text="��ϵ��ʽ��"></asp:Label></td>
                    <td width="180">
                        <asp:TextBox ID="TxtContact" runat="server" Width="154px"></asp:TextBox><span style="color: #ff0000"></span></td>
                    <td width="20">
                    </td>
                </tr>
                <tr>
                    <td width="80">
                        <asp:Label ID="Label9" runat="server" Text="�ֶΣ�"></asp:Label></td>
                    <td width="180">
                        <asp:TextBox ID="TxtBlock" runat="server" Width="154px"></asp:TextBox></td>
                    <td width="20">
                    </td>
                    <td width="80">
                        <asp:Label ID="Label10" runat="server" Text="ϵͳ��"></asp:Label></td>
                    <td width="180">
                        <asp:TextBox ID="TxtSystem" runat="server" Width="154px"></asp:TextBox></td>
                    <td width="20">
                    </td>
                    <td width="80">
                        <asp:Label ID="Label11" runat="server" Text="ʩ�����ݣ�"></asp:Label></td>
                    <td width="180">
                        <asp:TextBox ID="TxtWorkContent" runat="server" Width="154px"></asp:TextBox></td>
                    <td width="20">
                    </td>
                </tr>
            </table>
            <hr />
            <table>
                <tr>
                    <td width="120">
                        ���벿�ţ�
                    </td>
                    <td>
                        <asp:DropDownList ID="DdlReqGroup" runat="server" Width="245px">
                        </asp:DropDownList></td>
                    <td style="width: 20px">
                    </td>
                    <td width="120">
                        �����û���
                    </td>
                    <td>
                        <asp:TextBox ID="TxtReqUser" runat="server" Width="250px"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td width="120">
                        �������ڣ�</td>
                    <td>
                        <asp:TextBox ID="TxtReqDate" runat="server" Width="240px"></asp:TextBox></td>
                    <td>
                    </td>
                    <td width="120">
                        ����״̬��</td>
                    <td>
                        <asp:TextBox ID="TxtReqState" runat="server" Width="250px"></asp:TextBox></td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <hr />
            <asp:Button ID="BtnQuery" runat="server" Text="��ѯ" OnClick="BtnQuery_Click" CssClass="button_1"
                UseSubmitBehavior="False" />
        </div>
        <div style="overflow-x:auto; overflow-y:none; width: 100%" id="UserSaveData" runat="server">
            <asp:GridView ID="GVData" runat="server" AutoGenerateColumns="False" CssClass="gv"
                Width="2820px" OnRowDataBound="GVData_RowDataBound" BorderWidth="1px" OnRowCommand="GVData_RowCommand">
                <Columns>
                    <asp:TemplateField ShowHeader="False" HeaderText="�´�/ȡ��">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgRelease" runat="server" CausesValidation="False" CommandArgument='<%# Eval("objid")+"^"+Eval("rowversion") %>'
                                CommandName="Release" ImageUrl="~/images/issue.png" ToolTip="�´�"  />
                            <asp:ImageButton ID="ImgCancelRelease" runat="server" CausesValidation="False" CommandArgument='<%# Eval("objid")+"^"+Eval("rowversion") %>'
                                CommandName="CancelRelease" ImageUrl="~/images/GoPreviousState.png" OnClientClick='return confirm("ȷ��Ҫȡ���´���");'
                                ToolTip="ȡ���´�" />
                            <asp:Image ID="ImgNotAccess" runat="server" ImageUrl="~/images/notaccess1.gif" />
                        </ItemTemplate>
                        <HeaderStyle Width="100px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="�´�����">
                        <HeaderStyle Width="100px" />
                        <ItemTemplate>
                            <asp:TextBox ID="TxtReleaseQty" runat="server" Width="80px"></asp:TextBox>
                            <asp:Label ID="LblReleaseQty" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="require_qty" HeaderText="��������">
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="requisition_id" HeaderText="����ID">
                        <HeaderStyle Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="psflag_str" HeaderText="�������">
                        <HeaderStyle Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="part_no" HeaderText="������">
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="���������Ӣ�ģ�">
                        <ItemTemplate>
                            <p style="text-overflow: ellipsis; white-space: nowrap; overflow: hidden; width: 200px"
                                title="<%# Server.HtmlEncode(Eval("part_name_e").ToString())%>">
                                <%# Eval("part_name_e")%>
                            </p>
                        </ItemTemplate>
                        <HeaderStyle Width="200px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="package_no" HeaderText="�������">
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="�������">
                        <ItemTemplate>
                            <p style="text-overflow: ellipsis; white-space: nowrap; overflow: hidden; width: 200px"
                                title="<%# Server.HtmlEncode(Eval("package_name").ToString())%>">
                                <%# Eval("package_name")%>
                            </p>
                        </ItemTemplate>
                        <HeaderStyle Width="200px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="project_id" HeaderText="��Ŀ">
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="place_name" HeaderText="���ճ���">
                        <HeaderStyle Width="200px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="receipt_date_str" HeaderText="��������">
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="��װ">
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkCrane" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle Width="60px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="receiver" HeaderText="������">
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="receiver_ic" HeaderText="IC">
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="receiver_contract" HeaderText="��ϵ��ʽ">
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="project_block" HeaderText="�ֶ�">
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="project_system" HeaderText="ϵͳ">
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="������������ģ�">
                        <ItemTemplate>
                            <p style="text-overflow: ellipsis; white-space: nowrap; overflow: hidden; width: 200px"
                                title="<%# Server.HtmlEncode(Eval("part_name").ToString())%>">
                                <%# Eval("part_name")%>
                            </p>
                        </ItemTemplate>
                        <HeaderStyle Width="200px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="record_time_str" HeaderText="��������">
                        <HeaderStyle Width="200px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="recorder" HeaderText="������">
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="req_group_name" HeaderText="���벿��">
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="rowstate_zh" HeaderText="״̬">
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="objid" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                    <asp:BoundField DataField="rowversion" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                </Columns>
            </asp:GridView>
            <br />
        </div>
        <div id="popuContent">
            <table style="font-size: 10pt; background: #000080; color: #FFFFFF; width: 100%">
                <tr>
                    <td>
                        ѡ��ȱ��ԭ��</td>
                    <td style="text-align: right">
                        <input type="button" value="x" style="height: 18px; width: 18px; font-size: 7pt;
                            padding: 0; vertical-align: middle; text-align: center" id="BtnQuitPopuWin1" onclick="close_popudiv();" /></td>
                </tr>
            </table>
            <table style="width:100%">
                <tr>
                    <td style="padding:10px">
                        <asp:DropDownList ID="DdlLackReason" runat="server" Width="340px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <hr />
            <table style="width:100%">
                <tr>
                    <td style="text-align:right">
                        <asp:Button ID="BtnLackRelease" runat="server" Text="ȷ��" CssClass="button_1" OnClick="BtnLackRelease_Click" />&nbsp;
                        &nbsp;<input id="BtnCloseLackReasenPopuDiv" type="button" value="ȡ��" class="button_1" onclick="close_popudiv();"/>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HidObjId" runat="server" />
            <asp:HiddenField ID="HidRowversion" runat="server" />
            <asp:HiddenField ID="HidReleaseQty" runat="server" />
            <asp:HiddenField ID="HidAction" runat="server" />
        </div>
    </form>
</body>
</html>
