Tasky TODOs
===========

add a DONE indicator
--------------------

* BL/Task.cs : add Done property

* Screens/iPhone/Home/controller_iPhone.cs : change StringElement to CheckboxElement

* Screens/iPhone/TaskDetails/Screen.xib : add swiDone UISwitch & outlet

* Screens/iPhone/TaskDetails/Screen.cs : swiDone.On = task.Done; in ViewWillAppear()

* Screens/iPhone/TaskDetails/Screen.cs : task.Done = swiDone.On; in Save()


do some stylin'
---------------

* Screens/iPhone/Home/controller_iPhone.cs : RootElement("TaskyPro") in PopulateTable()

* AppDelegate.cs : in FinishedLaunching()
			// Styling
			UINavigationBar.Appearance.TintColor = ColorNavBarTint;	
			UITextAttributes ta = new UITextAttributes();
			ta.Font = UIFont.FromName ("AmericanTypewriter-Bold", 0f);
			UINavigationBar.Appearance.SetTitleTextAttributes(ta);
			ta.Font = UIFont.FromName ("AmericanTypewriter", 0f);
			UIBarButtonItem.Appearance.SetTitleTextAttributes(ta, UIControlState.Normal);

* Screens/iPhone/TaskDetails/Screen.cs : in ViewWillAppear()
			// Styling
			if (btnCancelDelete.Title (UIControlState.Normal) == "Delete")
				btnCancelDelete.SetTitleColor(UIColor.Red, UIControlState.Normal);
			else
				btnCancelDelete.SetTitleColor(UIColor.DarkGray, UIControlState.Normal);
			btnSave.SetTitleColor(AppDelegate.ColorNavBarTint, UIControlState.Normal)



"Task Ideas"
------------

Appearance theming (basic)

- Change app icon and default screen (ship with 'X', supply new artwork)

- Font and color in titlebar

- Font and color of buttons

- Change TableCellView style (default,

Data (intermediate)

- New fields: 'done' (bool), 'due by' (date time), 'category' (select from a list)

- New table: Category [Personal|Work|Important]

- New queries: show only 'done', sort 'due by', filter by 'category'

UI (basic-intermediate)

- Add 'about' screen (from navbar leftbuttonitem)

- Convert Notes input to multiline

- New inputs for new fields: 'done' (switch or toggle image), 'due by' (datepicker actionsheet), 'category' (push another view controller to select from list)

- Custom TableCellView to display new fields

- Add sections to table, to separate new from done.

- Add buttons to sort: 'due by' or alpha

Device capabilities (advanced)

- iOS: tweet 'just completed this task'

- Camera: add photo to task (store in filesystem or db?)

- Email: send this task

- Contact: add a contact to this task (also, storage)



