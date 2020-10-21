using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaripSozluk.Api.Models;
using GaripSozluk.Business.Interfaces;
using GaripSozluk.Data.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GaripSozluk.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public List<PostApiVM> Get()
        {

            return _postService.GetAll().ToList();
        }

    }
}
