namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// A Window is a top-level Window/widget in the Views hierarchy. By default it
    /// will have a non-client area with title bar, icon and buttons that supports
    /// moving and resizing. All size and position values are in density independent
    /// pixels (DIP) unless otherwise indicated. Methods must be called on the
    /// browser process UI thread unless otherwise indicated. (Views — CEF 146.)
    /// </summary>
    public sealed unsafe partial class CefWindow
    {
        /// <summary>
        /// Create a new Window.
        /// </summary>
        public static CefWindow CreateTopLevel(CefWindowDelegate @delegate)
        {
            if (@delegate == null) throw new ArgumentNullException(nameof(@delegate));
            return CefWindow.FromNative(cef_window_t.create_top_level(@delegate.ToNative()));
        }

        private cef_window_t* _windowSelf => (cef_window_t*)_self;

        /// <summary>
        /// Show the Window.
        /// </summary>
        public void Show()
        {
            cef_window_t.show(_windowSelf);
        }

        /// <summary>
        /// Show the Window as a browser modal dialog relative to |browserView|. A
        /// parent Window must be returned via CefWindowDelegate.GetParentWindow() and
        /// |browserView| must belong to that parent Window. While this Window is
        /// visible, |browserView| will be disabled while other controls in the parent
        /// Window remain enabled. Navigating or destroying the |browserView| will
        /// close this Window automatically. Alternately, use Show() and return true
        /// from CefWindowDelegate.IsWindowModalDialog() for a window modal dialog
        /// where all controls in the parent Window are disabled.
        /// </summary>
        public void ShowAsBrowserModalDialog(CefBrowserView browserView)
        {
            if (browserView == null) throw new ArgumentNullException(nameof(browserView));
            cef_window_t.show_as_browser_modal_dialog(_windowSelf, browserView.ToNative());
        }

        /// <summary>
        /// Hide the Window.
        /// </summary>
        public void Hide()
        {
            cef_window_t.hide(_windowSelf);
        }

        /// <summary>
        /// Sizes the Window to |size| and centers it in the current display.
        /// </summary>
        public void CenterWindow(CefSize size)
        {
            var n_size = new cef_size_t(size.Width, size.Height);
            cef_window_t.center_window(_windowSelf, &n_size);
        }

        /// <summary>
        /// Close the Window.
        /// </summary>
        public void Close()
        {
            cef_window_t.close(_windowSelf);
        }

        /// <summary>
        /// Returns true if the Window has been closed.
        /// </summary>
        public bool IsClosed
        {
            get { return cef_window_t.is_closed(_windowSelf) != 0; }
        }

        /// <summary>
        /// Activate the Window, assuming it already exists and is visible.
        /// </summary>
        public void Activate()
        {
            cef_window_t.activate(_windowSelf);
        }

        /// <summary>
        /// Deactivate the Window, making the next Window in the Z order the active
        /// Window.
        /// </summary>
        public void Deactivate()
        {
            cef_window_t.deactivate(_windowSelf);
        }

        /// <summary>
        /// Returns whether the Window is the currently active Window.
        /// </summary>
        public bool IsActive
        {
            get { return cef_window_t.is_active(_windowSelf) != 0; }
        }

        /// <summary>
        /// Bring this Window to the top of other Windows in the Windowing system.
        /// </summary>
        public void BringToTop()
        {
            cef_window_t.bring_to_top(_windowSelf);
        }

        /// <summary>
        /// Set the Window to be on top of other Windows in the Windowing system.
        /// </summary>
        public void SetAlwaysOnTop(bool onTop)
        {
            cef_window_t.set_always_on_top(_windowSelf, onTop ? 1 : 0);
        }

        /// <summary>
        /// Returns whether the Window has been set to be on top of other Windows in
        /// the Windowing system.
        /// </summary>
        public bool IsAlwaysOnTop
        {
            get { return cef_window_t.is_always_on_top(_windowSelf) != 0; }
        }

        /// <summary>
        /// Maximize the Window.
        /// </summary>
        public void Maximize()
        {
            cef_window_t.maximize(_windowSelf);
        }

        /// <summary>
        /// Minimize the Window.
        /// </summary>
        public void Minimize()
        {
            cef_window_t.minimize(_windowSelf);
        }

        /// <summary>
        /// Restore the Window.
        /// </summary>
        public void Restore()
        {
            cef_window_t.restore(_windowSelf);
        }

        /// <summary>
        /// Set fullscreen Window state. The
        /// CefWindowDelegate.OnWindowFullscreenTransition method will be called
        /// during the fullscreen transition for notification purposes.
        /// </summary>
        public void SetFullscreen(bool fullscreen)
        {
            cef_window_t.set_fullscreen(_windowSelf, fullscreen ? 1 : 0);
        }

        /// <summary>
        /// Returns true if the Window is maximized.
        /// </summary>
        public bool IsMaximized
        {
            get { return cef_window_t.is_maximized(_windowSelf) != 0; }
        }

        /// <summary>
        /// Returns true if the Window is minimized.
        /// </summary>
        public bool IsMinimized
        {
            get { return cef_window_t.is_minimized(_windowSelf) != 0; }
        }

        /// <summary>
        /// Returns true if the Window is fullscreen.
        /// </summary>
        public bool IsFullscreen
        {
            get { return cef_window_t.is_fullscreen(_windowSelf) != 0; }
        }

        /// <summary>
        /// Returns the View that currently has focus in this Window, or null if no
        /// View currently has focus. A Window may have a focused View even if it is
        /// not currently active. Any focus changes while a Window is not active may
        /// be applied after that Window next becomes active.
        /// </summary>
        public CefView GetFocusedView()
        {
            return CefView.FromNativeOrNull(cef_window_t.get_focused_view(_windowSelf));
        }

        /// <summary>
        /// Get/set the Window title.
        /// </summary>
        public string Title
        {
            get
            {
                var n_result = cef_window_t.get_title(_windowSelf);
                return cef_string_userfree.ToString(n_result);
            }
            set
            {
                fixed (char* value_ptr = value)
                {
                    var n_value = new cef_string_t(value_ptr, value != null ? value.Length : 0);
                    cef_window_t.set_title(_windowSelf, &n_value);
                }
            }
        }

        /// <summary>
        /// Set the Window icon. This should be a 16x16 icon suitable for use in the
        /// Windows's title bar.
        /// </summary>
        public void SetWindowIcon(CefImage image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            cef_window_t.set_window_icon(_windowSelf, image.ToNative());
        }

        /// <summary>
        /// Get the Window icon.
        /// </summary>
        public CefImage GetWindowIcon()
        {
            return CefImage.FromNativeOrNull(cef_window_t.get_window_icon(_windowSelf));
        }

        /// <summary>
        /// Set the Window App icon. This should be a larger icon for use in the host
        /// environment app switching UI. On Windows, this is the ICON_BIG used in
        /// Alt-Tab list and Windows taskbar. The Window icon will be used by default
        /// if no Window App icon is specified.
        /// </summary>
        public void SetWindowAppIcon(CefImage image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            cef_window_t.set_window_app_icon(_windowSelf, image.ToNative());
        }

        /// <summary>
        /// Get the Window App icon.
        /// </summary>
        public CefImage GetWindowAppIcon()
        {
            return CefImage.FromNativeOrNull(cef_window_t.get_window_app_icon(_windowSelf));
        }

        /// <summary>
        /// Add a View that will be overlayed on the Window contents with absolute
        /// positioning and high z-order. Positioning is controlled by |dockingMode|
        /// as described below. Setting |canActivate| to true will allow the overlay
        /// view to receive input focus. The returned CefOverlayController object is
        /// used to control the overlay. Overlays are hidden by default.
        /// With CEF_DOCKING_MODE_CUSTOM:
        ///   1. The overlay is initially hidden, sized to |view|'s preferred size,
        ///      and positioned in the top-left corner.
        ///   2. Optionally change the overlay position and/or size by calling
        ///      CefOverlayController methods.
        ///   3. Call CefOverlayController.SetVisible(true) to show the overlay.
        ///   4. The overlay will be automatically re-sized if |view|'s layout
        ///      changes. Optionally change the overlay position and/or size when
        ///      OnLayoutChanged is called on the Window's delegate to indicate a
        ///      change in Window bounds.
        /// With other docking modes:
        ///   1. The overlay is initially hidden, sized to |view|'s preferred size,
        ///      and positioned based on |dockingMode|.
        ///   2. Call CefOverlayController.SetVisible(true) to show the overlay.
        ///   3. The overlay will be automatically re-sized if |view|'s layout changes
        ///      and re-positioned as appropriate when the Window resizes.
        /// Overlays created by this method will receive a higher z-order then any
        /// child Views added previously. It is therefore recommended to call this
        /// method last after all other child Views have been added so that the
        /// overlay displays as the top-most child of the Window.
        /// </summary>
        public CefOverlayController AddOverlayView(CefView view, CefDockingMode dockingMode, bool canActivate)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            return CefOverlayController.FromNativeOrNull(
                cef_window_t.add_overlay_view(_windowSelf, view.ToNative(), dockingMode, canActivate ? 1 : 0));
        }

        /// <summary>
        /// Show a menu with contents |menuModel|. |screenPoint| specifies the menu
        /// position in screen coordinates. |anchorPosition| specifies how the menu
        /// will be anchored relative to |screenPoint|.
        /// </summary>
        public void ShowMenu(CefMenuModel menuModel, CefPoint screenPoint, CefMenuAnchorPosition anchorPosition)
        {
            if (menuModel == null) throw new ArgumentNullException(nameof(menuModel));
            var n_point = new cef_point_t(screenPoint.X, screenPoint.Y);
            cef_window_t.show_menu(_windowSelf, menuModel.ToNative(), &n_point, anchorPosition);
        }

        /// <summary>
        /// Cancel the menu that is currently showing, if any.
        /// </summary>
        public void CancelMenu()
        {
            cef_window_t.cancel_menu(_windowSelf);
        }

        /// <summary>
        /// Returns the Display that most closely intersects the bounds of this
        /// Window. May return null if this Window is not currently displayed.
        /// </summary>
        public CefDisplay GetDisplay()
        {
            return CefDisplay.FromNativeOrNull(cef_window_t.get_display(_windowSelf));
        }

        /// <summary>
        /// Returns the bounds (size and position) of this Window's client area.
        /// Position is in screen coordinates.
        /// </summary>
        public CefRectangle GetClientAreaBoundsInScreen()
        {
            var n = cef_window_t.get_client_area_bounds_in_screen(_windowSelf);
            return new CefRectangle(n.x, n.y, n.width, n.height);
        }

        /// <summary>
        /// Set the regions where mouse events will be intercepted by this Window to
        /// support drag operations. Call this method with an empty array to clear
        /// the draggable regions. The draggable region bounds should be in window
        /// coordinates.
        /// </summary>
        public void SetDraggableRegions(CefDraggableRegion[] regions)
        {
            var count = regions != null ? regions.Length : 0;
            if (count == 0)
            {
                cef_window_t.set_draggable_regions(_windowSelf, UIntPtr.Zero, null);
                return;
            }
            var n_regions = stackalloc cef_draggable_region_t[count];
            for (var i = 0; i < count; i++)
            {
                var b = regions[i].Bounds;
                n_regions[i].bounds = new cef_rect_t(b.X, b.Y, b.Width, b.Height);
                n_regions[i].draggable = regions[i].Draggable ? 1 : 0;
            }
            cef_window_t.set_draggable_regions(_windowSelf, (UIntPtr)count, n_regions);
        }

        /// <summary>
        /// Retrieve the platform window handle for this Window (HWND on Windows,
        /// NSView* on macOS).
        /// </summary>
        public IntPtr GetWindowHandle()
        {
            return cef_window_t.get_window_handle(_windowSelf);
        }

        /// <summary>
        /// Simulate a key press. |keyCode| is the VKEY_* value from Chromium's
        /// ui/events/keycodes/keyboard_codes.h header (VK_* values on Windows).
        /// |eventFlags| is some combination of EVENTFLAG_SHIFT_DOWN,
        /// EVENTFLAG_CONTROL_DOWN and/or EVENTFLAG_ALT_DOWN. This method is exposed
        /// primarily for testing purposes.
        /// </summary>
        public void SendKeyPress(int keyCode, CefEventFlags eventFlags)
        {
            cef_window_t.send_key_press(_windowSelf, keyCode, (uint)eventFlags);
        }

        /// <summary>
        /// Simulate a mouse move. The mouse cursor will be moved to the specified
        /// (screenX, screenY) position. This method is exposed primarily for testing
        /// purposes.
        /// </summary>
        public void SendMouseMove(int screenX, int screenY)
        {
            cef_window_t.send_mouse_move(_windowSelf, screenX, screenY);
        }

        /// <summary>
        /// Simulate mouse down and/or mouse up events. |button| is the mouse button
        /// type. If |mouseDown| is true a mouse down event will be sent. If
        /// |mouseUp| is true a mouse up event will be sent. If both are true a mouse
        /// down event will be sent followed by a mouse up event (equivalent to
        /// clicking the mouse button). The events will be sent using the current
        /// cursor position so make sure to call SendMouseMove() first to position
        /// the mouse. This method is exposed primarily for testing purposes.
        /// </summary>
        public void SendMouseEvents(CefMouseButtonType button, bool mouseDown, bool mouseUp)
        {
            cef_window_t.send_mouse_events(_windowSelf, button, mouseDown ? 1 : 0, mouseUp ? 1 : 0);
        }

        /// <summary>
        /// Set the keyboard accelerator for the specified |commandId|. |keyCode| can
        /// be any virtual key or character value. Required modifier keys are
        /// specified by |shiftPressed|, |ctrlPressed| and/or |altPressed|. If
        /// |highPriority| is true then the accelerator will be registered with
        /// Chromium's high priority accelerator handling and will be processed
        /// before the browser handles it; otherwise it is only processed after the
        /// browser declines it. CefWindowDelegate.OnAccelerator will be called if
        /// the accelerator is triggered.
        /// </summary>
        public void SetAccelerator(int commandId, int keyCode, bool shiftPressed, bool ctrlPressed, bool altPressed, bool highPriority)
        {
            cef_window_t.set_accelerator(_windowSelf, commandId, keyCode, shiftPressed ? 1 : 0, ctrlPressed ? 1 : 0, altPressed ? 1 : 0, highPriority ? 1 : 0);
        }

        /// <summary>
        /// Remove the keyboard accelerator for the specified |commandId|.
        /// </summary>
        public void RemoveAccelerator(int commandId)
        {
            cef_window_t.remove_accelerator(_windowSelf, commandId);
        }

        /// <summary>
        /// Remove all keyboard accelerators.
        /// </summary>
        public void RemoveAllAccelerators()
        {
            cef_window_t.remove_all_accelerators(_windowSelf);
        }

        /// <summary>
        /// Override a standard theme color or add a custom color associated with
        /// |colorId|. See cef_color_ids.h for standard ID values. Recommended usage
        /// is as follows:
        /// 1. Customize the default native/OS theme by calling SetThemeColor before
        ///    showing the first Window. When done setting colors call
        ///    CefWindow.ThemeChanged to trigger CefViewDelegate.OnThemeChanged
        ///    notifications.
        /// 2. Customize the current native/OS or Chrome theme after it changes by
        ///    calling SetThemeColor from the CefWindowDelegate.OnThemeColorsChanged
        ///    callback. CefViewDelegate.OnThemeChanged notifications will then be
        ///    triggered automatically.
        /// The configured color will be available immediately via
        /// CefView.GetThemeColor and will be applied to each View in this Window's
        /// component hierarchy when CefViewDelegate.OnThemeChanged is called. See
        /// OnThemeColorsChanged documentation for additional details.
        /// Clients wishing to add custom colors should use |colorId| values >=
        /// CEF_ChromeColorsEnd.
        /// </summary>
        public void SetThemeColor(int colorId, CefColor color)
        {
            cef_window_t.set_theme_color(_windowSelf, colorId, color.ToArgb());
        }

        /// <summary>
        /// Trigger CefViewDelegate.OnThemeChanged callbacks for each View in this
        /// Window's component hierarchy. Unlike a native/OS or Chrome theme change
        /// this function does not reset theme colors to standard values and does
        /// not result in a call to CefWindowDelegate.OnThemeColorsChanged.
        /// Do not call this method from CefWindowDelegate.OnThemeColorsChanged or
        /// CefViewDelegate.OnThemeChanged.
        /// </summary>
        public void ThemeChanged()
        {
            cef_window_t.theme_changed(_windowSelf);
        }

        /// <summary>
        /// Returns the runtime style for this Window (ALLOY or CHROME). See
        /// CefRuntimeStyle documentation for details.
        /// </summary>
        public CefRuntimeStyle RuntimeStyle
        {
            get { return cef_window_t.get_runtime_style(_windowSelf); }
        }
    }
}
