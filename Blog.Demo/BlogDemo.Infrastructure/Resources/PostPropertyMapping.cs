
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：BlogDemo.Infrastructure.Resources
* 项目描述 ：
* 类 名 称 ：PostPropertyMapping
* 类 描 述 ：
* 所在的域 ：DESKTOP-Q3G8VL8
* 命名空间 ：BlogDemo.Infrastructure.Resources
* 机器名称 ：DESKTOP-Q3G8VL8 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：zhangmengmeng
* 创建时间 ：2019/11/24 16:26:16
* 更新时间 ：2019/11/24 16:26:16
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ 14589 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion


using BlogDemo.Core.Entities;
using BlogDemo.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Infrastructure.Resources
{
    public class PostPropertyMapping : PropertyMapping<PostResource, Post>
    {
        public PostPropertyMapping() : base(
            new Dictionary<string, List<MappedProperty>>(StringComparer.OrdinalIgnoreCase)
            {
                [nameof(PostResource.Title)] = new List<MappedProperty>
                    {
                        new MappedProperty{ Name = nameof(Post.Title), Revert = false}
                    },
                [nameof(PostResource.Body)] = new List<MappedProperty>
                    {
                        new MappedProperty{ Name = nameof(Post.Body), Revert = false}
                    },
                [nameof(PostResource.Author)] = new List<MappedProperty>
                    {
                        new MappedProperty{ Name = nameof(Post.Author), Revert = false}
                    }
            }
            )
        {

        }

    }
}
