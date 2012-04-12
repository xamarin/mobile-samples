Tasky TODOs  (iOS)
==================

add a DONE indicator
--------------------

* BL/Task.cs : add Done property

* Screens/AL/TaskDialog.cs : add bool Done property *and* set in the ctor

* Screens/iPhone/Home/controller_iPhone.cs : currentTask.Done = taskDialog.Done; in SaveTask()

* Screens/iPhone/Home/controller_iPhone.cs : change StringElement to CheckboxElement


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


