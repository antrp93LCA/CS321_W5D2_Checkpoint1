using CS321_W5D2_BlogAPI.Core.Models;
using CS321_W5D2_BlogAPI.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CS321_W5D2_BlogAPI.Infrastructure.Data
{
    public class PostRepository : IPostRepository
    {
        private AppDbContext _dbContext;

        public PostRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Post Get(int id)
        {
            return _dbContext.Posts
                .Include(p => p.Blog)
                .Include(p => p.Blog.User)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Post> GetBlogPosts(int blogId)
        {
            //  Implement GetBlogPosts, return all posts for given blog id
            //  Include related Blog and AppUser
            return _dbContext.Posts
                .Include(p => p.Blog)
                .Include(p => p.Blog.User)
                .Where(p => p.BlogId == blogId)
                .ToList();
        }

        public Post Add(Post post)
        {
            _dbContext.Add(post);
            _dbContext.SaveChanges();
            return post;
        }

        public Post Update(Post updatedPost)
        {
            var currentPost = _dbContext.Posts.Find(updatedPost.Id);

            if (currentPost == null) return null;

            _dbContext.Entry(currentPost)
                .CurrentValues
                .SetValues(updatedPost);

            _dbContext.Posts.Update(currentPost);
            _dbContext.SaveChanges();
            return currentPost;
        }

        public IEnumerable<Post> GetAll()
        {
            // get all posts
            return _dbContext.Posts
                .ToList();
        }

        public void Remove(int id)
        {
            var delPost = _dbContext.Posts.Find(id);

            if (delPost != null)
            {
                _dbContext.Posts.Remove(delPost);
                _dbContext.SaveChanges();
            }
        }
    }
}