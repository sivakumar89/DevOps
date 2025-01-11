using BlogApp.Models;

namespace BlogApp.Services;

public class BlogService : IBlogService
{
    private readonly List<BlogPost> _blogPosts = new();

    public IEnumerable<BlogPost> GetAll() => _blogPosts;

    public BlogPost? GetById(int id) => _blogPosts.FirstOrDefault(p => p.Id == id);

    public BlogPost Create(BlogPost post)
    {
        post.Id = _blogPosts.Count + 1;
        _blogPosts.Add(post);
        return post;
    }

    public bool Update(int id, BlogPost updatedPost)
    {
        var existingPost = GetById(id);
        if (existingPost == null) return false;

        existingPost.Title = updatedPost.Title;
        existingPost.Content = updatedPost.Content;
        return true;
    }

    public bool Delete(int id)
    {
        var post = GetById(id);
        if (post == null) return false;

        _blogPosts.Remove(post);
        return true;
    }
}
