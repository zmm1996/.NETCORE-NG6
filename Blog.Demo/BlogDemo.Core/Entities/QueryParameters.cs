
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：BlogDemo.Core.Entities
* 项目描述 ：
* 类 名 称 ：QueryParameters
* 类 描 述 ：
* 所在的域 ：DESKTOP-Q3G8VL8
* 命名空间 ：BlogDemo.Core.Entities
* 机器名称 ：DESKTOP-Q3G8VL8 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：zhangmengmeng
* 创建时间 ：2019/11/24 13:09:05
* 更新时间 ：2019/11/24 13:09:05
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ 14589 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion


using BlogDemo.Core.Intefaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace BlogDemo.Core.Entities
{
    //INotifyPropertyChanged：用于绑定属性更新
    public abstract class QueryParameters : INotifyPropertyChanged
    {
        private const int DefaultPageSize = 10;
        private const int DefaultMaxPageSize = 100;

        private int _pageIndex;
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value > 0 ? value : 1;
        }

        private int _pageSize = DefaultPageSize;

        public int PageSize
        {
            get => _pageSize;
            set => SetField(ref _pageSize,value);
        }


        private string _orderBy;
        public string OrderBy
        {
            get => _orderBy;
            set => _orderBy = value ?? nameof(IEntity.Id);
        }


        private int _maxPageSize = DefaultMaxPageSize;
        public int MaxPageSize
        {
            get => _maxPageSize;
            set => SetField(ref _maxPageSize,value);
        }


        public string Fields { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            if(propertyName==nameof(PageSize)||propertyName==nameof(MaxPageSize))
            {
                SetPageSize();
            }
            return true;
            
        }
        private void SetPageSize()
        {
            if (_maxPageSize <= 0)
            {
                _maxPageSize = DefaultMaxPageSize;
            }
            if (_pageSize <= 0)
            {
                _pageSize = DefaultPageSize;
            }
            _pageSize = _pageSize > _maxPageSize ? _maxPageSize : _pageSize;
        }
    }
}
