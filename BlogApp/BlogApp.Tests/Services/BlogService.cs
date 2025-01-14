using BlogApp.Models;
using BlogApp.Services;
using Xunit;

namespace BlogApp.Tests.Services;

public class BlogServiceTests
{
    private readonly BlogService _service;

    public BlogServiceTests()
    {
        _service = new BlogService();
    }

    [Fact]
    public void Create_ShouldAddBlogPost()
    {
        var post = new BlogPost { Title = "Test", Content = "Content" };

        var createdPost = _service.Create(post);

        Assert.NotNull(createdPost);
        Assert.Equal("Test", createdPost.Title);
        Assert.Equal("Content", createdPost.Content);
        Assert.NotEqual(0, createdPost.Id);
    }

    [Fact]
    public void GetById_ShouldReturnCorrectPost()
    {
        var post = _service.Create(new BlogPost { Title = "Test", Content = "Content" });

        var result = _service.GetById(post.Id);

        Assert.NotNull(result);
        Assert.Equal(post.Id, result!.Id);
    }

    [Fact]
    public void Delete_ShouldRemovePost()
    {
        var post = _service.Create(new BlogPost { Title = "Test", Content = "Content" });

        var isDeleted = _service.Delete(post.Id);

        Assert.True(isDeleted);
        Assert.Null(_service.GetById(post.Id));
    }
}
