//
// This file manually written from cef/include/internal/cef_types.h.
//
namespace Xilium.CefGlue.Interop
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
    internal unsafe struct cef_linux_window_properties_t
    {
        public UIntPtr size;
        public cef_string_t wayland_app_id;
        public cef_string_t wm_class_class;
        public cef_string_t wm_class_name;
        public cef_string_t wm_role_name;

        #region Alloc & Free
        private static int _sizeof;

        static cef_linux_window_properties_t()
        {
            _sizeof = Marshal.SizeOf(typeof(cef_linux_window_properties_t));
        }

        public static cef_linux_window_properties_t* Alloc()
        {
            var ptr = (cef_linux_window_properties_t*)Marshal.AllocHGlobal(_sizeof);
            *ptr = new cef_linux_window_properties_t();
            ptr->size = (UIntPtr)_sizeof;
            return ptr;
        }

        public static void Free(cef_linux_window_properties_t* ptr)
        {
            libcef.string_clear(&ptr->wayland_app_id);
            libcef.string_clear(&ptr->wm_class_class);
            libcef.string_clear(&ptr->wm_class_name);
            libcef.string_clear(&ptr->wm_role_name);
            Marshal.FreeHGlobal((IntPtr)ptr);
        }
        #endregion
    }
}
