using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;

namespace DetailInfo.Categery
{
    public class SpoolMaterial
    {
        private string  _projectid;
        /// <summary>
        /// ��Ŀ��
        /// </summary>
        [BindingField]
        public string  Projectid
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
        private string _erpcode;
        /// <summary>
        /// ERP����
        /// </summary>
        [BindingField]
        public string ERPCode
        {
            get { return _erpcode; }
            set { _erpcode = value; }
        }
        private string _materialname;
        /// <summary>
        /// �����ͺ�
        /// </summary>
        [BindingField]
        public string MaterialName
        {
            get { return _materialname; }
            set { _materialname = value; }
        }
        private string _partweight;
        /// <summary>
        /// ����
        /// </summary>
        [BindingField]
        public string PartWeight
        {
            get { return _partweight; }
            set { _partweight = value; }
        }
        private string _mssno;
        /// <summary>
        /// MSS��
        /// </summary>
        [BindingField(FieldName="MSS_NO")]
        public string MSSNo
        {
            get { return _mssno; }
            set { _mssno = value; }
        }
	
	
	
        public static List<SpoolMaterial> Find(string drawingno,int flag)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            //OracleDatabase db = new OracleDatabase(UserSecurity.ConnectionString);
            string sql = string.Empty;
            if(flag==0)
                sql = "select * from plm.SP_SPOOLMATERIAL_TAB t where t.spoolname in (select s.spoolname from plm.SP_SPOOL_TAB s where s.drawingno='" + drawingno + "' and s.flag='Y') and t.flag='Y'";
            else
                sql = "select * from plm.SP_SPOOLMATERIAL_TAB t where t.spoolname in (select s.spoolname from plm.SP_SPOOL_TAB s where s.modifydrawingno='" + drawingno + "' and s.flag='Y') and t.flag='Y'";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return EntityBase<SpoolMaterial>.DReaderToEntityList(db.ExecuteReader(cmd));
        }
    }
}
