
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：BlogDemo.Core.Entities
* 项目描述 ：
* 类 名 称 ：PostParameters
* 类 描 述 ：
* 所在的域 ：DESKTOP-Q3G8VL8
* 命名空间 ：BlogDemo.Core.Entities
* 机器名称 ：DESKTOP-Q3G8VL8 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：zhangmengmeng
* 创建时间 ：2019/11/24 13:37:49
* 更新时间 ：2019/11/24 13:37:49
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
    public class PostParameters:QueryParameters
    {
        public string  Title { get; set; }
    }
}
