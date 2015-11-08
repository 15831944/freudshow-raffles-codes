using System;
using System.Reflection;

namespace DetailInfo
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BindingFieldAttribute : Attribute
    {
        private string fieldName;
        /// <summary>
        /// ��ȡ���������ݿ��ֶ����ơ�
        /// </summary>
        public string FieldName
        {
            get
            {
                return fieldName;
            }
            set
            {
                fieldName = value;
            }
        }
    }
}

