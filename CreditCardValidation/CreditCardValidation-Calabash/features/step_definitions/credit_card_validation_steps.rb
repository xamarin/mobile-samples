Given(/^I try to validate a credit card number that is (\d+) characters long$/) do |number_of_digits|
  touch("textField marked:'CreditCardNumberField'")
  wait_for_keyboard
  keyboard_enter_text("9" * number_of_digits.to_i)
  touch("button marked:'Validate'")
end

Then(/^I should see the error message "(.*?)"$/) do |error_message|
  text_view = query("textView marked:'ErrorMessagesField' {text CONTAINS '#{error_message}'}")
  raise "The error message '#{error_message}' is not visible in the View." unless text_view.any?
end
