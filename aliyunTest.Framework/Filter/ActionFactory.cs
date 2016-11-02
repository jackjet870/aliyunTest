using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace aliyunTest.Framework.Filter
{
    public class ActionFactory
    {
        /// <summary>
        /// 返回所有要授权的类信息
        /// </summary>
        /// <returns>Key:Contoller的名字,Value:类里所有的Action</returns>
        public static Dictionary<string, List<ActionInfoAttribute>> GetAllAction()
        {
            //action 列表
            Dictionary<string, List<ActionInfoAttribute>> ActionList = new Dictionary<string, List<ActionInfoAttribute>>();

            //要进行反射的DLL
            string[] S_Ref_Dll = aliyunTest.Framework.Config.FrameworkConfig.Instance<aliyunTest.Framework.Config.FrameworkConfig>().ControllerRefs.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            //遍历DLL文件
            foreach (string S_Dll in S_Ref_Dll)
            {
                //Dll文件数据
                Byte[] dllFileData = System.IO.File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath(S_Dll));

                //获得DLL里的所有类型
                Type[] ActionType = Assembly.Load(dllFileData).GetTypes();

                //遍历DLL里的Controllers
                foreach (Type Action_Ref in ActionType)
                {
                    //获取类上Controller的描述信息
                    object[] ClassObj = Action_Ref.GetCustomAttributes(typeof(ControllerInfoAttribute), false);

                    if (ClassObj.Length < 1)
                        continue;

                    ControllerInfoAttribute ClassObjAttr = ClassObj[0] as ControllerInfoAttribute;


                    //获得类的Action
                    List<ActionInfoAttribute> Type_Action_Lit = new List<ActionInfoAttribute>();

                    //遍历Action Name属性
                    foreach (MethodInfo MI in Action_Ref.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        object[] ActionObj = MI.GetCustomAttributes(typeof(ActionInfoAttribute), false);

                        if (ActionObj.Length < 1)
                            continue;

                        ActionInfoAttribute ActionRoleAttr = ActionObj[0] as ActionInfoAttribute;

                        if (ActionRoleAttr.IsAuthorize)
                        {
                            //给Action Url赋值
                            ActionRoleAttr.ActionUrl = string.Format("{0}/{1}", Action_Ref.Name.Replace("Controller", ""), MI.Name);

                            Type_Action_Lit.Add(ActionRoleAttr);
                        }
                    }

                    //将类信息加到返回集里
                    ActionList.Add(ClassObjAttr.Name, Type_Action_Lit);
                }
            }

            return ActionList;
        }


        public static SortedDictionary<string, Dictionary<string, List<ActionInfoAttribute>>> GetAllActionGroup()
        {
            #region 基本数据获取

            ///action 列表
            Dictionary<string, List<ActionInfoAttribute>> ActionList = new Dictionary<string, List<ActionInfoAttribute>>();

            //要进行反射的DLL
            string[] S_Ref_Dll = aliyunTest.Framework.Config.FrameworkConfig.Instance<aliyunTest.Framework.Config.FrameworkConfig>().ControllerRefs.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            //遍历DLL文件
            foreach (string S_Dll in S_Ref_Dll)
            {
                //Dll文件数据
                Byte[] dllFileData = System.IO.File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath(S_Dll));

                //获得DLL里的所有类型
                Type[] ActionType = Assembly.Load(dllFileData).GetTypes();

                //遍历DLL里的Controllers
                foreach (Type Action_Ref in ActionType)
                {
                    //获取类上Controller的描述信息
                    object[] ClassObj = Action_Ref.GetCustomAttributes(typeof(ControllerInfoAttribute), false);

                    if (ClassObj.Length < 1)
                        continue;

                    ControllerInfoAttribute ClassObjAttr = ClassObj[0] as ControllerInfoAttribute;


                    //获得类的Action
                    List<ActionInfoAttribute> Type_Action_Lit = new List<ActionInfoAttribute>();

                    //遍历Action Name属性
                    foreach (MethodInfo MI in Action_Ref.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        object[] ActionObj = MI.GetCustomAttributes(typeof(ActionInfoAttribute), false);

                        if (ActionObj.Length < 1)
                            continue;

                        ActionInfoAttribute ActionRoleAttr = ActionObj[0] as ActionInfoAttribute;

                        if (ActionRoleAttr.IsAuthorize)
                        {
                            //给Action Url赋值
                            ActionRoleAttr.ActionUrl = string.Format("{0}/{1}", Action_Ref.Name.Replace("Controller", ""), MI.Name);

                            Type_Action_Lit.Add(ActionRoleAttr);
                        }
                    }

                    //将类信息加到返回集里
                    ActionList.Add(ClassObjAttr.Name, Type_Action_Lit);
                }
            }

            #endregion

            #region 最终数据整理

            SortedDictionary<string, Dictionary<string, List<ActionInfoAttribute>>> result = new SortedDictionary<string, Dictionary<string, List<ActionInfoAttribute>>>();

            foreach (KeyValuePair<string, List<ActionInfoAttribute>> item in ActionList)
            {
                //默认添加Class
                result.Add(item.Key, new Dictionary<string, List<ActionInfoAttribute>>());

                foreach (ActionInfoAttribute acItem in item.Value)
                {
                    string[] seconds = acItem.Name.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

                    if (seconds == null || seconds.Length != 2)
                        continue;

                    //class下有没有添加数据
                    if (result[item.Key] == null)
                        result[item.Key] = new Dictionary<string, List<ActionInfoAttribute>>();

                    //添加二级名
                    if (!result[item.Key].ContainsKey(seconds[0]))
                        result[item.Key].Add(seconds[0], new List<ActionInfoAttribute>());

                    //二级下有没有数据
                    if (result[item.Key][seconds[0]] == null)
                        result[item.Key][seconds[0]] = new List<ActionInfoAttribute>();

                    acItem.Name = seconds[1];

                    result[item.Key][seconds[0]].Add(acItem);

                }
            }

            #endregion
             

            return result;
        }

    }
}
