namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// A Layout handles the sizing of the children of a Panel according to
    /// implementation-specific heuristics. Methods must be called on the browser
    /// process UI thread unless otherwise indicated. (Views — CEF 146. Root of
    /// CefBoxLayout/CefFillLayout.)
    /// </summary>
    public unsafe partial class CefLayout
    {
        /// <summary>
        /// Returns this Layout as a BoxLayout or null if this is not a BoxLayout.
        /// </summary>
        public CefBoxLayout AsBoxLayout()
        {
            return CefBoxLayout.FromNativeOrNull(cef_layout_t.as_box_layout(_self));
        }

        /// <summary>
        /// Returns this Layout as a FillLayout or null if this is not a FillLayout.
        /// </summary>
        public CefFillLayout AsFillLayout()
        {
            return CefFillLayout.FromNativeOrNull(cef_layout_t.as_fill_layout(_self));
        }

        /// <summary>
        /// Returns true if this Layout is valid.
        /// </summary>
        public bool IsValid
        {
            get { return cef_layout_t.is_valid(_self) != 0; }
        }
    }
}
