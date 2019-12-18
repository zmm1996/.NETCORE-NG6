using AutoMapper;
using BlogDemo.Core.Entities;
using BlogDemo.Core.Intefaces;
using BlogDemo.Infrastructure.Database;
using BlogDemo.Infrastructure.Extensions;
using BlogDemo.Infrastructure.Resources;
using BlogDemo.Infrastructure.Services;
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
        private readonly IValidator<PostUpdateResource> _validator;
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IPropertyMappingContainer _propertyMappingContainer;
        private readonly ILogger _logger;

        public PostController(IPostRepository postRepository,
            IUnitOfWork unitOfWork,
            //ILogger<PostController> logger//log两种方式
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper,//AotuMapper
            IValidator<PostUpdateResource> validator,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService,
            IPropertyMappingContainer propertyMappingContainer
            )
        {
            this._postRepository = postRepository;
            this._unitOfWork = unitOfWork;
            this._configuration = configuration;
            this._mapper = mapper;
            this._validator = validator;
            this._urlHelper = urlHelper;
            this._typeHelperService = typeHelperService;
            this._propertyMappingContainer = propertyMappingContainer;

            //this._logger = logger;
            this._logger = loggerFactory.CreateLogger(nameof(PostController));
        }

        [HttpGet(Name = "GetPosts")]
        public async Task<ActionResult> Get(PostParameters parameters)
        {
            if (!_propertyMappingContainer.ValidateMappingExistsFor<PostResource, Post>(parameters.OrderBy))
            {
                return BadRequest("OrderBy not exist ");
            }

            if (!_typeHelperService.TypeHasProperties<PostResource>(parameters.Fields))
            {
                return BadRequest("fields not exist ");
            }

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

        [HttpGet("{Id}",Name = "GetPost")]
        public async Task<ActionResult> Get(int Id, string fields = null)
        {
            if (!_typeHelperService.TypeHasProperties<PostResource>(fields))
            {
                return BadRequest("fields not exist ");
            }
            var post = await _postRepository.GetPostByIdAsync(Id);
            if (post == null)
            {
                return NotFound();
            }

            var postResource = _mapper.Map<Post, PostResource>(post);

            var result = postResource.ToDynamic(fields);
            return Ok(result);
        }
        [HttpPost(Name ="CreatePost")]
        public async Task<ActionResult> Post([FromBody]PostAddResource postResource)
        {
            if (postResource == null)
            {
                return BadRequest();
            }

            //使用Fluent Validator
            var resultValidator = _validator.Validate(postResource);

            if (!resultValidator.IsValid)
            {
                //验证失败
                string ErrorMessage = string.Join(";", resultValidator.Errors);
              //  return new ObjectResult(new { code = 422, ErrorMessage = ErrorMessage });
                return UnprocessableEntity(ErrorMessage);//422错误
            }
            var newPost = _mapper.Map<PostAddResource, Post>(postResource);

            newPost.Author = "Admin";
            newPost.LastModified = DateTime.Now;

            _postRepository.AddPost(newPost);

            //工作单元
            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception("save fields");
            }
            var resultResource = _mapper.Map<Post, PostResource>(newPost);
            //返回当前创建的内容,通过headers中的Localhost
            return CreatedAtRoute("GetPost", new { Id = resultResource.Id }, resultResource);
        }

        [HttpDelete("{id}",Name ="DeletePost")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if(post==null)
            {
                return NotFound();
            }

            _postRepository.DeletePost(post);

            if( !await _unitOfWork.SaveAsync())
            {
                throw new Exception($"deleting post {id} fields when saving");
            }
            return NoContent();

        }
        [HttpPut("{id}",Name ="UpdatePost")]
        public async Task<IActionResult> UpdatePost(int id,[FromBody] PostUpdateResource postUpdateResource)
        {
            if(postUpdateResource==null)
            {
                return BadRequest();//400错误
            }
          var resultValidator=  _validator.Validate(postUpdateResource);

            if(!resultValidator.IsValid)
            {
                string errorMessage = string.Join(",", resultValidator.Errors);
                return UnprocessableEntity(errorMessage);//返回422状态码
            }

          var post= await _postRepository.GetPostByIdAsync(id);

            if(post==null)
            {
                return NotFound();
            }

            _mapper.Map(postUpdateResource, post);

            if(!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"updating post {id} fields when saving  ");
            }

            return NoContent();
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
