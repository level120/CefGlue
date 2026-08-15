namespace Xilium.CefGlue
{
    using System;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// Settings used when initializing a CefBoxLayout (Views — CEF 146).
    /// </summary>
    public struct CefBoxLayoutSettings
    {
        /// <summary>
        /// If true the layout will be horizontal, otherwise the layout will be
        /// vertical.
        /// </summary>
        public bool Horizontal;

        /// <summary>
        /// Adds additional horizontal space between the child view area and the host
        /// view border.
        /// </summary>
        public int InsideBorderHorizontalSpacing;

        /// <summary>
        /// Adds additional vertical space between the child view area and the host
        /// view border.
        /// </summary>
        public int InsideBorderVerticalSpacing;

        /// <summary>
        /// Adds additional space around the child view area.
        /// </summary>
        public CefInsets InsideBorderInsets;

        /// <summary>
        /// Adds additional space between child views.
        /// </summary>
        public int BetweenChildSpacing;

        /// <summary>
        /// Specifies where along the main axis the child views should be laid out.
        /// </summary>
        public CefAxisAlignment MainAxisAlignment;

        /// <summary>
        /// Specifies where along the cross axis the child views should be laid out.
        /// </summary>
        public CefAxisAlignment CrossAxisAlignment;

        /// <summary>
        /// Minimum cross axis size.
        /// </summary>
        public int MinimumCrossAxisSize;

        /// <summary>
        /// Default flex for views when none is specified via CefBoxLayout methods.
        /// Using the preferred size as the basis, free space along the main axis is
        /// distributed to views in the ratio of their flex weights. Similarly, if the
        /// views will overflow the parent, space is subtracted in these ratios. A
        /// flex of 0 means this view is not resized. Flex values must not be
        /// negative.
        /// </summary>
        public int DefaultFlex;

        internal unsafe cef_box_layout_settings_t ToNative()
        {
            var n = new cef_box_layout_settings_t();
            n.size = (UIntPtr)sizeof(cef_box_layout_settings_t);
            n.horizontal = Horizontal ? 1 : 0;
            n.inside_border_horizontal_spacing = InsideBorderHorizontalSpacing;
            n.inside_border_vertical_spacing = InsideBorderVerticalSpacing;
            n.inside_border_insets = new cef_insets_t(InsideBorderInsets.Top, InsideBorderInsets.Left, InsideBorderInsets.Bottom, InsideBorderInsets.Right);
            n.between_child_spacing = BetweenChildSpacing;
            n.main_axis_alignment = MainAxisAlignment;
            n.cross_axis_alignment = CrossAxisAlignment;
            n.minimum_cross_axis_size = MinimumCrossAxisSize;
            n.default_flex = DefaultFlex;
            return n;
        }
    }
}
