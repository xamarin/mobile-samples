using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using System.Threading;

namespace MWC.WP7.ViewModels
{
    /// <summary>
    /// Generic View Model for Grouped List Views (especially those using LongListSelector).
    /// </summary>
    /// <typeparam name="TItem">The Model type</typeparam>
    /// <typeparam name="TItemViewModel">The View Model type for the Model type</typeparam>
    public abstract class GroupedListViewModel<TItem, TItemViewModel> : ViewModelBase
        where TItemViewModel : GroupedListItemViewModel<TItem>, new()
    {
        public ObservableCollection<GroupedListGroupViewModel<TItem, TItemViewModel>> Groups { get; set; }

        public bool IsUpdating { get; set; }
        public Visibility ListVisibility { get; set; }
        public Visibility NoDataVisibility { get; set; }

        public GroupedListViewModel ()
        {
            Groups = new ObservableCollection<GroupedListGroupViewModel<TItem, TItemViewModel>> ();
            IsUpdating = true;
            ListVisibility = Visibility.Collapsed;
            NoDataVisibility = Visibility.Visible;
        }

        protected abstract IEnumerable<IGrouping<string, TItem>> GetGroupedItems ();

        protected virtual string GetGroupTitle (string groupKey)
        {
            return groupKey;
        }

        protected abstract object GetItemKey (TItem item);

        public void BeginUpdate (Dispatcher dispatcher)
        {
            //
            // Get the data off-thread, then do the rest of our work on-thread
            //
            ThreadPool.QueueUserWorkItem (delegate {
                //
                // Get the data
                //
                var speakerGroups = GetGroupedItems ();

                //
                // Update the UI
                //
                dispatcher.BeginInvoke (delegate {
                    var oldGroups = Groups.ToList ();
                    var newGroups = new List<GroupedListGroupViewModel<TItem, TItemViewModel>> ();

                    foreach (var sg in speakerGroups) {

                        var group = oldGroups.FirstOrDefault (g => g.Key == sg.Key);

                        if (group == null) {
                            group = new GroupedListGroupViewModel<TItem, TItemViewModel> {
                                Key = sg.Key,
                                Title = GetGroupTitle (sg.Key),
                            };
                        }

                        group.Update (sg, GetItemKey);
                        newGroups.Add (group);
                    }

                    Groups = new ObservableCollection<GroupedListGroupViewModel<TItem, TItemViewModel>> (newGroups.OrderBy (x => x.Key));
                    OnPropertyChanged ("Groups");

                    IsUpdating = false;
                    NoDataVisibility = Visibility.Collapsed;
                    ListVisibility = Visibility.Visible;
                    OnPropertyChanged ("IsUpdating");
                    OnPropertyChanged ("NoDataVisibility");
                    OnPropertyChanged ("ListVisibility");

                    OnUpdated ();
                });
            });
        }

        public event EventHandler Updated;

        protected virtual void OnUpdated ()
        {
            var ev = Updated;
            if (ev != null) {
                ev (this, EventArgs.Empty);
            }
        }
    }

    public class GroupedListGroupViewModel<TItem, TItemViewModel> : ViewModelBase, IEnumerable<TItemViewModel>
        where TItemViewModel : GroupedListItemViewModel<TItem>, new()
    {
        public string Key { get; set; }

        public string Title { get; set; }

        public ObservableCollection<TItemViewModel> Items { get; set; }

        public GroupedListGroupViewModel ()
        {
            Key = "";
            Title = "";
            Items = new ObservableCollection<TItemViewModel> ();
        }

        public void Update (IEnumerable<TItem> items, Func<TItem, object> getItemKey)
        {
            //
            // Find or create ViewModels for each item
            //
            var oldViewModels = Items.ToDictionary (i => i.Key);
            var newItems = new List<TItemViewModel> ();
            foreach (var i in items) {
                var vm = default (TItemViewModel);
                var ikey = getItemKey (i);
                if (!oldViewModels.TryGetValue (ikey, out vm)) {
                    vm = new TItemViewModel () {
                        Key = ikey,
                    };
                }
                vm.Update (i);
                newItems.Add (vm);
            }

            //
            // Update the list
            //
            Items = new ObservableCollection<TItemViewModel> (newItems.OrderBy (x => x.SortKey));
            OnPropertyChanged ("Items");
        }

        public override bool Equals (object obj)
        {
            var o = obj as GroupedListGroupViewModel<TItem, TItemViewModel>;
            return (o != null) && Key.Equals (o.Key);
        }

        public override int GetHashCode ()
        {
            return Key.GetHashCode ();
        }

        public IEnumerator<TItemViewModel> GetEnumerator ()
        {
            return Items.GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return Items.GetEnumerator ();
        }
    }

    public abstract class GroupedListItemViewModel<TItem> : ViewModelBase
    {
        public object Key { get; set; }
        public string SortKey { get; protected set; }

        public abstract void Update (TItem item);        
    }
}
