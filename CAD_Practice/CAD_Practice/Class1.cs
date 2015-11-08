using System;
using System.Collections.Generic;
using System.Text;

using AcadApp= Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Colors;

using System.Configuration;
//acdbmgd.dll����ObjectDBX�й��� ��������е��ཫ���������ʺͱ༭AutoCADͼ���е�ʵ�壬
//��acmgd.dll����AutoCAD�й��ࡣ
//�������ļ�������.NET API�����е�����ࡣ
using System.Management;
using System.IO;
using System.Threading;
using UpdateSoft;
using System.Windows.Forms;
using Microsoft.Win32;
using AdeskInter = Autodesk.AutoCAD.Interop;



namespace CAD_Practice
{

    public class Initializa : IExtensionApplication //��ʼ��������
    {
        public void Initialize()
        {
            
            Editor ed = AcadApp.Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("��ʼ��....");
           // Commands.test0817();
        }
        public void Terminate()
        {
            System.Diagnostics.Trace.WriteLine("Cleaning up...");
        }

    }


    public class Practice
    {
        [CommandMethod("hello")]//Ҫ��������AutoCAD �е��õ�����,�����ʹ�á�CommandMethod�����ԡ�
        public static void test0817()
        {
            Editor ed = AcadApp.Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("Hello AutoCAD!!");

            Runmain();

            NewForm nf = new NewForm();
            nf.Show();

        }

        //C:\Program Files\YCRO\Digital ����·��
        public static  void Runmain()
        {

            bool bCreatedNew;
            Mutex m = new Mutex(false, "myUniqueName", out bCreatedNew);



            RegistryKey YCRO = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Autodesk\AutoCAD\R16.2\ACAD-4001:409\Applications\YRO_Digital");

            object path=  YCRO.GetValue("LOADER");

            SoftUpdate app = new SoftUpdate(path.ToString(), "UpdateProgram.zip");

            app.UpdateFinish += new UpdateState(app_UpdateFinish);

            if (app.IsUpdate)
            {


                System.Diagnostics.Process.Start(Path.GetDirectoryName(path.ToString()) + "\\" + "UpdateSoftProgram.exe");
                Application.Exit();
                closeapp();

            }

            else
            {
                if (bCreatedNew)
                {
                   
                    //try
                    //{
                    //    ////����δ������쳣   
                    //    //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                    //    ////����UI�߳��쳣   
                    //    //Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                    //    ////�����UI�߳��쳣   
                    //    //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                    //    Application.EnableVisualStyles();
                    //    Application.SetCompatibleTextRenderingDefault(false);
                    //    //LoginForm lform = new LoginForm();
                    //    //if (lform.ShowDialog() == DialogResult.OK)
                    //    //{
                    //    //    string sqlStr = string.Empty;
                    //    //    string user_name = lform.LoginUserName;
                    //    //    User.Get_CurrentUser(user_name);
                    //    //    Application.Run(new MDIForm());
                    //    //}
                    //}
                    //catch (System.Exception ex)
                    //{
                    //    string str = "";
                    //    string strDateInfo = "����Ӧ�ó���δ������쳣��" + DateTime.Now.ToString() + "\r\n";

                    //    if (ex != null)
                    //    {
                    //        str = string.Format(strDateInfo + "�쳣���ͣ�{0}\r\n�쳣��Ϣ��{1}\r\n�쳣��Ϣ��{2}\r\n",
                    //             ex.GetType().Name, ex.Message, ex.StackTrace);
                    //    }
                    //    else
                    //    {
                    //        str = string.Format("Ӧ�ó����̴߳���:{0}", ex);
                    //    }


                    //    writeLog(str);
                    //    MessageBox.Show(str);
                    //    //MessageBox.Show("�������������뼰ʱ��ϵ���ߣ�", "ϵͳ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    Application.Exit();

                    //}
                }
            }
        }

        public  static  void closeapp()
        {
            string progID = "AutoCAD.Application";
            AdeskInter.AcadApplication CADAPP = null;
            try
            {
                CADAPP = (AdeskInter.AcadApplication)System.Runtime.InteropServices.Marshal.GetActiveObject(progID);
                CADAPP.Quit();
                
            }
            catch
            {
                try
                {
                    Type SType = Type.GetTypeFromProgID(progID);
                    CADAPP = (AdeskInter.AcadApplication)System.Activator.CreateInstance(SType, true);
                    CADAPP.Quit();
                }
                catch
                {

                }
            }
        }

        static void app_UpdateFinish()
        {
            //MessageBox.Show("������ɣ���������������");
        }

        static void writeLog(string str)
        {
            //string pathstr = Path.Combine(User.rootpath, "\\ErrLog");
            //string pathstr = User.rootpath + "\\" + "ErrLog";
            //if (!Directory.Exists(pathstr))
            //{
            //    Directory.CreateDirectory(pathstr);
            //}

            //using (StreamWriter sw = new StreamWriter(pathstr + "\\" + "ErrLog.txt", true))
            //{
            //    sw.WriteLine(str);
            //    sw.WriteLine("---------------------------------------------------------");
            //    sw.Close();
            //}
        }



        // ����һ��ֱ�� 
        [CommandMethod("addlc")]
        public void Main()
        {
            ObjectIdCollection objcoll = new ObjectIdCollection();

            //   objcoll= AddLineandcirclr();
            //CreateGroup(objcoll, "ASDK_TEST_GROUP");����
        }

    }
}



//[assembly: ExtensionApplication(typeof(LDotNetApi.Initializa))]
//[assembly: CommandClass(typeof(LDotNetApi.Commands))]

namespace LDotNetApi
{

    public class Initializa : IExtensionApplication //��ʼ��������
    {
        public void Initialize()
        {
            Editor ed = AcadApp.Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("��ʼ��....");
           // Commands.test0817();
        }
        public void Terminate()
        {
            System.Diagnostics.Trace.WriteLine("Cleaning up...");
        }
    }


