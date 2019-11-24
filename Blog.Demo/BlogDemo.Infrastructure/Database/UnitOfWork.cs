
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：BlogDemo.Infrastructure.Database
* 项目描述 ：
* 类 名 称 ：UnitOfWork
* 类 描 述 ：
* 所在的域 ：DESKTOP-Q3G8VL8
* 命名空间 ：BlogDemo.Infrastructure.Database
* 机器名称 ：DESKTOP-Q3G8VL8 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：zhangmengmeng
* 创建时间 ：2019/11/23 11:15:16
* 更新时间 ：2019/11/23 11:15:16
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
using System.Threading.Tasks;

namespace BlogDemo.Infrastructure.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyContext _myContext;

        public UnitOfWork(MyContext myContext)
        {
            this._myContext = myContext;
        }
        public async Task<bool> SaveAsync()
        {
           return await _myContext.SaveChangesAsync()>0;
        }
    }
}
