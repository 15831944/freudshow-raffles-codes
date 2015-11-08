Option Explicit
Dim fso, fso2
Dim fls, fls2, fl, sfd
Dim date_str As String
Dim var As String
Dim sp As Variant
Dim tuhao As String
Dim banbenhao As String
Dim deviser_IC As String
Dim bili As String
Dim g As Integer
Dim u As Integer
Dim list()
Dim dwgpath As String
Dim jpgPath As String
Dim FilterType(0) As Integer
Dim FilterData(0) As Variant

Private Sub cmdInsert_Click()
UserForm1.Hide
Set fso = CreateObject("scripting.filesystemobject")

Dim w As Integer
w = g
ThisDrawing.ActiveLayer = ThisDrawing.Layers.Item("0")

Dim i, j As Integer
Dim nameNo As String

Dim scalefactor As Double
Dim rotationAngle As Double
Dim imageName As String
Dim rasterObj As AcadRasterImage
Dim n As Integer
Dim strname As String
Dim str As String
Dim st As String
Dim fileobj As File

Dim blockRefObj As AcadBlockReference
Dim insertionPoint(0 To 2) As Double
Dim insertionPnt As Variant
Dim distance_H As Variant
Dim distance_W As Variant
Dim namepath As String
Dim nextpoint As Variant

If tbScale.Text = "" Then
   MsgBox "���������ֵ��"
   UserForm1.Show
Else

st = "\\172.16.7.55\sign$\dwg\"    '�����ļ���
 
Dim ta2 As String
Dim tb2 As String
Dim re2 As Integer
For re2 = 1 To g + 1
    ta2 = Mid(ListBox1.list(re2 - 1), 1, 7)
    tb2 = st + "\" + ta2 & ".dwg"
    If fso2.FileExists(tb2) Then
      ' MsgBox "�ļ����ڣ�"
    Else
       MsgBox "δ�ҵ� " & ListBox1.list(re2 - 1) & " ����ǩ��ͼƬ������ϵ��������飡"
       UserForm1.Show
       Exit Sub
    End If
Next re2

ThisDrawing.SendCommand "UCS" & vbCr & "W" & vbCr & vbCr

insertionPnt = ThisDrawing.Utility.GetPoint(, "Enter a insert point: ")
distance_H = ThisDrawing.Utility.GetDistance(insertionPnt, "Enter a height point: ")

nextpoint = ThisDrawing.Utility.GetPoint(insertionPnt, "Enter the Width point: ")
distance_W = Abs(insertionPnt(0) - nextpoint(0))
scalefactor = 1 * CDbl(distance_H) / 17  '������������,10���Ըĵ�СЩ.
rotationAngle = 0

