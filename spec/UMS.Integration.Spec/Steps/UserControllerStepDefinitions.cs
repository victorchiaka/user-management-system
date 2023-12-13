using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using UMS.Integration.Spec.Hooks;
using Xunit;

namespace UMS.Integration.Spec.Steps;

[Binding]
public sealed class UserControllerStepDefinition
{
    private readonly HttpClient _httpClient;
    private readonly CustomWebApplicationFactory<Program> _customWebApplicationFactory;
    private HttpResponseMessage _httpResponseMessage;

    public UserControllerStepDefinition(CustomWebApplicationFactory<Program> customWebApplicationFactory)
    {
        this._customWebApplicationFactory = customWebApplicationFactory;
        this._httpClient = _customWebApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Given(@"a user is registered with valid data")]
    public void GivenAUserIsRegisteredWithValidData()
    {
        var registerUserRequestDto = new
        {
            Username = "johnDoe",
            EmailAddress = "johndoe@mail.com",
            Password = "password"
        };
        
        _httpResponseMessage = _httpClient.PostAsJsonAsync("api/v1/User/Register",
            registerUserRequestDto).Result;
    }

    [Then(@"the response status code should be (.*) Created")]
    public void ThenTheResponseStatusCodeShouldBeCreated(int expectedStatusCode)
    {
        Assert.Equal((HttpStatusCode)expectedStatusCode, _httpResponseMessage.StatusCode);
    }
}