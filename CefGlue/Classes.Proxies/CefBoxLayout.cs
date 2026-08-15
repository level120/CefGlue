namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// A Layout manager that arranges child views vertically or horizontally in a
    /// side-by-side fashion with spacing around and between the child views. The
    /// child views are always sized according to their preferred size. If the
    /// host's bounds provide insufficient space, child views will be clamped.
    /// Excess space will not be distributed. Methods must be called on the browser
    /// process UI thread unless otherwise indicated. (Views — CEF 146.)
    /// </summary>
    public sealed unsafe partial class CefBoxLayout
    {
        /// <summary>
        /// Set the flex weight for the given |view|. Using the preferred size as
        /// the basis, free space along the main axis is distributed to views in the
        /// ratio of their flex weights. Similarly, if the views will overflow the
        /// parent, space is subtracted in these ratios. A flex of 0 means this view
        /// is not resized. Flex values must not be negative.
        /// </summary>
        public void SetFlexForView(CefView view, int flex)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            cef_box_layout_t.set_flex_for_view((cef_box_layout_t*)_self, view.ToNative(), flex);
        }

        /// <summary>
        /// Clears the flex for the given |view|, causing it to use the default flex
        /// specified via CefBoxLayoutSettings.DefaultFlex.
        /// </summary>
        public void ClearFlexForView(CefView view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            cef_box_layout_t.clear_flex_for_view((cef_box_layout_t*)_self, view.ToNative());
        }
    }
}
