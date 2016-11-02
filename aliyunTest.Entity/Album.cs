using aliyunTest.Framework.Database.DBMapAttr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aliyunTest.Entity
{
    /// <summary>
    /// 相册类
    /// </summary>
    [DBTable("Album")]
    public class Album
    {
        /// <summary>
        /// 图片ID
        /// </summary>
        [DBPrimaryKey("ImgID", true)]
        public int ImgID
        {
            get;
            set;
        }

        /// <summary>
        /// 上传时间
        /// </summary>
        [DBColumn("CreateTime")]
        public DateTime CreateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 介绍
        /// </summary>
        [DBColumn("Description")]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 图片名称
        /// </summary>
        [DBColumn("ImgName")]
        public string ImgName
        {
            get;
            set;
        }

        /// <summary>
        /// 图片路径
        /// </summary>
        [DBColumn("ImgSrc")]
        public string ImgSrc
        {
            get;
            set;
        }


    }

}
