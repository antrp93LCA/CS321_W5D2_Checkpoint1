using CS321_W5D2_BlogAPI.Core.Models;
using System;
using System.Collections.Generic;

namespace CS321_W5D2_BlogAPI.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly IUserService _userService;

        public PostService(IPostRepository postRepository, IBlogRepository blogRepository, IUserService userService)
        {
            _postRepository = postRepository;
            _blogRepository = blogRepository;
            _userService = userService;
        }

        public Post Add(Post newPost)
        {
            var currentUserId = _userService.CurrentUserId;

            var blog = _blogRepository.Get(newPost.BlogId);

            if (currentUserId != blog.UserId)
            {
                throw new ApplicationException("You are unauthorized to add to this blog");
            }
            newPost.DatePublished = DateTime.Now;
            return _postRepository.Add(newPost);
                
        }

        public Post Get(int id)
        {
            return _postRepository.Get(id);
        }

        public IEnumerable<Post> GetAll()
        {
            return _postRepository.GetAll();
        }

        public IEnumerable<Post> GetBlogPosts(int blogId)
        {
            return _postRepository.GetBlogPosts(blogId);
        }

        public void Remove(int id)
        {
            var post = Get(id);

            //var currentBlog = post.BlogId;
            
            // prevent user from deleting from a blog that isn't theirs
           
            var currentUser = _userService.CurrentUserId;

            

            if(post.Blog.UserId != currentUser)
            {
                throw new ApplicationException("You are unauthorized to remove this blog");
            }

            _postRepository.Remove(id);
        }

        public Post Update(Post updatedPost)
        {
            // prevent user from updating a blog that isn't theirs

            // variable to hold UserId
            var currentUser = _userService.CurrentUserId;



            
            if(currentUser != updatedPost.Blog.UserId)
            {
                throw new ApplicationException("You are unauthorized to update this blog");
            }
            return _postRepository.Update(updatedPost);
        }
    }
}