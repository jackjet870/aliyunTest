using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aliyunTest.Framework;

namespace aliyunTest.Entity.Helper
{
    public class Album
    {

        public static void add()
        {

        }

        /// <summary>
        /// 获取所有照片
        /// </summary>
        /// <returns></returns>
        public static List<Entity.Album> GetAll()
        {
            List<Entity.Album> albumList = DBSession.TryGet().GetList<Entity.Album>(" 1=1 ", "ImgID");
            return albumList;
        }

        /// <summary>
        /// 通过ID获取照片实体
        /// </summary>
        /// <returns></returns>
        public static Entity.Album GetObjectByImgID(string imgID)
        {
            Entity.Album img = DBSession.TryGet().GetObject<Entity.Album>(" ImgID=? ",Convert.ToInt32(imgID));
            return img;
        }

    }
}
