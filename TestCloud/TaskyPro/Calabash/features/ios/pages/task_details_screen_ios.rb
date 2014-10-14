require 'calabash-cucumber/ibase'

class TaskDetailsScreen < Calabash::IBase
  include TaskyPro::IOSHelpers

  def name_field
    "textField placeholder:'task name'"
  end

  def notes_field
    "textField placeholder:'other task info'"
  end

  def cancel_button
    "label marked:'Delete'"
  end

  def delete_button
    "label marked:'Delete'"
  end

  def save_button
    "label marked:'Save'"
  end

  def trait
    "label text:'Task Details'"
  end

  def done_checkbox
    "switch"
  end

  def change_name(new_name)
    clear_text(name_field)
    enter_text(name_field, new_name)
    touch(save_button)
  end

  def change_note(new_note)
    clear_text(name_field)
    enter_text(notes_field, new_note)
    touch(save_button)
  end


  def enter_new_task(new_task)
    clear_text(name_field)
    enter_text(name_field, new_task[:name])
    clear_text(notes_field)
    enter_text(notes_field, new_task[:notes])
  end

  def add_new_task(new_task)
    enter_new_task(new_task)
    touch(save_button)
    page(TaskyProScreen).await
  end

  def mark_as_done
    touch(done_checkbox)
    touch(save_button)
  end

  def is_done?
    query_results = query("switch", "isOn")
    return query_results[0] == "1"
  end

  def tap_cancel_button
    touch(cancel_button)
  end

  def tap_delete_button
    touch(delete_button)
  end
end