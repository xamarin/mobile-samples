require 'calabash-android/operations'

INSTALLATION_STATE = {
    :installed => false,
}

Before do |scenario|
  unless (ENV['SKIP_REINSTALL'] == '1')
    if !INSTALLATION_STATE[:installed]
      uninstall_apps
      install_app(ENV["TEST_APP_PATH"])
      install_app(ENV["APP_PATH"])
      INSTALLATION_STATE[:installed] = true
    end
  end
  start_test_server_in_background
end

After do |scenario|
  if scenario.failed?
    screenshot_embed
  end
  shutdown_test_server
end
