using AutoMapper;
using BlogDemo.Core.Entities;
using BlogDemo.Core.Intefaces;
using BlogDemo.Infrastructure.Database;
using BlogDemo.Infrastructure.Extensions;
using BlogDemo.Infrastructure.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
        private readonly IUrlHelper _urlHelper;
        private readonly ILogger _logger;

        public PostController(IPostRepository postRepository,
            IUnitOfWork unitOfWork,
            //ILogger<PostController> logger//log两种方式
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper,//AotuMapper
            IValidator<PostResource> validator
            , IUrlHelper urlHelper
            )
        {
            this._postRepository = postRepository;
            this._unitOfWork = unitOfWork;
            this._configuration = configuration;
            this._mapper = mapper;
            this._validator = validator;
            this._urlHelper = urlHelper;

            //this._logger = logger;
            this._logger = loggerFactory.CreateLogger(nameof(PostController));
        }

        [HttpGet(Name = "GetPosts")]
        public async Task<ActionResult> Get(PostParameters parameters)
        {
            var postsList = await _postRepository.GetAllPostsAsync(parameters);
            //  _logger.LogError(12,"get post all/.....");
            var resources = _mapper.Map<IEnumerable<Post>, IEnumerable<PostResource>>(postsList);

            //动态查询字段
            var result = resources.ToDynamicIEnumerable(parameters.Fields);

            //创建上一页
            var previousPagelink = postsList.HasPrevious ? CreatePostUri(parameters, PaginationResourceUriType.PreviousPage) : null;
            //创建下一页
            var nextPagelink = postsList.HasNext ? CreatePostUri(parameters, PaginationResourceUriType.NextPage) : null;


            var meta = new
            {
                postsList.PageSize,
                postsList.PageIndex,
                postsList.TotalItemCount,
                postsList.PageCount,
                previousPagelink,
                nextPagelink
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta, new JsonSerializerSettings
            {
                //大写转小写
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> Get(int Id,string fields=null)
        {
            var post = await _postRepository.GetPostByIdAsync(Id);
            if (post == null)
            {
                return NotFound();
            }
            
            var postResource = _mapper.Map<Post, PostResource>(post);

            var result = postResource.ToDynamic(fields);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]PostResource postResource)
        {

            //使用Fluent Validator
            var resultValidator = _validator.Validate(postResource);

            if (!resultValidator.IsValid)
            {
                //验证失败
                string ErrorMessage = string.Join(";", resultValidator.Errors);
                return new ObjectResult(new { code = 422, ErrorMessage = ErrorMessage });
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


        /// <summary>
        /// 生成链接
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="uriType"></param>
        /// <returns></returns>
        private string CreatePostUri(PostParameters parameters, PaginationResourceUriType uriType)
        {
            switch (uriType)
            {
                case PaginationResourceUriType.PreviousPage:
                    var previousParameters = new
                    {
                        pageIndex = parameters.PageIndex - 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                        title = parameters.Title
                    };
                    return _urlHelper.Link("GetPosts", previousParameters);
                case PaginationResourceUriType.NextPage:
                    var nextParameters = new
                    {
                        pageIndex = parameters.PageIndex + 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                        title = parameters.Title
                    };
                    return _urlHelper.Link("GetPosts", nextParameters);
                default:
                    var currentParameters = new
                    {
                        pageIndex = parameters.PageIndex,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                        title = parameters.Title
                    };
                    return _urlHelper.Link("GetPosts", currentParameters);
            }
        }

    }
}
