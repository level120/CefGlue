//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_show_state_t.
//
namespace Xilium.CefGlue
{
    using System;

    /// <summary>
    /// Show states supported by CefWindowDelegate::GetInitialShowState.
    /// </summary>
    public enum CefShowState
    {
        /// <summary>Show the window as normal.</summary>
        Normal,
        /// <summary>Show the window as minimized.</summary>
        Minimized,
        /// <summary>Show the window as maximized.</summary>
        Maximized,
        /// <summary>Show the window as fullscreen.</summary>
        Fullscreen,
        /// <summary>Show the window as hidden (no dock thumbnail). Only supported on MacOS.</summary>
        Hidden,
        NumValues,
    }
}
