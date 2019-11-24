
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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlogDemo.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly MyContext _myContext;

        public PostRepository(MyContext myContext)
        {
            this._myContext = myContext;
        }

       

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return  await _myContext.Posts.ToListAsync();
        }

        public void AddPost(Post post)
        {
            _myContext.Posts.Add(post);
        }
    }
}
