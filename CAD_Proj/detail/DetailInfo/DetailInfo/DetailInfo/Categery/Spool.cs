using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
namespace DetailInfo.Categery
{
    public class Spool
    {
        private string _projectid;
        /// <summary>
        /// ��Ŀ��
        /// </summary>
        [BindingField]
        public string Projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private string _spoolname;
        /// <summary>
        /// СƱ��
        /// </summary>
        [BindingField]
        public string SpoolName
        {
            get { return _spoolname; }
            set { _spoolname = value; }
        }
        private string _systemid;
        /// <summary>
        /// ϵͳ��
        /// </summary>
        [BindingField]
        public string Systemid
        {
            get { return _systemid; }
            set { _systemid = value; }
        }
        private string _systemname;
        /// <summary>
        /// ϵͳ��
        /// </summary>
        [BindingField]
        public string SystemName
        {
            get { return _systemname; }
            set { _systemname = value; }
        }
        private string _pipegrade;
        /// <summary>
        /// ��·�ȼ�
        /// </summary>
        [BindingField]
        public string PipeGrade
        {
            get { return _pipegrade; }
            set { _pipegrade = value; }
        }
        private string _surfacetreatment;
        /// <summary>
        /// ���洦��
        /// </summary>
        [BindingField]
        public string SurfaceTreatment
        {
            get { return _surfacetreatment; }
            set { _surfacetreatment = value; }
        }
        private string _workingpressure;
        /// <summary>
        /// ����ѹ��
        /// </summary>
        [BindingField]
        public string WorkingPressure
        {
            get { return _workingpressure; }
            set { _workingpressure = value; }
        }
        private string _pressuretestfield;
        /// <summary>
        /// ѹ�����鳡��
        /// </summary>
        [BindingField]
        public string PressureTestField
        {
            get { return _pressuretestfield; }
            set { _pressuretestfield = value; }
        }
        private string _pipecheckfield;
        /// <summary>
        /// У�ܳ���
        /// </summary>
        [BindingField]
        public string PipeCheckField
        {
            get { return _pipecheckfield; }
            set { _pipecheckfield = value; }
        }
        private string _spoolweight;
        /// <summary>
        /// СƱ����
        /// </summary>
        [BindingField]
        public string SpoolWeight
        {
            get { return _spoolweight; }
            set { _spoolweight = value; }
        }
        private string _paintcolor;
        /// <summary>
        /// ������ɫ
        /// </summary>
        [BindingField]
        public string PaintColor
        {
            get { return _paintcolor; }
            set { _paintcolor = value; }
        }
        private string _cabintype;
        /// <summary>
        /// ��������
        /// </summary>
        [BindingField]
        public string CabinType
        {
            get { return _cabintype; }
            set { _cabintype = value; }
        }
        private string _revision;
        /// <summary>
        /// СƱ�汾
        /// </summary>
        [BindingField]
        public string Revision
        {
            get { return _revision; }
            set { _revision = value; }
        }
        private string _spoolmodifytype;
        /// <summary>
        /// СƱ�޸�����
        /// </summary>
        [BindingField]
        public string SpoolModifyType
        {
            get { return _spoolmodifytype; }
            set { _spoolmodifytype = value; }
        }
        private string _elbowtype;
        /// <summary>
        /// ��ͷ��ʽ
        /// </summary>
        [BindingField]
        public string ElbowType
        {
            get { return _elbowtype; }
            set { _elbowtype = value; }
        }
        private string _weldtype;
        /// <summary>
        /// �㺸��
        /// </summary>
        [BindingField]
        public string Weldtype
        {
            get { return _weldtype; }
            set { _weldtype = value; }
        }
        private string _drawingno;
        /// <summary>
        /// ͼ��
        /// </summary>
        [BindingField]
        public string Drawingno
        {
            get { return _drawingno; }
            set { _drawingno = value; }
        }
        private string _blockno;
        /// <summary>
        /// �ֶκ�
        /// </summary>
        [BindingField]
        public string Blockno
        {
            get { return _blockno; }
            set { _blockno = value; }
        }
        private string _modifydrawingno;
        /// <summary>
        /// �޸�֪ͨ����
        /// </summary>
        [BindingField]
        public string ModifyDrawingno
        {
            get { return _modifydrawingno; }
            set { _modifydrawingno = value; }
        }
        private string _remark;
        /// <summary>
        /// ��ע
        /// </summary>
        [BindingField]
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private string _flag;
        /// <summary>
        /// �汾��ʶ
        /// </summary>
        [BindingField]
        public string Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }

        public static string GetBlockNo(string drawingno,int flag)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            string sql = string.Empty;
            if (flag == 0)
                sql = "select distinct t.blockno from SP_SPOOL_TAB t where t.drawingno='" + drawingno + "' AND T.FLAG='Y'";
            else
                sql = "select distinct t.blockno from SP_SPOOL_TAB t where t.modifydrawingno='" + drawingno + "' and t.flag='Y'";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd));
        }
        public static List<Spool> GetSpoolName(string drawingno,int flag)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            //OracleDatabase db = new OracleDatabase(UserSecurity.ConnectionString);
            string sql = string.Empty;
            if (flag == 0)
                sql = "select * from SP_SPOOL_TAB t where t.drawingno='" + drawingno + "' and t.flag='Y'";
            else
                sql = "select * from SP_SPOOL_TAB t where t.modifydrawingno='" + drawingno + "' and t.flag='Y'";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return EntityBase<Spool>.DReaderToEntityList(db.ExecuteReader(cmd));
        }
    }
}
