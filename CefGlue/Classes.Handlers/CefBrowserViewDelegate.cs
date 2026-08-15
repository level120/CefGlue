namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// Implement this interface to handle BrowserView events.
    /// The methods of this class will be called on the browser process UI thread
    /// unless otherwise indicated.
    /// (Views — CEF 146. Delegate inheritance chain is flattened per class: this
    /// class carries the CefViewDelegate slots directly.)
    /// </summary>
    public abstract unsafe partial class CefBrowserViewDelegate
    {
        private cef_size_t get_preferred_size(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_browser_view_delegate_t*)self);
            var m_view = CefView.FromNative(view);
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
            CheckSelf((cef_browser_view_delegate_t*)self);
            var m_view = CefView.FromNative(view);
            var size = GetMinimumSize(m_view);
            return new cef_size_t(size.Width, size.Height);
        }

        /// <summary>Return the minimum size for |view|.</summary>
        protected virtual CefSize GetMinimumSize(CefView view) => new CefSize(0, 0);


        private cef_size_t get_maximum_size(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_browser_view_delegate_t*)self);
            var m_view = CefView.FromNative(view);
            var size = GetMaximumSize(m_view);
            return new cef_size_t(size.Width, size.Height);
        }

        /// <summary>Return the maximum size for |view|.</summary>
        protected virtual CefSize GetMaximumSize(CefView view) => new CefSize(0, 0);


        private int get_height_for_width(cef_view_delegate_t* self, cef_view_t* view, int width)
        {
            CheckSelf((cef_browser_view_delegate_t*)self);
            var m_view = CefView.FromNative(view);
            return GetHeightForWidth(m_view, width);
        }

        /// <summary>
        /// Return the height necessary to display |view| with the provided |width|.
        /// If not specified (0) the result of GetPreferredSize().Height will be used.
        /// </summary>
        protected virtual int GetHeightForWidth(CefView view, int width) => 0;


        private void on_parent_view_changed(cef_view_delegate_t* self, cef_view_t* view, int added, cef_view_t* parent)
        {
            CheckSelf((cef_browser_view_delegate_t*)self);
            var m_view = CefView.FromNative(view);
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
            CheckSelf((cef_browser_view_delegate_t*)self);
            var m_view = CefView.FromNative(view);
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
            CheckSelf((cef_browser_view_delegate_t*)self);
            var m_view = CefView.FromNative(view);
            OnWindowChanged(m_view, added != 0);
        }

        /// <summary>Called when |view| is added or removed from the CefWindow.</summary>
        protected virtual void OnWindowChanged(CefView view, bool added) { }


        private void on_layout_changed(cef_view_delegate_t* self, cef_view_t* view, cef_rect_t* new_bounds)
        {
            CheckSelf((cef_browser_view_delegate_t*)self);
            var m_view = CefView.FromNative(view);
            var m_bounds = new CefRectangle(new_bounds->x, new_bounds->y, new_bounds->width, new_bounds->height);
            OnLayoutChanged(m_view, m_bounds);
        }

        /// <summary>Called when the layout of |view| has changed.</summary>
        protected virtual void OnLayoutChanged(CefView view, CefRectangle newBounds) { }


        private void on_focus(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_browser_view_delegate_t*)self);
            var m_view = CefView.FromNative(view);
            OnFocus(m_view);
        }

        /// <summary>Called when |view| gains focus.</summary>
        protected virtual void OnFocus(CefView view) { }


        private void on_blur(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_browser_view_delegate_t*)self);
            var m_view = CefView.FromNative(view);
            OnBlur(m_view);
        }

        /// <summary>Called when |view| loses focus.</summary>
        protected virtual void OnBlur(CefView view) { }


        private void on_theme_changed(cef_view_delegate_t* self, cef_view_t* view)
        {
            CheckSelf((cef_browser_view_delegate_t*)self);
            var m_view = CefView.FromNative(view);
            OnThemeChanged(m_view);
        }

        /// <summary>
        /// Called when the theme for |view| has changed, after the new theme colors
        /// have already been applied. Optionally override per-View theme colors here
        /// (CefView.SetBackgroundColor etc.).
        /// </summary>
        protected virtual void OnThemeChanged(CefView view) { }


        private void on_browser_created(cef_browser_view_delegate_t* self, cef_browser_view_t* browser_view, cef_browser_t* browser)
        {
            CheckSelf(self);
            OnBrowserCreated(CefBrowserView.FromNative(browser_view), CefBrowser.FromNative(browser));
        }

        /// <summary>
        /// Called when |browser| associated with |browserView| is created. This
        /// method will be called after CefLifeSpanHandler.OnAfterCreated() is called
        /// for |browser| and before OnPopupBrowserViewCreated() is called for
        /// |browser|'s parent delegate if |browser| is a popup.
        /// </summary>
        protected virtual void OnBrowserCreated(CefBrowserView browserView, CefBrowser browser) { }


        private void on_browser_destroyed(cef_browser_view_delegate_t* self, cef_browser_view_t* browser_view, cef_browser_t* browser)
        {
            CheckSelf(self);
            OnBrowserDestroyed(CefBrowserView.FromNative(browser_view), CefBrowser.FromNative(browser));
        }

        /// <summary>
        /// Called when |browser| associated with |browserView| is destroyed. Release
        /// all references to |browser| and do not attempt to execute any methods on
        /// |browser| after this callback returns. This method will be called before
        /// CefLifeSpanHandler.OnBeforeClose() is called for |browser|.
        /// </summary>
        protected virtual void OnBrowserDestroyed(CefBrowserView browserView, CefBrowser browser) { }


        private cef_browser_view_delegate_t* get_delegate_for_popup_browser_view(cef_browser_view_delegate_t* self, cef_browser_view_t* browser_view, cef_browser_settings_t* settings, cef_client_t* client, int is_devtools)
        {
            CheckSelf(self);
            var m_settings = new CefBrowserSettings(settings);
            var m_client = CefClient.FromNativeOrNull(client);
            var result = GetDelegateForPopupBrowserView(CefBrowserView.FromNative(browser_view), m_settings, m_client, is_devtools != 0);
            m_settings.Dispose();
            return result != null ? result.ToNative() : null;
        }

        /// <summary>
        /// Called before a new popup BrowserView is created. The popup originated
        /// from |browserView|. |settings| and |client| are the values returned from
        /// CefLifeSpanHandler.OnBeforePopup(). |isDevTools| will be true if the
        /// popup will be a DevTools browser. Return the delegate that will be used
        /// for the new popup BrowserView. Default: this.
        /// </summary>
        protected virtual CefBrowserViewDelegate GetDelegateForPopupBrowserView(CefBrowserView browserView, CefBrowserSettings settings, CefClient client, bool isDevTools) => this;


        private int on_popup_browser_view_created(cef_browser_view_delegate_t* self, cef_browser_view_t* browser_view, cef_browser_view_t* popup_browser_view, int is_devtools)
        {
            CheckSelf(self);
            return OnPopupBrowserViewCreated(CefBrowserView.FromNative(browser_view), CefBrowserView.FromNative(popup_browser_view), is_devtools != 0) ? 1 : 0;
        }

        /// <summary>
        /// Called after |popupBrowserView| is created. This method will be called
        /// after CefLifeSpanHandler.OnAfterCreated() and OnBrowserCreated() are
        /// called for the new popup browser. The popup originated from |browserView|.
        /// |isDevTools| will be true if the popup is a DevTools browser. Optionally
        /// add |popupBrowserView| to the views hierarchy yourself and return true.
        /// Otherwise return false and a default CefWindow will be created for the
        /// popup.
        /// </summary>
        protected virtual bool OnPopupBrowserViewCreated(CefBrowserView browserView, CefBrowserView popupBrowserView, bool isDevTools) => false;


        private CefChromeToolbarType get_chrome_toolbar_type(cef_browser_view_delegate_t* self, cef_browser_view_t* browser_view)
        {
            CheckSelf(self);
            return GetChromeToolbarType(CefBrowserView.FromNative(browser_view));
        }

        /// <summary>
        /// Returns the Chrome toolbar type that will be available via
        /// CefBrowserView.GetChromeToolbar(). See that method for related
        /// documentation.
        /// </summary>
        protected virtual CefChromeToolbarType GetChromeToolbarType(CefBrowserView browserView) => CefChromeToolbarType.None;


        private int use_frameless_window_for_picture_in_picture(cef_browser_view_delegate_t* self, cef_browser_view_t* browser_view)
        {
            CheckSelf(self);
            return UseFramelessWindowForPictureInPicture(CefBrowserView.FromNative(browser_view)) ? 1 : 0;
        }

        /// <summary>
        /// Return true to create frameless windows for Document picture-in-picture
        /// popups. Content in frameless windows should specify draggable regions
        /// using "-webkit-app-region: drag" CSS.
        /// </summary>
        protected virtual bool UseFramelessWindowForPictureInPicture(CefBrowserView browserView) => false;


        private int allow_move_for_picture_in_picture(cef_browser_view_delegate_t* self, cef_browser_view_t* browser_view)
        {
            CheckSelf(self);
            return AllowMoveForPictureInPicture(CefBrowserView.FromNative(browser_view)) ? 1 : 0;
        }

        /// <summary>
        /// Return true to allow the use of JavaScript moveTo/By() and resizeTo/By()
        /// (without user activation) with Document picture-in-picture popups.
        /// </summary>
        protected virtual bool AllowMoveForPictureInPicture(CefBrowserView browserView) => false;


        private int allow_picture_in_picture_without_user_activation(cef_browser_view_delegate_t* self, cef_browser_view_t* browser_view)
        {
            CheckSelf(self);
            return AllowPictureInPictureWithoutUserActivation(CefBrowserView.FromNative(browser_view)) ? 1 : 0;
        }

        /// <summary>
        /// Return true to allow opening Document picture-in-picture without user
        /// activation. Default is false (user activation required).
        /// </summary>
        protected virtual bool AllowPictureInPictureWithoutUserActivation(CefBrowserView browserView) => false;


        private int on_gesture_command(cef_browser_view_delegate_t* self, cef_browser_view_t* browser_view, CefGestureCommand gesture_command)
        {
            CheckSelf(self);
            return OnGestureCommand(CefBrowserView.FromNative(browser_view), gesture_command) ? 1 : 0;
        }

        /// <summary>
        /// Called when |browserView| receives a gesture command. Return true to
        /// handle (or disable) a |gestureCommand| or false to propagate the gesture
        /// to the browser for default handling. With Chrome style these commands can
        /// also be handled via CefCommandHandler.OnChromeCommand.
        /// </summary>
        protected virtual bool OnGestureCommand(CefBrowserView browserView, CefGestureCommand gestureCommand) => false;


        private CefRuntimeStyle get_browser_runtime_style(cef_browser_view_delegate_t* self)
        {
            CheckSelf(self);
            return GetBrowserRuntimeStyle();
        }

        /// <summary>
        /// Optionally change the runtime style for this BrowserView. See
        /// CefRuntimeStyle documentation for details.
        /// </summary>
        protected virtual CefRuntimeStyle GetBrowserRuntimeStyle() => CefRuntimeStyle.Default;
    }
}
