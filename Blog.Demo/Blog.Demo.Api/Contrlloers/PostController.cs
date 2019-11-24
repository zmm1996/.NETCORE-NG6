using AutoMapper;
using BlogDemo.Core.Entities;
using BlogDemo.Core.Intefaces;
using BlogDemo.Infrastructure.Database;
using BlogDemo.Infrastructure.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogDemo.Api.Contrlloers
{
    [Route("api/posts")]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IValidator _validator;
        private readonly ILogger _logger;

        public PostController(IPostRepository postRepository,
            IUnitOfWork unitOfWork,
            //ILogger<PostController> logger//log两种方式
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper,//AotuMapper
            IValidator<PostResource> validator
            )
        {
            this._postRepository = postRepository;
            this._unitOfWork = unitOfWork;
            this._configuration = configuration;
            this._mapper = mapper;
            this._validator = validator;

            //this._logger = logger;
            this._logger= loggerFactory.CreateLogger(nameof(PostController));
        }
        public async Task<ActionResult> Get()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            //  _logger.LogError(12,"get post all/.....");
          var result=  _mapper.Map<IEnumerable<Post>, IEnumerable<PostResource>>(posts);
         

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]PostResource  postResource)
        {
          var resultValidator=  _validator.Validate(postResource);

            if(!resultValidator.IsValid)
            {
                //验证失败
                string ErrorMessage = string.Join(";", resultValidator.Errors);
                return new ObjectResult(new { code=422,ErrorMessage=ErrorMessage});
            }

            Post post = new Post
            {
                Author = "zmm",
                Body = "sbak",
                LastModified = DateTime.Now,
                Title = "add"
            };
            _postRepository.AddPost(post);

            //工作单元
            await _unitOfWork.SaveAsync();
            return Ok();

          
        }

    }
}