    public class Commands //����ʵ����
    {



        public ObjectIdCollection AddLineandcirclr()
        {
            ObjectIdCollection objcoll = new ObjectIdCollection();
            //���̶����ȵõ����ݿ⣬Ȼ�����δ򿪿������¼���������ʵ�壬���رտ������¼��

            //    Database db = HostApplicationServices.WorkingDatabase;//��õ�ǰ�����ռ�����ݿ�
            AcadApp.Document doc = AcadApp.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            //  ��ʼ������Ҳ������CAD�м��붫��
            Circle circle;
            ObjectId objcid, objlid;
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tran.GetObject(db.BlockTableId, OpenMode.ForRead);// �õ����
                // //  ���AutoCAD���AutoCAD�����뵽ͼ���еĶ������Ϣ�������������
                BlockTableRecord btr = (BlockTableRecord)tran.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);//�õ�ģ�Ϳռ�Ŀ���¼
                //***                 // ʹ�õ�ǰ�Ŀռ�Id����ȡ����¼����ע�������Ǵ�������д��
                // ***   BlockTableRecord btr = (BlockTableRecord)tran.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);


                Line l = new Line(new Point3d(0, 0, 0), new Point3d(200, 200, 0));

                // ����һ��Circle��������ʾ��Ҫ���ɵ�Բ������ĵڶ�������ΪԲ�ķ��򣬾��ǰ�Բ������ʲô���ϣ���ΪAutoCAD����һ�㶼��ƽ�����⣬�����һ�㶼����������������//z�᷽��

                circle = new Circle(new Point3d(200, 200, 0), new Vector3d(0.0, 0.0, 1.0), 50); // Vector3d.ZAxis


                objlid = btr.AppendEntity(l);//��ֱ�߼��뵽ģ�Ϳռ���    //   �����¼����Բ�������Ϣ
                tran.AddNewlyCreatedDBObject(l, true);

                objcoll.Add(objlid);


                objcid = btr.AppendEntity(circle);
                objcoll.Add(objcid);
                tran.AddNewlyCreatedDBObject(circle, true);
                tran.Commit();  //һ��������ϲ��������Ǿ��ύ�������������������ĸı�ͱ������ˡ���

                //Ȼ��������������Ϊ�����Ѿ��������صĲ����������������ݿ�פ�����󣬿������٣�

            }

            System.Diagnostics.Trace.WriteLine(objcid.ToString());

