
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：BlogDemo.Infrastructure.Services
* 项目描述 ：
* 类 名 称 ：TypeHelperService
* 类 描 述 ：
* 所在的域 ：DESKTOP-Q3G8VL8
* 命名空间 ：BlogDemo.Infrastructure.Services
* 机器名称 ：DESKTOP-Q3G8VL8 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：zhangmengmeng
* 创建时间 ：2019/12/4 20:19:55
* 更新时间 ：2019/12/4 20:19:55
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ 14589 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion


using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BlogDemo.Infrastructure.Services
{
    public class TypeHelperService : ITypeHelperService
    {
        public bool TypeHasProperties<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            string[] fieldsAfterSplit = fields.Split(',');

            foreach (var field in fieldsAfterSplit)
            {
                string propertyName = field.Trim();

                if(string.IsNullOrEmpty(propertyName))
                {
                    continue;
                }
                var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase|BindingFlags.Public|BindingFlags.Instance);
                if(propertyInfo==null)
                {
                    return false;
                }
            }
            return true;      

        }
    }
}
