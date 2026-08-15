namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// This class typically, but not always, corresponds to a physical display
    /// connected to the system. A fake Display may exist on a headless system, or a
    /// Display may correspond to a remote, virtual display. All size and position
    /// values are in density independent pixel (DIP) coordinates unless otherwise
    /// indicated. Methods must be called on the browser process UI thread unless
    /// otherwise indicated. (Views — CEF 146.)
    /// </summary>
    public sealed unsafe partial class CefDisplay
    {
        /// <summary>
        /// Returns the primary Display.
        /// </summary>
        public static CefDisplay GetPrimaryDisplay()
        {
            return CefDisplay.FromNative(cef_display_t.get_primary());
        }

        /// <summary>
        /// Returns the Display nearest |point|. Set |inputPixelCoords| to true if
        /// |point| is in pixel screen coordinates instead of DIP screen coordinates.
        /// </summary>
        public static CefDisplay GetDisplayNearestPoint(CefPoint point, bool inputPixelCoords)
        {
            var n_point = new cef_point_t(point.X, point.Y);
            return CefDisplay.FromNative(cef_display_t.get(&n_point, inputPixelCoords ? 1 : 0));
        }

        /// <summary>
        /// Returns the Display that most closely intersects |bounds|. Set
        /// |inputPixelCoords| to true if |bounds| is in pixel screen coordinates
        /// instead of DIP screen coordinates.
        /// </summary>
        public static CefDisplay GetDisplayMatchingBounds(CefRectangle bounds, bool inputPixelCoords)
        {
            var n_bounds = new cef_rect_t(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            return CefDisplay.FromNative(cef_display_t.get(&n_bounds, inputPixelCoords ? 1 : 0));
        }

        /// <summary>
        /// Returns the total number of Displays. Mirrored displays are excluded; this
        /// method is intended to return the number of distinct, usable displays.
        /// </summary>
        public static int GetDisplayCount()
        {
            return (int)cef_display_t.get();
        }

        /// <summary>
        /// Returns all Displays. Mirrored displays are excluded; this method is
        /// intended to return distinct, usable displays.
        /// </summary>
        public static CefDisplay[] GetAllDisplays()
        {
            var count = (UIntPtr)GetDisplayCount();
            var n = (int)count;
            if (n == 0) return new CefDisplay[0];
            var buffer = stackalloc cef_display_t*[n];
            cef_display_t.get_all(&count, buffer);
            var result = new CefDisplay[(int)count];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = CefDisplay.FromNative(buffer[i]);
            }
            return result;
        }

        /// <summary>
        /// Convert |point| from DIP screen coordinates to pixel screen coordinates.
        /// This method is only used on Windows.
        /// </summary>
        public static CefPoint ConvertScreenPointToPixels(CefPoint point)
        {
            var n_point = new cef_point_t(point.X, point.Y);
            var r = cef_display_t.convert_screen_point_to_pixels(&n_point);
            return new CefPoint(r.x, r.y);
        }

        /// <summary>
        /// Convert |point| from pixel screen coordinates to DIP screen coordinates.
        /// This method is only used on Windows.
        /// </summary>
        public static CefPoint ConvertScreenPointFromPixels(CefPoint point)
        {
            var n_point = new cef_point_t(point.X, point.Y);
            var r = cef_display_t.convert_screen_point_from_pixels(&n_point);
            return new CefPoint(r.x, r.y);
        }

        /// <summary>
        /// Convert |rect| from DIP screen coordinates to pixel screen coordinates.
        /// This method is only used on Windows.
        /// </summary>
        public static CefRectangle ConvertScreenRectToPixels(CefRectangle rect)
        {
            var n_rect = new cef_rect_t(rect.X, rect.Y, rect.Width, rect.Height);
            var r = cef_display_t.convert_screen_rect_to_pixels(&n_rect);
            return new CefRectangle(r.x, r.y, r.width, r.height);
        }

        /// <summary>
        /// Convert |rect| from pixel screen coordinates to DIP screen coordinates.
        /// This method is only used on Windows.
        /// </summary>
        public static CefRectangle ConvertScreenRectFromPixels(CefRectangle rect)
        {
            var n_rect = new cef_rect_t(rect.X, rect.Y, rect.Width, rect.Height);
            var r = cef_display_t.convert_screen_rect_from_pixels(&n_rect);
            return new CefRectangle(r.x, r.y, r.width, r.height);
        }

        /// <summary>
        /// Returns the unique identifier for this Display.
        /// </summary>
        public long ID
        {
            get { return cef_display_t.get_id(_self); }
        }

        /// <summary>
        /// Returns this Display's device pixel scale factor. This specifies how much
        /// the UI should be scaled when the actual output has more pixels than
        /// standard displays (which is around 100~120dpi). The potential return
        /// values differ by platform.
        /// </summary>
        public float DeviceScaleFactor
        {
            get { return cef_display_t.get_device_scale_factor(_self); }
        }

        /// <summary>
        /// Convert |point| from DIP coordinates to pixel coordinates using this
        /// Display's device scale factor.
        /// </summary>
        public CefPoint ConvertPointToPixels(CefPoint point)
        {
            var n = new cef_point_t(point.X, point.Y);
            cef_display_t.convert_point_to_pixels(_self, &n);
            return new CefPoint(n.x, n.y);
        }

        /// <summary>
        /// Convert |point| from pixel coordinates to DIP coordinates using this
        /// Display's device scale factor.
        /// </summary>
        public CefPoint ConvertPointFromPixels(CefPoint point)
        {
            var n = new cef_point_t(point.X, point.Y);
            cef_display_t.convert_point_from_pixels(_self, &n);
            return new CefPoint(n.x, n.y);
        }

        /// <summary>
        /// Returns this Display's bounds in DIP screen coordinates. This is the full
        /// size of the display.
        /// </summary>
        public CefRectangle Bounds
        {
            get
            {
                var n = cef_display_t.get_bounds(_self);
                return new CefRectangle(n.x, n.y, n.width, n.height);
            }
        }

        /// <summary>
        /// Returns this Display's work area in DIP screen coordinates. This excludes
        /// areas of the display that are occupied with window manager toolbars, etc.
        /// </summary>
        public CefRectangle WorkArea
        {
            get
            {
                var n = cef_display_t.get_work_area(_self);
                return new CefRectangle(n.x, n.y, n.width, n.height);
            }
        }

        /// <summary>
        /// Returns this Display's rotation in degrees.
        /// </summary>
        public int Rotation
        {
            get { return cef_display_t.get_rotation(_self); }
        }
    }
}
