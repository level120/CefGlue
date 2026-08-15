namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// A View hosting a CefBrowser instance. Methods must be called on the browser
    /// process UI thread unless otherwise indicated. (Views — CEF 146.)
    /// </summary>
    public sealed unsafe partial class CefBrowserView
    {
        /// <summary>
        /// Create a new BrowserView. The underlying CefBrowser will not be created
        /// until this view is added to the views hierarchy. The optional |extraInfo|
        /// parameter provides an opportunity to specify extra information specific
        /// to the created browser that will be passed to
        /// CefRenderProcessHandler.OnBrowserCreated() in the render process.
        /// </summary>
        public static CefBrowserView Create(CefClient client, string url, CefBrowserSettings settings, CefDictionaryValue extraInfo, CefRequestContext requestContext, CefBrowserViewDelegate @delegate)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            var n_client = client != null ? client.ToNative() : null;
            var n_settings = settings.ToNative();
            var n_extraInfo = extraInfo != null ? extraInfo.ToNative() : null;
            var n_requestContext = requestContext != null ? requestContext.ToNative() : null;
            var n_delegate = @delegate != null ? @delegate.ToNative() : null;

            fixed (char* url_ptr = url)
            {
                var n_url = new cef_string_t(url_ptr, url != null ? url.Length : 0);
                var n_result = cef_browser_view_t.create(n_client, &n_url, n_settings, n_extraInfo, n_requestContext, n_delegate);
                return CefBrowserView.FromNativeOrNull(n_result);
            }
        }

        /// <summary>
        /// Returns the BrowserView associated with |browser|.
        /// </summary>
        public static CefBrowserView GetForBrowser(CefBrowser browser)
        {
            if (browser == null) throw new ArgumentNullException(nameof(browser));
            return CefBrowserView.FromNativeOrNull(cef_browser_view_t.get_for_browser(browser.ToNative()));
        }

        private cef_browser_view_t* _bvSelf => (cef_browser_view_t*)_self;

        /// <summary>
        /// Returns the CefBrowser hosted by this BrowserView. Will return null if the
        /// browser has not yet been created or has already been destroyed.
        /// </summary>
        public CefBrowser GetBrowser()
        {
            return CefBrowser.FromNativeOrNull(cef_browser_view_t.get_browser(_bvSelf));
        }

        /// <summary>
        /// Returns the Chrome toolbar associated with this BrowserView. Only
        /// supported when using Chrome style. The CefBrowserViewDelegate.
        /// GetChromeToolbarType() method must return a value other than
        /// CefChromeToolbarType.None for the toolbar to be available.
        /// </summary>
        public CefView GetChromeToolbar()
        {
            return CefView.FromNativeOrNull(cef_browser_view_t.get_chrome_toolbar(_bvSelf));
        }

        /// <summary>
        /// Sets whether normal priority accelerators are first forwarded to the web
        /// content (`keydown` event handler) or CefKeyboardHandler. Normal priority
        /// accelerators can be registered via CefWindow.SetAccelerator (with
        /// |highPriority|=false) or internally for standard accelerators supported
        /// by Chrome style. If |preferAccelerators| is true then the matching
        /// accelerator will be triggered immediately (calling
        /// CefWindowDelegate.OnAccelerator or CefCommandHandler.OnChromeCommand
        /// respectively) and the event will not be forwarded to the web content or
        /// CefKeyboardHandler first. If |preferAccelerators| is false then the
        /// matching accelerator will only be triggered if the event is not handled
        /// by web content (`keydown` event handler that calls `event.preventDefault()`)
        /// or by CefKeyboardHandler. The default value is false.
        /// </summary>
        public void SetPreferAccelerators(bool preferAccelerators)
        {
            cef_browser_view_t.set_prefer_accelerators(_bvSelf, preferAccelerators ? 1 : 0);
        }

        /// <summary>
        /// Returns the runtime style for this BrowserView (ALLOY or CHROME). See
        /// CefRuntimeStyle documentation for details.
        /// </summary>
        public CefRuntimeStyle RuntimeStyle
        {
            get { return cef_browser_view_t.get_runtime_style(_bvSelf); }
        }
    }
}
