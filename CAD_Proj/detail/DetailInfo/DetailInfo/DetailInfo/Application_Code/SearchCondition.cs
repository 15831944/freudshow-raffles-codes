using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace DetailInfo
{
    public class SearchCondition
    {
        private Hashtable conditionTable = new Hashtable();
        public Hashtable ConditionTable
        {
            get { return this.conditionTable; }
        }
        /// <summary>
        /// Ϊ��ѯ�������
        /// <example>
        /// �÷�һ��
        /// SearchCondition searchObj = new SearchCondition();
        /// searchObj.AddCondition("Test", 1, SqlOperator.NotEqual);
        /// searchObj.AddCondition("Test2", "Test2Value", SqlOperator.Like);
        /// string conditionSql = searchObj.BuildConditionSql();
        /// 
        /// �÷�����AddCondition�������Դ�������Ӷ������
        /// SearchCondition searchObj = new SearchCondition();
        /// searchObj.AddCondition("Test", 1, SqlOperator.NotEqual).AddCondition("Test2", "Test2Value", SqlOperator.Like);
        /// string conditionSql = searchObj.BuildConditionSql();
        /// </example>
        /// </summary>
        /// <param name="fielName">�ֶ�����</param>
        /// <param name="fieldValue">�ֶ�ֵ</param>
        /// <param name="sqlOperator">SqlOperatorö������</param>
        /// <returns>�����������Hashtable</returns>
        public SearchCondition AddCondition(string fielName, object fieldValue, string datatype,SqlOperator sqlOperator)
        {
            this.conditionTable.Add(fielName, new SearchInfo(fielName, fieldValue, datatype, sqlOperator));
            return this;
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
        /// <param name="fielName">�ֶ�����</param>
        /// <param name="fieldValue">�ֶ�ֵ</param>
        /// <param name="sqlOperator">SqlOperatorö������</param>
        /// <param name="excludeIfEmpty">����ֶ�Ϊ�ջ���Null����Ϊ��ѯ����</param>
        /// <returns></returns>
        public SearchCondition AddCondition(string fielName, object fieldValue, string datatype,SqlOperator sqlOperator, bool excludeIfEmpty)
        {
            this.conditionTable.Add(fielName, new SearchInfo(fielName, fieldValue,datatype,sqlOperator, excludeIfEmpty));
            return this;
        }

        /// <summary>
        /// ���ݶ�������ص�������䣨��ʹ�ò��������緵�ص������:
        /// <![CDATA[
        /// Where (1=1)  AND Test4  <  'Value4' AND Test6  >=  'Value6' AND Test7  <=  'value7' AND Test  <>  '1' AND Test5  >  'Value5' AND Test2  Like  '%Value2%' AND Test3  =  'Value3'
        /// ]]>
        /// </summary>
        /// <returns></returns> 
        public string BuildConditionSql()
        {
            string sql = " Where (1=1) ";
            string fieldName = string.Empty;
            SearchInfo searchInfo = null;

            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry de in this.conditionTable)
            {
                searchInfo = (SearchInfo)de.Value;

                //���ѡ��ExcludeIfEmptyΪTrue,���Ҹ��ֶ�Ϊ��ֵ�Ļ�,����
                if (searchInfo.ExcludeIfEmpty && string.IsNullOrEmpty((string)searchInfo.FieldValue))
                {
                    continue;
                }

                string sqlOp = this.ConvertSqlOperator(searchInfo.SqlOperator);
                
                sb.Append(" AND (");

                string[] valueArray = searchInfo.FieldValue.ToString().Split(';');

                foreach (string var in valueArray)
                {
                    if (searchInfo.SqlOperator == SqlOperator.Like)
                    {
                        sb.AppendFormat("{0} {1} '{2}' OR ", searchInfo.FieldName, sqlOp, string.Format("%{0}%", var));
                    }

                    else if (searchInfo.SqlOperator == SqlOperator.Between)
                    {
                        if (searchInfo.Datatype.ToString() == "DATE")
                        {
                            if (valueArray.Length != 2)
                            {
                                if (var == valueArray[0])
                                {
                                    sb.AppendFormat("{0}{1}", searchInfo.FieldName, sqlOp);
                                    sb.AppendFormat("{0} AND ", MakeDateTimeSql("0001-01-01"));
                                    sb.AppendFormat("{0} ) ", MakeDateTimeSql("3001-01-01"));
                                }
                            }
                            else
                            {
                                try
                                {
                                    if (var == valueArray[1]) continue;
                                    sb.AppendFormat("{0}{1}", searchInfo.FieldName, sqlOp);
                                    //System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[0-9]{1,4}-[0-9]{1,2}-[0-9]{1,2}");
                                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
                                    if (reg.IsMatch(valueArray[0]))
                                    {
                                        sb.AppendFormat("{0} AND ", MakeDateTimeSql(valueArray[0]));
                                    }
                                    else
                                    {
                                        sb.AppendFormat("{0} AND ", MakeDateTimeSql("0001-01-01"));
                                    }

                                    if (reg.IsMatch(valueArray[1]))
                                    {
                                        sb.AppendFormat("{0} ) ", MakeDateTimeSql(valueArray[1]));
                                    }
                                    else
                                    {
                                        sb.AppendFormat("{0} ) ", MakeDateTimeSql("3001-01-01"));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message.ToString());
                                }
                            }
                        }

                        else if (searchInfo.Datatype.ToString() == "NUMBER")
                        {
                            if (valueArray.Length != 2)
                            {
                                if (var == valueArray[0])
                                {
                                    sb.AppendFormat("{0}{1}", searchInfo.FieldName, sqlOp);
                                    sb.AppendFormat("{0} AND ", 0);
                                    sb.AppendFormat("{0} ) ", 10000);
                                    //MessageBox.Show(sb.ToString());
                                }
                            }
                            else
                            {
                                System.Text.RegularExpressions.Regex regnum = new System.Text.RegularExpressions.Regex(@"^[0-9.]*$");
                                if (var == valueArray[1]) continue;
                                sb.AppendFormat("{0}{1}", searchInfo.FieldName, sqlOp);
                                if (regnum.IsMatch(valueArray[0]))
                                {
                                    sb.AppendFormat("{0} AND ", Convert.ToDouble(valueArray[0]));
                                }
                                else
                                {
                                    sb.AppendFormat("{0} AND ", 0);
                                }
                                if (regnum.IsMatch(valueArray[1]))
                                {
                                    sb.AppendFormat("{0} ) ", Convert.ToDouble(valueArray[1]));
                                }
                                else
                                {
                                    sb.AppendFormat("{0} ) ", 1000);
                                }
                            }
                        }
                        //����ֶ�����Ϊ�ַ���
                        else if(searchInfo.Datatype.ToString().Contains("VARCHAR"))
                        {
                            if (valueArray.Length != 2)
                            {
                                if (var == valueArray[0])
                                {
                                    sb.AppendFormat("{0}{1}", searchInfo.FieldName, sqlOp);
                                    sb.AppendFormat("{0} AND ", valueArray[0]);
                                    sb.AppendFormat("{0} ) ", valueArray[0]);
                                }
                            }
                            else
                            {
                                try
                                {
                                    if (var == valueArray[1]) continue;
                                    sb.AppendFormat("{0}{1}", searchInfo.FieldName, sqlOp);
                                    sb.AppendFormat("'{0}' AND ", valueArray[0]);
                                    sb.AppendFormat("'{0}' ) ", valueArray[1]);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message.ToString());
                                }
                            }
                            //sb.AppendFormat("{0} is null   ",searchInfo.FieldName);
                        }
                    }
                    //���Ų�Ϊlike ����between��ʱ��
                    else 
                    {
                        if (searchInfo.Datatype.ToString() == "NUMBER")
                        {
                            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9.]*$");
                            if (reg1.IsMatch(var))
                            {
                                if (var.Contains("."))
                                {
                                    sb.AppendFormat("{0} {1} {2} OR ", searchInfo.FieldName, sqlOp, Convert.ToDouble(var));
                                }
                                else
                                {
                                    sb.AppendFormat("{0} {1} {2} OR ", searchInfo.FieldName, sqlOp, Convert.ToInt32(var));
                                }
                            }

                            else
                            {
                                sb.AppendFormat("{0} {1} {2} OR ", searchInfo.FieldName, sqlOp, -1);
                            }
                        }

                        else if (searchInfo.Datatype.ToString() == "DATE")
                        {
                            try
                            {
                                System.Text.RegularExpressions.Regex reg2 = new System.Text.RegularExpressions.Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
                                if (reg2.IsMatch(var))
                                {
                                    if (searchInfo.SqlOperator == SqlOperator.Equal)
                                    {
                                        string ttt = "to_date(to_char("+searchInfo.FieldName+", 'yyyy-mm-dd'), 'yyyy-mm-dd')";
                                        sb.AppendFormat("{0} {1} {2} OR", ttt, sqlOp,MakeDateTimeSql(var) );

                                    }
                                    else
                                    {
                                        sb.AppendFormat("{0} {1} {2} OR ", searchInfo.FieldName, sqlOp, MakeDateTimeSql(var));
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("���ڸ�ʽ���ԣ���ȷ��(yyyy-mm-dd)", "WARNNING", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                    if (searchInfo.SqlOperator == SqlOperator.LessThan || searchInfo.SqlOperator == SqlOperator.LessThanOrEqual)
                                    {
                                        sb.AppendFormat("{0} {1} {2} OR ", searchInfo.FieldName, sqlOp, MakeDateTimeSql("3001-01-01"));
                                    }
                                    else
                                    {
                                        sb.AppendFormat("{0} {1} {2} OR ", searchInfo.FieldName, sqlOp, MakeDateTimeSql("1001-01-01"));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString());
                            }
                        }

                        else
                        {
                            sb.AppendFormat("{0} {1} '{2}' OR ", searchInfo.FieldName, sqlOp, var);
                        }
                    }
                }
                sb.Remove(sb.Length - 3, 3);
                sb.Append(")");                
            }

            sql += sb.ToString();
            return sql;
        }

        #region ��������

        /// <summary>
        /// ת��ö������Ϊ��Ӧ��Sql����������
        /// </summary>
        /// <param name="sqlOperator">SqlOperatorö�ٶ���</param>
        /// <returns><![CDATA[��Ӧ��Sql���������ţ��� ">" "<>" ">=")]]></returns>
        private string ConvertSqlOperator(SqlOperator sqlOperator)
        {
            string stringOperator = " = ";
            switch (sqlOperator)
            {
                case SqlOperator.Equal:
                    stringOperator = " = ";
                    break;
                case SqlOperator.LessThan:
                    stringOperator = " < ";
                    break;
                case SqlOperator.LessThanOrEqual:
                    stringOperator = " <= ";
                    break;
                case SqlOperator.Like:
                    stringOperator = " Like ";
                    break;
                case SqlOperator.MoreThan:
                    stringOperator = " > ";
                    break;
                case SqlOperator.MoreThanOrEqual:
                    stringOperator = " >= ";
                    break;
                case SqlOperator.NotEqual:
                    stringOperator = " <> ";
                    break;
                case SqlOperator.Between:
                    stringOperator = " Between ";
                    break;
                default:
                    break;
            }

            return stringOperator;
        }

        /// <summary>
        /// ���ݴ�������ֵ���ͻ�ȡ���Ӧ��DbType����
        /// </summary>
        /// <param name="fieldValue">�����ֵ</param>
        /// <returns>DbType����</returns>
        public DbType GetFieldDbType(object fieldValue)
        {
            DbType type = DbType.String;

            switch (fieldValue.GetType().ToString())
            {
                case "System.Int16":
                    type = DbType.Int16;
                    break;
                case "System.UInt16":
                    type = DbType.UInt16;
                    break;
                case "System.Single":
                    type = DbType.Single;
                    break;
                case "System.UInt32":
                    type = DbType.UInt32;
                    break;
                case "System.Int32":
                    type = DbType.Int32;
                    break;
                case "System.UInt64":
                    type = DbType.UInt64;
                    break;
                case "System.Int64":
                    type = DbType.Int64;
                    break;
                case "System.String":
                    type = DbType.String;
                    break;
                case "System.Double":
                    type = DbType.Double;
                    break;
                case "System.Decimal":
                    type = DbType.Decimal;
                    break;
                case "System.Byte":
                    type = DbType.Byte;
                    break;
                case "System.Boolean":
                    type = DbType.Boolean;
                    break;
                case "System.DateTime":
                    type = DbType.DateTime;
                    break;
                case "System.Guid":
                    type = DbType.Guid;
                    break;
                default:
                    break;
            }
            return type;
        }
        #endregion

    public static string MakeDateTimeSql(string dateTime)
    {
        return " to_date('" + DateTime.Parse(dateTime).ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-mm-dd hh24:mi:ss')";
    }
    }
}
