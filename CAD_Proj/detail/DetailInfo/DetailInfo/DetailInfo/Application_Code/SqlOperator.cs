using System;
using System.Collections.Generic;
using System.Text;

namespace DetailInfo
{
    public enum SqlOperator
    {
        /// <summary>
        /// Like ģ����ѯ
        /// </summary>
        Like,

        /// <summary>
        /// �� is equal to ���ں� 
        /// </summary>
        Equal,

        /// <summary>
        /// <> (��) is not equal to �����ں�
        /// </summary>
        NotEqual,

        /// <summary>
        /// �� is more than ���ں�
        /// </summary>
        MoreThan,

        /// <summary>
        /// �� is less than С�ں� 
        /// </summary>
        LessThan,

        /// <summary>
        /// �� is more than or equal to ���ڻ���ں� 
        /// </summary>
        MoreThanOrEqual,

        /// <summary>
        /// �� is less than or equal to С�ڻ���ں�
        /// </summary>
        LessThanOrEqual,

        
        /// <summary>
        /// ��ĳ��ֵ���м䣬����������� >= �� <=
        /// </summary>
        Between,

    }
}