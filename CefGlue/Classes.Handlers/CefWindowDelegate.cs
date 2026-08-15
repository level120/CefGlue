namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// Implement this interface to handle window events.
    /// The methods of this class will be called on the browser process UI thread
    /// unless otherwise indicated.
    /// (Views — CEF 146. Delegate inheritance chain is flattened per class: this
    /// class carries the CefViewDelegate slots (via CefPanelDelegate) directly. View/Window/BrowserView
    /// arguments may be null — CEF queries some delegate methods, e.g.
    /// AllowPictureInPictureWithoutUserActivation, before the view exists.)
    /// </summary>
    public abstract unsafe partial class CefWindowDelegate
    {
        private cef_size_t get_preferred_size(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_window_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            var size = GetPreferredSize(m_view);
            return new cef_size_t(size.Width, size.Height);
        }

        /// <summary>
        /// Return the preferred size for |view|. The Layout will use this information
        /// to determine the display size. Default (empty) lets the View decide.
        /// </summary>
        protected virtual CefSize GetPreferredSize(CefView view) => new CefSize(0, 0);


        private cef_size_t get_minimum_size(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_window_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            var size = GetMinimumSize(m_view);
            return new cef_size_t(size.Width, size.Height);
        }

        /// <summary>Return the minimum size for |view|.</summary>
        protected virtual CefSize GetMinimumSize(CefView view) => new CefSize(0, 0);


        private cef_size_t get_maximum_size(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_window_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            var size = GetMaximumSize(m_view);
            return new cef_size_t(size.Width, size.Height);
        }

        /// <summary>Return the maximum size for |view|.</summary>
        protected virtual CefSize GetMaximumSize(CefView view) => new CefSize(0, 0);


        private int get_height_for_width(cef_view_delegate_t* self, cef_view_t* view, int width)
        {
            CheckSelf((cef_window_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            return GetHeightForWidth(m_view, width);
        }

        /// <summary>
        /// Return the height necessary to display |view| with the provided |width|.
        /// If not specified (0) the result of GetPreferredSize().Height will be used.
        /// </summary>
        protected virtual int GetHeightForWidth(CefView view, int width) => 0;


        private void on_parent_view_changed(cef_view_delegate_t* self, cef_view_t* view, int added, cef_view_t* parent)
        {
            CheckSelf((cef_window_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            var m_parent = CefView.FromNativeOrNull(parent);
            OnParentViewChanged(m_view, added != 0, m_parent);
        }

        /// <summary>
        /// Called when the parent of |view| has changed. If |view| is being added to
        /// |parent| then |added| will be true. Do not modify the view hierarchy in
        /// this callback.
        /// </summary>
        protected virtual void OnParentViewChanged(CefView view, bool added, CefView parent) { }


        private void on_child_view_changed(cef_view_delegate_t* self, cef_view_t* view, int added, cef_view_t* child)
        {
            CheckSelf((cef_window_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            var m_child = CefView.FromNativeOrNull(child);
            OnChildViewChanged(m_view, added != 0, m_child);
        }

        /// <summary>
        /// Called when a child of |view| has changed. If |child| is being added to
        /// |view| then |added| will be true. Do not modify the view hierarchy in this
        /// callback.
        /// </summary>
        protected virtual void OnChildViewChanged(CefView view, bool added, CefView child) { }


        private void on_window_changed(cef_view_delegate_t* self, cef_view_t* view, int added)
        {
            CheckSelf((cef_window_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            OnWindowChanged(m_view, added != 0);
        }

        /// <summary>Called when |view| is added or removed from the CefWindow.</summary>
        protected virtual void OnWindowChanged(CefView view, bool added) { }


        private void on_layout_changed(cef_view_delegate_t* self, cef_view_t* view, cef_rect_t* new_bounds)
        {
            CheckSelf((cef_window_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            var m_bounds = new CefRectangle(new_bounds->x, new_bounds->y, new_bounds->width, new_bounds->height);
            OnLayoutChanged(m_view, m_bounds);
        }

        /// <summary>Called when the layout of |view| has changed.</summary>
        protected virtual void OnLayoutChanged(CefView view, CefRectangle newBounds) { }


        private void on_focus(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_window_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            OnFocus(m_view);
        }

        /// <summary>Called when |view| gains focus.</summary>
        protected virtual void OnFocus(CefView view) { }


        private void on_blur(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_window_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            OnBlur(m_view);
        }

        /// <summary>Called when |view| loses focus.</summary>
        protected virtual void OnBlur(CefView view) { }


        private void on_theme_changed(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_window_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            OnThemeChanged(m_view);
        }

        /// <summary>
        /// Called when the theme for |view| has changed, after the new theme colors
        /// have already been applied. Optionally override per-View theme colors here
        /// (CefView.SetBackgroundColor etc.).
        /// </summary>
        protected virtual void OnThemeChanged(CefView view) { }


        private void on_window_created(cef_window_delegate_t* self, cef_window_t* window)
        {
            CheckSelf(self);
            OnWindowCreated(CefWindow.FromNativeOrNull(window));
        }

        /// <summary>Called when |window| is created.</summary>
        protected virtual void OnWindowCreated(CefWindow window) { }


        private void on_window_closing(cef_window_delegate_t* self, cef_window_t* window)
        {
            CheckSelf(self);
            OnWindowClosing(CefWindow.FromNativeOrNull(window));
        }

        /// <summary>Called when |window| is closing.</summary>
        protected virtual void OnWindowClosing(CefWindow window) { }


        private void on_window_destroyed(cef_window_delegate_t* self, cef_window_t* window)
        {
            CheckSelf(self);
            OnWindowDestroyed(CefWindow.FromNativeOrNull(window));
        }

        /// <summary>
        /// Called when |window| is destroyed. Release all references to |window| and
        /// do not attempt to execute any methods on |window| after this callback
        /// returns.
        /// </summary>
        protected virtual void OnWindowDestroyed(CefWindow window) { }


        private void on_window_activation_changed(cef_window_delegate_t* self, cef_window_t* window, int active)
        {
            CheckSelf(self);
            OnWindowActivationChanged(CefWindow.FromNativeOrNull(window), active != 0);
        }

        /// <summary>Called when |window| is activated or deactivated.</summary>
        protected virtual void OnWindowActivationChanged(CefWindow window, bool active) { }


        private void on_window_bounds_changed(cef_window_delegate_t* self, cef_window_t* window, cef_rect_t* new_bounds)
        {
            CheckSelf(self);
            var m_bounds = new CefRectangle(new_bounds->x, new_bounds->y, new_bounds->width, new_bounds->height);
            OnWindowBoundsChanged(CefWindow.FromNativeOrNull(window), m_bounds);
        }

        /// <summary>Called when |window| bounds have changed. |newBounds| is in DIP screen coordinates.</summary>
        protected virtual void OnWindowBoundsChanged(CefWindow window, CefRectangle newBounds) { }


        private void on_window_fullscreen_transition(cef_window_delegate_t* self, cef_window_t* window, int is_completed)
        {
            CheckSelf(self);
            OnWindowFullscreenTransition(CefWindow.FromNativeOrNull(window), is_completed != 0);
        }

        /// <summary>
        /// Called when |window| is transitioning to or from fullscreen mode. On MacOS
        /// the transition occurs asynchronously with |isCompleted| set to false when
        /// the transition starts and true after the transition completes. On other
        /// platforms the transition occurs synchronously with |isCompleted| set to
        /// true after the transition completes.
        /// </summary>
        protected virtual void OnWindowFullscreenTransition(CefWindow window, bool isCompleted) { }


        private cef_window_t* get_parent_window(cef_window_delegate_t* self, cef_window_t* window, int* is_menu, int* can_activate_menu)
        {
            CheckSelf(self);
            var m_isMenu = *is_menu != 0;
            var m_canActivateMenu = *can_activate_menu != 0;
            var parent = GetParentWindow(CefWindow.FromNativeOrNull(window), ref m_isMenu, ref m_canActivateMenu);
            *is_menu = m_isMenu ? 1 : 0;
            *can_activate_menu = m_canActivateMenu ? 1 : 0;
            return parent != null ? parent.ToNative() : null;
        }

        /// <summary>
        /// Return the parent for |window| or null if the |window| does not have a
        /// parent. Windows with parents will not get a taskbar button. Set |isMenu|
        /// to true if |window| will be displayed as a menu, in which case it will not
        /// be clipped to the parent window bounds. Set |canActivateMenu| to false if
        /// |isMenu| is true and |window| should not be activated (given keyboard
        /// focus) when displayed.
        /// </summary>
        protected virtual CefWindow GetParentWindow(CefWindow window, ref bool isMenu, ref bool canActivateMenu) => null;


        private int is_window_modal_dialog(cef_window_delegate_t* self, cef_window_t* window)
        {
            CheckSelf(self);
            return IsWindowModalDialog(CefWindow.FromNativeOrNull(window)) ? 1 : 0;
        }

        /// <summary>
        /// Return true if |window| should be created as a window modal dialog. Only
        /// called when a Window is returned via GetParentWindow() with |isMenu| set
        /// to false. All controls in the parent Window will be disabled while
        /// |window| is visible. This functionality is not supported by all Linux
        /// window managers. Alternately, use CefWindow.ShowAsBrowserModalDialog()
        /// for a browser modal dialog that works on all platforms.
        /// </summary>
        protected virtual bool IsWindowModalDialog(CefWindow window) => false;


        private cef_rect_t get_initial_bounds(cef_window_delegate_t* self, cef_window_t* window)
        {
            CheckSelf(self);
            var r = GetInitialBounds(CefWindow.FromNativeOrNull(window));
            return new cef_rect_t(r.X, r.Y, r.Width, r.Height);
        }

        /// <summary>
        /// Return the initial bounds for |window| in density independent pixel (DIP)
        /// coordinates. If this method returns an empty rect (default) then the
        /// preferred size will be used and the window will be centered on the
        /// display where the cursor is currently located.
        /// </summary>
        protected virtual CefRectangle GetInitialBounds(CefWindow window) => new CefRectangle(0, 0, 0, 0);


        private CefShowState get_initial_show_state(cef_window_delegate_t* self, cef_window_t* window)
        {
            CheckSelf(self);
            return GetInitialShowState(CefWindow.FromNativeOrNull(window));
        }

        /// <summary>Return the initial show state for |window|.</summary>
        protected virtual CefShowState GetInitialShowState(CefWindow window) => CefShowState.Normal;


        private int is_frameless(cef_window_delegate_t* self, cef_window_t* window)
        {
            CheckSelf(self);
            return IsFrameless(CefWindow.FromNativeOrNull(window)) ? 1 : 0;
        }

        /// <summary>
        /// Return true if |window| should be created without a frame or title bar.
        /// The window will be resizable if CanResize() returns true. Use
        /// CefWindow.SetDraggableRegions() to specify draggable regions.
        /// </summary>
        protected virtual bool IsFrameless(CefWindow window) => false;


        private int with_standard_window_buttons(cef_window_delegate_t* self, cef_window_t* window)
        {
            CheckSelf(self);
            return WithStandardWindowButtons(CefWindow.FromNativeOrNull(window)) ? 1 : 0;
        }

        /// <summary>
        /// Return true if |window| should be created with standard window buttons
        /// like close, minimize and zoom. This method is only supported on macOS.
        /// Default: !IsFrameless(window).
        /// </summary>
        protected virtual bool WithStandardWindowButtons(CefWindow window) => !IsFrameless(window);


        private int get_titlebar_height(cef_window_delegate_t* self, cef_window_t* window, float* titlebar_height)
        {
            CheckSelf(self);
            var m_height = *titlebar_height;
            if (GetTitlebarHeight(CefWindow.FromNativeOrNull(window), ref m_height))
            {
                *titlebar_height = m_height;
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Return whether the titlebar height should be overridden, and sets the
        /// height of the titlebar in |titlebarHeight|. On macOS, it can also be used
        /// to adjust the vertical position of the traffic light buttons in frameless
        /// windows. The buttons will be positioned halfway down the titlebar at a
        /// height of |titlebarHeight| / 2.
        /// </summary>
        protected virtual bool GetTitlebarHeight(CefWindow window, ref float titlebarHeight) => false;


        private CefState accepts_first_mouse(cef_window_delegate_t* self, cef_window_t* window)
        {
            CheckSelf(self);
            return AcceptsFirstMouse(CefWindow.FromNativeOrNull(window));
        }

        /// <summary>
        /// Return whether the view should accept the initial mouse-down event,
        /// allowing it to respond to click-through behavior. If Default is returned
        /// then the value of CefSettings.AcceptsFirstMouse is used (macOS only).
        /// </summary>
        protected virtual CefState AcceptsFirstMouse(CefWindow window) => CefState.Default;


        private int can_resize(cef_window_delegate_t* self, cef_window_t* window)
        {
            CheckSelf(self);
            return CanResize(CefWindow.FromNativeOrNull(window)) ? 1 : 0;
        }

        /// <summary>Return true if |window| can be resized.</summary>
        protected virtual bool CanResize(CefWindow window) => true;


        private int can_maximize(cef_window_delegate_t* self, cef_window_t* window)
        {
            CheckSelf(self);
            return CanMaximize(CefWindow.FromNativeOrNull(window)) ? 1 : 0;
        }

        /// <summary>Return true if |window| can be maximized.</summary>
        protected virtual bool CanMaximize(CefWindow window) => true;


        private int can_minimize(cef_window_delegate_t* self, cef_window_t* window)
        {
            CheckSelf(self);
            return CanMinimize(CefWindow.FromNativeOrNull(window)) ? 1 : 0;
        }

        /// <summary>Return true if |window| can be minimized.</summary>
        protected virtual bool CanMinimize(CefWindow window) => true;


        private int can_close(cef_window_delegate_t* self, cef_window_t* window)
        {
            CheckSelf(self);
            return CanClose(CefWindow.FromNativeOrNull(window)) ? 1 : 0;
        }

        /// <summary>
        /// Return true if |window| can be closed. This will be called for user-
        /// initiated window close actions and when CefWindow.Close() is called.
        /// </summary>
        protected virtual bool CanClose(CefWindow window) => true;


        private int on_accelerator(cef_window_delegate_t* self, cef_window_t* window, int command_id)
        {
            CheckSelf(self);
            return OnAccelerator(CefWindow.FromNativeOrNull(window), command_id) ? 1 : 0;
        }

        /// <summary>
        /// Called when a keyboard accelerator registered with
        /// CefWindow.SetAccelerator is triggered. Return true if the accelerator was
        /// handled or false otherwise.
        /// </summary>
        protected virtual bool OnAccelerator(CefWindow window, int commandId) => false;


        private int on_key_event(cef_window_delegate_t* self, cef_window_t* window, cef_key_event_t* @event)
        {
            CheckSelf(self);
            var m_event = CefKeyEvent.FromNative(@event);
            return OnKeyEvent(CefWindow.FromNativeOrNull(window), m_event) ? 1 : 0;
        }

        /// <summary>
        /// Called after all other controls in the window have had a chance to handle
        /// the event. |keyEvent| contains information about the keyboard event.
        /// Return true if the keyboard event was handled or false otherwise.
        /// </summary>
        protected virtual bool OnKeyEvent(CefWindow window, CefKeyEvent keyEvent) => false;


        private void on_theme_colors_changed(cef_window_delegate_t* self, cef_window_t* window, int chrome_theme)
        {
            CheckSelf(self);
            OnThemeColorsChanged(CefWindow.FromNativeOrNull(window), chrome_theme != 0);
        }

        /// <summary>
        /// Called after the native/OS or Chrome theme has changed. |chromeTheme|
        /// will be true if the notification is for a Chrome theme. Optionally use
        /// this callback to override the new per-Window theme colors by calling
        /// CefWindow.SetThemeColor.
        /// </summary>
        protected virtual void OnThemeColorsChanged(CefWindow window, bool chromeTheme) { }


        private CefRuntimeStyle get_window_runtime_style(cef_window_delegate_t* self)
        {
            CheckSelf(self);
            return GetWindowRuntimeStyle();
        }

        /// <summary>
        /// Optionally change the runtime style for this Window. See CefRuntimeStyle
        /// documentation for details.
        /// </summary>
        protected virtual CefRuntimeStyle GetWindowRuntimeStyle() => CefRuntimeStyle.Default;


        private int get_linux_window_properties(cef_window_delegate_t* self, cef_window_t* window, cef_linux_window_properties_t* properties)
        {
            CheckSelf(self);
            // Linux-only hook (Wayland app id / X11 WM_CLASS). Not surfaced by this binding — default (false).
            return 0;
        }
    }
}
