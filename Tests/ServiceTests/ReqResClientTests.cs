using Homework1.Data;
using Homewrok1.Models;
using Homewrok1.Options;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

public class ReqResClientTests
{
    private HttpClient CreateMockHttpClient(HttpResponseMessage response)
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        return new HttpClient(handler.Object);
    }

    private ReqResClient CreateClient(HttpResponseMessage response)
    {
        var options = Options.Create(new ReqResOptions { BaseUrl = "https://reqres.in/" });
        var httpClient = CreateMockHttpClient(response);
        return new ReqResClient(httpClient, options);
    }

    [Fact]
    public async Task GetUser_ReturnsUser_WhenUserExists()
    {
        var user = new User { Id = 1, Email = "test@example.com" };
        var responseObj = new SingleUserResponse { Data = user };
        var responseJson = JsonSerializer.Serialize(responseObj);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        };

        var client = CreateClient(response);
        var result = await client.GetUser(1);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task CreateUser_ReturnsCreatedUser()
    {
        var user = new User { Id = 2, Email = "new@example.com" };
        var responseJson = JsonSerializer.Serialize(user);
        var response = new HttpResponseMessage(HttpStatusCode.Created)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        };

        var client = CreateClient(response);
        var result = await client.CreateUser(user);

        Assert.NotNull(result);
        Assert.Equal("new@example.com", result!.Email);
    }

    [Fact]
    public async Task UpdateUser_ReturnsUpdatedUser()
    {
        var updatedUser = new User { Id = 2, Email = "updated@example.com" };
        var responseJson = JsonSerializer.Serialize(updatedUser);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        };

        var client = CreateClient(response);
        var result = await client.UpdateUser(2, updatedUser);

        Assert.NotNull(result);
        Assert.Equal("updated@example.com", result!.Email);
    }

    [Fact]
    public async Task DeleteUser_ReturnsTrue_WhenSuccess()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NoContent);
        var client = CreateClient(response);

        var result = await client.DeleteUser(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteUser_ReturnsFalse_WhenFailed()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);
        var client = CreateClient(response);

        var result = await client.DeleteUser(999);

        Assert.False(result);
    }
}
