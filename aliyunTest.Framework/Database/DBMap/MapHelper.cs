using System;
using System.Collections.Generic;
using System.Reflection;

namespace aliyunTest.Framework.Database.DBMap
{
    public class MapHelper
    {
        public static IDictionary<string, DBTable> TableDictionary = new Dictionary<string, DBTable>();

        /// <summary>
        /// 映射字段字典
        /// key	  -- [Table FullName + DataBaseType]
        /// value -- [DBSql]
        /// </summary>
        public static IDictionary<string, DBSql> SqlDictionary = new Dictionary<string, DBSql>();

        /// <summary>
        /// 初始化实体类
        /// </summary>
        /// <param name="assemblies">待初始化的程序集</param>
        public static void InitDBMap(DataBaseType dataType, params Assembly[] assemblies)
        {
            lock (TableDictionary)
            {
                foreach (Assembly assembly in assemblies)
                {
                    Type[] types = assembly.GetTypes();

                    foreach (Type type in types)
                    {
                        //所有映射表只添加一次
                        string tableKey = type.FullName;
                        DBTable table = null;
                        if (!TableDictionary.ContainsKey(tableKey))
                        {
                            table = new DBTable(type);
                            TableDictionary.Add(tableKey, table);
                        }
                        else
                        {
                            table = TableDictionary[tableKey];
                        }
                        //为每种类型的数据库添加一个 DBSql,并且指向相应的Table; 同一种数据库的不同连接只添加一个DBSql
                        string dbKey = string.Format("{0}_{1}", tableKey, dataType.ToString());
                        if (!SqlDictionary.ContainsKey(dbKey))
                        {
                            DBSql dbsql = new DBSql();
                            dbsql.Table = table;
                            SqlDictionary.Add(dbKey, dbsql);
                        }
                    }
                }
            }
        }

        public static DBTable GetDBTable(Type type)
        {
            string key = type.FullName;
            if (TableDictionary.ContainsKey(key))
            {
                return TableDictionary[key];
            }
            throw new ArgumentException("[" + type.FullName + "]是一个无效的映射类!");
        }

        public static DBSql GetDBSql(Type type, DataBaseType _dataType)
        {
            string key = string.Format("{0}_{1}", type.FullName, _dataType.ToString());

            if (SqlDictionary.ContainsKey(key))
            {
                return SqlDictionary[key];
            }
            throw new ArgumentException("[" + type.FullName + "]是一个无效的映射类!");
        }
    }

    public class DBSql
    {
        public DBTable Table { get; internal set; }        

        /// <summary>
        /// Insert SQL Statement
        /// </summary>
        public string InsertSql { get; internal set; }

        /// <summary>
        /// Update SQL Statement
        /// </summary>
        public string UpdateSql { get; internal set; }

        /// <summary>
        /// Delete SQL Statement
        /// </summary>
        public string DeleteSql { get; internal set; }
    }
}
