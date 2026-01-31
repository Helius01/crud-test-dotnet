Feature: Create customer
  As an API client
  I want to create a customer
  So that I can retrieve it from the read model

  Scenario: Create and fetch customer
    Given I have a new customer id
    When I create a customer with email "test@example.com"
    Then the response status should be 201
    And fetching the customer should return email "test@example.com"