''''''''''''''''''''''ǩ����������ʾ
'MsgBox "�߶�= " & distance_H
'MsgBox "����= " & distance_W
Dim datetext As AcadText
Dim insertionpntdate(2) As Double
Dim height As Double

insertionpntdate(0) = insertionPnt(0) + distance_W + 5 * tbScale.Text
insertionpntdate(1) = insertionPnt(1) + 2 * tbScale.Text
insertionpntdate(2) = insertionPnt(2)
insertionPnt(0) = insertionPnt(0) + (distance_W - 15 * tbScale.Text) / 2   '12.5->
height = distance_H / 3

Dim r As Integer
If w > 0 Then
For i = 0 To w
    namepath = st & "\" & list(i) & ".dwg"
    Set blockRefObj = ThisDrawing.ModelSpace.InsertBlock(insertionPnt, namepath, 1 * scalefactor, 1 * scalefactor, 1#, 0) '5+  �ڣط����ϷŴ�
    Set datetext = ThisDrawing.ModelSpace.AddText(date_str, insertionpntdate, height)
    insertionPnt(1) = insertionPnt(1) - distance_H     '�޸�ͼƬ���¼�� 0.7 ->
    insertionpntdate(1) = insertionpntdate(1) - distance_H
    ThisDrawing.SendCommand "dr" & vbCr & "object" & vbCr & "(handent " & Chr(34) & blockRefObj.Handle & Chr(34) & ")" & vbCr & vbCr & vbCr
Next i
End If
End If
UserForm1.Hide
End Sub

Private Sub cmdExit_Click()
End
End Sub

Private Sub cmdPreview_Click()
Dim url, xmlhttp, dom, node, xmlDOC, SoapRequest
UserForm1.Hide
ListBox1.Clear

Set fso2 = CreateObject("scripting.filesystemobject")
Image1.Picture = LoadPicture
Image2.Picture = LoadPicture
Image3.Picture = LoadPicture
Image4.Picture = LoadPicture
Image5.Picture = LoadPicture

'date_str = Date
'date_str = Now
Dim date1 As String
date1 = Now
Dim pc As Variant
pc = Split(date1, " ")
date_str = pc(0)

tuhao = tbDrawNumb.Text
banbenhao = tbVersionNumb.Text
deviser_IC = tbIcNumb.Text
bili = tbScale.Text

If deviser_IC = "Y00" Or deviser_IC = "" Then
   MsgBox "�����Ա����IC���ţ�"
   UserForm1.Show
Else
   If Len(deviser_IC) <> 7 Then
      MsgBox "���Ա����IC��������,���������룡"
      UserForm1.Show
   Else

If tuhao = "" And banbenhao = "" Then
   MsgBox "������ͼ�źͰ汾�ţ�"
   UserForm1.Show
ElseIf tuhao = "" And banbenhao <> "" Then
   MsgBox "������ͼ�ţ�"
   UserForm1.Show
ElseIf tuhao <> "" And banbenhao = "" Then
   MsgBox "������汾�ţ�"
   UserForm1.Show
ElseIf tuhao <> "" And banbenhao <> "" Then
    
    SoapRequest = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "utf-8" & Chr(34) & "?>" & _
    "<soap:Envelope xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & " " & _
    "xmlns:xsd=" & Chr(34) & "http://www.w3.org/2001/XMLSchema" & Chr(34) & " " & _
    "xmlns:soap=" & Chr(34) & "http://schemas.xmlsoap.org/soap/envelope/" & Chr(34) & ">" & _
    "<soap:Body>" & _
    "<GetApproveTemplate xmlns=" & Chr(34) & "http://tempuri.org/" & Chr(34) & ">" & _
    "<drawnICNo>" & deviser_IC & "</drawnICNo>" & _
    "<drawingNo>" & tuhao & "</drawingNo>" & _
    "<rev>" & banbenhao & "</rev>" & _
    "</GetApproveTemplate>" & _
    "</soap:Body>" & _
    "</soap:Envelope>"
    url = "http://172.16.7.55/services/drawing.asmx?op=GetApproveTemplate"
    Set xmlDOC = CreateObject("MSXML.DOMDocument")
    xmlDOC.loadXML (SoapRequest)
'    MsgBox SoapRequest

    Set xmlhttp = CreateObject("Msxml2.XMLHTTP")
    xmlhttp.Open "POST", url, False
    xmlhttp.setRequestHeader "Content-Type", "text/xml;charset=utf-8"
    xmlhttp.setRequestHeader "SOAPAction", "http://tempuri.org/GetApproveTemplate"
    xmlhttp.setRequestHeader "Content-Length", Len(SoapRequest)
    xmlhttp.Send (xmlDOC)

    If xmlhttp.Status = 200 Then
        xmlDOC.Load (xmlhttp.responseXML)
'        MsgBox "ִ�н��Ϊ��" & xmlDOC.getElementsByTagName("GetApproveTemplateResult")(0).Text
    Else
        MsgBox "failed"
    End If

var = xmlDOC.getElementsByTagName("GetApproveTemplateResult")(0).Text

If var = "NoDrawing" Then
   MsgBox "��ϵͳ��û���ҵ��ð�ͼֽ��"
ElseIf var = "NoApproveTemplate" Then
   MsgBox "ϵͳ����ڸð�ͼֽ���������ģ�壬����ϵ�����ܵ�½��ECDMS���޸Ĵ�ֽ������ģ�棡"
Else
sp = Split(var, ";")

g = UBound(sp)
ReDim list(g)
For u = 0 To g
   list(u) = Mid(sp(u), 1, 7)
Next

For u = 0 To g
   ListBox1.AddItem sp(u)
Next u

If Mid(var, 9, 1) = ";" Then
   MsgBox "���Ա�Ŀ�����ϵͳ��û���ҵ������������Ƿ�����"
   UserForm1.Show
Else

'Dim jpgPath As String
jpgPath = "\\172.16.7.55\sign$\jpg\"

Dim z As Integer
z = g + 1

Dim ta As String
Dim tb As String
Dim re As Integer
For re = 1 To z
    ta = Mid(ListBox1.list(re - 1), 1, 7)
    tb = jpgPath + ta & ".jpg"
    If fso2.FileExists(tb) Then
      ' MsgBox "�ļ����ڣ�"
    Else
       MsgBox "δ�ҵ� " & ListBox1.list(re - 1) & " ��Ԥ��ǩ��ͼƬ������ϵ��������飡"
       UserForm1.Show
       Exit Sub
    End If
Next re

If z >= 2 Then
   If z = 3 Then
      Image3.Picture = LoadPicture(jpgPath & Mid(ListBox1.list(2), 1, 7) + ".jpg")
   ElseIf z = 4 Then
      Image3.Picture = LoadPicture(jpgPath & Mid(ListBox1.list(2), 1, 7) + ".jpg")
      Image4.Picture = LoadPicture(jpgPath & Mid(ListBox1.list(3), 1, 7) + ".jpg")
   ElseIf z = 5 Then
      Image3.Picture = LoadPicture(jpgPath & Mid(ListBox1.list(2), 1, 7) + ".jpg")
      Image4.Picture = LoadPicture(jpgPath & Mid(ListBox1.list(3), 1, 7) + ".jpg")
      Image5.Picture = LoadPicture(jpgPath & Mid(ListBox1.list(4), 1, 7) + ".jpg")
   End If

   If fso2.FileExists(jpgPath + Mid(ListBox1.list(0), 1, 7) & ".jpg") Then
      Image1.Picture = LoadPicture(jpgPath & Mid(ListBox1.list(0), 1, 7) + ".jpg")
   End If
   Image2.Picture = LoadPicture(jpgPath & Mid(ListBox1.list(1), 1, 7) + ".jpg")
Else
   MsgBox "�������Ϣ��������ϵ�����ܵ�½��ECDMS���޸Ĵ�ֽ������ģ��"
End If
End If
End If
End If
End If
End If
cmdInsert.Enabled = True
UserForm1.Show
End Sub

Private Sub UserForm_Click()

End Sub


Sub AcadStartup()
    On Error Resume Next
    Dim currMenuGroup As AcadMenuGroup
    Set currMenuGroup = ThisDrawing.Application.MenuGroups.Item(1)

    ' �����¹�����
    Dim newToolbar As AcadToolbar
    Set newToolbar = currMenuGroup.Toolbars.Add("YCRO")  '����ǩ��&&ͼֽģ��
    Dim submacro As String

    Dim newMenu As AcadPopupMenu
    Set newMenu = currMenuGroup.Menus.Add("YCRO")  '����ǩ��&&ͼֽģ��
    
    Dim newbutton As AcadToolbarItem
    Dim subMenuItem As AcadPopupMenuItem
    Dim menuseparator As AcadPopupMenuItem

    submacro = "-vbarun userfor" + Chr(32)
    Set subMenuItem = newMenu.AddMenuItem(newMenu.count + 1, "����ǩ��", submacro)
    Set newbutton = newToolbar.AddToolbarButton(newToolbar.count + 1, "����ǩ��", "����ǩ��", submacro)
    newbutton.SetBitmaps "C:\icon\ǩ�ֱ�.bmp", "C:\icon\ǩ�ֱ�.bmp"
    
    submacro = "-vbarun addtemplate" + Chr(32)
    Set subMenuItem = newMenu.AddMenuItem(newMenu.count + 1, "ͼֽģ��", submacro)
    Set newbutton = newToolbar.AddToolbarButton(newToolbar.count + 1, "ͼֽģ��", "ͼֽģ��", submacro)
    newbutton.SetBitmaps "C:\icon\ͼֽģ��.bmp", "C:\icon\ͼֽģ��.bmp"
    
    
    ' ��ʾ������
    newToolbar.Visible = True
    newMenu.InsertInMenuBar (ThisDrawing.Application.MenuBar.count + 1)

    '' ���������̶�����Ļ����ߡ�
    newToolbar.Dock acToolbarDockLeft

End Sub