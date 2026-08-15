//
// This file manually written from cef/include/internal/cef_types.h.
//
namespace Xilium.CefGlue.Interop
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
    internal unsafe struct cef_box_layout_settings_t
    {
        public UIntPtr size;
        public int horizontal;
        public int inside_border_horizontal_spacing;
        public int inside_border_vertical_spacing;
        public cef_insets_t inside_border_insets;
        public int between_child_spacing;
        public CefAxisAlignment main_axis_alignment;
        public CefAxisAlignment cross_axis_alignment;
        public int minimum_cross_axis_size;
        public int default_flex;

        #region Alloc & Free
        private static int _sizeof;

        static cef_box_layout_settings_t()
        {
            _sizeof = Marshal.SizeOf(typeof(cef_box_layout_settings_t));
        }

        public static cef_box_layout_settings_t* Alloc()
        {
            var ptr = (cef_box_layout_settings_t*)Marshal.AllocHGlobal(_sizeof);
            *ptr = new cef_box_layout_settings_t();
            ptr->size = (UIntPtr)_sizeof;
            return ptr;
        }

        public static void Free(cef_box_layout_settings_t* ptr)
        {
            Marshal.FreeHGlobal((IntPtr)ptr);
        }
        #endregion
    }
}
