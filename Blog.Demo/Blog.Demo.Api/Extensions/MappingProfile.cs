using AutoMapper;
using BlogDemo.Core.Entities;
using BlogDemo.Infrastructure.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogDemo.Api.Extensions
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, PostResource>()
                .ForMember(x=>x.UpdateTime,opt=> opt.MapFrom(src=>src.LastModified));//UpdateTime来自LastTime
            CreateMap<PostResource, Post>();
        }
    }
}
