Feature: Credit card validation.
  Credit card numbers must be exactly 16 characters.

  Scenario: Credit card number is too short
    Given I use the native keyboard to enter "123456" into text field number 1
    And I touch the "Validate" button
    Then I see the text "Credit card number is too short."

  Scenario: Credit card number is too long
      Given I try to validate a credit card number that is 17 characters long
      Then I should see the error message "Credit card number is too long."
