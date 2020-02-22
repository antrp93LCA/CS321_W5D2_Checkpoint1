using CS321_W5D2_BlogAPI.ApiModels;
using CS321_W5D2_BlogAPI.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CS321_W5D2_BlogAPI.Controllers
{
    // TODO: secure controller actions that change data
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;

        // inject PostService
        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        //  get posts for blog
        //  allow anyone to get, even if not logged in
        // GET /api/blogs/{blogId}/posts
        [AllowAnonymous]
        [HttpGet("/api/blogs/{blogId}/posts")]
        public IActionResult Get(int blogId)
        {
            var blogPosts = _postService.GetBlogPosts(blogId);

            if (blogPosts == null) return NotFound();

            return Ok(blogPosts.ToApiModels());
        }

        // get post by id
        // GET api/blogs/{blogId}/posts/{postId}
        [AllowAnonymous]
        [HttpGet("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Get(int blogId, int postId)
        {
            var post = _postService.Get(postId);

            if (post == null) return NotFound();

            return Ok(post.ToApiModel());
        }

        // TODO: add a new post to blog
        // POST /api/blogs/{blogId}/post
        [Authorize]
        [HttpPost("/api/blogs/{blogId}/posts")]
        public IActionResult Post(int blogId, [FromBody]PostModel postModel)
        {
            try
            {
                //add the post
                var newPost = _postService.Add(postModel.ToDomainModel());
                return Ok(newPost);
            }
            catch (System.Exception ex)
            {
                //handle bad model
                ModelState.AddModelError("AddPost", ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        // PUT /api/blogs/{blogId}/posts/{postId}
        [Authorize]
        [HttpPut("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Put(int blogId, int postId, [FromBody]PostModel postModel)
        {
            try
            {
                var updatedPost = _postService.Update(postModel.ToDomainModel());
                return Ok(updatedPost);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("UpdatePost", ex.Message);
                return BadRequest(ModelState);
            }
        }

        // delete post by id
        // DELETE /api/blogs/{blogId}/posts/{postId}
        [Authorize]
        [HttpDelete("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Delete(int blogId, int postId)
        {
            var post = _postService.Get(postId);
            if (post == null) return NotFound();
            try
            {
                _postService.Remove(post.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex);
            }
        }
    }
}