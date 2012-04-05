Tasky TODOs  (iOS)
==================

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

