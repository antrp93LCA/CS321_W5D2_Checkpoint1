using System;
using System.Collections.Generic;
using System.Linq;
using CS321_W5D2_BlogAPI.Core.Models;
using CS321_W5D2_BlogAPI.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CS321_W5D2_BlogAPI.Infrastructure.Data
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _dbContext;

        public BlogRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Blog> GetAll()
        {
            return _dbContext.Blogs
                .Include(b => b.User)
                .ToList();
        }

        public Blog Get(int id)
        {
            return _dbContext.Blogs
                .Include(b => b.User)
                .SingleOrDefault(b => b.Id == id);
                
        }

        public Blog Add(Blog blog)
        {
            // TODO: Add new blog
            _dbContext.Blogs.Add(blog);
            _dbContext.SaveChanges();
            return blog;
        }

        public Blog Update(Blog updatedBlog)
        {

            var currentBlog = _dbContext.Blogs.Find(updatedBlog.Id);

            if (currentBlog == null) return null;

            _dbContext.Entry(currentBlog)
                .CurrentValues
                .SetValues(updatedBlog);

            _dbContext.Blogs.Update(currentBlog);
            _dbContext.SaveChanges();
            return currentBlog;
                
        }

        public void Remove(int id)
        {
            var delBlog = _dbContext.Blogs.Find(id);

            if(delBlog != null)
            {
                _dbContext.Blogs.Remove(delBlog);
                _dbContext.SaveChanges();
                    
                
            }

        }
    }
}
