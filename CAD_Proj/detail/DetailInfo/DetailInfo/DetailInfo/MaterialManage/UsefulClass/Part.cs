using System;
using System.Data;
using System.IO;
using System.Data.Common;
using System.Data.OracleClient;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Common;
using DreamStu.Common.Log;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Framework;

namespace Framework
{
    public class Part
    {
        #region ������
        private int _partId;
        /// <summary>
        /// ���
        /// </summary>
        [BindingField]
        public int ID
        {
            get { return _partId; }
            set { _partId = value; }
        }
        
        private int _parentId;
        /// <summary>
        /// ��ID
        /// </summary>
        [BindingField]
        public int PARENTID
        {
            get { return _parentId; }
            set { _parentId = value; }
        }
        private string _parttype;
        /// <summary>
        /// �������
        /// </summary>
        [BindingField]
        public string PART_TYPE
        {
            get { return _parttype; }
            set { _parttype = value; }
        }
        private string _replacepartno;
        /// <summary>
        /// ����������б�
        /// </summary>
        [BindingField]
        public string REPLACE_CODE
        {
            get { return _replacepartno; }
            set { _replacepartno = value; }
        }
        private string _partno;
        /// <summary>
        /// ������
        /// </summary>
        [BindingField]
        public string PART_NO
        {
            get { return _partno; }
            set { _partno = value; }
        }
        private string _partmat;
        /// <summary>
        /// �������
        /// </summary>
        [BindingField]
        public string PART_MAT
        {
            get { return _partmat; }
            set { _partmat = value; }
        }
        private string _site;
        /// <summary>
        /// �����
        /// </summary>
        [BindingField]
        public string CONTRACT
        {
            get { return _site; }
            set { _site = value; }
        }
        private string _weightSingle;
        /// <summary>
        /// �������
        /// </summary>
        [BindingField]
        public string PART_LEVEL
        {
            get { return _weightSingle; }
            set { _weightSingle = value; }
        }
        private string _partcert;
        /// <summary>
        /// ������֤
        /// </summary>
        [BindingField]
        public string PART_CERT
        {
            get { return _partcert; }
            set { _partcert = value; }
        }
        private string _preAlert;
        /// <summary>
        /// �����λ
        /// </summary>
        [BindingField]
        public string PART_UNIT
        {
            get { return _preAlert; }
            set { _preAlert = value; }
        }
        private string _coeficient;
        /// <summary>
        /// ���ù��1
        /// </summary>
        [BindingField]
        public string PART_SPEC1
        {
            get { return _coeficient; }
            set { _coeficient = value; }
        }
        private string _coeficient2;
        /// <summary>
        /// ���ù��2
        /// </summary>
        [BindingField]
        public string PART_SPEC2
        {
            get { return _coeficient2; }
            set { _coeficient2 = value; }
        }
        private string _coeficient3;
        /// <summary>
        /// ���ù��3
        /// </summary>
        [BindingField]
        public string PART_SPEC3
        {
            get { return _coeficient3; }
            set { _coeficient3 = value; }
        }

