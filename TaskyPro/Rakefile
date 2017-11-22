require 'fileutils'
require 'rubygems'

# Some utility functions
def set(key, value)
  if false == value
    value = 'FALSE'
  elsif true == value
    value = 'TRUE'
  end
  ENV[key.to_s.upcase] = value
end

def fetch(key)
  val = ENV[key.to_s.upcase]
  if 'FALSE' == val
    val = false
  elsif 'TRUE' == val
    val = true
  end
  val
end

if File.exists? ('rake_env')
  load 'rake_env'
else
  puts "Using environment variables."
end

# Hardcoding these paths here - adjust them as necessary. These shouldn't change much (if at all)
@mono = '/usr/bin/mono'
@mdtool = '/Applications/Xamarin\\ Studio.app/Contents/MacOS/mdtool build'
@xbuild = '/usr/bin/xbuild'
@test_cloud = "#{@mono} ./packages/Xamarin.UITest.0.5.0/tools/test-cloud.exe"
@xamarin_component = "#{@mono}  ./xamarin-component.exe"

@test_assembly_dir = "./Tasky.UITest/bin/Debug/"
@apk = './Tasky.Droid/bin/Release/com.xamarin.samples.taskydroid-Signed.apk'
@ipa = './Tasky.iOS/bin/iPhone/Debug/TaskyiOS.app'
@dsym = './Tasky.iOS/bin/iPhone/Debug/TaskyiOS.app.dSYM'

# These values will come from either the file rake_env or environment variables
@testcloud_api_key = fetch(:testcloud_api_key)
@android_device_id = fetch(:android_device_id)
@ios_device_id = fetch(:ios_device_id)

task :default => [:clean, :build]

task :require_environment do
  if @testcloud_api_key.nil? || 0 == @testcloud_api_key.length
    raise Exception.new("No Test Cloud API specified.")
  end

  if @android_device_id.nil? || 0 == @android_device_id.length
    raise Exception.new("No Android Device ID specified.")
  end

  if @ios_device_id.nil? || 0 == @ios_device_id.length
    raise Exception.new("No iOS Device ID specified.")
  end

end

desc "Cleans the project"
task :clean => [:clean_screenshots] do
  directories_to_delete = [
      "./bin",
      "./obj",
      "./test_servers",
      "./testresults.html",
      "./Tasky.iOS/bin",
      "./Tasky.iOS/obj",
      "./Tasky.Droid/bin",
      "./Tasky.Droid/obj",
      "./Tasky.Core/bin",
      "./Tasky.Core/obj"
  ]

  directories_to_delete.each { |x|
    rm_rf x
  }
end

desc "Compiles the Android and iOS projects."
task :build => [:build_android, :build_ios] do

end

desc "Delete the existing screen shots and calabash reports."
task :clean_screenshots do
  directories_to_delete = [
      "./screenshots/"
  ]

  directories_to_delete.each { |directory|
    rm_rf directory
  }
end

task :build_android => [:clean] do
  system("#{@xbuild} /verbosity:diagnostic /t:SignAndroidPackage /p:Configuration=Release ./Tasky.Droid/Tasky.Droid.csproj")
end

task :build_ios => [:clean] do
  puts "Build the IPA:"
  system("#{@mdtool} \"--configuration:Debug|iPhone\" TaskyXS_Mac.sln")
  system("#{@mdtool} \"--configuration:Release|iPhone\" TaskyXS.sln")

  puts "Build the iPhoneSimulator:"
  system("#{@mdtool} build \"--configuration:Debug|iPhoneSimulator\" TaskyXS.sln")
end

desc "Submits the APK and then the IPA to Test Cloud"
task :xtc => [:xtc_android, :xtc_ios] do

end

desc "Compile the APK and submits it to Test Cloud"
task :xtc_android => [:require_environment, :build_android] do
  raise "Missing the APK #{@apk}." unless File.exists?(@apk)
  tests_passed = system("#{@test_cloud} submit #{@apk} #{@testcloud_api_key} --devices #{@android_device_id} --series \"Android\"  --assembly-dir #{@test_assembly_dir} --app-name \"TaskyPro (UITest)\"")
  raise "Some tests failed in test cloud - check the build log. #{$?}" unless tests_passed
end

desc "Recompiles the IPA and submits it to Test Cloud."
task :xtc_ios => [:require_environment, :build_ios] do
  raise "Missing the IPA #{@ipa}." unless File.exists?(@ipa)
  tests_passed = system("#{@test_cloud} submit #{@ipa} #{@testcloud_api_key} --devices #{@ios_device_id} --series \"iOS\"  --assembly-dir #{@test_assembly_dir} --app-name \"TaskyPro (UITest)\" --dsym #{@dsym}")
  raise "Some tests failed in test cloud - check the build log. #{$?}" unless tests_passed
end

desc "Compiles a release APK and then will install it in the default emulator or attached device."
task :test_android => [:build_android] do
  system "calabash-android run #{@apk} -p android"
end


desc "Installs the components from the Component store"
task :install_components do
    system " #{@xamarin_component} restore TaskyXS_Mac.sln"
end