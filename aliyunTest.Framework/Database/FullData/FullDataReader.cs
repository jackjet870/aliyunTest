using System.Data;
using System.Reflection.Emit;
using System;
using System.Collections.Generic;

namespace aliyunTest.Framework.Database.FullData
{
    /// <summary>
    /// 泛型类填充数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FullDataReader
    {
        private FullDataHelper _fullDH;

        public FullDataReader()
        {
            _fullDH = new FullDataHelper();
        }

        #region 填充自定义类

        /// <summary>
        /// 自定义类型填充，已经存在的填充动态方法
        /// </summary>
        private Dictionary<string, Delegate> _idiFullCustomObj = new Dictionary<string, Delegate>();
        /// <summary>
        /// 构造自定义类 填充Object委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public DegFullCustomObj<T> CreateDegFullCustomObj<T>(IDataReader reader)
        {
            string key = string.Format("{0}{1}", typeof(T).FullName, _fullDH.GetDataReaderInfo(reader));

            Delegate degMethod;
            if (_idiFullCustomObj.TryGetValue(key, out degMethod))
                return (DegFullCustomObj<T>)degMethod;

            DegFullCustomObj<T> deg = null;
            //构造填充方法
            DynamicMethod method = _fullDH.CreateFullObjMethod(reader, typeof(T), false);

            //为动态方法构造调用委托
            deg = (DegFullCustomObj<T>)method.CreateDelegate(typeof(DegFullCustomObj<T>));
            _idiFullCustomObj.Add(key, deg);

            return deg;
        }

        /// <summary>
        /// 自定义类型填充，已经存在的填充动态方法
        /// </summary>
        private Dictionary<string, Delegate> _idiFullCustomList = new Dictionary<string, Delegate>();
        /// <summary>
        /// 构造自定义类 填充List委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public DegFullCustomList<T> CreateDegFullCustomList<T>(IDataReader reader)
        {
            string key = string.Format("{0}{1}", typeof(T).FullName, _fullDH.GetDataReaderInfo(reader));

            Delegate degMethod;
            if (_idiFullCustomList.TryGetValue(key, out degMethod))
                return (DegFullCustomList<T>)degMethod;

            DegFullCustomList<T> deg = null;

            //构造填充方法
            DynamicMethod method = _fullDH.CreateFullListMethod(reader, typeof(T), false);

            //为动态方法构造调用委托
            deg = (DegFullCustomList<T>)method.CreateDelegate(typeof(DegFullCustomList<T>));
            _idiFullCustomList.Add(key, deg);

            return deg;
        }

        #endregion

        #region 填充映射类型

        /// <summary>
        /// 填充映射类委托
        /// </summary>
        private Dictionary<string, Delegate> _idiFullMapObj = new Dictionary<string, Delegate>();
        /// <summary>
        /// 构造填充映射类对象 填充Object委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public DegFullMapObj<T> CreateDegFullMapObj<T>(IDataReader reader)
        {
            //判断是否已经存在该类型的动态方法
            Type type = typeof(T);
            Delegate degMethod;
            if (_idiFullMapObj.TryGetValue(type.FullName, out degMethod))
                return (DegFullMapObj<T>)degMethod;

            DegFullMapObj<T> deg = null;

            //构造填充方法
            DynamicMethod method = _fullDH.CreateFullObjMethod(reader, typeof(T), true);

            //为动态方法构造调用委托
            deg = (DegFullMapObj<T>)method.CreateDelegate(typeof(DegFullMapObj<T>));
            _idiFullMapObj.Add(type.FullName, deg);

            return deg;
        }

        /// <summary>
        /// 填充映射类集合委托
        /// </summary>
        private Dictionary<string, Delegate> _idiFullMapList = new Dictionary<string, Delegate>();
        /// <summary>
        /// 构造填充映射类集合 填充List委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public DegFullMapList<T> CreateDegFullMapList<T>(IDataReader reader)
        {
            //判断是否已经存在该类型的动态方法
            Type type = typeof(T);
            Delegate degMethod;
            if (_idiFullMapList.TryGetValue(type.FullName, out degMethod))
                return (DegFullMapList<T>)degMethod;

            DegFullMapList<T> deg = null;

            //构造填充方法
            DynamicMethod method = _fullDH.CreateFullListMethod(reader, typeof(T), true);

            //为动态方法构造调用委托
            deg = (DegFullMapList<T>)method.CreateDelegate(typeof(DegFullMapList<T>));
            _idiFullMapList.Add(type.FullName, deg);

            return deg;
        }

        #endregion

        #region 填充动态类

        private Dictionary<string, DegFullDynamicObj> _idiFullDynamicObj = new Dictionary<string, DegFullDynamicObj>();
        /// <summary>
        /// 构造动态类 填充Object委托
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public DegFullDynamicObj CreateDegDynamicFullObj(IDataReader reader)
        {
            Type type = _fullDH.CreateType(reader);

            DegFullDynamicObj degMethod;
            if (_idiFullDynamicObj.TryGetValue(type.FullName, out degMethod))
                return degMethod;

            DegFullDynamicObj deg = null;

            //构造填充方法
            DynamicMethod method = _fullDH.CreateFullObjMethod(reader, type, false);

            //为动态方法构造调用委托
            deg = (DegFullDynamicObj)method.CreateDelegate(typeof(DegFullDynamicObj));
            _idiFullDynamicObj.Add(type.FullName, deg);

            return deg;
        }


        private Dictionary<string, DegFullDynamicList> _idiFullDynamicList = new Dictionary<string, DegFullDynamicList>();
        /// <summary>
        /// 构造动态类 填充List委托
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public DegFullDynamicList CreateDegFullDynamicList(IDataReader reader)
        {
            Type type = _fullDH.CreateType(reader);

            DegFullDynamicList degMethod;
            if (_idiFullDynamicList.TryGetValue(type.FullName, out degMethod))
                return degMethod;

            DegFullDynamicList deg = null;

            //构造填充方法
            DynamicMethod method = _fullDH.CreateFullListMethod(reader, type, false);

            //为动态方法构造调用委托
            deg = (DegFullDynamicList)method.CreateDelegate(typeof(DegFullDynamicList));
            _idiFullDynamicList.Add(type.FullName, deg);

            return deg;
        }

        #endregion

    }
}
