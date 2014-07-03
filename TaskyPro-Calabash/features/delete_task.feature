Feature: Delete an existing task

  Scenario: Delete an existing task
    Given I am on the Task Details screen for the "LearnJava" task
    When I press the Delete button

    Then I should be on the TaskyPro screen
    And I should not see the "Learn Java programming" task in the list