        private string _coeficient4;
        /// <summary>
        /// ���ù��4
        /// </summary>
        [BindingField]
        public string PART_SPEC4
        {
            get { return _coeficient4; }
            set { _coeficient4 = value; }
        }
        private string _coeficient5;
        /// <summary>
        /// ���ù��5
        /// </summary>
        [BindingField]
        public string PART_SPEC5
        {
            get { return _coeficient5; }
            set { _coeficient5 = value; }
        }
        private string _creator;
        /// <summary>
        /// ������
        /// </summary>
        [BindingField]
        public string CREATOR
        {
            get { return _creator; }
            set { _creator = value; }
        }
        private string _partspec;
        /// <summary>
        /// ���
        /// </summary>
        [BindingField]
        public string PART_SPEC
        {
            get { return _partspec; }
            set { _partspec = value; }
        }
        private decimal _partstand;
        /// <summary>
        /// ��λ�ܶ�
        /// </summary>
        [BindingField]
        public decimal PART_UNITDENSITY
        {
            get { return _partstand; }
            set { _partstand = decimal.Round(value,2); }
        }
        //private string _partstype;
        ///// <summary>
        ///// ��������
        ///// </summary>
        //[BindingField]
        //public string PART_TYPE
        //{
        //    get { return _partstype; }
        //    set { _partstype = value; }
        //}
        private string _punit;
        /// <summary>
        /// �����ܶȵ�λ
        /// </summary>
        [BindingField]
        public string PART_DENSITYUNIT
        {
            get { return _punit; }
            set { _punit = value; }
        }
        private int _lastflag;
        /// <summary>
        /// ����ʾ��
        /// </summary>
        [BindingField]
        public int LAST_FLAG
        {
            get { return _lastflag; }
            set { _lastflag = value; }
        }
        private DateTime _ctime;
        /// <summary>
        ///����ʱ��
        /// </summary>
        [BindingField]
        public DateTime CREATEDATE
        {
            get { return _ctime; }
            set { _ctime = value; }
        }
        private DateTime _utime;
        /// <summary>
        ///����ʱ��
        /// </summary>
        [BindingField]
        public DateTime UPDATEDATE
        {
            get { return _utime; }
            set { _utime = value; }
        }
        private string _updater;
        /// <summary>
        /// �����ܶȵ�λ
        /// </summary>
        [BindingField]
        public string UPDATER
        {
            get { return _updater; }
            set { _updater = value; }
        }
        private int _pcircle;
        /// <summary>
        /// �ɹ�����
        /// </summary>
        [BindingField]
        public int SUPPLYCIRCLE
        {
            get { return _pcircle; }
            set { _pcircle = value; }
        }
        #endregion
        #region ���Ϸ���
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public int Add()
        {
            // Database db = DatabaseFactory.CreateDatabase("oidsConnection");
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            DbCommand cmd = db.GetSqlStringCommand("INSERT INTO PLM.MM_PART_TAB"+
                "(part_densityunit,SUPPLYCIRCLE,PART_NO,CONTRACT,PART_SPEC,Part_TYpe,parentid,part_mat,part_cert,PART_level,part_unit,part_unitdensity,part_spec1,part_spec2,part_spec3,part_spec4,CREATOR)" +
            " VALUES (:partdensityunit,:spcircle,:partno,:site,:partspec,:parttype,:parentid,:partmat,:partcert,:partlevel,:partunit,:partunitdensity,:partspec1,:partspec2,:partspec3,:partspec4,:creator)");
            db.AddInParameter(cmd, "partno", DbType.String, PART_NO);
            db.AddInParameter(cmd, "site", DbType.String, CONTRACT);
            db.AddInParameter(cmd, "partspec", DbType.String, PART_SPEC);
            db.AddInParameter(cmd, "parttype", DbType.String, PART_TYPE);
            db.AddInParameter(cmd, "parentid", DbType.Int32, PARENTID);
            db.AddInParameter(cmd, "spcircle", DbType.Int32, SUPPLYCIRCLE);
            db.AddInParameter(cmd, "partmat", DbType.String, PART_MAT);
            db.AddInParameter(cmd, "partcert", DbType.String, PART_CERT);
            db.AddInParameter(cmd, "partlevel", DbType.String, PART_LEVEL);
            db.AddInParameter(cmd, "creator", DbType.String, CREATOR);
            db.AddInParameter(cmd, "partunit", DbType.String, PART_UNIT);
            db.AddInParameter(cmd, "partunitdensity", DbType.Decimal, PART_UNITDENSITY);
            db.AddInParameter(cmd, "partspec1", DbType.String,PART_SPEC1);
            db.AddInParameter(cmd, "partspec2", DbType.String, PART_SPEC2);
            db.AddInParameter(cmd, "partspec3", DbType.String, PART_SPEC3);
            db.AddInParameter(cmd, "partspec4", DbType.String, PART_SPEC4);
            db.AddInParameter(cmd, "partdensityunit", DbType.String, PART_DENSITYUNIT);
            return db.ExecuteNonQuery(cmd);
        }
        /// <summary>
        /// ���²���
        /// </summary>
        /// <returns></returns>
        public int Update()
        {
            //Database db = DatabaseFactory.CreateDatabase("oidsConnection");
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            DbCommand cmd = db.GetSqlStringCommand("Update PLM.MM_PART_TAB set" +
                " part_densityunit=:partdensityunit, supplycircle=:spcircle,PART_NO=:partno,CONTRACT=:site,PART_SPEC=:partspec,Part_TYpe=:parttype,parentid=:parentid,part_mat=:partmat,part_cert=:partcert,PART_level=:partlevel,part_unit=:partunit,part_unitdensity=:partunitdensity,part_spec1=:partspec1,part_spec2=:partspec2,part_spec3=:partspec3,part_spec4=:partspec4,UPDATER=:creator,UPDATEDATE=:updatetime,Replace_code=:replace_code" +
                " where id=:partid");
            db.AddInParameter(cmd, "spcircle", DbType.Int32, SUPPLYCIRCLE);
            db.AddInParameter(cmd, "partno", DbType.String, PART_NO);
            db.AddInParameter(cmd, "site", DbType.String, CONTRACT);
            db.AddInParameter(cmd, "partspec", DbType.String, PART_SPEC);
            db.AddInParameter(cmd, "parttype", DbType.String, PART_TYPE);
            db.AddInParameter(cmd, "parentid", DbType.Int32, PARENTID);
            db.AddInParameter(cmd, "partmat", DbType.String, PART_MAT);
            db.AddInParameter(cmd, "partcert", DbType.String, PART_CERT);
            db.AddInParameter(cmd, "partlevel", DbType.String, PART_LEVEL);
            db.AddInParameter(cmd, "creator", DbType.String, UPDATER);
            db.AddInParameter(cmd, "partunit", DbType.String, PART_UNIT);
            db.AddInParameter(cmd, "partunitdensity", DbType.Decimal, PART_UNITDENSITY);
            db.AddInParameter(cmd, "partspec1", DbType.String, PART_SPEC1);
            db.AddInParameter(cmd, "partspec2", DbType.String, PART_SPEC2);
            db.AddInParameter(cmd, "partspec3", DbType.String, PART_SPEC3);
            db.AddInParameter(cmd, "partspec4", DbType.String, PART_SPEC4);
            db.AddInParameter(cmd, "partdensityunit", DbType.String, PART_DENSITYUNIT);
            db.AddInParameter(cmd, "partid", DbType.Int32, ID);
            db.AddInParameter(cmd, "updatetime", DbType.DateTime, UPDATEDATE);
            db.AddInParameter(cmd, "replace_code", DbType.String, REPLACE_CODE);
            return db.ExecuteNonQuery(cmd);
        }
        /// <summary>
        /// ����PartNo�ж�����Ƿ����
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool Exist(string partno)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            //Database db = DatabaseFactory.CreateDatabase("oidsConnection");
            string sql = "SELECT part_no FROM PLM.MM_PART_TAB WHERE LOWER(part_no)=:partno";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "partno", DbType.String, partno.ToLower());
            object rname = db.ExecuteScalar(cmd);
            return (rname == null || rname == DBNull.Value) ? false : true;
        }
        /// <summary>
        /// ����PartNo�ж�����Ƿ����
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool Exist(string partno,int partid)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            //Database db = DatabaseFactory.CreateDatabase("oidsConnection");
            string sql = "SELECT part_no FROM PLM.MM_PART_TAB WHERE LOWER(part_no)=:partno and ID<>:partid";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "partno", DbType.String, partno.ToLower());
            db.AddInParameter(cmd, "partid", DbType.Int32, partid);
            object rname = db.ExecuteScalar(cmd);
            return (rname == null || rname == DBNull.Value) ? false : true;
        }
        /// <summary>
        /// ɾ��ѡ�����
        /// </summary>
        /// <param name="partnolist"></param>
        /// <returns></returns>
        public static int Delete(string  partnolist)
        {
            // Database db = DatabaseFactory.CreateDatabase("oidsConnection");
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            DbCommand cmd = db.GetSqlStringCommand("delete PLM.MM_PART_TAB t where ID in "+partnolist);
            
            return db.ExecuteNonQuery(cmd);
        }
        /// <summary>
        /// �������IDȡ�������Ϣ
        /// </summary>
        /// <param name="partid"></param>
        /// <returns></returns>
        public static Part Find(string partid)
        {
            OracleDatabase db = new OracleDatabase(DataAccess.OIDSConnStr);
            //Database db = DatabaseFactory.CreateDatabase("oidsConnection");
            //OracleDatabase db = new OracleDatabase(UserSecurity.ConnectionString);
            string sql = "SELECT * FROM plm.MM_PART_TAB WHERE id="+partid;
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return Populate(db.ExecuteReader(cmd));
        }
        /// <summary>
        /// ��IDataReader�����Commentʵ��
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static Part Populate(IDataReader dr)
        {
            return EntityBase<Part>.DReaderToEntity(dr);
        }
        #endregion
    }
}
