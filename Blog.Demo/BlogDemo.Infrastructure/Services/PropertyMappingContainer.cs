
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：BlogDemo.Infrastructure.Services
* 项目描述 ：
* 类 名 称 ：PropertyMappingContainer
* 类 描 述 ：
* 所在的域 ：DESKTOP-Q3G8VL8
* 命名空间 ：BlogDemo.Infrastructure.Services
* 机器名称 ：DESKTOP-Q3G8VL8 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：zhangmengmeng
* 创建时间 ：2019/11/24 16:22:51
* 更新时间 ：2019/11/24 16:22:51
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ 14589 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion


using BlogDemo.Core.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogDemo.Infrastructure.Services
{
    public class PropertyMappingContainer:IPropertyMappingContainer
    {
        protected internal readonly IList<IPropertyMapping> PropertyMappings = new List<IPropertyMapping>();

        public void Register<T>() where T : IPropertyMapping, new()
        {
            if (PropertyMappings.All(x => x.GetType() != typeof(T)))
            {
                PropertyMappings.Add(new T());
            }
        }

        public IPropertyMapping Resolve<TSource, TDestination>() where TDestination : IEntity
        {
            var matchingMapping = PropertyMappings.OfType<PropertyMapping<TSource, TDestination>>().ToList();
            if (matchingMapping.Count == 1)
            {
                return matchingMapping.First();
            }

            throw new Exception($"Cannot find property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool ValidateMappingExistsFor<TSource, TDestination>(string fields) where TDestination : IEntity
        {
            var propertyMapping = Resolve<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var fieldsAfterSplit = fields.Split(',');
            foreach (var field in fieldsAfterSplit)
            {
                var trimmedField = field.Trim();
                var indexOfFirstSpace = trimmedField.IndexOf(" ", StringComparison.Ordinal);
                var propertyName = indexOfFirstSpace == -1 ? trimmedField : trimmedField.Remove(indexOfFirstSpace);
                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    continue;
                }
                if (!propertyMapping.MappingDictionary.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