            //��Ĵ����Ǹ����û����������е�ѡ�����ı�Բ����ɫ��
            Editor ed = doc.Editor;
            //Editor ed = Entities.Editor;
            // PromptKeywordOptions����һ���ؼ����б�ѡ��
            PromptKeywordOptions opt = new PromptKeywordOptions("ѡ����ɫ[��ɫ(G)/��ɫ(B)]<��ɫ(R)>");
            //����ؼ����б�
            opt.Keywords.Add("R");
            opt.Keywords.Add("G");
            opt.Keywords.Add("B");
            //��ȡ�û�����Ĺؼ���
            PromptResult result = ed.GetKeywords(opt);
            //�ж��Ƿ������˶���Ĺؼ���
            if (result.Status == PromptStatus.OK)
            {
                //�����û�ѡ��Ĺؼ��֣����ı�Բ����ɫ
                switch (result.StringResult)
                {
                    case "R":
                        // PutColorIndex��ZHFARX���иı������ɫ�ĺ���

                        ChangeColorIndex(objcoll, 1);
                        break;
                    case "G":
                        ChangeColorIndex(objcoll, 3);
                        break;
                    case "B":
                        ChangeColorIndex(objcoll, 5);
                        break;
                }
            }
            return objcoll;
        }

        public static void ChangeColorIndex(ObjectIdCollection c, int i)
        {
            AcadApp.Document doc = AcadApp.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tran.GetObject(db.BlockTableId, OpenMode.ForRead);// �õ����
                BlockTableRecord btr = (BlockTableRecord)tran.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                foreach (ObjectId obj in c)
                {
                    Entity acEnt = tran.GetObject(obj, OpenMode.ForWrite) as Entity;

                    acEnt.ColorIndex = i;
                }
                tran.Commit();
            }
        }

        public void CreateGroup(ObjectIdCollection objIds, string groupName)
        {
            Group gp = new Group(groupName, true);         //������ΪgroupName����
            Database db = AcadApp.Application.DocumentManager.MdiActiveDocument.Database;
            //��ȡ����������������������ݿ���в���
            using (Transaction ta = db.TransactionManager.StartTransaction())
            {
                DBDictionary dict = (DBDictionary)ta.GetObject(db.GroupDictionaryId, OpenMode.ForWrite, false);����//��ȡ�����ڵ�"Group"�ֵ�		
                dict.SetAt(groupName, gp);����//��"Group"�ֵ��м��������		
                foreach (ObjectId thisId in objIds)
                {
                    gp.Append(thisId);������//�����м���ObjectIdΪthisId��ʵ��
                }
                ta.AddNewlyCreatedDBObject(gp, true);
                ta.Commit();
            }
        }

    }

    public class CADNote //��ʾע��
    {

        [CommandMethod("Getpoint")]
        public void SelectOt()  //"please select a point ,then get the coordinate :"
        {
            PromptPointOptions pmops = new PromptPointOptions("please select a point ,then get the coordinate :");
            
            PromptPointResult pmres;
            Editor ed = AcadApp.Application.DocumentManager.MdiActiveDocument.Editor;
            
            pmres = ed.GetPoint(pmops);

            if (pmres.Status != PromptStatus.OK)
                ed.WriteMessage("Error");
            else
            {
                ed.WriteMessage("You selected point " + pmres.Value.ToString());
            }

        }

        [CommandMethod("zhushi")]
        public static void zhushi()
        {

            //1getstsring()

            AcadApp.Document acDoc = AcadApp.Application.DocumentManager.MdiActiveDocument;

            //PromptStringOptions pStrOpts = new PromptStringOptions("\nEnter your name: ");

            //pStrOpts.AllowSpaces = true;//AllowSpaces���Կ�����ʾ�Ƿ��������ո��������Ϊfalse�����ո������ֹ���롣

            //PromptResult pStrRes = acDoc.Editor.GetString(pStrOpts);

            //AcadApp.Application.ShowAlertDialog("The name entered was: " + pStrRes.StringResult);



            //GetPoint������ʾ�û���Command��ʾʱָ��һ���㡣PromptPointOptions�����UseBasePoint���Ժ�BasePoint���Կ����Ƿ�ӻ������һ����Ƥ���ߡ�PromptPointOptions�����Keywords���������������ָ�����⻹������Command��ʾ��괦����Ĺؼ��֡�

            //Database acCurDb = acDoc.Database;

            //PromptPointResult pPtRes;

            //PromptPointOptions pPtOpts = new PromptPointOptions("1");

            //// Prompt for the start point

            //pPtOpts.Message = "\nEnter the start point of the line: ";

            //pPtRes = acDoc.Editor.GetPoint(pPtOpts);

            //Point3d ptStart = pPtRes.Value;

            //// Exit if the user presses ESC or cancels the command

            //if (pPtRes.Status == PromptStatus.Cancel) return;

            //// Prompt for the end point

            //pPtOpts.Message = "\nEnter the end point of the line: ";

            //pPtOpts.UseBasePoint = true;

            //pPtOpts.BasePoint = ptStart;

            //pPtRes = acDoc.Editor.GetPoint(pPtOpts);

            //Point3d ptEnd = pPtRes.Value;

            //if (pPtRes.Status == PromptStatus.Cancel) return;

            //// Start a transaction

            //using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            //{

            //    BlockTable acBlkTbl;

            //    BlockTableRecord acBlkTblRec;

            //    // Open Model space for write

            //    acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

            //    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

            //    // Define the new line

            //    Line acLine = new Line(ptStart, ptEnd);

            //    // Add the line to the drawing

            //    acBlkTblRec.AppendEntity(acLine);

            //    acTrans.AddNewlyCreatedDBObject(acLine, true);

            //    // Zoom to the extents or limits of the drawing

            //    acDoc.SendStringToExecute("._zoom _all ", true, false, false);

            //          acTrans.Commit();

            //}


            //GetKeywords������ʾ�û���Command��ʾ��괦����һ���ؼ��֡�PromptKeywordOptions�����������Ƽ��뼰��ʾ��Ϣ���ַ�ʽ��PromptKeywordOptions�����Keywords���������������Command��ʾ��괦����Ĺؼ��֡�
            //��AutoCAD�����л�ȡ�û�����Ĺؼ���
            //������PromptKeywordOptions�����AllowNone��������Ϊfalse��������ֱ�ӻس���������ʹ�û���������һ���ؼ��֡�Keywords���������������ĺϷ��ؼ��֡�

            PromptKeywordOptions pKeyOpts = new PromptKeywordOptions("");

            pKeyOpts.Message = "\nEnter an option ";

            pKeyOpts.Keywords.Add("Line");

            pKeyOpts.Keywords.Add("Circle");

            pKeyOpts.Keywords.Add("Arc");

            //pKeyOpts.AllowNone = false;
            pKeyOpts.Keywords.Default = "Arc";

            pKeyOpts.AllowNone = true;

            PromptResult pKeyRes = acDoc.Editor.GetKeywords(pKeyOpts);

            AcadApp.Application.ShowAlertDialog("Entered keyword: " + pKeyRes.StringResult);

            ////���û��ѺõĹؼ�����ʾ��ʽ������û���Enter����û�����룩ʱ�ṩһ��ȱʡֵ��ע����������СС�Ķ�


            //4
            //PromptIntegerOptions pIntOpts = new PromptIntegerOptions("");

            //pIntOpts.Message = "\nEnter the size or ";

            //// Restrict input to positive and non-negative values

            ////��������������0;

            //pIntOpts.AllowZero = false;

            //pIntOpts.AllowNegative = false;

            //// Define the valid keywords and allow Enter

            ////����Ϸ��ؼ��ֲ�����ֱ�Ӱ�Enter��;

            //pIntOpts.Keywords.Add("Big");

            //pIntOpts.Keywords.Add("Small");

            //pIntOpts.Keywords.Add("Regular");

            //pIntOpts.Keywords.Default = "Regular";

            //pIntOpts.AllowNone = true;

            //// Get the value entered by the user

            ////��ȡ�û������ֵ

            //PromptIntegerResult pIntRes = acDoc.Editor.GetInteger(pIntOpts);

            //if (pIntRes.Status == PromptStatus.Keyword)
            //{

            //  AcadApp.  Application.ShowAlertDialog("Entered keyword: " + pIntRes.StringResult);

            //}

            //else
            //{

            //    AcadApp.Application.ShowAlertDialog("Entered value: " + pIntRes.Value.ToString());

            //}

            ////PromptKeywordOptions opts=new PromptKeywordOptions("Offset Project");������//�û�ѡ�������Բ�������ؼ������û����������н���ѡ��������Բ����ͶӰ��ƽ��
            ////opts.Keywords.Add("Project");��������������������//����ؼ���"Project"������ʾ����Բ����ͶӰ	
            ////opts.Keywords.Add("Offset");��������������������//����ؼ���"Offset"������ʾ����Բ����ƽ��	
            ////opts.Keywords.Default="Project";����������������//����ȱʡ�Ĺؼ���Ϊ"Project"�����û�ֱ�Ӱ��ո��س��Ļ����൱�����������м�����"Project"		
            ////PromptResult resKey=ed.GetKeywords(opts);       //��ȡ�û�����Ĺؼ���




        }

    }



    public class CreateCircle
    {
        [CommandMethod("foreach")]
        public void StepthroughBlockrecord()
        {
            AcadApp.Document doc = AcadApp.Application.DocumentManager.MdiActiveDocument;
            Database d = doc.Database;

            using (Transaction t = d.TransactionManager.StartTransaction())
            {

                doc.Editor.WriteMessage(d.TransactionManager.NumberOfActiveTransactions.ToString());//�����ĸ���
                doc.Editor.WriteMessage("\n" + d.TransactionManager.TopTransaction.ToString()); //������Ļ���˵���µ�����
                doc.Editor.WriteMessage("\n" + t.ToString()); //������Ļ���˵���µ�����
                //�ڳ���ִ�й����У�Ϊ�˻ع������Ĳ����޸ģ����Խ�һ������Ƕ������һ��������

                BlockTable acbt = t.GetObject(d.BlockTableId, OpenMode.ForRead) as BlockTable;
                //BlockTableRecord acBlkTblRec = t.GetObject(acbt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
                BlockTableRecord acBlkTblRec = t.GetObject(d.CurrentSpaceId, OpenMode.ForRead) as BlockTableRecord;

                // Step through the Block table record��������¼
                foreach (ObjectId asObjId in acBlkTblRec)
                {
                    // doc.Editor.WriteMessage("\nDXF name: " + asObjId.ObjectClass.DxfName);
                    doc.Editor.WriteMessage("\nObjectID: " + asObjId.ToString());

                    doc.Editor.WriteMessage("\nHandle: " + asObjId.Handle.ToString());

                    doc.Editor.WriteMessage("\n");
                }
            }

            ////acbt.UpgradeOpen();�������򿪶���
            ////acbt.DowngradeOpen();

            //// �Զ��ķ�ʽ��ͼ���
            //LayerTable acLyrTbl = acTrans.GetObject(d.LayerTableId, OpenMode.ForRead) as LayerTable;
            ////����ͼ�㲢��ͼ�����ԡ�Door����ͷ��ͼ������Ϊд�򿪷�ʽ;
            //foreach (ObjectId acObjId in acLyrTbl)
            //{
            //    LayerTableRecord acLyrTblRec = acTrans.GetObject(acObjId, OpenMode.ForRead) as LayerTableRecord;

            //    //���ͼ�����Ƿ��ԡ�Door����ͷ

            //    if (acLyrTblRec.Name.StartsWith("Door", StringComparison.OrdinalIgnoreCase) == true)
            //    {

            //        //����Ƿ�Ϊ��ǰ�㣬���򲻶���  // Check to see if the layer is current, if so then do not freeze it

            //        if (acLyrTblRec.ObjectId != d.Clayer)
            //        {
            //            acLyrTblRec.UpgradeOpen();  // Change from read to write mode�����򿪷�ʽ
            //            acLyrTblRec.IsFrozen = true;    // Freeze the layer����ͼ��
            //        }
            //    }
            //}

            //acTrans.Commit(); //�ύ�޸Ĳ��ر�����




        }  //��������¼ ++ �������򿪶���

        //����һ����
        public ObjectId CreateLayer()
        {


            ObjectId layerId; //�����غ�����ֵ

            Database db = HostApplicationServices.WorkingDatabase;

            Transaction trans = db.TransactionManager.StartTransaction();

            //����ȡ�ò����

            LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForWrite);

            //���EmployeeLayer���Ƿ���ڡ���


            if (lt.Has("EmployeeLayer"))
            {

                layerId = lt["EmployeeLayer"];
            }
            else
            {

                //���EmployeeLayer�㲻���ڣ��ʹ�����

                LayerTableRecord ltr = new LayerTableRecord();

                ltr.Name = "EmployeeLayer"; //���ò������

                ltr.Color = Color.FromColorIndex(ColorMethod.ByAci, 2);

                layerId = lt.Add(ltr);

                trans.AddNewlyCreatedDBObject(ltr, true);

            }

            trans.Commit();

            trans.Dispose();

            return layerId;

        }

        [CommandMethod("trannest")]
        public void transNest()
        {
            #region ����Ƕ��

            //���������������������  
            AcadApp.Document doc = AcadApp.Application.DocumentManager.MdiActiveDocument;
            Database d = AcadApp.Application.DocumentManager.MdiActiveDocument.Database;

            TransactionManager acTransMgr = d.TransactionManager;
            using (Transaction acTrans1 = acTransMgr.StartTransaction())
            {
                //��ӡ��ǰ�����ĸ���     
                doc.Editor.WriteMessage("\nNumber of transactions active: " + acTransMgr.NumberOfActiveTransactions.ToString());

                BlockTable acBlkTbl = acTrans1.GetObject(d.BlockTableId, OpenMode.ForRead) as BlockTable;

                BlockTableRecord acBlkTblRec = acTrans1.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                Circle acCirc = new Circle(); acCirc.Center = new Point3d(5, 5, 0); acCirc.Radius = 3;

                acBlkTblRec.AppendEntity(acCirc);

                acTrans1.AddNewlyCreatedDBObject(acCirc, true);

                //�����ڶ�������

                using (Transaction acTrans2 = acTransMgr.StartTransaction())
                {

                    doc.Editor.WriteMessage("\nNumber of transactions active: " + acTransMgr.NumberOfActiveTransactions.ToString());

                    acCirc.ColorIndex = 5;          // Change the circle's color�޸�Բ����ɫ

                    Line acLine = new Line(new Point3d(2, 5, 0), new Point3d(10, 7, 0));

                    acLine.ColorIndex = 3;

                    acBlkTblRec.AppendEntity(acLine);

                    acTrans2.AddNewlyCreatedDBObject(acLine, true);

                    //��������������

                    using (Transaction acTrans3 = acTransMgr.StartTransaction())
                    {

                        doc.Editor.WriteMessage("\nNumber of transactions active: " + acTransMgr.NumberOfActiveTransactions.ToString());

                        acCirc.ColorIndex = 3;

                        // Update the display of the drawing����ͼ����ʾ

                        doc.Editor.WriteMessage("\n");

                        // doc.Editor.Regen();

                        //ѯ�ʱ��ֻ���ȡ�������������е��޸�   

                        PromptKeywordOptions pKeyOpts = new PromptKeywordOptions("");

                        pKeyOpts.Message = "\nKeep color change ";
                        pKeyOpts.Keywords.Add("Yes"); pKeyOpts.Keywords.Add("No");
                        pKeyOpts.Keywords.Default = "No"; pKeyOpts.AllowNone = true;

                        PromptResult pKeyRes = doc.Editor.GetKeywords(pKeyOpts);

                        if (pKeyRes.StringResult == "No")
                        {
                            //   t.Abort(); // �������������ǿ���ʹ��Abort�����ع��������������޸�

                            acTrans3.Abort(); //ȡ������3�е��޸�
                        }
                        else
                        {
                            acTrans3.Commit(); //��������3�е��޸�
                        }
                    }

                    doc.Editor.WriteMessage("\nNumber of transactions active: " + acTransMgr.NumberOfActiveTransactions.ToString());

                    acTrans2.Commit();
                }

                doc.Editor.WriteMessage("\nNumber of transactions active: " + acTransMgr.NumberOfActiveTransactions.ToString());

                acTrans1.Commit();      // Keep the changes to transaction 1��������1�е��޸�


            }
            #endregion
        }



        //����ģ�Ϳռ䡢ͼֽ�ռ��ǰ�ռ�  ��öԿ���¼�����ú��������һ����ֱ�ߡ�
        [CommandMethod("as")]
        public static void AccessSpace()
        {

            AcadApp.Document acDoc = AcadApp.Application.DocumentManager.MdiActiveDocument;

            Database acCurDb = acDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                BlockTableRecord acBlkTblRec;

                // Request which table record to openѯ�ʴ��������¼���ռ䣩

                PromptKeywordOptions pKeyOpts = new PromptKeywordOptions("");

                pKeyOpts.Message = "\nEnter whichspace to create the line in ";

                pKeyOpts.Keywords.Add("Model");

                pKeyOpts.Keywords.Add("Paper");

                pKeyOpts.Keywords.Add("Current");

                pKeyOpts.AllowNone = false;

                pKeyOpts.AppendKeywordsToMessage = true;

                PromptResult pKeyRes = acDoc.Editor.GetKeywords(pKeyOpts);

                if (pKeyRes.StringResult == "Model")                    //��Block���ȡModel�ռ��ObjectID
                {

                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                }

                else if (pKeyRes.StringResult == "Paper")              //��Block���ȡPaper�ռ��ObjectID
                {

                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.PaperSpace], OpenMode.ForWrite) as BlockTableRecord;
                }
                else
                {
                    //�����ݿ��ȡ��ǰ�ռ��ObjectID
                    acBlkTblRec = acTrans.GetObject(acCurDb.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;
                }


                ////����ͼ�������е�������ʽ

                acCurDb.Pdmode = 34;

                acCurDb.Pdsize = 1;

                //******************************
                // Create an in memory circle���ڴ洴��һ��Բ

                using (Circle acCirc = new Circle())
                {

                    acCirc.Center = new Point3d(2, 2, 0);

                    acCirc.Radius = 5;

                    // Adds the circle to an object array��Բ��ӵ���������

                    DBObjectCollection acDBObjColl = new DBObjectCollection();

                    acDBObjColl.Add(acCirc);

                    // Calculate the regions based oneach closed loop

                    //����ÿ���ջ���������

                    DBObjectCollection myRegionColl = new DBObjectCollection();

                    myRegionColl = Region.CreateFromCurves(acDBObjColl);

                    Region acRegion = myRegionColl[0] as Region;

                    acBlkTblRec.AppendEntity(acRegion);

                    acTrans.AddNewlyCreatedDBObject(acRegion, true);

                    acCirc.ColorIndex = 1;
                    acBlkTblRec.AppendEntity(acCirc);

                    acTrans.AddNewlyCreatedDBObject(acCirc, true);

                    // Dispose of the in memory circlenot appended to the database

                    //�����ڴ��е�Բ������ӵ����ݿ�;

                }

                acTrans.Commit();

            }

        }


        #region  ѡ�񼯲���
        //ѡ��
        [CommandMethod("sel", CommandFlags.UsePickSet)]
        public static void CheckForPickfirstSelection()
        {

            AcadApp.Document acDoc = AcadApp.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            Editor acDocEd = AcadApp.Application.DocumentManager.MdiActiveDocument.Editor;

            //// Get the PickFirst selection set��ȡPickFirstѡ��

            //PromptSelectionResult acSSPrompt;
            //acSSPrompt = acDocEd.SelectImplied();
            //SelectionSet acSSet;

            //if (acSSPrompt.Status == PromptStatus.OK) // �����ʾ״̬OK��˵����������ǰѡ���˶���;
            //{
            //    acSSet = acSSPrompt.Value;
            //    AcadApp.Application.ShowAlertDialog("Number of objects in Pickfirst selection: " + acSSet.Count.ToString());
            //}
            //else
            //{
            //    AcadApp.Application.ShowAlertDialog("Number of objects in Pickfirst selection: 0");
            //}

            // ���ѡ��????
            //  //��ʾѡ����Ļ�ϵĶ��󲢱���ѡ��
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                PromptSelectionResult acSSPrompt = acDoc.Editor.GetSelection();// �����ͼ������ѡ�����

                // �����ʾ״̬OK����ʾ��ѡ�����
                if (acSSPrompt.Status == PromptStatus.OK)
                {
                    SelectionSet acSSet = acSSPrompt.Value;

                    // ����ѡ���ڵĶ���
                    foreach (SelectedObject acSSObj in acSSet)
                    {
                        // ȷ�Ϸ��ص��ǺϷ���SelectedObject����

                        if (acSSObj != null)
                        {
                            // ��д����ѡ����

                            Entity acEnt = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as Entity;

                            if (acEnt != null)
                            {
                                acEnt.ColorIndex = 3; // ��������ɫ�޸�Ϊ��ɫ
                            }
                        }
                    } acTrans.Commit();
                }
            }


        }



        //�ϲ�ѡ��
        [CommandMethod("ms")]
        public static void MergeSelectionSets()
        {

            Editor acDocEd = AcadApp.Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSelectionResult acSSPrompt;

            acSSPrompt = acDocEd.GetSelection();

            SelectionSet acSSet1;

            ObjectIdCollection acObjIdColl = new ObjectIdCollection();

            if (acSSPrompt.Status == PromptStatus.OK)
            {
                acSSet1 = acSSPrompt.Value; // ��ȡ��ѡ����
                acObjIdColl = new ObjectIdCollection(acSSet1.GetObjectIds());// ��ObjectIdCollection��׷����ѡ����
            }

            //���Ժϲ����ѡ�񼯣������Ǵ���һ��ObjectIdCollection���϶���Ȼ��Ӷ��ѡ���н�����id����ӵ������С�������ObjectIdCollection������Ӷ���id�������Դ���ɾ������id�����ж���id����ӵ�ObjectIdCollection���϶���󣬿��Ա����ü��ϲ�������Ҫ����ÿ������

            acSSPrompt = acDocEd.GetSelection();
            SelectionSet acSSet2;

            if (acSSPrompt.Status == PromptStatus.OK)
            {
                acSSet2 = acSSPrompt.Value;

                if (acObjIdColl.Count == 0)  // ���ObjectIdCollection���ϴ�С�����Ϊ0�Ͷ����ʼ��
                {

                    acObjIdColl = new ObjectIdCollection(acSSet2.GetObjectIds());
                }

                else
                {
                    foreach (ObjectId acObjId in acSSet2.GetObjectIds())// Step through the second selection set�����ڶ���ѡ��
                    {
                        acObjIdColl.Add(acObjId); // ���ڶ���ѡ���е�ÿ������id��ӵ�������

                    }
                }

            }

            foreach (ObjectId acObjId in acObjIdColl)
            {

                AcadApp.Application.ShowAlertDialog("example : " + acObjId.ToString() + "," + acObjId.Handle.Value);
            }

            AcadApp.Application.ShowAlertDialog("Number of objects selected: " + acObjIdColl.Count.ToString());

        }



        //����ѡ��
        [CommandMethod("FilterSelectionSet")]
        public static void FilterSelectionSet()
        {

            Editor acDocEd = AcadApp.Application.DocumentManager.MdiActiveDocument.Editor;

            //ʹ��ѡ�����������ѡ�񼯹���

            //ѡ���������TypedValue��ʽ��һ�Բ������ɡ�TypedValue�ĵ�һ���������������������ͣ�������󣩣��ڶ�������ΪҪ���˵�ֵ������Բ����������������һ��DXF���룬����ָ��ʹ�����ֹ�������һЩ���ù����������б����¡�


            //ѡ����������԰������˶�����Ի���������������ͨ������һ�������㹻����Ԫ�ص������������ܵĹ��������������ÿ��Ԫ�ش���һ������������

            TypedValue[] acTypValAr = new TypedValue[1]; // ����һ��TypedValue�������������������

            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "CIRCLE"), 0);

            //**************
            // ����TypedValue���鶨���������

            //TypedValue[] acTypValAr = new TypedValue[3];

            //acTypValAr.SetValue(new TypedValue((int)DxfCode.Color, 5), 0);

            //acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "CIRCLE"), 1);

            //acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, "0"), 2);
            //**************


            // ��������������ֵ��SelectionFilter����

            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);

            PromptSelectionResult acSSPrompt;

            acSSPrompt = acDocEd.GetSelection(acSelFtr);

            // ��ʾ״̬OK����ʾ�û���ѡ��

            if (acSSPrompt.Status == PromptStatus.OK)
            {
                SelectionSet acSSet = acSSPrompt.Value;

                AcadApp.Application.ShowAlertDialog("Number of objects selected: " + acSSet.Count.ToString());
            }

            else
            {

                AcadApp.Application.ShowAlertDialog("Numberof objects selected: 0");

            }

        }


        #endregion


        //����������CAD
        //  [CommandMethod("0810")]
        public static void bb1109()
        {
            //������COM���ú󣬳���Ϳ���ʹ�����VBA�еĹ�����


            //���� using Autodesk.AutoCAD.Interop;// 'ʹ��COM��Interop������AutoCAD�еĲ˵�API  
            //����com�¸�AutoCAD 2006 Type Library��AutoCAD/ObjectDBX Common 16.0 Type Library
            //using Autodesk.AutoCAD.Interop.Common;//include acadobject;

            //AcadApplication acAppComObj = null;
            //const string strProgId = "AutoCAD.Application.16.2";// "AutoCAD.Application.18";

            //// Get a running instance of AutoCAD��ȡ�������е�AutoCADʵ����
            //try
            //{
            //    acAppComObj = (AcadApplication)Marshal.GetActiveObject(strProgId);
            //}
            //catch // An error occurs if no instance is runningû���������е�ʵ��ʱ����
            //{
            //    try
            //    {
            //        // Create a new instance of AutoCAD�����µ�AutoCADʵ����
            //        acAppComObj = (AcadApplication)Activator.CreateInstance(Type.GetTypeFromProgID(strProgId), true);
            //    }
            //    catch
            //    {
            //        // If an instance of AutoCAD is not created then message and exit������ʵ�����ɹ�����ʾ��Ϣ���˳���
            //        System.Diagnostics.Trace.WriteLine("Instance of 'AutoCAD.Application'" + " could not be created.");

            //        return;
            //    }
            //}

            //// Display the application and return the name and version��ʾ��õ�Ӧ�ó���ʵ�����������ơ��汾��
            //acAppComObj.Visible = true;
            //System.Diagnostics.Trace.WriteLine("Now running " + acAppComObj.Name + " version " + acAppComObj.Version);

            //// Get the active document
            //AcadDocument acDocComObj;
            //acDocComObj = acAppComObj.ActiveDocument;

            ////// Optionally, load your assembly and start your command or if your assembly
            ////// is demand-loaded, simply start the command of your in-process assembly.
            //////��ѡ�ģ����س��򼯲����������������ڳ����ѱ����أ�ֱ����������ɣ�
            ////acDocComObj.SendCommand("(command " + (char)34 + "NETLOAD" + (char)34 + " " +
            ////                        (char)34 + "c:/myapps/mycommands.dll" + (char)34 + ") ");

            ////acDocComObj.SendCommand("MyCommand ");


            //another
            //AcadMenuGroups mnus = (AcadMenuGroups)app.MenuGroups;
            //AcadPopupMenus pmnus = mnus.Item(1).Menus;
            //int count = 0;
            //foreach (AcadPopupMenu mnu in pmnus)
            //{
            //    if (mnu.OnMenuBar == true) count++;
            //}
            //AcadPopupMenu Menu_SModel = pmnus.Add("&Module");
            //string macro = Convert.ToChar(System.Windows.Forms.Keys.Escape).ToString();
            //AcadPopupMenuItem MenuItem_MainForm = Menu_SModel.AddMenuItem(Menu_SModel.Count, "&MainForm", macro + "SMF ");
            //MenuItem_MainForm.HelpString = "Show main window";
            //AcadPopupMenuItem MenuItem_SetBoard = Menu_SModel.AddMenuItem(Menu_SModel.Count, "Set &Board", macro + "mBoardW ");
            //MenuItem_SetBoard.HelpString = "Set Board Width";

            //if (count == 0)//�����˵���ͬʱ���װ������᲻һ��
            //    pmnus.InsertMenuInMenuBar("&Module", count + 12);//AutoCAD 2006 ��13�������˵���
            //else
            //    pmnus.InsertMenuInMenuBar("&Module", ++count);

        }


        //��ָ���ļ�
        [CommandMethod("open")]
        public static void bn11009()
        {

            string strFileName = "e:\\grid.dwg";
            AcadApp.DocumentCollection acDocMgr = AcadApp.Application.DocumentManager;

            if (System.IO.File.Exists(strFileName))
            {
                acDocMgr.Open(strFileName, false);
                acDocMgr.MdiActiveDocument.Editor.WriteMessage("File " + strFileName + "  exist.");
            }

            else
            {
                acDocMgr.MdiActiveDocument.Editor.WriteMessage("File " + strFileName + " does not exist.");
            }

        }


        #region //�˵�
        Autodesk.AutoCAD.Windows.ContextMenuExtension m_ContextMenu;
        [CommandMethod("ctest")]
        public void AddM()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = AcadApp.Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                m_ContextMenu = new Autodesk.AutoCAD.Windows.ContextMenuExtension();
                m_ContextMenu.Title = "����ͼϵͳ";
                Autodesk.AutoCAD.Windows.MenuItem mi;
                mi = new Autodesk.AutoCAD.Windows.MenuItem("�û�����");
                mi.Click += new EventHandler(MenuUserM_OnClick);
                //mi.Click += MenuUserM_OnClick;
                m_ContextMenu.MenuItems.Add(mi);

                Autodesk.AutoCAD.ApplicationServices.Application.AddDefaultContextMenuExtension(m_ContextMenu);
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("\nError: " + ex.Message + "\n");
            }
        }

        //void RemoveContextMenu()
        //{
        //    try
        //    {
        //        if (m_ContextMenu != null)
        //        {
        //            Autodesk.AutoCAD.ApplicationServices.Application.RemoveDefaultContextMenuExtension(m_ContextMenu);
        //            m_ContextMenu = null;
        //        }
        //    }
        //    catch
        //    { }
        //}

        private void MenuUserM_OnClick(object Sender, EventArgs e)
        {

            //System.Windows.Forms.MessageBox.Show("�û�����");
            AcadApp.DocumentLock docLock = AcadApp.Application.DocumentManager.MdiActiveDocument.LockDocument();

            // Create();

            docLock.Dispose();
        }

        //    PaletteSet ps = null;
        //    [CommandMethod("CreatePalette")]
        //    public void CreatePalette()
        //    {
        //        ps = new PaletteSet("My Test Palette Set");
        //        ps.MinimumSize = new System.Drawing.Size(300, 300);
        //        System.Windows.Forms.UserControl myctrl = new CadLoad.MyCrl();
        //        ps.Add("test", myctrl);
        //        ps.Style = PaletteSetStyles.ShowTabForSingle;
        //        ps.Opacity = 60;
        //        ps.Visible = true;
        //    }

        #endregion

        //�����
        [CommandMethod("IB")]
        public void ImportBlocks()
        {
            
            AcadApp.DocumentCollection dm = AcadApp.Application.DocumentManager;
            Editor ed = dm.MdiActiveDocument.Editor;
            Database destDb = dm.MdiActiveDocument.Database;
            Database sourceDb = new Database(false, true);
            PromptResult sourceFileName;
            try
            {
                //��������Ҫ���û������Եõ�Ҫ����Ŀ����ڵ�ԴDWG�ļ�������
                sourceFileName = ed.GetString("\nEnter the name of the source drawing: ");
                //��ԴDWG���븨�����ݿ�
                sourceDb.ReadDwgFile(sourceFileName.StringResult, System.IO.FileShare.Write, true, "");
                //�ü��ϱ������洢��ID���б�
                ObjectIdCollection blockIds = new ObjectIdCollection();
                Autodesk.AutoCAD.DatabaseServices.TransactionManager tm =
                sourceDb.TransactionManager;
                using (Transaction myT = tm.StartTransaction())
                {
                    //�򿪿��
                    BlockTable bt = (BlockTable)tm.GetObject(sourceDb.BlockTableId,
                    OpenMode.ForRead, false);
                    //�ڿ���м��ÿ����
                    foreach (ObjectId btrId in bt)
                    {
                        BlockTableRecord btr = (BlockTableRecord)tm.GetObject(btrId,
                        OpenMode.ForRead, false);
                        //ֻ���������ͷ�layout��(layout���Ƿ�MS�ͷ�PS�Ŀ�) 
                        if (!btr.IsAnonymous && !btr.IsLayout)
                            blockIds.Add(btrId);
                        btr.Dispose();	//�ͷſ���¼���ñ�����ռ�õ���Դ
                    }
                    bt.Dispose();//�ͷſ�����ñ�����ռ�õ���Դ
                    //û�����ı䣬����Ҫ�ύ����
                    myT.Dispose();
                }
                
                IdMapping mapping = new IdMapping();
                mapping = sourceDb.WblockCloneObjects(blockIds, destDb.BlockTableId, DuplicateRecordCloning.Replace, false);

                ed.WriteMessage("\nCopied " + blockIds.Count.ToString() + " block definitions from " + sourceFileName.StringResult + " to the current drawing.");
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                ed.WriteMessage("\nError during copy: " + ex.Message);
            }
            sourceDb.Dispose();
        }
    }

    #region  ����һImage
    class ViewDWG
    {
        //struct BITMAPFILEHEADER
        //{
        //    public short bfType;
        //    public int bfSize;
        //    public short bfReserved1;
        //    public short bfReserved2;
        //    public int bfOffBits;
        //}



        //public static System.Drawing.Image GetDwgImage(string FileName)
        //{
        //    if (!(File.Exists(FileName)))
        //    {
        //        throw new FileNotFoundException("�ļ�û�б��ҵ�");
        //    }
        //    FileStream DwgF;  //�ļ���
        //    int PosSentinel;  //�ļ��������λ��
        //    BinaryReader br;  //��ȡ�������ļ�
        //    int TypePreview;  //����ͼ��ʽ
        //    int PosBMP;       //����ͼλ�� 
        //    int LenBMP;       //����ͼ��С
        //    short biBitCount; //����ͼ������� 
        //    BITMAPFILEHEADER biH; //BMP�ļ�ͷ��DWG�ļ��в�����λͼ�ļ�ͷ��Ҫ���м���ȥ
        //    byte[] BMPInfo;       //������DWG�ļ��е�BMP�ļ���
        //    MemoryStream BMPF = new MemoryStream(); //����λͼ���ڴ��ļ���
        //    BinaryWriter bmpr = new BinaryWriter(BMPF); //д�������ļ���

        //    System.Drawing.Image myImg = null;
        //    try
        //    {
        //        DwgF = new FileStream(FileName, FileMode.Open, FileAccess.Read);   //�ļ���
        //        br = new BinaryReader(DwgF);
        //        DwgF.Seek(13, SeekOrigin.Begin); //�ӵ�ʮ���ֽڿ�ʼ��ȡ
        //        PosSentinel = br.ReadInt32();  //��13��17�ֽ�ָʾ����ͼ�������λ��
        //        DwgF.Seek(PosSentinel + 30, SeekOrigin.Begin);  //��ָ���Ƶ�����ͼ������ĵ�31�ֽ�
        //        TypePreview = br.ReadByte();  //��31�ֽ�Ϊ����ͼ��ʽ��Ϣ��2 ΪBMP��ʽ��3ΪWMF��ʽ
        //        if (TypePreview == 1)
        //        {
        //        }
        //        else if (TypePreview == 2 || TypePreview == 3)
        //        {
        //            PosBMP = br.ReadInt32(); //DWG�ļ������λͼ����λ��
        //            LenBMP = br.ReadInt32(); //λͼ�Ĵ�С
        //            DwgF.Seek(PosBMP + 14, SeekOrigin.Begin); //�ƶ�ָ�뵽λͼ��
        //            biBitCount = br.ReadInt16(); //��ȡ�������
        //            DwgF.Seek(PosBMP, SeekOrigin.Begin); //��λͼ�鿪ʼ����ȡȫ��λͼ���ݱ���
        //            BMPInfo = br.ReadBytes(LenBMP); //�������ļ�ͷ��λͼ��Ϣ
        //            br.Close();
        //            DwgF.Close();
        //            biH.bfType = 19778; //����λͼ�ļ�ͷ
        //            if (biBitCount < 9)
        //            {
        //                biH.bfSize = 54 + 4 * (int)(Math.Pow(2, biBitCount)) + LenBMP;
        //            }
        //            else
        //            {
        //                biH.bfSize = 54 + LenBMP;
        //            }
        //            biH.bfReserved1 = 0; //�����ֽ�
        //            biH.bfReserved2 = 0; //�����ֽ�
        //            biH.bfOffBits = 14 + 40 + 1024; //ͼ������ƫ��
        //            //���¿�ʼд��λͼ�ļ�ͷ
        //            bmpr.Write(biH.bfType); //�ļ�����
        //            bmpr.Write(biH.bfSize);  //�ļ���С
        //            bmpr.Write(biH.bfReserved1); //0
        //            bmpr.Write(biH.bfReserved2); //0
        //            bmpr.Write(biH.bfOffBits); //ͼ������ƫ��
        //            bmpr.Write(BMPInfo); //д��λͼ
        //            BMPF.Seek(0, SeekOrigin.Begin); //ָ���Ƶ��ļ���ʼ��
        //            myImg = System.Drawing.Image.FromStream(BMPF); //����λͼ�ļ�����
        //            bmpr.Close();
        //            BMPF.Close();
        //        }
        //        return myImg;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw new System.Exception(ex.Message);
        //    }
        //}
    }
}

    #endregion


/*  


























 
 
 
 
 */