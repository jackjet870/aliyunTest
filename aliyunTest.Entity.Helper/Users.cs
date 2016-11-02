using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aliyunTest.Framework;

namespace aliyunTest.Entity.Helper
{
    public class Users
    {
        /// <summary>
        /// 注册(添加一个用户)
        /// </summary>
        /// <param name="u"></param>
        public static void add(Entity.Users u)
        {
            int i = Convert.ToInt32(DBSession.TryGet().Insert<Entity.Users>(u));
        }

        /// <summary>
        /// 获取用户实体
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static Entity.Users GetUser(Entity.Users u)
        {
            Entity.Users user = DBSession.TryGet().GetObject<Entity.Users>(" UserName =? ", u.UserName);
            return user;
        }

        /// <summary>
        /// 获取用户实体
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static Entity.Users GetUserByUserName(string userName)
        {
            Entity.Users user = DBSession.TryGet().GetObject<Entity.Users>(" UserName =? ", userName);
            return user;
        }

    }
}
