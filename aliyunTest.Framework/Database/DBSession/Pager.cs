using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;
using aliyunTest.Framework.Database.DBMap;
using aliyunTest.Framework.Database.DBMapAttr;

namespace aliyunTest.Framework
{
    /// <summary>
    /// 数据排序方式
    /// </summary>
    public enum OrderTypes
    {
        降序 = 1,
        升序 = 0
    }

    /// <summary>
    /// 通用数据分页类
    /// </summary>
    public class Pager<T> where T : class, new()
    {
        #region 属性与私有变量



        private string _tableName = "";

        private int _pageIndex = 0;
        private int _pageSize = 20;
        private IList<T> _records;
        private int _totalRecords = 0;
        private string _strOrder;
        private OrderTypes _orderType = OrderTypes.降序;
        private bool _isShowInfo = false;


        /// <summary>
        /// 是否显示数据信息
        /// </summary>
        public bool IsShowInfo { get { return _isShowInfo; } }

        /// <summary>
        /// 当前数据分页
        /// </summary>
        public int PageIndex
        {
            get
            {
                return this._pageIndex;
            }
        }

        /// <summary>
        /// 分页数据大小
        /// </summary>
        public int PageSize
        {
            get
            {
                return this._pageSize;
            }
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        public IList<T> Records
        {
            get
            {
                if (this._records == null)
                {
                    this._records = new List<T>();
                }
                return this._records;
            }
        }

        /// <summary>
        /// 总共有多少条数据
        /// </summary>
        public int TotalRecords
        {
            get
            {
                return this._totalRecords;
            }
        }

        /// <summary>
        /// 排序方式
        /// </summary>
        public string StrOrder
        {
            get
            {
                return _strOrder;
            }
        }

        /// <summary>
        /// 排序方式
        /// </summary>
        public OrderTypes OrderType
        {
            get { return _orderType; }
        }

        /// <summary>
        /// 是否能显示分页
        /// </summary>
        public bool CanShowPager { get { return this.TotalRecords > this.PageSize ? true : false; } }

        /// <summary>
        /// 数据库链接字符串Sesion Key
        /// </summary>
        public string DbKey { get; set; }

        #endregion

        #region 构造函数与方法


        /// <summary>
        /// 创建分页数据，多表查询方式
        /// </summary> 
        /// <param name="primaryKey">主键例</param>
        /// <param name="where">查询条件</param>
        /// <param name="orders">排序字段</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前分页号码</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="dbKey">DBSession Key</param>
        /// <param name="isShowInfo">是否显示数据信息</param>
        public Pager(string where, string orders, int pageSize, int pageIndex, OrderTypes orderType, bool isShowInfo, string tableName, string returnFeilds)
        {
            DbKey = DBSession.DefaultDBKey;
            _isShowInfo = isShowInfo;

            InitDate(where, orders, pageSize, pageIndex, orderType, tableName, returnFeilds);
        }


        /// <summary>
        /// 创建分页数据，多表查询方式
        /// </summary> 
        /// <param name="primaryKey">主键例</param>
        /// <param name="where">查询条件</param>
        /// <param name="orders">排序字段</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前分页号码</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="dbKey">DBSession Key</param>
        /// <param name="isShowInfo">是否显示数据信息</param>
        public Pager(string where, string orders, int pageSize, int pageIndex, OrderTypes orderType, string dbKey, bool isShowInfo, string tableName, string returnFeilds)
        {
            DbKey = dbKey;
            _isShowInfo = isShowInfo;

            InitDate(where, orders, pageSize, pageIndex, orderType, tableName, returnFeilds);
        }


        /// <summary>
        /// 创建分页数据
        /// </summary> 
        /// <param name="primaryKey">主键例</param>
        /// <param name="where">查询条件</param>
        /// <param name="orders">排序字段</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前分页号码</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="dbKey">DBSession Key</param>
        public Pager(string where, string orders, int pageSize, int pageIndex, OrderTypes orderType, string dbKey)
        {
            DbKey = dbKey;

            InitDate(where, orders, pageSize, pageIndex, orderType);
        }

        /// <summary>
        /// 创建分页数据
        /// </summary> 
        /// <param name="primaryKey">主键例</param>
        /// <param name="where">查询条件</param>
        /// <param name="orders">排序字段</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前分页号码</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="dbKey">DBSession Key</param>
        /// <param name="isShowInfo">是否显示数据信息</param>
        public Pager(string where, string orders, int pageSize, int pageIndex, OrderTypes orderType, string dbKey, bool isShowInfo)
        {
            DbKey = dbKey;
            _isShowInfo = isShowInfo;

            InitDate(where, orders, pageSize, pageIndex, orderType);
        }

        /// <summary>
        /// 创建分页数据
        /// </summary> 
        /// <param name="primaryKey">主键例</param>
        /// <param name="where">查询条件</param>
        /// <param name="orders">排序字段</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前分页号码</param>
        /// <param name="orderType">排序类型</param>
        public Pager(string where, string orders, int pageSize, int pageIndex, OrderTypes orderType)
        {
            DbKey = DBSession.DefaultDBKey;

            InitDate(where, orders, pageSize, pageIndex, orderType);
        }

        /// <summary>
        /// 创建分页数据
        /// </summary> 
        /// <param name="primaryKey">主键例</param>
        /// <param name="where">查询条件</param>
        /// <param name="orders">排序字段</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前分页号码</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="isShowInfo">是否显示数据信息</param>
        public Pager(string where, string orders, int pageSize, int pageIndex, OrderTypes orderType, bool isShowInfo)
        {
            DbKey = DBSession.DefaultDBKey;

            _isShowInfo = isShowInfo;

            InitDate(where, orders, pageSize, pageIndex, orderType);
        }

        /// <summary>
        /// 创建分页数据，默认排序列为主键例，分页大小为20
        /// </summary>  
        /// <param name="where">查询条件</param>
        /// <param name="pageIndex">当前分页号</param>
        public Pager(string where, int pageIndex)
        {
            DbKey = DBSession.DefaultDBKey;

            InitDate(where, null, 20, pageIndex, OrderTypes.降序);
        }

        /// <summary>
        /// 创建分页数据，默认排序列为主键例，分页大小为20
        /// </summary> 
        /// <param name="where">查询条件</param>
        /// <param name="pageIndex">当前分页号</param>
        /// <param name="isShowInfo">是否显示数据信息</param>
        public Pager(string where, int pageIndex, bool isShowInfo)
        {
            DbKey = DBSession.DefaultDBKey;

            _isShowInfo = isShowInfo;

            InitDate(where, null, 20, pageIndex, OrderTypes.降序);
        }

        /// <summary>
        /// 初始化分页数据
        /// </summary>  
        /// <param name="where">查询条件</param>
        /// <param name="orders">排序字段</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前分页号码</param>
        /// <param name="orderType">排序类型</param>
        private void InitDate(string where, string orders, int pageSize, int pageIndex, OrderTypes orderType)
        {
            InitDate(where, orders, pageSize, pageIndex, orderType, null, "*");
        }

        /// <summary>
        /// 初始化分页数据
        /// </summary>  
        /// <param name="where">查询条件</param>
        /// <param name="orders">排序字段</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前分页号码</param>
        /// <param name="orderType">排序类型</param>
        private void InitDate(string where, string orders, int pageSize, int pageIndex, OrderTypes orderType, string tableName, string returnFeilds)
        {
            DBTable table = MapHelper.GetDBTable(typeof(T));

            string primaryKey = table.PrimaryKey.Name;

            _tableName = string.IsNullOrEmpty(tableName) ? table.Name : tableName;

            this._pageSize = pageSize < 1 ? 1 : pageSize;

            this._pageIndex = pageIndex < 1 ? 1 : pageIndex;

            this._totalRecords = GetPageCount(where, _tableName);

            this._strOrder = string.IsNullOrEmpty(orders) ? primaryKey : orders;

            this._orderType = orderType;

            this._strOrder += (this._orderType == OrderTypes.升序 ? " asc" : " desc");

            this._records = new List<T>();

            this._records = GetList(_pageSize, _pageIndex, where, primaryKey, _strOrder, _tableName, returnFeilds).ToList<T>();

            if (_records.Count == 0)
            {
                _pageIndex = _totalRecords % _pageSize > 0 ? _totalRecords / _pageSize + 1 : _totalRecords / _pageSize;
                this._records = GetList(_pageSize, _pageIndex, where, primaryKey, _strOrder, _tableName, returnFeilds).ToList<T>();
            }
        }


        /// <summary>
        /// 获得当前的条件的总记录
        /// </summary> 
        /// <param name="where">条件</param>
        /// <returns></returns>
        public int GetPageCount(string where, string tableName)
        {
            return DBSession.TryGet(DbKey).ExecuteScalar<int>(string.Format("select count(*) from {0} where {1}", tableName, where.IsEmpty() ? "1=1" : where));
        }


        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataTable GetList(int PageSize, int PageIndex, string strWhere, string primaryKey, string orderKey, string tableName, string returnFeilds)
        {
            return DBSession.TryGet(DbKey).GetDataTablePaging(PageIndex, PageSize, returnFeilds, tableName, strWhere, null, orderKey, null);
        }

        #endregion

        #region 数据分页处理

        /// <summary>
        /// 显示分页控件,默认显示10个
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        public void RenderPager(System.Web.Mvc.HtmlHelper helper)
        {
            RenderPager(helper, 10);
        }

        /// <summary>
        /// 显示分页控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="showPage">显示多少个分页数</param>
        public void RenderPager(System.Web.Mvc.HtmlHelper helper, int? showPage)
        {
            //Weike.CoustomerViewExtensions.Pager(helper, TotalRecords, PageSize, null, PageIndex);
            System.Web.Mvc.Html.ChildActionExtensions.RenderAction(helper, "PagerControl", "Pager", new { recordCount = this.TotalRecords, pageSize = this.PageSize, pageIndex = this.PageIndex, showPage = showPage, isShowInfo = IsShowInfo });
        }

        #endregion
    }

    /// <summary>
    /// 分页控件Controller
    /// </summary>
    public class PagerController : ControllerBase
    {

        /// <summary>
        /// 分页控件
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="recordCount">记录总数</param>
        /// <param name="pageIndex">当前分页号</param>
        /// <param name="showPage">分页控件显示多少个分页数</param>
        /// <returns></returns>
        public ActionResult PagerControl(int? pageSize, int recordCount, int? pageIndex, int? showPage, bool isShowInfo)
        {
            Dictionary<string, int> pageDate = new Dictionary<string, int>();


            pageDate.Add("PageSize", pageSize == null ? 20 : pageSize.Value);

            pageDate.Add("RecordCount", recordCount);

            pageDate.Add("IsShowInfo", isShowInfo ? 1 : 0);

            pageDate.Add("PageIndex", pageIndex == null ? StringExtensions.isInt(Request.QueryString["Page"]) ? int.Parse(Request.QueryString["Page"]) : 1 : pageIndex.Value);

            pageDate.Add("ShowPage", showPage == null ? 10 : showPage.Value);

            return View(ControlPath + "PagerControl.cshtml", pageDate);
        }
    }
}
