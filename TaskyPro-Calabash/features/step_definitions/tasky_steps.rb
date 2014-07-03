Given(/^I am on the Task Details screen$/) do
  tasks_page = page(TaskyProScreen).await
  @current_page = tasks_page.tap_add_task_button
end

Given(/^I am on the Task Details screen for the "(.*?)" task$/) do |task_key|
  task = TASKS[task_key.downcase.to_sym]
  task_list = page(TaskyProScreen).await

  if (task_list.has_in_list(task[:name]))
    @current_page = task_list
  else
    @current_page = task_list.tap_add_task_button
    @current_page = @current_page.add_new_task(task)
  end

  @current_page = @current_page.select_task(task[:name])
end

Given(/^I look at the details for the task "(.*?)"$/) do |task_name|
  @current_page = @current_page.select_task(task_name)
end

When(/^I add the "(.*?)" task$/) do |task_name|
  new_task = TASKS[task_name.downcase.to_sym]
  @current_page = @current_page.add_new_task(new_task)
end

Then(/^I should be on the TaskyPro screen$/) do
  assert_screen(TaskyProScreen)
end

Then(/^I should see the "(.*?)" task in the list$/) do |task_name|
  @current_page.assert_should_have_in_list(task_name)
end

Then(/^I should see "(.*?)" for the note$/) do |task_note|
  element_exists("edittext marked:'#{task_note}'")
end

Then(/^the task "(.*?)" should be checked$/) do |task_name|
  @current_page.is_checked(task_name)
end

Then(/^Done should be checked$/) do
  fail(msg="This task should be marked as done.") unless @current_page.is_done?
end

When(/^I start to add the "(.*?)" task$/) do |task_key|
  task = TASKS[task_key.downcase.to_sym]
  @current_page.enter_new_task(task)
end

When(/^I press the Cancel button$/) do
  @current_page.tap_cancel_button
  @current_page = page(TaskyProScreen).await
end

Then(/^I should not see the "(.*?)" task in the list$/) do |arg1|
  element_exists("edittext marked:'#{arg1}'")
end

