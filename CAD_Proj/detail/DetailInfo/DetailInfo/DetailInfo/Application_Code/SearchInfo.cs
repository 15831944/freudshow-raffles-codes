using System;
using System.Collections.Generic;
using System.Text;

namespace DetailInfo
{
    /// <summary>
    /// ��ѯ��Ϣʵ����
    /// </summary>
    public class SearchInfo
    {
        public SearchInfo() { }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="fieldName">�ֶ�����</param>
        /// <param name="fieldValue">�ֶε�ֵ</param>
        /// <param name="sqlOperator">�ֶε�Sql��������</param>
        public SearchInfo(string fieldName, object fieldValue, string datatype, SqlOperator sqlOperator)
            : this(fieldName, fieldValue,datatype,sqlOperator, false)
        { }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="fieldName">�ֶ�����</param>
        /// <param name="fieldValue">�ֶε�ֵ</param>
        /// <param name="sqlOperator">�ֶε�Sql��������</param>
        /// <param name="excludeIfEmpty">����ֶ�Ϊ�ջ���Null����Ϊ��ѯ����</param>
        public SearchInfo(string fieldName, object fieldValue, string datatype,SqlOperator sqlOperator, bool excludeIfEmpty)
        {
            this.fieldName = fieldName;
            this.fieldValue = fieldValue;
            this.datatype = datatype;
            this.sqlOperator = sqlOperator;
            this.excludeIfEmpty = excludeIfEmpty;
        }

        private string fieldName;
        private object fieldValue;
        private SqlOperator sqlOperator;
        private bool excludeIfEmpty = false;
        private string datatype;

        public string Datatype
        {
            get { return datatype; }
            set { datatype = value; }
        }


        /// <summary>
        /// �ֶ�����
        /// </summary>
        public string FieldName 
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        /// <summary>
        /// �ֶ�����
        /// </summary>


        /// <summary>
        /// �ֶε�ֵ
        /// </summary>
        public object FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }

        /// <summary>
        /// �ֶε�Sql��������
        /// </summary>
        public SqlOperator SqlOperator
        {
            get { return sqlOperator; }
            set { sqlOperator = value; }
        }

        /// <summary>
        /// ����ֶ�Ϊ�ջ���Null����Ϊ��ѯ����
        /// </summary>
        public bool ExcludeIfEmpty
        {
            get { return excludeIfEmpty; }
            set { excludeIfEmpty = value; }
        }



        


        /// <summary>
        /// Ϊ��ѯ�������
        /// <example>
        /// �÷�һ��
        /// SearchCondition searchObj = new SearchCondition();
        /// searchObj.AddCondition("Test", 1, SqlOperator.NotEqual, false);
        /// searchObj.AddCondition("Test2", "Test2Value", SqlOperator.Like, true);
        /// string conditionSql = searchObj.BuildConditionSql();
        /// 
        /// �÷�����AddCondition�������Դ�������Ӷ������
        /// SearchCondition searchObj = new SearchCondition();
        /// searchObj.AddCondition("Test", 1, SqlOperator.NotEqual, false).AddCondition("Test2", "Test2Value", SqlOperator.Like, true);
        /// string conditionSql = searchObj.BuildConditionSql();
        /// </example>
        /// </summary>
    }
}