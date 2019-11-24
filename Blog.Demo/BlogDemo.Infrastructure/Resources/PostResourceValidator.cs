
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：BlogDemo.Infrastructure.Resources
* 项目描述 ：
* 类 名 称 ：PostResourceValidator
* 类 描 述 ：
* 所在的域 ：DESKTOP-Q3G8VL8
* 命名空间 ：BlogDemo.Infrastructure.Resources
* 机器名称 ：DESKTOP-Q3G8VL8 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：zhangmengmeng
* 创建时间 ：2019/11/23 19:50:25
* 更新时间 ：2019/11/23 19:50:25
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ 14589 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion


using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Infrastructure.Resources
{
    public class PostResourceValidator:AbstractValidator<PostResource>
    {
        public PostResourceValidator()
        {
            RuleFor(x => x.Author).NotNull()
                .WithName("作者")
                .NotEmpty()
                .WithMessage("{PropertyName}必填")
                .MaximumLength(50)
                .WithMessage("{PropertyName}长度是{MaxLength}");
        }
                
    }
}
