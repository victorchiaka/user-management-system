using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using UMS.Api.DTOs;
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
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwiZXhwIjoxNzAyODM0MjEwLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUxNjYiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUxNjYifQ.-ABg4nt_y-st3aIT-yxJDA5OpZNQ_nLdaxF6tXcFIXpjqL029wEZ1_CU7aDgH93Fr_6iFzqPromhH65iikHKeQ");
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
}