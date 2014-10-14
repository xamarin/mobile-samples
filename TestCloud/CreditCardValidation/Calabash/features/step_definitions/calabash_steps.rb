require 'calabash-cucumber/calabash_steps'

module CreditCardValidation
  module IOSHelpers
    def enter_text(field, text, options={:wait_for_keyboard => true})
      touch(field)
      await_keyboard unless options[:wait_for_keyboard] == false
      keyboard_enter_text(text)
    end
  end
end

World(CreditCardValidation::IOSHelpers)