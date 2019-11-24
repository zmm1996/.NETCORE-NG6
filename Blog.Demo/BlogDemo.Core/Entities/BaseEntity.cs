using BlogDemo.Core.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Core.Entities
{
    public abstract class BaseEntity : IEntity
    {
        public  int Id { get; set; }
    }
}
