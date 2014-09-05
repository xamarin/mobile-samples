using System;

using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;

namespace CreditCardValidation.iOS
{
    public static class UIViewHelpers
    {
        static readonly IntPtr selAccessibilityIdentifier_Handle = Selector.GetHandle("accessibilityIdentifier");
        static readonly IntPtr setAccessibilityIdentifier_Handle = Selector.GetHandle("setAccessibilityIdentifier:");

        /// <summary>
        ///   Sets the Acessibility ID on a UIView.
        /// </summary>
        /// <remarks>>The Accessibility ID is not bound in Xamarin.iOS, so this method provides a way to set it programatically.</remarks>
        /// <returns>The accessibility identifier.</returns>
        /// <param name="view">View.</param>
        /// <param name="id">Identifier.</param>
        public static UIView SetAccessibilityId(this UIView view, string id)
        {
            IntPtr intPtr = NSString.CreateNative(id);
            Messaging.void_objc_msgSend_IntPtr(view.Handle, setAccessibilityIdentifier_Handle, intPtr);
            NSString.ReleaseNative(intPtr);
            return view;
        }

        /// <summary>
        ///   Retrieves the Accessibility ID for a given UIView.
        /// </summary>
        /// <remarks>>The Accessibility ID is not bound in Xamarin.iOS, so this method provides a way to set it programatically.</remarks>
        /// <returns>The accessibility identifier.</returns>
        /// <param name="view">View.</param>
        public static string GetAccessibilityId(this UIView view)
        {
            return NSString.FromHandle(Messaging.IntPtr_objc_msgSend(view.Handle, selAccessibilityIdentifier_Handle));
        }
    }
}
