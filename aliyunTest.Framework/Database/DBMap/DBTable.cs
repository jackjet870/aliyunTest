using System;
using System.Collections.Generic;
using System.Reflection;
using aliyunTest.Framework.Database.DBMapAttr;

namespace aliyunTest.Framework.Database.DBMap
{
    /// <summary>
    /// 数据库表映射实体
    /// </summary>
    public class DBTable
    {
        /// <summary>
        /// 别名
        /// </summary>
        public string AliasName { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public DBPrimaryKey PrimaryKey { get; private set; }

        /// <summary>
        /// 字段列表(不包含主键)
        /// </summary>
        public List<DBColumn> ColumnList { get; private set; }

        public DBTable(Type type)
        {
            //初始化表信息
            foreach (DBTableAttribute attribute in type.GetCustomAttributes(typeof(DBTableAttribute), false))
            {
                this.Name = string.IsNullOrEmpty(attribute.Name) ? type.Name : attribute.Name;
            }
            AliasName = type.Name;

            string tablePreFix = Config.FrameworkConfig.Instance<Config.FrameworkConfig>().TablePrefix;

            Name = tablePreFix + Name;

            ColumnList = new List<DBColumn>();
            //初始化字段列表
            GetFieldList(type);
        }

        private void GetFieldList(Type type)
        {
            foreach (MemberInfo mi in type.FindMembers(MemberTypes.Property | MemberTypes.Field, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, null))
            {
                string field_name = "";
                foreach (DBPrimaryKeyAttribute pk in mi.GetCustomAttributes(typeof(DBPrimaryKeyAttribute), false))
                {
                    if (this.PrimaryKey != null)
                    {
                        throw new MyDBException("[" + mi.DeclaringType.FullName + "]指定了两个主键字段!");
                    }

                    //如果没有指定字段名称,使用MemberInfo的Name作为字段名称
                    field_name = string.IsNullOrEmpty(pk.FieldName) ? mi.Name : pk.FieldName;

                    PrimaryKey = new DBPrimaryKey(mi, field_name, pk.IsIdentity, string.Format("SEQ_{0}", this.AliasName));
                }

                //如果是其他普通字段,只需要处理自己内部的属性
                if (mi.DeclaringType != type)
                {
                    continue;
                }
                foreach (DBColumnAttribute ca in mi.GetCustomAttributes(typeof(DBColumnAttribute), false))
                {
                    if (field_name == "")
                    {
                        //如果没有指定字段名称,使用MemberInfo的Name作为字段名称
                        field_name = string.IsNullOrEmpty(ca.FieldName) ? mi.Name : ca.FieldName;
                        ColumnList.Add(new DBColumn(mi, field_name));
                    }
                }
            }
        }
    }
}
