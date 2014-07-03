require 'calabash-android/abase'

class TaskyProScreen < Calabash::ABase
  include TaskyPro::AndroidHelpers

  def add_task_button
    trait
  end

  def tap_add_task_button
    touch(add_task_button)
    page(TaskDetailsScreen).await
  end

  def trait
    "ActionMenuItemView marked:'Add Task'"
  end

  def has_in_list(task_name)
    item = query(task_with_name(task_name))
    item.any?
  end

  def assert_task_name_should_be_in_list(task_name)
    raise "Could not find #{task_name} in the TableView." unless has_in_list(task_name)
  end

  def task_with_name(task_name)
    "LinearLayout TextView text:'#{task_name}'"
  end

  def select_task_with_name(task_name)
    touch(task_with_name(task_name))
    page(TaskDetailsScreen).await
  end

  def is_checked(task_name)
    # This is a verbose query - we first find the TextView holding our task name, then we look for the parent layout
    # which should be LinearLayout. The ImageView holding the check mark is a sibling to this LinearLayout. We can
    # find this out by using Android Device Monitor and then switching to the Hierarchy Viewer.

    item = query("TextView text:'#{task_name}' parent LinearLayout sibling ImageView marked:'checkMark'")
    raise "Task #{task_name} is not checked, and it should be." unless item[0]
  end

  def delete_task(task_name)
    if (has_in_list(task_name))
      touch(task_with_name(task_name))
      details_page = page(TaskDetailsScreen).await
      touch(details_page.home_button)
      page(TaskyProScreen).await
    end
  end

end