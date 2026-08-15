namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// Implement this interface to handle MenuButton events.
    /// The methods of this class will be called on the browser process UI thread
    /// unless otherwise indicated.
    /// (Views — CEF 146. Delegate inheritance chain is flattened per class: this
    /// class carries the CefViewDelegate slots (via CefButtonDelegate) directly. View/Window/BrowserView
    /// arguments may be null — CEF queries some delegate methods, e.g.
    /// AllowPictureInPictureWithoutUserActivation, before the view exists.)
    /// </summary>
    public abstract unsafe partial class CefMenuButtonDelegate
    {
        private cef_size_t get_preferred_size(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_menu_button_delegate_t*)self);
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
            CheckSelf((cef_menu_button_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            var size = GetMinimumSize(m_view);
            return new cef_size_t(size.Width, size.Height);
        }

        /// <summary>Return the minimum size for |view|.</summary>
        protected virtual CefSize GetMinimumSize(CefView view) => new CefSize(0, 0);


        private cef_size_t get_maximum_size(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_menu_button_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            var size = GetMaximumSize(m_view);
            return new cef_size_t(size.Width, size.Height);
        }

        /// <summary>Return the maximum size for |view|.</summary>
        protected virtual CefSize GetMaximumSize(CefView view) => new CefSize(0, 0);


        private int get_height_for_width(cef_view_delegate_t* self, cef_view_t* view, int width)
        {
            CheckSelf((cef_menu_button_delegate_t*)self);
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
            CheckSelf((cef_menu_button_delegate_t*)self);
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
            CheckSelf((cef_menu_button_delegate_t*)self);
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
            CheckSelf((cef_menu_button_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            OnWindowChanged(m_view, added != 0);
        }

        /// <summary>Called when |view| is added or removed from the CefWindow.</summary>
        protected virtual void OnWindowChanged(CefView view, bool added) { }


        private void on_layout_changed(cef_view_delegate_t* self, cef_view_t* view, cef_rect_t* new_bounds)
        {
            CheckSelf((cef_menu_button_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            var m_bounds = new CefRectangle(new_bounds->x, new_bounds->y, new_bounds->width, new_bounds->height);
            OnLayoutChanged(m_view, m_bounds);
        }

        /// <summary>Called when the layout of |view| has changed.</summary>
        protected virtual void OnLayoutChanged(CefView view, CefRectangle newBounds) { }


        private void on_focus(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_menu_button_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            OnFocus(m_view);
        }

        /// <summary>Called when |view| gains focus.</summary>
        protected virtual void OnFocus(CefView view) { }


        private void on_blur(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_menu_button_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            OnBlur(m_view);
        }

        /// <summary>Called when |view| loses focus.</summary>
        protected virtual void OnBlur(CefView view) { }


        private void on_theme_changed(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_menu_button_delegate_t*)self);
            var m_view = CefView.FromNativeOrNull(view);
            OnThemeChanged(m_view);
        }

        /// <summary>
        /// Called when the theme for |view| has changed, after the new theme colors
        /// have already been applied. Optionally override per-View theme colors here
        /// (CefView.SetBackgroundColor etc.).
        /// </summary>
        protected virtual void OnThemeChanged(CefView view) { }


        private void on_button_pressed(cef_button_delegate_t* self, cef_button_t* button)
        {
            CheckSelf((cef_menu_button_delegate_t*)self);
            OnButtonPressed(CefButton.FromNativeOrNull(button));
        }

        /// <summary>Called when |button| is pressed.</summary>
        protected virtual void OnButtonPressed(CefButton button) { }


        private void on_button_state_changed(cef_button_delegate_t* self, cef_button_t* button)
        {
            CheckSelf((cef_menu_button_delegate_t*)self);
            OnButtonStateChanged(CefButton.FromNativeOrNull(button));
        }

        /// <summary>Called when the state of |button| changes.</summary>
        protected virtual void OnButtonStateChanged(CefButton button) { }


        private void on_menu_button_pressed(cef_menu_button_delegate_t* self, cef_menu_button_t* menu_button, cef_point_t* screen_point, cef_menu_button_pressed_lock_t* button_pressed_lock)
        {
            CheckSelf(self);
            var m_point = new CefPoint(screen_point->x, screen_point->y);
            OnMenuButtonPressed(CefMenuButton.FromNativeOrNull(menu_button), m_point, CefMenuButtonPressedLock.FromNative(button_pressed_lock));
        }

        /// <summary>
        /// Called when |button| is pressed. Call CefMenuButton.ShowMenu() to show a
        /// popup menu at |screenPoint|. When showing a custom popup such as a window
        /// keep a reference to |buttonPressedLock| until the popup is hidden to
        /// maintain the pressed button state.
        /// </summary>
        protected virtual void OnMenuButtonPressed(CefMenuButton menuButton, CefPoint screenPoint, CefMenuButtonPressedLock buttonPressedLock) { }
    }
}
