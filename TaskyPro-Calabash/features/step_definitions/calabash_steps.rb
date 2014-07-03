if ENV["PLATFORM"] == "android"
  require 'calabash-android/calabash_steps'
elsif ENV["PLATFORM"] == "ios"
  require 'calabash-cucumber/calabash_steps'
end

def assert_screen(screen)
  unless @screen.is_a?(screen)
    screenshot_embed label: "Expected #{screen} screen but was #{@screen.class}"
  end
end