
Given(/^I try to validate a credit card number that is (\d+) characters long$/) do |number_of_digits|
  touch("textField marked:'CreditCardTextField'")
  wait_for_keyboard
  keyboard_enter_text("9" * number_of_digits.to_i)
  touch("button marked:'ValidateButton'")
end

Then(/^I should see the error message "(.*?)"$/) do |error_message|
  wait_for_elements_exist(["label id:'ErrorMessagesTextField'"], :timeout=>5)

  label_view = query("label id:'ErrorMessagesTextField' {text CONTAINS '#{error_message}'}")
  raise "The error message '#{error_message}' is not visible in the View." unless label_view.any?
end

Then(/^I should see the Valid Credit Card screen with success message$/) do
  wait_for_elements_exist(["UINavigationBar marked:'Valid Credit Card'"], :timeout => 30)

  label_view = query("label marked:'CreditCardIsValidLabel' {text CONTAINS 'The credit card number is valid!'}")
  raise "The success messages was not displayed" unless label_view.any?
end
