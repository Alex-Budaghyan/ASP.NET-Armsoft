using Homework1.Data;
using Homewrok1.Controllers;
using Homewrok1.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class UsersControllerTests
{
    private readonly Mock<ReqResClient> _mockClient;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mockClient = new Mock<ReqResClient>();
        _controller = new UsersController(_mockClient.Object);
    }

    [Fact]
    public async Task GetUser_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var user = new User { Id = 10, FirstName = "Armen" };
        _mockClient.Setup(c => c.GetUser(1)).ReturnsAsync(user);

        // Act
        var result = await _controller.GetUser(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(user, okResult.Value);
    }

    [Fact]
    public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        _mockClient.Setup(c => c.GetUser(2)).ReturnsAsync((User?)null);

        var result = await _controller.GetUser(2);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateUser_ReturnsCreatedAtAction_WhenSuccessful()
    {
        var input = new User { FirstName = "Arman" };
        var created = new User { Id = 2, FirstName = "Arman" };

        _mockClient.Setup(c => c.CreateUser(input)).ReturnsAsync(created);

        var result = await _controller.CreateUser(input);

        var createdAt = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetUser", createdAt.ActionName);
        Assert.Equal(created.Id, ((User)createdAt.Value!).Id);
    }

    [Fact]
    public async Task CreateUser_ReturnsBadRequest_WhenNullReturned()
    {
        var input = new User { FirstName = "Arman" };
        _mockClient.Setup(c => c.CreateUser(input)).ReturnsAsync((User?)null);

        var result = await _controller.CreateUser(input);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Failed to create user.", badRequest.Value);
    }

    [Fact]
    public async Task UpdateUser_ReturnsOk_WhenSuccessful()
    {
        var updated = new User { Id = 2, FirstName = "Updated" };
        _mockClient.Setup(c => c.UpdateUser(2, updated)).ReturnsAsync(updated);

        var result = await _controller.UpdateUser(2, updated);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(updated, ok.Value);
    }

    [Fact]
    public async Task UpdateUser_ReturnsNotFound_WhenNotFound()
    {
        var updated = new User { Id = 2, FirstName = "Updated" };
        _mockClient.Setup(c => c.UpdateUser(1, updated)).ReturnsAsync((User?)null);

        var result = await _controller.UpdateUser(1, updated);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNoContent()
    {
        _mockClient.Setup(c => c.DeleteUser(1)).ReturnsAsync(true);

        var result = await _controller.DeleteUser(1);

        Assert.IsType<NoContentResult>(result);
    }
}
