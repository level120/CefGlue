namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// A View is a rectangle within the views View hierarchy. It is the base class
    /// for all Views. All size and position values are in density independent
    /// pixels (DIP) unless otherwise indicated. Methods must be called on the
    /// browser process UI thread unless otherwise indicated.
    /// (Views — CEF 146. Root of the proxy inheritance chain: CefPanel/CefWindow,
    /// CefBrowserView, CefButton/…, CefScrollView, CefTextfield derive from this.)
    /// </summary>
    public unsafe partial class CefView
    {
        /// <summary>
        /// Returns this View as a BrowserView or null if this is not a BrowserView.
        /// </summary>
        public CefBrowserView AsBrowserView()
        {
            return CefBrowserView.FromNativeOrNull(cef_view_t.as_browser_view(_self));
        }

        /// <summary>
        /// Returns this View as a Button or null if this is not a Button.
        /// </summary>
        public CefButton AsButton()
        {
            return CefButton.FromNativeOrNull(cef_view_t.as_button(_self));
        }

        /// <summary>
        /// Returns this View as a Panel or null if this is not a Panel.
        /// </summary>
        public CefPanel AsPanel()
        {
            return CefPanel.FromNativeOrNull(cef_view_t.as_panel(_self));
        }

        /// <summary>
        /// Returns this View as a ScrollView or null if this is not a ScrollView.
        /// </summary>
        public CefScrollView AsScrollView()
        {
            return CefScrollView.FromNativeOrNull(cef_view_t.as_scroll_view(_self));
        }

        /// <summary>
        /// Returns this View as a Textfield or null if this is not a Textfield.
        /// </summary>
        public CefTextfield AsTextfield()
        {
            return CefTextfield.FromNativeOrNull(cef_view_t.as_textfield(_self));
        }

        /// <summary>
        /// Returns the type of this View as a string. Used primarily for testing
        /// purposes.
        /// </summary>
        public string TypeString
        {
            get
            {
                var n_result = cef_view_t.get_type_string(_self);
                return cef_string_userfree.ToString(n_result);
            }
        }

        /// <summary>
        /// Returns a string representation of this View which includes the type and
        /// various type-specific identifying attributes. If |includeChildren| is
        /// true any child Views will also be included. Used primarily for testing
        /// purposes.
        /// </summary>
        public string ToString(bool includeChildren)
        {
            var n_result = cef_view_t.to_string(_self, includeChildren ? 1 : 0);
            return cef_string_userfree.ToString(n_result);
        }

        /// <summary>
        /// Returns true if this View is valid.
        /// </summary>
        public bool IsValid
        {
            get { return cef_view_t.is_valid(_self) != 0; }
        }

        /// <summary>
        /// Returns true if this View is currently attached to another View. A View
        /// can only be attached to one View at a time.
        /// </summary>
        public bool IsAttached
        {
            get { return cef_view_t.is_attached(_self) != 0; }
        }

        /// <summary>
        /// Returns true if this View is the same as |that| View.
        /// </summary>
        public bool IsSame(CefView that)
        {
            if (that == null) return false;
            return cef_view_t.is_same(_self, that.ToNative()) != 0;
        }

        // GetDelegate() is intentionally not exposed: delegate handlers are flattened per
        // concrete class (no C# inheritance), so the returned cef_view_delegate_t* cannot be
        // mapped back to the managed instance generically. Keep your own reference instead.

        /// <summary>
        /// Returns the top-level Window hosting this View, if any.
        /// </summary>
        public CefWindow GetWindow()
        {
            return CefWindow.FromNativeOrNull(cef_view_t.get_window(_self));
        }

        /// <summary>
        /// Returns the ID for this View. Should be unique within the containing
        /// Window. Set via SetID().
        /// </summary>
        public int ID
        {
            get { return cef_view_t.get_id(_self); }
            set { cef_view_t.set_id(_self, value); }
        }

        /// <summary>
        /// Returns the group id of this View, or -1 if not set. Set via SetGroupID().
        /// Views with the same group id are considered part of the same logical
        /// group (used for example by radio buttons).
        /// </summary>
        public int GroupID
        {
            get { return cef_view_t.get_group_id(_self); }
            set { cef_view_t.set_group_id(_self, value); }
        }

        /// <summary>
        /// Returns the View that contains this View, if any.
        /// </summary>
        public CefView GetParentView()
        {
            return CefView.FromNativeOrNull(cef_view_t.get_parent_view(_self));
        }

        /// <summary>
        /// Recursively descends the view tree starting at this View, and returns the
        /// first child that it encounters with the given ID. Returns null if no
        /// matching child view is found.
        /// </summary>
        public CefView GetViewForID(int id)
        {
            return CefView.FromNativeOrNull(cef_view_t.get_view_for_id(_self, id));
        }

        /// <summary>
        /// Sets the bounds (size and position) of this View. |bounds| is in parent
        /// coordinates, or DIP screen coordinates if there is no parent.
        /// </summary>
        public void SetBounds(CefRectangle bounds)
        {
            var n_bounds = new cef_rect_t(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            cef_view_t.set_bounds(_self, &n_bounds);
        }

        /// <summary>
        /// Returns the bounds (size and position) of this View in parent coordinates,
        /// or DIP screen coordinates if there is no parent.
        /// </summary>
        public CefRectangle GetBounds()
        {
            var n = cef_view_t.get_bounds(_self);
            return new CefRectangle(n.x, n.y, n.width, n.height);
        }

        /// <summary>
        /// Returns the bounds (size and position) of this View in DIP screen
        /// coordinates.
        /// </summary>
        public CefRectangle GetBoundsInScreen()
        {
            var n = cef_view_t.get_bounds_in_screen(_self);
            return new CefRectangle(n.x, n.y, n.width, n.height);
        }

        /// <summary>
        /// Sets the size of this View without changing the position. |size| in
        /// parent coordinates, or DIP screen coordinates if there is no parent.
        /// </summary>
        public void SetSize(CefSize size)
        {
            var n_size = new cef_size_t(size.Width, size.Height);
            cef_view_t.set_size(_self, &n_size);
        }

        /// <summary>
        /// Returns the size of this View in parent coordinates, or DIP screen
        /// coordinates if there is no parent.
        /// </summary>
        public CefSize GetSize()
        {
            var n = cef_view_t.get_size(_self);
            return new CefSize(n.width, n.height);
        }

        /// <summary>
        /// Sets the position of this View without changing the size. |position| is
        /// in parent coordinates, or DIP screen coordinates if there is no parent.
        /// </summary>
        public void SetPosition(CefPoint position)
        {
            var n_position = new cef_point_t(position.X, position.Y);
            cef_view_t.set_position(_self, &n_position);
        }

        /// <summary>
        /// Returns the position of this View. Position is in parent coordinates, or
        /// DIP screen coordinates if there is no parent.
        /// </summary>
        public CefPoint GetPosition()
        {
            var n = cef_view_t.get_position(_self);
            return new CefPoint(n.x, n.y);
        }

        /// <summary>
        /// Sets the insets for this View. |insets| is in parent coordinates, or DIP
        /// screen coordinates if there is no parent.
        /// </summary>
        public void SetInsets(CefInsets insets)
        {
            var n_insets = new cef_insets_t(insets.Top, insets.Left, insets.Bottom, insets.Right);
            cef_view_t.set_insets(_self, &n_insets);
        }

        /// <summary>
        /// Returns the insets for this View in parent coordinates, or DIP screen
        /// coordinates if there is no parent.
        /// </summary>
        public CefInsets GetInsets()
        {
            var n = cef_view_t.get_insets(_self);
            return new CefInsets(n.top, n.left, n.bottom, n.right);
        }

        /// <summary>
        /// Returns the size this View would like to be if enough space is available.
        /// Size is in parent coordinates, or DIP screen coordinates if there is no
        /// parent.
        /// </summary>
        public CefSize GetPreferredSize()
        {
            var n = cef_view_t.get_preferred_size(_self);
            return new CefSize(n.width, n.height);
        }

        /// <summary>
        /// Size this View to its preferred size. Size is in parent coordinates, or
        /// DIP screen coordinates if there is no parent.
        /// </summary>
        public void SizeToPreferredSize()
        {
            cef_view_t.size_to_preferred_size(_self);
        }

        /// <summary>
        /// Returns the minimum size for this View. Size is in parent coordinates, or
        /// DIP screen coordinates if there is no parent.
        /// </summary>
        public CefSize GetMinimumSize()
        {
            var n = cef_view_t.get_minimum_size(_self);
            return new CefSize(n.width, n.height);
        }

        /// <summary>
        /// Returns the maximum size for this View. Size is in parent coordinates, or
        /// DIP screen coordinates if there is no parent.
        /// </summary>
        public CefSize GetMaximumSize()
        {
            var n = cef_view_t.get_maximum_size(_self);
            return new CefSize(n.width, n.height);
        }

        /// <summary>
        /// Returns the height necessary to display this View with the provided width.
        /// </summary>
        public int GetHeightForWidth(int width)
        {
            return cef_view_t.get_height_for_width(_self, width);
        }

        /// <summary>
        /// Indicate that this View and all parent Views require a re-layout. This
        /// ensures the next call to Layout() will propagate to this View even if the
        /// bounds of parent Views do not change.
        /// </summary>
        public void InvalidateLayout()
        {
            cef_view_t.invalidate_layout(_self);
        }

        /// <summary>
        /// Sets whether this View is visible. Windows are hidden by default and other
        /// views are visible by default. This View and any parent views must be set
        /// as visible for this View to be drawn in a Window. If this View is set as
        /// hidden then it and any child views will not be drawn and, if any of those
        /// views currently have focus, then focus will also be cleared. Painting is
        /// scheduled as needed. If this View is a Window then calling this method is
        /// equivalent to calling the Window Show() and Hide() methods.
        /// </summary>
        public void SetVisible(bool visible)
        {
            cef_view_t.set_visible(_self, visible ? 1 : 0);
        }

        /// <summary>
        /// Returns whether this View is visible. A view may be visible but still not
        /// drawn in a Window if any parent views are hidden. If this View is a Window
        /// then a return value of true indicates that this Window is currently
        /// visible to the user on-screen. If this View is not a Window then call
        /// IsDrawn() to determine whether this View and all parent views are visible
        /// and will be drawn.
        /// </summary>
        public bool IsVisible
        {
            get { return cef_view_t.is_visible(_self) != 0; }
        }

        /// <summary>
        /// Returns whether this View is visible and drawn in a Window. A view is
        /// drawn if it and all parent views are visible. If this View is a Window
        /// then calling this method is equivalent to calling IsVisible(). Otherwise,
        /// to determine if the containing Window is visible to the user on-screen
        /// call IsVisible() on the Window.
        /// </summary>
        public bool IsDrawn
        {
            get { return cef_view_t.is_drawn(_self) != 0; }
        }

        /// <summary>
        /// Set whether this View is enabled. A disabled View does not receive
        /// keyboard or mouse inputs. If |enabled| differs from the current value the
        /// View will be repainted. Also, clears focus if the focused View is
        /// disabled.
        /// </summary>
        public void SetEnabled(bool enabled)
        {
            cef_view_t.set_enabled(_self, enabled ? 1 : 0);
        }

        /// <summary>
        /// Returns whether this View is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get { return cef_view_t.is_enabled(_self) != 0; }
        }

        /// <summary>
        /// Sets whether this View is capable of taking focus. It will clear focus if
        /// the focused View is set to be non-focusable. This is false by default so
        /// that focus doesn't cycle into unwanted places.
        /// </summary>
        public void SetFocusable(bool focusable)
        {
            cef_view_t.set_focusable(_self, focusable ? 1 : 0);
        }

        /// <summary>
        /// Returns true if this View is focusable, enabled and drawn.
        /// </summary>
        public bool IsFocusable
        {
            get { return cef_view_t.is_focusable(_self) != 0; }
        }

        /// <summary>
        /// Return whether this View is focusable when the user requires full keyboard
        /// access, even though it may not be normally focusable.
        /// </summary>
        public bool IsAccessibilityFocusable
        {
            get { return cef_view_t.is_accessibility_focusable(_self) != 0; }
        }

        /// <summary>
        /// Returns true if this View has focus.
        /// </summary>
        public bool HasFocus
        {
            get { return cef_view_t.has_focus(_self) != 0; }
        }

        /// <summary>
        /// Request keyboard focus. If this View is focusable it will become the
        /// focused View.
        /// </summary>
        public void RequestFocus()
        {
            cef_view_t.request_focus(_self);
        }

        /// <summary>
        /// Sets the background color for this View. The background color will be
        /// automatically reset when CefViewDelegate.OnThemeChanged is called.
        /// </summary>
        public void SetBackgroundColor(CefColor color)
        {
            cef_view_t.set_background_color(_self, color.ToArgb());
        }

        /// <summary>
        /// Returns the background color for this View. If the background color is
        /// unset then the current GetThemeColor(CEF_ColorPrimaryBackground) value
        /// will be returned.
        /// </summary>
        public CefColor GetBackgroundColor()
        {
            return new CefColor(cef_view_t.get_background_color(_self));
        }

        /// <summary>
        /// Returns the current theme color associated with |colorId|, or the
        /// placeholder color (red) if unset. See cef_color_ids.h for standard ID
        /// values. Standard colors can be overridden and custom colors can be added
        /// using CefWindow.SetThemeColor.
        /// </summary>
        public CefColor GetThemeColor(int colorId)
        {
            return new CefColor(cef_view_t.get_theme_color(_self, colorId));
        }

        /// <summary>
        /// Convert |point| from this View's coordinate system to DIP screen
        /// coordinates. This View must belong to a Window when calling this method.
        /// Returns true if the conversion is successful or false otherwise. Use
        /// CefDisplay.ConvertPointToPixels() after calling this method if further
        /// conversion to display-specific pixel coordinates is desired.
        /// </summary>
        public bool ConvertPointToScreen(ref CefPoint point)
        {
            var n = new cef_point_t(point.X, point.Y);
            var ok = cef_view_t.convert_point_to_screen(_self, &n) != 0;
            if (ok) point = new CefPoint(n.x, n.y);
            return ok;
        }

        /// <summary>
        /// Convert |point| to this View's coordinate system from DIP screen
        /// coordinates. This View must belong to a Window when calling this method.
        /// Returns true if the conversion is successful or false otherwise. Use
        /// CefDisplay.ConvertPointFromPixels() before calling this method if
        /// conversion from display-specific pixel coordinates is necessary.
        /// </summary>
        public bool ConvertPointFromScreen(ref CefPoint point)
        {
            var n = new cef_point_t(point.X, point.Y);
            var ok = cef_view_t.convert_point_from_screen(_self, &n) != 0;
            if (ok) point = new CefPoint(n.x, n.y);
            return ok;
        }

        /// <summary>
        /// Convert |point| from this View's coordinate system to that of the Window.
        /// This View must belong to a Window when calling this method. Returns true
        /// if the conversion is successful or false otherwise.
        /// </summary>
        public bool ConvertPointToWindow(ref CefPoint point)
        {
            var n = new cef_point_t(point.X, point.Y);
            var ok = cef_view_t.convert_point_to_window(_self, &n) != 0;
            if (ok) point = new CefPoint(n.x, n.y);
            return ok;
        }

        /// <summary>
        /// Convert |point| to this View's coordinate system from that of the Window.
        /// This View must belong to a Window when calling this method. Returns true
        /// if the conversion is successful or false otherwise.
        /// </summary>
        public bool ConvertPointFromWindow(ref CefPoint point)
        {
            var n = new cef_point_t(point.X, point.Y);
            var ok = cef_view_t.convert_point_from_window(_self, &n) != 0;
            if (ok) point = new CefPoint(n.x, n.y);
            return ok;
        }

        /// <summary>
        /// Convert |point| from this View's coordinate system to that of |view|.
        /// |view| needs to be in the same Window but not necessarily the same view
        /// hierarchy. Returns true if the conversion is successful or false
        /// otherwise.
        /// </summary>
        public bool ConvertPointToView(CefView view, ref CefPoint point)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            var n = new cef_point_t(point.X, point.Y);
            var ok = cef_view_t.convert_point_to_view(_self, view.ToNative(), &n) != 0;
            if (ok) point = new CefPoint(n.x, n.y);
            return ok;
        }

        /// <summary>
        /// Convert |point| to this View's coordinate system from that |view|. |view|
        /// needs to be in the same Window but not necessarily the same view
        /// hierarchy. Returns true if the conversion is successful or false
        /// otherwise.
        /// </summary>
        public bool ConvertPointFromView(CefView view, ref CefPoint point)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            var n = new cef_point_t(point.X, point.Y);
            var ok = cef_view_t.convert_point_from_view(_self, view.ToNative(), &n) != 0;
            if (ok) point = new CefPoint(n.x, n.y);
            return ok;
        }
    }
}
