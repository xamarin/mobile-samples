// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using HighlightedSearchResults;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace HighlightedSearchResults
{
    [Register ("TableViewController")]
    partial class TableViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UISearchBar SearchBar { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (SearchBar != null) {
                SearchBar.Dispose ();
                SearchBar = null;
            }
        }
    }
}