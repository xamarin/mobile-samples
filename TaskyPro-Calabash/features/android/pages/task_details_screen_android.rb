require 'calabash-android/abase'

class TaskDetailsScreen < Calabash::ABase
  include TaskyPro::AndroidHelpers

  def name_field
    "edittext id:'txtName'"
  end

  def notes_field
    "edittext id:'txtNotes'"
  end

  def home_button
    "ImageView marked:'home'"
  end

  def delete_button
    "ActionMenuItemView marked:'menu_delete_task'"
  end

  def save_button
    trait
  end

  def trait
    "ActionMenuItemView marked:'menu_save_task'"
  end

  def done_checkbox
    "checkbox marked:'chkDone'"
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
    enter_text(name_field, new_task[:name])
    enter_text(notes_field, new_task[:notes])
  end

  def add_new_task(new_task)
    enter_new_task(new_task)
    touch(save_button)
    page(TaskyProScreen).await()
  end

  def mark_as_done
    touch(done_checkbox)
    touch(save_button)
  end

  def is_done?
    query_results = query("#{done_checkbox} checked:true")
    return !query_results.empty?
  end

  def tap_cancel_button
    touch(home_button)
  end

  def tap_delete_button
    touch(delete_button)
  end

end