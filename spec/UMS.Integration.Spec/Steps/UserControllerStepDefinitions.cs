using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TechTalk.SpecFlow.Assist;
using UMS.Api.DTOs;
using UMS.Integration.Spec.Hooks;

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
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
    }

    [Given(@"A user is authenticated with an ID of ""(.*)""")]
    public void GivenAUserIsAuthenticatedWithAnIdOf(string userId)
    {
        _httpClient.DefaultRequestHeaders.Add(TestAuthenticationHandler.UserId, userId);
    }

    [Given(@"A user is registered with valid data")]
    public void GivenAUserIsRegisteredWithValidData()
    {

        RegisterUserRequestDto registerUserRequestDto = new()
        {
            Username = "johnDoe",
            EmailAddress = "johndoe@mail.com",
            Password = "password"
        };
        
        _httpResponseMessage = _httpClient.PostAsJsonAsync("api/v1/User/Register",
            registerUserRequestDto).Result;
    }

    [Then(@"The response status code should be (.*) Created")]
    public void ThenTheResponseStatusCodeShouldBeCreated(int expectedStatusCode)
    {
        _httpResponseMessage.StatusCode.Should().Be((HttpStatusCode)expectedStatusCode);
    }
    
    [Given(@"A user tries to register with an already existing email")]
    public void GivenAUserTriesToRegisterWithAnAlreadyExistingEmail()
    {
        RegisterUserRequestDto registerUserRequestDto = new()
        {
            Username = "johnDoe",
            EmailAddress = "johndoe@mail.com",
            Password = "password"
        };

        _httpResponseMessage = _httpClient.PostAsJsonAsync("api/v1/User/Register",
            registerUserRequestDto).Result;
    }
    
    [When(@"A user login with valid data")]
    public void WhenAUserLoginWithValidData()
    {
        LoginUserRequestDto loginUserRequestDto = new()
        {
            EmailAddress = "johndoe@mail.com",
            Password = "password"
        };

        _httpResponseMessage = _httpClient.PostAsJsonAsync("api/v1/User/Login", loginUserRequestDto).Result;
    }

    [When(@"A user tries to login using invalid data")]
    public void WhenAUserTriesToLoginUsingInvalidData()
    {
        LoginUserRequestDto loginUserRequestDto = new()
        {
            EmailAddress = "johndoe@mail.com",
            Password = "wrongPassword"
        };

        _httpResponseMessage = _httpClient.PostAsJsonAsync("api/v1/User/Login", loginUserRequestDto).Result;
    }
    
    [Then(@"The response status code should be (.*) Bad Request")]
    public void ThenTheResponseStatusCodeShouldBeBadRequest(int expectedStatusCode)
    {
        _httpResponseMessage.StatusCode.Should().Be((HttpStatusCode)expectedStatusCode);
    }

    [When(@"The user resets its password with valid data")]
    public void WhenTheUserResetsItsPasswordWithValidData()
    {
        UpdatePasswordRequestDto updatePasswordRequestDto = new()
        {
            EmailAddress = "johndoe@mail.com",
            NewPassword = "password"
        };
            
        _httpResponseMessage = _httpClient.PutAsJsonAsync("api/v1/User/UpdatePassword",
            updatePasswordRequestDto).Result;
    }

    [Then(@"The response status code should be (.*) Ok")]
    public void ThenTheResponseStatusCodeShouldBeOk(int expectedStatusCode)
    {
        _httpResponseMessage.StatusCode.Should().Be((HttpStatusCode)expectedStatusCode);
    }
    
    [When(@"The user attempts to update password")]
    public void WhenTheUserAttemptsToUpdatePassword()
    {
        UpdatePasswordRequestDto updatePasswordRequestDto = new()
        {
            EmailAddress = "janedoe@mail.com",
            NewPassword = "password"
        };

        _httpResponseMessage =
            _httpClient.PutAsJsonAsync("api/v1/User/UpdatePassword", updatePasswordRequestDto).Result;
    }

    [Then(@"The response status code should be (.*) NotFound")]
    public void ThenTheResponseStatusCodeShouldBeNotFound(int expectedStatusCode)
    {
        _httpResponseMessage.StatusCode.Should().Be((HttpStatusCode)expectedStatusCode);
    }

    [Then(@"Jwt parameter should not be null or empty")]
    public void ThenJwtParameterShouldNotBeNullOrEmpty()
    {
        if (_httpResponseMessage.Headers.TryGetValues("Authorization", out IEnumerable<string>? tokenValues))
        {
            string? jwtToken = tokenValues.FirstOrDefault();
            jwtToken.Should().NotBeNullOrEmpty();
        }
    }

    [When(@"The user updates its username")]
    public void WhenTheUserUpdatesItsPassword()
    {
        UpdateUsernameRequestDto updateUsernameRequestDto = new()
        {
            EmailAddress = "johndoe@mail.com",
            Password = "password",
            NewUsername = "johnWik"
        };

        _httpResponseMessage =
            _httpClient.PutAsJsonAsync("api/v1/User/UpdateUsername", updateUsernameRequestDto).Result;
    }

    [When(@"The user attempts to update username")]
    public void WhenTheUserAttemptsToUpdateUsername()
    {
        UpdateUsernameRequestDto updateUsernameRequestDto = new()
        {
            EmailAddress = "joe@mail.com",
            Password = "password",
            NewUsername = "bigJow"
        };

        _httpResponseMessage =
            _httpClient.PutAsJsonAsync("api/v1/User/UpdateUsername", updateUsernameRequestDto).Result;
    }
    
    [When(@"The user updates its email address")]
    public void WhenTheUserUpdatesItsEmailAddress()
    {
        UpdateEmailAddressRequestDto updateEmailAddressRequestDto = new()
        {
            EmailAddress = "johndoe@mail.com",
            Password = "password",
            NewEmailAddress = "johndoe@mail.com"
        };

        _httpResponseMessage =
            _httpClient.PutAsJsonAsync("api/v1/User/UpdateEmailAddress", updateEmailAddressRequestDto).Result;
    }

    [When(@"The user attempts to update email address")]
    public void WhenTheUserAttemptsToUpdateEmailAddress()
    {
        UpdateEmailAddressRequestDto updateEmailAddressRequestDto = new()
        {
            EmailAddress = "joe@mail.com",
            Password = "password",
            NewEmailAddress = "bigjoe@mail.com"
        };

        _httpResponseMessage =
            _httpClient.PutAsJsonAsync("api/v1/User/UpdateEmailAddress", updateEmailAddressRequestDto).Result;
    }

    [When(@"The user accesses the WhoAmI endpoint")]
    public void WhenTheUserAccessesTheWhoAmIEndpoint()
    {
        WhoAmIRequestDto whoAmIRequestDto = new()
        {
            EmailAddress = "victor@mail.com",
            Password = "password"
        };
        
        // _httpResponseMessage = _httpClient.GetAsync("api/v1/User/WhoAmI").Result;

        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "api/v1/User/WhoAmI");
        httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(whoAmIRequestDto), Encoding.UTF8,
            "application/json");
        
        _httpResponseMessage = _httpClient.SendAsync(httpRequestMessage).Result;

    }
    
    [Then(@"The WhoAmI response status code should be (.*) OK")]
    public void ThenTheWhoAmIResponseStatusCodeShouldBeOk(int expectedStatusCode)
    {
        _httpResponseMessage.StatusCode.Should().Be((HttpStatusCode)expectedStatusCode);
    }
    
    [Then(@"The response should contain user data:")]
    public void AndThenTheResponseShouldContainUserData(Table table)
    {
        WhoAmIDto? whoAmIDto = _httpResponseMessage.Content.ReadFromJsonAsync<WhoAmIDto>().Result;
        whoAmIDto.Should().BeEquivalentTo(table.CreateInstance<WhoAmIDto>());
    }
}