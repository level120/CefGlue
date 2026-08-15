namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// Controller for an overlay that contains a contents View added via
    /// CefWindow.AddOverlayView. Methods exposed by this controller should be
    /// called in preference to methods of the same name exposed by the contents
    /// View unless otherwise indicated. Methods must be called on the browser
    /// process UI thread unless otherwise indicated. (Views — CEF 146.)
    /// </summary>
    public sealed unsafe partial class CefOverlayController
    {
        /// <summary>
        /// Returns true if this object is valid.
        /// </summary>
        public bool IsValid
        {
            get { return cef_overlay_controller_t.is_valid(_self) != 0; }
        }

        /// <summary>
        /// Returns true if this object is the same as |that| object.
        /// </summary>
        public bool IsSame(CefOverlayController that)
        {
            if (that == null) return false;
            return cef_overlay_controller_t.is_same(_self, that.ToNative()) != 0;
        }

        /// <summary>
        /// Returns the contents View for this overlay.
        /// </summary>
        public CefView GetContentsView()
        {
            return CefView.FromNativeOrNull(cef_overlay_controller_t.get_contents_view(_self));
        }

        /// <summary>
        /// Returns the top-level Window hosting this overlay. Use this method instead
        /// of calling GetWindow() on the contents View.
        /// </summary>
        public CefWindow GetWindow()
        {
            return CefWindow.FromNativeOrNull(cef_overlay_controller_t.get_window(_self));
        }

        /// <summary>
        /// Returns the docking mode for this overlay.
        /// </summary>
        public CefDockingMode DockingMode
        {
            get { return cef_overlay_controller_t.get_docking_mode(_self); }
        }

        /// <summary>
        /// Destroy this overlay.
        /// </summary>
        public void Destroy()
        {
            cef_overlay_controller_t.destroy(_self);
        }

        /// <summary>
        /// Sets the bounds (size and position) of this overlay. This will set the
        /// bounds of the contents View to match and trigger a re-layout if
        /// necessary. |bounds| is in parent coordinates and any insets configured on
        /// this overlay will be ignored. Use this method only for overlays created
        /// with a docking mode value of CefDockingMode.Custom. With other docking
        /// modes modify the insets of this overlay and/or layout of the contents
        /// View and call SizeToPreferredSize() instead to calculate the new size and
        /// re-position the overlay if necessary.
        /// </summary>
        public void SetBounds(CefRectangle bounds)
        {
            var n = new cef_rect_t(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            cef_overlay_controller_t.set_bounds(_self, &n);
        }

        /// <summary>
        /// Returns the bounds (size and position) of this overlay in parent
        /// coordinates.
        /// </summary>
        public CefRectangle GetBounds()
        {
            var n = cef_overlay_controller_t.get_bounds(_self);
            return new CefRectangle(n.x, n.y, n.width, n.height);
        }

        /// <summary>
        /// Returns the bounds (size and position) of this overlay in DIP screen
        /// coordinates.
        /// </summary>
        public CefRectangle GetBoundsInScreen()
        {
            var n = cef_overlay_controller_t.get_bounds_in_screen(_self);
            return new CefRectangle(n.x, n.y, n.width, n.height);
        }

        /// <summary>
        /// Sets the size of this overlay without changing the position. See
        /// SetBounds() documentation for details.
        /// </summary>
        public void SetSize(CefSize size)
        {
            var n = new cef_size_t(size.Width, size.Height);
            cef_overlay_controller_t.set_size(_self, &n);
        }

        /// <summary>
        /// Returns the size of this overlay in parent coordinates.
        /// </summary>
        public CefSize GetSize()
        {
            var n = cef_overlay_controller_t.get_size(_self);
            return new CefSize(n.width, n.height);
        }

        /// <summary>
        /// Sets the position of this overlay without changing the size. See
        /// SetBounds() documentation for details.
        /// </summary>
        public void SetPosition(CefPoint position)
        {
            var n = new cef_point_t(position.X, position.Y);
            cef_overlay_controller_t.set_position(_self, &n);
        }

        /// <summary>
        /// Returns the position of this overlay in parent coordinates.
        /// </summary>
        public CefPoint GetPosition()
        {
            var n = cef_overlay_controller_t.get_position(_self);
            return new CefPoint(n.x, n.y);
        }

        /// <summary>
        /// Sets the insets for this overlay. |insets| is in parent coordinates. Use
        /// this method only for overlays created with a docking mode value other
        /// than CefDockingMode.Custom.
        /// </summary>
        public void SetInsets(CefInsets insets)
        {
            var n = new cef_insets_t(insets.Top, insets.Left, insets.Bottom, insets.Right);
            cef_overlay_controller_t.set_insets(_self, &n);
        }

        /// <summary>
        /// Returns the insets for this overlay in parent coordinates.
        /// </summary>
        public CefInsets GetInsets()
        {
            var n = cef_overlay_controller_t.get_insets(_self);
            return new CefInsets(n.top, n.left, n.bottom, n.right);
        }

        /// <summary>
        /// Size this overlay to its preferred size and trigger a re-layout if
        /// necessary. The position of overlays created with a docking mode value of
        /// CefDockingMode.Custom will not be modified by calling this method. With
        /// other docking modes this method may re-position the overlay if necessary
        /// to accommodate the new size and any insets configured on the contents
        /// View.
        /// </summary>
        public void SizeToPreferredSize()
        {
            cef_overlay_controller_t.size_to_preferred_size(_self);
        }

        /// <summary>
        /// Sets whether this overlay is visible. Overlays are hidden by default. If
        /// this overlay is hidden then it and any child Views will not be drawn and,
        /// if any of those Views currently have focus, then focus will also be
        /// cleared. Painting is scheduled as needed.
        /// </summary>
        public void SetVisible(bool visible)
        {
            cef_overlay_controller_t.set_visible(_self, visible ? 1 : 0);
        }

        /// <summary>
        /// Returns whether this overlay is visible. A View may be visible but still
        /// not drawn in a Window if any parent Views are hidden. Call IsDrawn() to
        /// determine whether this overlay and all parent Views are visible and will
        /// be drawn.
        /// </summary>
        public bool IsVisible
        {
            get { return cef_overlay_controller_t.is_visible(_self) != 0; }
        }

        /// <summary>
        /// Returns whether this overlay is visible and drawn in a Window. A View is
        /// drawn if it and all parent Views are visible. To determine if the
        /// containing Window is visible to the user on-screen call IsVisible() on
        /// the Window.
        /// </summary>
        public bool IsDrawn
        {
            get { return cef_overlay_controller_t.is_drawn(_self) != 0; }
        }
    }
}
