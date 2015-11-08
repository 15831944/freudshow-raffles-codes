using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Net;
using System.Xml;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;

using AdeskRun = Autodesk.AutoCAD.Runtime;
using AdeskWin = Autodesk.AutoCAD.Windows;
using AdeskGeo = Autodesk.AutoCAD.Geometry;
using AdeskEdIn = Autodesk.AutoCAD.EditorInput;
using AdeskGra = Autodesk.AutoCAD.GraphicsInterface;
using AdeskDBSvr = Autodesk.AutoCAD.DatabaseServices;
using AdeskAppSvr = Autodesk.AutoCAD.ApplicationServices;
using AdeskInter = Autodesk.AutoCAD.Interop;

using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.GZip;

namespace YCROUpdate
{
    public delegate void UpdateState();

    public class YCROUpdate : AdeskRun.IExtensionApplication
    {
        public void Initialize()
        {
            CheckUpdate();
        }

        public void Terminate()
        {

        }

        public void CheckUpdate()
        {
            if ((MessageBox.Show("��鵽�в����Ҫ���£���Ҫ�ر�CAD����Ҫ������", "��ѡ�� ", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) == System.Windows.Forms.DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("YCROAppUp.exe");
                //YCROAppUp.YCROAPPForm mApp = new YCROAppUp.YCROAPPForm();
                //Thread update = new Thread(new ThreadStart(mApp.Show));
                //update.Start();
                //Thread.Sleep(30000);
                CloseApp();
            }
        }

        public void CloseApp()
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

    }

    public class SoftUpdate
    {

        private string download;
        private const string updateUrl = @"http://172.16.7.55/Services/PipingLifeUpdate/Update.xml";//�������õ�XML�ļ���ַ  
        private List<string> newFilePath = new List<string>();
        private List<string> oldFilePath = new List<string>();
        public int updatefilecount = 0;
        public List<string> updatefilename = new List<string>();
        public string updateinfo;
        public string currentverson;
        public bool isfinish;

        public SoftUpdate() { }  

        public SoftUpdate(string file, string softName)
        {
            this.LoadFile = file;
            this.SoftName = softName;
        }

        #region ����

        private string loadFile;
        private string newVerson;
        private string oldVerson;
        private string softName;
        private bool isUpdate = false;

        /// <summary>   
        /// ��ȡ�Ƿ���Ҫ����   
        /// </summary>   

        public bool IsUpdate
        {
            get
            {
                checkUpdate();
                return isUpdate;
            }
        }

        /// <summary>   
        /// Ҫ�����µ��ļ�   
        /// </summary>   

        public string LoadFile
        {
            get { return loadFile; }
            set { loadFile = value; }
        }

        /// <summary>   
        /// �����°汾   
        /// </summary>   

        public string NewVerson
        {
            get { return newVerson; }
        }

        /// <summary>   
        /// ����������   
        /// </summary>   

        public string SoftName
        {
            get { return softName; }
            set { softName = value; }
        }

