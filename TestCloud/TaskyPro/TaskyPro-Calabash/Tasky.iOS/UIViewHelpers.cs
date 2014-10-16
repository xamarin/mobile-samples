using System;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Tasky.iOS
{
    public static class UIViewHelpers
    {
        static readonly IntPtr selAccessibilityIdentifier_Handle = Selector.GetHandle("accessibilityIdentifier");
        static readonly IntPtr setAccessibilityIdentifier_Handle = Selector.GetHandle("setAccessibilityIdentifier:");

        public static UIView SetAccessibilityId(this UIView view, string id)
        {
            var intPtr = NSString.CreateNative(id);
            Messaging.void_objc_msgSend_IntPtr(view.Handle, setAccessibilityIdentifier_Handle, intPtr);
            NSString.ReleaseNative(intPtr);
            return view;
        }

        public static string GetAccessibilityId(this UIView view)
        {
            return NSString.FromHandle(Messaging.IntPtr_objc_msgSend(view.Handle, selAccessibilityIdentifier_Handle));
        }
    }}

