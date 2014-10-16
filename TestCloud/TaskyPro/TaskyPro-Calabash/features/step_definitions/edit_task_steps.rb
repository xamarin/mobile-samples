When(/^I change the name to "(.*?)"$/) do |new_name|
  @current_page.change_name(new_name)
  @current_page = page(TaskyProScreen).await
end

When(/^I change the note to "(.*?)"$/) do |new_note|
  @current_page.change_note(new_note)
  @current_page = page(TaskyProScreen).await
end

When(/^I mark the task as Done and save it$/) do
  @current_page.mark_as_done
  @current_page = page(TaskyProScreen).await
end
