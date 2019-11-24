
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：BlogDemo.Infrastructure.Repositories
* 项目描述 ：
* 类 名 称 ：IPostRepository
* 类 描 述 ：
* 所在的域 ：DESKTOP-Q3G8VL8
* 命名空间 ：BlogDemo.Infrastructure.Repositories
* 机器名称 ：DESKTOP-Q3G8VL8 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：14589
* 创建时间 ：2019/11/23 11:02:24
* 更新时间 ：2019/11/23 11:02:24
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ 14589 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion


using BlogDemo.Core.Entities;
using BlogDemo.Core.Intefaces;
using BlogDemo.Infrastructure.Database;
using BlogDemo.Infrastructure.Extensions;
using BlogDemo.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDemo.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly MyContext _myContext;
        private readonly IPropertyMappingContainer _propertyMappingContainer;

        public PostRepository(MyContext myContext,
            IPropertyMappingContainer propertyMappingContainer
                )
        {
            this._myContext = myContext;
            this._propertyMappingContainer = propertyMappingContainer;
        }



        public async Task<PaginatedList<Post>> GetAllPostsAsync(PostParameters parameters)
        {
            var query = _myContext.Posts.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.Title))
            {
                var whereStr = parameters.Title.ToLower();
                query = query.Where(x => x.Title.ToLower().Contains(parameters.Title));
            }

            query = query.ApplySort(parameters.OrderBy, _propertyMappingContainer.Resolve<Resources.PostResource, Post>());
             var totalCount = query.Count();

            var data = await query
                 .Skip((parameters.PageIndex - 1) * parameters.PageSize)
                 .Take(parameters.PageSize)
                 .ToListAsync();

            return new PaginatedList<Post>(parameters.PageSize, parameters.PageIndex, totalCount, data);
        }

        public async Task<Post> GetPostByIdAsync(int Id)
        {
            return await _myContext.Posts.FindAsync(Id);
        }

        public void AddPost(Post post)
        {
            _myContext.Posts.Add(post);
        }
    }
}
