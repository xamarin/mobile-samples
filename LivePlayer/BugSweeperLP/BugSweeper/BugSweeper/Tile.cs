#define FIX_WINDOWS_DOUBLE_TAPS         // Double-taps don't work well on Windows Runtime as of 2.3.0

using System;
using Xamarin.Forms;

namespace BugSweeper
{
    public enum TileStatus
    {
        Hidden,
        Flagged,
        Exposed
    }

    public class Tile // : Frame
    {
        const string UrlPrefix = "http://xamarin.github.io/xamarin-forms-book-samples/BugSweeper/";
        static ImageSource flagImageSource = ImageSource.FromUri(new Uri(UrlPrefix + "Xamarin120.png"));
        static ImageSource bugImageSource = ImageSource.FromUri(new Uri(UrlPrefix + "RedBug.png"));

        Label hiddenLabel = new Label
        {
            Text = " ",
            TextColor = Color.Yellow,
            BackgroundColor = Color.Yellow
        };

        Label exposedLabel = new Label
        {
            Text = " ",
            TextColor = Color.Yellow,
            BackgroundColor = Color.Blue,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
        };
        
        Image flagImage = new Image
        {
            Source = flagImageSource 
        };

        Image bugImage = new Image
        {
            Source = bugImageSource 
        };

        TileStatus tileStatus = TileStatus.Hidden;
        bool doNotFireEvent;

        public event EventHandler TileStatusChanged;

        public Tile(int row, int col)
        {
            Row = row;
            Col = col;

            TileView = new Frame
            {
                Content = hiddenLabel,
                BackgroundColor = Color.Black,
                OutlineColor = Color.Black,
                Padding = new Thickness(1)
            };

            TapGestureRecognizer singleTap = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1
            };
            singleTap.Tapped += OnSingleTap;
            TileView.GestureRecognizers.Add(singleTap);

        #if FIX_WINDOWS_DOUBLE_TAPS

            if (Device.RuntimePlatform != Device.UWP)  
            {

        #endif

                TapGestureRecognizer doubleTap = new TapGestureRecognizer
                {
                    NumberOfTapsRequired = 2
                };
                doubleTap.Tapped += OnDoubleTap;
                TileView.GestureRecognizers.Add(doubleTap);

        #if FIX_WINDOWS_DOUBLE_TAPS

            }

        #endif

        }

        public ContentView TileView { private set; get; }

        public int Row { private set; get; }

        public int Col { private set; get; }

        public bool IsBug { set; get; }

        public int SurroundingBugCount { set; get; }

        public TileStatus Status
        {
            set
            {
                if (tileStatus != value)
                {
                    tileStatus = value;

                    switch (tileStatus)
                    {
                        case TileStatus.Hidden:
                            TileView.Content = hiddenLabel;
                            break;

                        case TileStatus.Flagged:
                            TileView.Content = flagImage;
                            break;

                        case TileStatus.Exposed:
                            if (IsBug)
                            {
                                TileView.Content = bugImage;
                            }
                            else
                            {
                                exposedLabel.Text = (SurroundingBugCount > 0) ? SurroundingBugCount.ToString() : " ";
                                TileView.Content = exposedLabel;
                            }
                            break;
                    }

                    if (!doNotFireEvent)
                    {
                        TileStatusChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            get
            {
                return tileStatus;
            }
        }

        // Does not fire TileStatusChanged events.
        public void Initialize()
        {
            doNotFireEvent = true;
            Status = TileStatus.Hidden;
            IsBug = false;
            SurroundingBugCount = 0;
            doNotFireEvent = false;
        } 

#if FIX_WINDOWS_DOUBLE_TAPS

        bool lastTapSingle;
        DateTime lastTapTime;

#endif

        void OnSingleTap(object sender, object args)
        {

#if FIX_WINDOWS_DOUBLE_TAPS

            if (Device.RuntimePlatform == Device.UWP)
            {
                if (lastTapSingle && DateTime.Now - lastTapTime < TimeSpan.FromMilliseconds (500))
                {
                    OnDoubleTap (sender, args);
                    lastTapSingle = false;
                }
                else
                {
                    lastTapTime = DateTime.Now;
                    lastTapSingle = true;
                }
        	}

#endif

            switch (Status)
            {
                case TileStatus.Hidden:
                    Status = TileStatus.Flagged;
                    break;

                case TileStatus.Flagged:
                    Status = TileStatus.Hidden;
                    break;

                case TileStatus.Exposed:
                    // Do nothing
                    break;
            }
        }

        void OnDoubleTap (object sender, object args)
        {
            Status = TileStatus.Exposed;
        }
    }
}
