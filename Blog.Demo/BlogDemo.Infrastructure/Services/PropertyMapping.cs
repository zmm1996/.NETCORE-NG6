
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：BlogDemo.Infrastructure.Services
* 项目描述 ：
* 类 名 称 ：PropertyMapping
* 类 描 述 ：
* 所在的域 ：DESKTOP-Q3G8VL8
* 命名空间 ：BlogDemo.Infrastructure.Services
* 机器名称 ：DESKTOP-Q3G8VL8 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：zhangmengmeng
* 创建时间 ：2019/11/24 16:13:50
* 更新时间 ：2019/11/24 16:13:50
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ 14589 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion


using BlogDemo.Core.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Infrastructure.Services
{
    public class PropertyMapping<TSource, TDestination> : IPropertyMapping where TDestination:IEntity
    {
        public Dictionary<string, List<MappedProperty>> MappingDictionary { get; }

        protected PropertyMapping(Dictionary<string, List<MappedProperty>>mappingDictionary)
        {
            MappingDictionary = mappingDictionary;
            MappingDictionary[nameof(IEntity.Id)] = new List<MappedProperty>
            {
                new MappedProperty{ Name=nameof(IEntity.Id), Revert=false}
            };
        }

    }
}
