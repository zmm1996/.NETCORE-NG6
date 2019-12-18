
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：BlogDemo.Core.Intefaces
* 项目描述 ：
* 类 名 称 ：IPostRepository
* 类 描 述 ：
* 所在的域 ：DESKTOP-Q3G8VL8
* 命名空间 ：BlogDemo.Core.Intefaces
* 机器名称 ：DESKTOP-Q3G8VL8 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：14589
* 创建时间 ：2019/11/23 10:59:19
* 更新时间 ：2019/11/23 10:59:19
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ 14589 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion


using BlogDemo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlogDemo.Core.Intefaces
{
    public interface IPostRepository
    {
        Task<PaginatedList<Post>> GetAllPostsAsync(PostParameters parameters);
        Task<Post> GetPostByIdAsync(int Id);
        void AddPost(Post post);
        void DeletePost(Post post);

    }
}
