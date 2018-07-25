using System;
using Foundation;
using UIKit;

namespace HighlightedSearchResults
{
    public static class UILabelExtensions
    {
        public static void HighlightSubstring(this UILabel label, string substring, UIColor highlightColor)
        {
            if (!label.RespondsToSelector(new ObjCRuntime.Selector("setAttributedText:")))
                return;
            if (string.IsNullOrEmpty(substring))
                return;

            var indexes = label.Text.AllIndexesOf(substring);
            var attributedText = new NSMutableAttributedString(label.Text);

            foreach (var index in indexes)
            {
                var range = new NSRange(index, substring.Length);
                attributedText.SetAttributes(new UIStringAttributes { BackgroundColor = highlightColor }, range);
            }
            label.AttributedText = attributedText;
        }
    }
}

