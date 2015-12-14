using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;
using System.Linq;

namespace HighlightedSearchResults
{
    public partial class TableViewController : UITableViewController, IUISearchBarDelegate
    {
        private readonly List<string> names = new List<string>();
        private List<string> filteredNames;
        private bool searching = false;

        public TableViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SearchBar.Delegate = this;

            GenerateNames();
        }

        #region UITableViewController Methods
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = TableView.DequeueReusableCell("TableViewCell");

            if (searching)
            {
                cell.TextLabel.Text = filteredNames.ElementAt(indexPath.Row);
                cell.TextLabel.HighlightSubstring(SearchBar.Text, UIColor.LightGray);
            }
            else
            {
                cell.TextLabel.AttributedText = new NSAttributedString("");
                cell.TextLabel.Text = names.ElementAt(indexPath.Row);
            }

            return cell;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return searching ? filteredNames.Count : names.Count;
        }
        #endregion

        #region UISearchBar Methods
        [Export("searchBar:textDidChange:")]
        public void SearchBarTextDidChange(UISearchBar searchBar, string searchPattern)
        {
            searching = !string.IsNullOrEmpty(searchPattern);
            filteredNames = names.Select(x => x).Where(x => x.Contains(searchPattern, StringComparison.OrdinalIgnoreCase)).ToList();
            TableView.ReloadData();
        }

        [Export("searchBarShouldBeginEditing:")]
        public bool ShouldBeginEditing(UISearchBar searchBar)
        {
            searching = true;
            return true;
        }

        [Export("searchBarShouldEndEditing:")]
        public bool ShouldEndEditing(UISearchBar searchBar)
        {
            searching = false;
            return true;
        }
        #endregion


        private void GenerateNames()
        {
            names.AddRange(new[] { "David", "James", "Jan", "John", "Jennifer", "Michael", "Martin", "Steven", "Bruce", "Robert", "Leonardo" });
        }
    }
}