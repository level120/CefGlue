namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// A Panel is a container in the views hierarchy that can contain other Views
    /// as children. Methods must be called on the browser process UI thread unless
    /// otherwise indicated. (Views — CEF 146.)
    /// </summary>
    public unsafe partial class CefPanel
    {
        /// <summary>
        /// Create a new Panel.
        /// </summary>
        public static CefPanel CreatePanel(CefPanelDelegate @delegate = null)
        {
            return CefPanel.FromNative(
                cef_panel_t.create(@delegate != null ? @delegate.ToNative() : null)
                );
        }

        private cef_panel_t* _panelSelf => (cef_panel_t*)_self;

        /// <summary>
        /// Returns this Panel as a Window or null if this is not a Window.
        /// </summary>
        public CefWindow AsWindow()
        {
            return CefWindow.FromNativeOrNull(cef_panel_t.as_window(_panelSelf));
        }

        /// <summary>
        /// Set this Panel's Layout to FillLayout and return the FillLayout object.
        /// </summary>
        public CefFillLayout SetToFillLayout()
        {
            return CefFillLayout.FromNative(cef_panel_t.set_to_fill_layout(_panelSelf));
        }

        /// <summary>
        /// Set this Panel's Layout to BoxLayout and return the BoxLayout object.
        /// </summary>
        public CefBoxLayout SetToBoxLayout(CefBoxLayoutSettings settings)
        {
            var n_settings = settings.ToNative();
            var result = cef_panel_t.set_to_box_layout(_panelSelf, &n_settings);
            return CefBoxLayout.FromNative(result);
        }

        /// <summary>
        /// Get the Layout.
        /// </summary>
        public CefLayout GetLayout()
        {
            return CefLayout.FromNativeOrNull(cef_panel_t.get_layout(_panelSelf));
        }

        /// <summary>
        /// Lay out the child Views (set their bounds based on sizing heuristics
        /// specific to the current Layout).
        /// </summary>
        public void Layout()
        {
            cef_panel_t.layout(_panelSelf);
        }

        /// <summary>
        /// Add a child View.
        /// </summary>
        public void AddChildView(CefView view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            cef_panel_t.add_child_view(_panelSelf, view.ToNative());
        }

        /// <summary>
        /// Add a child View at the specified |index|. If |index| matches the result
        /// of GetChildCount() then the View will be added at the end.
        /// </summary>
        public void AddChildViewAt(CefView view, int index)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            cef_panel_t.add_child_view_at(_panelSelf, view.ToNative(), index);
        }

        /// <summary>
        /// Move the child View to the specified |index|. A negative value for |index|
        /// will move the View to the end.
        /// </summary>
        public void ReorderChildView(CefView view, int index)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            cef_panel_t.reorder_child_view(_panelSelf, view.ToNative(), index);
        }

        /// <summary>
        /// Remove a child View. The View can then be added to another Panel.
        /// </summary>
        public void RemoveChildView(CefView view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            cef_panel_t.remove_child_view(_panelSelf, view.ToNative());
        }

        /// <summary>
        /// Remove all child Views. The removed Views will be deleted if the client
        /// holds no references to them.
        /// </summary>
        public void RemoveAllChildViews()
        {
            cef_panel_t.remove_all_child_views(_panelSelf);
        }

        /// <summary>
        /// Returns the number of child Views.
        /// </summary>
        public int ChildViewCount
        {
            get { return (int)cef_panel_t.get_child_view_count(_panelSelf); }
        }

        /// <summary>
        /// Returns the child View at the specified |index|.
        /// </summary>
        public CefView GetChildViewAt(int index)
        {
            return CefView.FromNativeOrNull(cef_panel_t.get_child_view_at(_panelSelf, index));
        }
    }
}
