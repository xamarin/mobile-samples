using Xamarin.UITest;

namespace Tasky.UITest
{
    public class TaskDetailsScreen: Page
    {
        public TaskDetailsScreen()
        {
            Marked = ScreenQuery.ForId("action_bar_title");
            DeleteButton = ScreenQuery.ForId("menu_delete_task");
            Home = ScreenQuery.ForId("home");
            Up = ScreenQuery.ForId("up");
            NameTextView = ScreenQuery.ForId("txtName");
            NotesTextView = ScreenQuery.ForId("txtNotes");
            DoneCheckBox = ScreenQuery.ForId("chkDone");
            SaveButton = ScreenQuery.ForId("menu_save_task");
        }
        public ScreenQuery DeleteButton { get; private set; }
        public ScreenQuery SaveButton { get; private set;}
        public ScreenQuery Up { get; private set;}
        public ScreenQuery Home { get; private set; }
        public ScreenQuery NameTextView { get; private set; }
        public ScreenQuery NotesTextView { get; private set; }
        public ScreenQuery DoneCheckBox { get; private set;} 

        public void EnterTask(IApp app, string name, string notes, bool done)
        {
            app.EnterText(NameTextView, name);
            app.EnterText(NotesTextView, notes);
        }
    }
    
}
