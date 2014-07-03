Feature: Edit an existing task

  Background:
    Given I am on the Task Details screen for the "LearnObjectiveC" task

  Scenario: Change the task name
    The user should be able to change the name of task.

    When I change the name to "Learn iOS programming"

    Then I should be on the TaskyPro screen
    And I should see the "Learn iOS programming" task in the list

  Scenario: Mark the task as done
    The user should be able to mark all of their completed tasks as done.

    When I mark the task as Done and save it
    Then the task "Learn Objective-C" should be checked

    And I look at the details for the task "Learn Objective-C"
    Then Done should be checked
