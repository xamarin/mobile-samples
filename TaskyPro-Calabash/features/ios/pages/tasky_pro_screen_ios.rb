require 'calabash-cucumber/ibase'

class TaskyProScreen < Calabash::IBase
  include TaskyPro::IOSHelpers

  def add_task_button
    trait
  end

  def tap_add_task_button
    touch(add_task_button)
    page(TaskDetailsScreen).await
  end

  def trait
    "button marked:'Add'"
  end

  def task_label(task_name)
    "label marked:'#{task_name}'"
  end

  def has_in_list(task_name)
    item = query(task_label(task_name))
    item.any?
  end

  def assert_should_have_in_list(task_name)
    raise "Could not find #{task_name} in the TableView." unless has_in_list(task_name)
  end

  def select_task(task_name)
    touch(task_label(task_name))
    page(TaskDetailsScreen).await
  end

  def is_checked(task_name)
    # The iOS verison of Tasky doesn't have check marks in the TableView
    true
  end

  def delete_task(task_name)
    if has_in_list(task_name)
      touch(task_label(task_name))
      details_page = page(TaskDetailsScreen).await
      touch(details_page.delete_button)
      page(TaskyProScreen).await
    end
  end
end
