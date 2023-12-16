Feature: UserController
The Controller that manages everything related to users account
In order accurately manage everything related to creating users account and overall operations related to the users account
I want to register user, delete user and perform any other operation related to managing the users account

Scenario: Register a new user with valid data
    Given A user is registered with valid data
    Then The response status code should be 201 Created
    
Scenario: Attempting to register user with an already existing email
    Given A user tries to register with an already existing email
    Then The response status code should be 400 Bad Request
    
Scenario: A user login in with valid data
    When A user login with valid data
    Then The response status code should be 200 Ok
    And Jwt parameter should not be null or empty
    
Scenario: A user tries to login using an invalid data
    When A user tries to login using invalid data
    Then The response status code should be 400 Bad Request
    
Scenario: Update username field if a user is authenticated
    Given A user is authenticated with an ID of "3"
    When The user updates its username
    Then The response status code should be 200 Ok
    
    
Scenario: A user that doesn't exist trying to access the UpdateUsername endpoint
    When The user attempts to update username
    Then The response status code should be 404 NotFound
    
    
Scenario: Update Email field if a user is authenticated
    Given A user is authenticated with an ID of "4"
    When The user updates its email address
    Then The response status code should be 200 Ok


Scenario: A user that doesn't exist trying to access the UpdateEmailAddress endpoint
    When The user attempts to update email address
    Then The response status code should be 404 NotFound
    
Scenario: Update password field if a user authenticated
    Given A user is authenticated with an ID of "2"
    When The user resets its password with valid data
    Then The response status code should be 200 Ok
    
Scenario: A user that doesn't exist trying to access the UpdatePassword endpoint
    When The user attempts to update password
    Then The response status code should be 404 NotFound