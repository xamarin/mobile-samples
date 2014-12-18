Feature: Credit card validation.
  Credit card numbers must be exactly 16 characters.

  Scenario: Credit card number is too short (using redefined steps)
    # this is an example of using predefined steps - not really recommended.
    Given I use the native keyboard to enter "123456" into text field number 1
    And I touch the "ValidateButton" button
    Then I see the text "Credit card number is too short."

  Scenario: Credit card number is too long
      Given I try to validate a credit card number that is 17 characters long
      Then I should see the error message "Credit card number is too long."

  Scenario: Credit card number is an empty string
      Given I touch the "ValidateButton" button
      Then I should see the error message "This is not a credit card number."

  Scenario: Credit card number is exactly 16 characters
    Given I try to validate a credit card number that is 16 characters long
    Then I should see the Valid Credit Card screen with success message
