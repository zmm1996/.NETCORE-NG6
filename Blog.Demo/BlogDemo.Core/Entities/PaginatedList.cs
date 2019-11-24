
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：BlogDemo.Core.Entities
* 项目描述 ：
* 类 名 称 ：PaginatedList
* 类 描 述 ：
* 所在的域 ：DESKTOP-Q3G8VL8
* 命名空间 ：BlogDemo.Core.Entities
* 机器名称 ：DESKTOP-Q3G8VL8 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：zhangmengmeng
* 创建时间 ：2019/11/24 13:53:37
* 更新时间 ：2019/11/24 13:53:37
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ 14589 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion


using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Core.Entities
{
    public class PaginatedList<T> :List<T> where T :class
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        private int _totalItemCount;
        public int TotalItemCount {
            get => _totalItemCount;
            set => _totalItemCount = value>=0?value:0;
        }
        public int PageCount => TotalItemCount / PageSize + (TotalItemCount % PageSize > 0 ? 1 : 0);

        public bool HasPrevious => PageIndex > 1;
        public bool HasNext => PageIndex < PageCount;

        public PaginatedList(int pageSize,int pageIndex,int totalItemCount,IEnumerable<T> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalItemCount = totalItemCount;
            AddRange(data);
        }


    }
}
