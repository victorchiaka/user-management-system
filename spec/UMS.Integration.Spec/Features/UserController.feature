Feature: UserController
The Controller that manages everything related to users account
In order accurately manage everything related to creating users account and overall operations related to the users account
I want to register user, delete user and perform any other operation related to managing the users account

Scenario: Register a new user with valid data
    Given a user is registered with valid data
    Then the response status code should be 201 Created