using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogDemo.Api.Contrlloers
{
    [Route("api/values")]
    public class ValuesController:Controller
    {

        [HttpGet]
        public ActionResult Get()
        {

            return Ok("hello");
        }

    }
}
