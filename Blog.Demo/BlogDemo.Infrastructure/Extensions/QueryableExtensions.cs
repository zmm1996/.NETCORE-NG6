
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：BlogDemo.Infrastructure.Extensions
* 项目描述 ：
* 类 名 称 ：QueryableExtensions
* 类 描 述 ：
* 所在的域 ：DESKTOP-Q3G8VL8
* 命名空间 ：BlogDemo.Infrastructure.Extensions
* 机器名称 ：DESKTOP-Q3G8VL8 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：zhangmengmeng
* 创建时间 ：2019/11/24 16:31:19
* 更新时间 ：2019/11/24 16:31:19
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ 14589 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion


using BlogDemo.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;
namespace BlogDemo.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy, IPropertyMapping propertyMapping)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (propertyMapping == null)
            {
                throw new ArgumentNullException(nameof(propertyMapping));
            }

            var mappingDictionary = propertyMapping.MappingDictionary;
            if (mappingDictionary == null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByAfterSplit = orderBy.Split(',');
            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                var trimmedOrderByClause = orderByClause.Trim();
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ", StringComparison.Ordinal);
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);
                if (string.IsNullOrEmpty(propertyName))
                {
                    continue;
                }
                if (!mappingDictionary.TryGetValue(propertyName, out List<MappedProperty> mappedProperties))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }
                if (mappedProperties == null)
                {
                    throw new ArgumentNullException(propertyName);
                }
                mappedProperties.Reverse();
                foreach (var destinationProperty in mappedProperties)
                {
                    if (destinationProperty.Revert)
                    {
                        orderDescending = !orderDescending;
                    }
                    source = source.OrderBy(destinationProperty.Name + (orderDescending ? " descending" : " ascending"));
                }
            }

            return source;
        }

        public static IQueryable<object> ToDynamicQueryable<TSource>
            (this IQueryable<TSource> source, string fields, Dictionary<string, List<MappedProperty>> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            if (string.IsNullOrWhiteSpace(fields))
            {
                return (IQueryable<object>)source;
            }

            fields = fields.ToLower();
            var fieldsAfterSplit = fields.Split(',').ToList();
            if (!fieldsAfterSplit.Contains("id", StringComparer.InvariantCultureIgnoreCase))
            {
                fieldsAfterSplit.Add("id");
            }
            var selectClause = "new (";

            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();
                if (string.IsNullOrEmpty(propertyName))
                {
                    continue;
                }

                var key = mappingDictionary.Keys.SingleOrDefault(k => String.CompareOrdinal(k.ToLower(), propertyName.ToLower()) == 0);
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }
                var mappedProperties = mappingDictionary[key];
                if (mappedProperties == null)
                {
                    throw new ArgumentNullException(key);
                }
                foreach (var destinationProperty in mappedProperties)
                {
                    selectClause += $" {destinationProperty.Name},";
                }
            }

            selectClause = selectClause.Substring(0, selectClause.Length - 1) + ")";
            return (IQueryable<object>)source.Select(selectClause);
        }

    }
}
