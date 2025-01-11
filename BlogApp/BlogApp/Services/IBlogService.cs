using BlogApp.Models;

namespace BlogApp.Services;

public interface IBlogService
{
    IEnumerable<BlogPost> GetAll();
    BlogPost? GetById(int id);
    BlogPost Create(BlogPost post);
    bool Update(int id, BlogPost updatedPost);
    bool Delete(int id);
}