        #endregion
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public void Updatetxtinfo()
        {
            try
            {
                GetUrl();
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(download.Substring(0, download.LastIndexOf(@"/") + 1) + "Update.xml");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(stream);
                XmlNode list = xmlDoc.SelectSingleNode("Update");
                foreach (XmlNode node in list)
                {
                    if (node.Name == "Updateinfo")
                    {
                        updateinfo = node.InnerText;
                    }
                }
                stream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// �õ�URl,oldverson
        /// </summary>
        private void GetUrl()
        {
            try
            {
                WebClient cwc = new WebClient();
                Stream cstream = cwc.OpenRead(Path.GetDirectoryName(loadFile) + @"\Resources\ClientUpdate.xml");
                XmlDocument cxmlDoc = new XmlDocument();
                cxmlDoc.Load(cstream);
                XmlNode clist = cxmlDoc.SelectSingleNode("Update");
                foreach (XmlNode node in clist)
                {
                    if (node.Name == "Soft" && node.Attributes["Name"].Value.ToLower() == SoftName.ToLower())
                    {
                        foreach (XmlNode xml in node)
                        {
                            if (xml.Name == "Verson")
                                oldVerson = xml.InnerText;
                            else
                                download = xml.InnerText;
                        }
                    }
                }
                currentverson = oldVerson;
                cstream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        /// <summary>   
        /// ����Ƿ���Ҫ����   
        /// </summary>  
        private void checkUpdate()
        {
            try
            {
                GetUrl();
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(download.Substring(0, download.LastIndexOf(@"/") + 1) + "Update.xml");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(stream);
                XmlNode list = xmlDoc.SelectSingleNode("Update");
                foreach (XmlNode node in list)
                {
                    if (node.Name == "Soft" && node.Attributes["Name"].Value.ToLower() == SoftName.ToLower())
                    {
                        foreach (XmlNode xml in node)
                        {
                            if (xml.Name == "Verson")
                                newVerson = xml.InnerText;
                            else
                                download = xml.InnerText;
                        }
                    }
                }
                stream.Close();
                Version ver = new Version(newVerson);
                currentverson = newVerson;
                Version verson = new Version(oldVerson);
                int tm = verson.CompareTo(ver);
                //Stream wstream = cwc.OpenWrite(Path.GetDirectoryName(loadFile) + @"\ClientUpdate.xml");
                if (tm >= 0)
                    isUpdate = false;
                else
                {
                    isUpdate = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw new Exception("���³��ִ�����ȷ������������������ԣ�");
            }
        }

        public event UpdateState UpdateFinish;

        private void isFinish()
        {
            if (UpdateFinish != null)
                UnZip();
            File.Delete(Path.GetDirectoryName(loadFile) + @"\" + softName);
            //overfile();
            setclientver();
            isfinish = true;
            UpdateFinish();
        }
        /// <summary>
        /// ��ʼ����
        /// </summary>
        public void StartDownload()
        {
            try
            {
                if (!isUpdate)
                    return;
                WebClient wc = new WebClient();
                wc.DownloadFile(new Uri(download), Path.GetDirectoryName(LoadFile) + @"\" + softName);
                wc.Dispose();
                isFinish();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// ��ѹѹ����
        /// </summary>
        private void UnZip()
        {
            ZipInputStream s = new ZipInputStream(File.OpenRead(Path.GetDirectoryName(loadFile) + @"\" + softName));
            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                try
                {
                    string directoryName = Path.GetDirectoryName(loadFile);
                    string fileName = Path.GetFileName(theEntry.Name);
                    //���ɽ�ѹĿ¼
                    //Directory.CreateDirectory(directoryName + @"\" + theEntry.Name.Replace("/", @"\").Substring(0, theEntry.Name.Replace("/", @"\").LastIndexOf(@"\")));

                    if (fileName != String.Empty)
                    {
                        if (fileName.ToLower() != "thumbs.db")
                        {
                            updatefilename.Add(fileName);
                            updatefilecount += 1;
                            //��ѹ�ļ���ָ����Ŀ¼
                            //MessageBox.Show(theEntry.Name.Replace("/", @"\").Substring(0, theEntry.Name.Replace("/", @"\").LastIndexOf(@"\")));
                            FileStream streamWriter = File.Create(directoryName + @"\" + theEntry.Name.Replace("/", @"\"));
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            streamWriter.Close();
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(directoryName + @"\" + theEntry.Name.Replace("/", @"\").Substring(0, theEntry.Name.Replace("/", @"\").LastIndexOf(@"\")));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            s.Close();
        }
        private void setclientver()
        {
            WebClient cwc = new WebClient();
            Stream cstream = cwc.OpenRead(Path.GetDirectoryName(loadFile) + @"\Resources\ClientUpdate.xml");
            XmlDocument cxmlDoc = new XmlDocument();
            cxmlDoc.Load(Path.GetDirectoryName(loadFile) + @"\Resources\ClientUpdate.xml");
            XmlNode wlist = cxmlDoc.SelectSingleNode("Update");
            foreach (XmlNode node in wlist)
            {
                if (node.Name == "Soft" && node.Attributes["Name"].Value.ToLower() == SoftName.ToLower())
                {
                    foreach (XmlNode xml in node)
                    {
                        if (xml.Name == "Verson")
                            xml.InnerText = newVerson;
                        break;
                    }
                }
            }
            cstream.Close();
            cxmlDoc.Save(Path.GetDirectoryName(loadFile) + @"\Resources\ClientUpdate.xml");

        }
    }
}