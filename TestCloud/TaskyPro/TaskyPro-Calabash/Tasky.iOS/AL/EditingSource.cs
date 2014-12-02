using MonoTouch.Dialog;
using Foundation;
using UIKit;

using Tasky.Screens.iPhone;

namespace Tasky.AL
{
    // This is our subclass of the fixed-size Source that allows editing
    public class EditingSource : DialogViewController.Source
    {
        public EditingSource(DialogViewController dvc) : base(dvc)
        {
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            // Trivial implementation: we let all rows be editable, regardless of section or row
            return true;
        }

        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            // trivial implementation: show a delete button always
            return UITableViewCellEditingStyle.Delete;
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            //
            // In this method, we need to actually carry out the request
            //
            Section section = Container.Root[indexPath.Section];
            StringElement element = section[indexPath.Row] as StringElement;
            section.Remove(element);

            HomeScreen dvc = Container as HomeScreen;
            dvc.DeleteTaskRow(indexPath.Row);
        }
    }
}
