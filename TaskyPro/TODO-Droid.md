Tasky TODOs (Android)
=====================

add a DONE indicator
--------------------

* BL/Task.cs : add Done property

* Adapters/TaskListAdapter.cs : change the GetView method to use the built-in checkbox item
			var view = (convertView ?? 
					context.LayoutInflater.Inflate(
					Android.Resource.Layout.SimpleListItemChecked,
					parent, 
					false)) as CheckedTextView;
			view.SetText (item.Name, TextView.BufferType.Normal);
			view.Checked = item.Done;

* Resources/Layout/TaskListItem.axml : add
		<CheckBox 
			android:id="@+id/chkDone"
			android:layout_below="@+id/txtNotes" 
			android:layout_width="wrap_content"
			android:layout_height="wrap_content"
			android:text="Done" />

* Resources/Layout/TaskListItem.axml : change 'layout_below'
		<Button android:id="@+id/btnCancelDelete" android:text="Cancel" 
			android:layout_width="fill_parent" android:layout_height="wrap_content" 
			android:layout_below="@+id/chkDone"/>	

* Screens/TaskDetailsScreen.cs : in class
			CheckBox doneCheckbox;

* Screens/TaskDetailsScreen.cs : in OnCreate()
			doneCheckbox = FindViewById<CheckBox>(Resource.Id.chkDone);
			doneCheckbox.Checked = task.Done;

* Screens/TaskDetailsScreen.cs : in Save()
			task.Done = doneCheckbox.Checked;



do some stylin'
---------------

* Screens/HomeScreen.cs : change Label="TaskyPro"





