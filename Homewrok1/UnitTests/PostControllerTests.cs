using Homewrok1;
using Homewrok1.Controllers;
using Homewrok1.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class PostControllerTests
{
    private readonly Mock<JsonPlaceholderClient> _mockClient;
    private readonly PostController _controller;

    public PostControllerTests()
    {
        _mockClient = new Mock<JsonPlaceholderClient>();
        _controller = new PostController(_mockClient.Object);
    }

    [Fact]
    public async Task GetPostById_ReturnsOk_WhenPostExists()
    {
        var post = new Post { Id = 1, Title = "AAA" };
        _mockClient.Setup(c => c.GetPostById(1)).ReturnsAsync(post);

        var result = await _controller.GetPostById(1);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(post, ok.Value);
    }

    [Fact]
    public async Task GetPostById_ReturnsNotFound_WhenNull()
    {
        _mockClient.Setup(c => c.GetPostById(1)).ReturnsAsync((Post?)null);

        var result = await _controller.GetPostById(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreatePost_ReturnsCreatedAt_WhenSuccessful()
    {
        var post = new Post { Title = "New" };
        var created = new Post { Id = 10, Title = "New" };
        _mockClient.Setup(c => c.CreatePost(post)).ReturnsAsync(created);

        var result = await _controller.CreatePost(post);

        var createdAt = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(created.Id, ((Post)createdAt.Value!).Id);
    }

    [Fact]
    public async Task CreatePost_ReturnsBadRequest_WhenFailed()
    {
        var post = new Post { Title = "New" };
        _mockClient.Setup(c => c.CreatePost(post)).ReturnsAsync((Post?)null);

        var result = await _controller.CreatePost(post);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Failed to create post.", badRequest.Value);
    }

    [Fact]
    public async Task DeletePost_ReturnsNoContent()
    {
        _mockClient.Setup(c => c.DeletePost(1)).ReturnsAsync(true);

        var result = await _controller.DeletePost(1);

        Assert.IsType<NoContentResult>(result);
    }
}
