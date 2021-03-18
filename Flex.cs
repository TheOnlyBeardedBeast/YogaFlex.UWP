using Facebook.Yoga;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace YogaFlex.UWP
{
    public class Flex : Panel
    {
        public static readonly DependencyProperty GrowProperty = DependencyProperty.RegisterAttached("Grow", typeof(float), typeof(Flex), new PropertyMetadata(0, new PropertyChangedCallback(OnParentChanged)));
        public static int GetGrow(DependencyObject obj) => (int)obj.GetValue(GrowProperty);
        public static void SetGrow(DependencyObject obj, int value) => obj.SetValue(GrowProperty, value);

        public static readonly DependencyProperty ShrinkProperty = DependencyProperty.RegisterAttached("Shrink", typeof(float), typeof(Flex), new PropertyMetadata(0, new PropertyChangedCallback(OnParentChanged)));
        public static int GetShrink(DependencyObject obj) => (int)obj.GetValue(ShrinkProperty);
        public static void SetShrink(DependencyObject obj, int value) => obj.SetValue(ShrinkProperty, value);

        public static readonly DependencyProperty AlignSelfProperty = DependencyProperty.RegisterAttached("AlignSelf", typeof(YogaAlign), typeof(Flex), new PropertyMetadata(YogaAlign.Auto, new PropertyChangedCallback(OnParentChanged)));
        public static YogaAlign GetAlignSelf(DependencyObject obj) => (YogaAlign)obj.GetValue(AlignSelfProperty);
        public static void SetAlignSelf(DependencyObject obj, YogaAlign value) => obj.SetValue(AlignSelfProperty, value);

        public static readonly DependencyProperty HeighUnitProperty = DependencyProperty.RegisterAttached("HeighUnit", typeof(RenderUnit), typeof(Flex), new PropertyMetadata(RenderUnit.Point, new PropertyChangedCallback(OnParentChanged)));
        public static RenderUnit GetHeightUnit(DependencyObject obj) => (RenderUnit)obj.GetValue(HeighUnitProperty);
        public static void SetHeightUnit(DependencyObject obj, RenderUnit value) => obj.SetValue(HeighUnitProperty, value);

        public static readonly DependencyProperty WidthUnitProperty = DependencyProperty.RegisterAttached("WidthUnit", typeof(RenderUnit), typeof(Flex), new PropertyMetadata(RenderUnit.Point, new PropertyChangedCallback(OnParentChanged)));
        public static RenderUnit GetWidthUnit(DependencyObject obj) => (RenderUnit)obj.GetValue(WidthUnitProperty);
        public static void SetWidthUnit(DependencyObject obj, RenderUnit value) => obj.SetValue(WidthUnitProperty, value);

        public static readonly DependencyProperty BasisProperty = DependencyProperty.RegisterAttached("FlexBasis", typeof(float), typeof(Flex), new PropertyMetadata(float.NaN));
        public static float GetBasis(DependencyObject obj) => (float)obj.GetValue(BasisProperty);
        public static void SetBasis(DependencyObject obj, double value) => obj.SetValue(BasisProperty, value);

        public static readonly DependencyProperty BasisUnitProperty = DependencyProperty.RegisterAttached("BasisUnit", typeof(RenderUnit), typeof(Flex), new PropertyMetadata(RenderUnit.Point, new PropertyChangedCallback(OnParentChanged)));
        public static RenderUnit GetBasistUnit(DependencyObject obj) => (RenderUnit)obj.GetValue(BasisUnitProperty);
        public static void SetBasishtUnit(DependencyObject obj, RenderUnit value) => obj.SetValue(HeighUnitProperty, value);

        public static readonly DependencyProperty FlexWidthProperty = DependencyProperty.RegisterAttached("FlexWidth", typeof(double), typeof(Flex), new PropertyMetadata(double.NaN, new PropertyChangedCallback(OnParentChanged)));
        public static double GetFlexWidth(DependencyObject obj) => (double)obj.GetValue(FlexWidthProperty);
        public static void SetFlexWidth(DependencyObject obj, double value) => obj.SetValue(FlexWidthProperty, value);

        public static readonly DependencyProperty FlexHeightProperty = DependencyProperty.RegisterAttached("FlexHeight", typeof(double), typeof(Flex), new PropertyMetadata(double.NaN, new PropertyChangedCallback(OnParentChanged)));
        public static double GetFlexHeight(DependencyObject obj) => (double)obj.GetValue(FlexHeightProperty);
        public static void SetFlexHeight(DependencyObject obj, double value) => obj.SetValue(FlexHeightProperty, value);

        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(YogaFlexDirection), typeof(Flex), new PropertyMetadata(YogaFlexDirection.Row, new PropertyChangedCallback((d, e) => { ((UIElement)d).InvalidateMeasure(); })));
        public YogaFlexDirection Direction { get => (YogaFlexDirection)GetValue(DirectionProperty); set => SetValue(DirectionProperty, value); }

        public static readonly DependencyProperty WrapProperty = DependencyProperty.Register("Wrap", typeof(YogaWrap), typeof(Flex), new PropertyMetadata(YogaWrap.NoWrap, new PropertyChangedCallback((d, e) => { ((UIElement)d).InvalidateMeasure(); })));
        public YogaWrap Wrap { get => (YogaWrap)GetValue(WrapProperty); set => SetValue(WrapProperty, value); }

        public static readonly DependencyProperty JustifyContentProperty = DependencyProperty.Register("JustifyContent", typeof(YogaJustify), typeof(Flex), new PropertyMetadata(YogaJustify.FlexStart, new PropertyChangedCallback((d, e) => { ((UIElement)d).InvalidateMeasure(); })));
        public YogaJustify JustifyContent { get => (YogaJustify)GetValue(JustifyContentProperty); set => SetValue(JustifyContentProperty, value); }

        public static readonly DependencyProperty AlignItemsProperty = DependencyProperty.Register("AlignItems", typeof(YogaAlign), typeof(Flex), new PropertyMetadata(YogaAlign.FlexStart, new PropertyChangedCallback((d, e) => { ((UIElement)d).InvalidateMeasure(); })));
        public YogaAlign AlignItems { get => (YogaAlign)GetValue(AlignItemsProperty); set => SetValue(AlignItemsProperty, value); }

        public static readonly DependencyProperty AlignContentProperty = DependencyProperty.Register("AlignContent", typeof(YogaAlign), typeof(Flex), new PropertyMetadata(YogaAlign.FlexStart, new PropertyChangedCallback((d, e) => { ((UIElement)d).InvalidateMeasure(); })));
        public YogaAlign AlignContent { get => (YogaAlign)GetValue(AlignContentProperty); set => SetValue(AlignContentProperty, value); }

        protected YogaNode Root { get; set; }

        protected static void OnParentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (VisualTreeHelper.GetParent(d) as UIElement)?.InvalidateMeasure();
            (d as UIElement).InvalidateMeasure();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.Root = GetRoot(availableSize);

            foreach (var c in Children.Cast<UIElement>())
            {
                this.Root.AddChild(GetChild(c, availableSize));
            }

            this.Root.CalculateLayout();

            foreach (var c in Root)
            {
                (c.Data as UIElement).Measure(new Size(c.LayoutWidth, c.LayoutHeight));
            }

            return new Size(this.Root.LayoutWidth, this.Root.LayoutHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count == 0)
            {
                return finalSize;
            }

            foreach (var c in Root)
            {
                (c.Data as UIElement).Arrange(new Rect(c.LayoutX, c.LayoutY, c.LayoutWidth, c.LayoutHeight));
            }

            return new Size(this.Root.LayoutWidth, this.Root.LayoutHeight);

        }

        protected YogaValue GetYogaValue(double value, RenderUnit unit = RenderUnit.Point)
        {
            if (double.IsInfinity(value) || double.IsNaN(value))
            {
                return YogaValue.Auto();
            }

            if (unit == RenderUnit.Percentage)
            {
                return (YogaValue.Percent((float)value));
            }

            return (YogaValue.Point((float)value));
        }

        private YogaNode GetChild(UIElement c, Size availableSize)
        {
            var item = new YogaNode();

            var margin = GetMartgin(c);

            var minHeight = (double)c.GetValue(MinHeightProperty);
            var minWidth = (double)c.GetValue(MinWidthProperty);
            var maxWidth = (double)c.GetValue(MaxWidthProperty);
            var maxHeight = (double)c.GetValue(HeightProperty);

            item.AlignSelf = GetAlignSelf(c);
            item.FlexShrink = GetShrink(c);
            item.FlexGrow = GetGrow(c);

            item.MarginLeft = (float)margin.Left;
            item.MarginRight = (float)margin.Right;
            item.MarginTop = (float)margin.Top;
            item.MarginBottom = (float)margin.Bottom;

            if (minWidth > 0)
            {
                item.MinWidth = (float)minWidth;
            }
            if (minHeight > 0)
            {
                item.MinHeight = (float)minHeight;
            }

            if (maxWidth > 0)
            {
                item.MaxWidth = (float)maxWidth + 20;
            }
            if (maxHeight > 0)
            {
                item.MaxHeight = (float)maxHeight;
            }


            // clear default width and height to ensure no rendering issues
            // FlexWidth and FlexHeight ovverides width and height
            if (!double.IsNaN((double)c.GetValue(WidthProperty)))
            {
                if (double.IsNaN(GetFlexWidth(c)))
                {
                    SetFlexWidth(c, (double)c.GetValue(WidthProperty));
                }

                c.ClearValue(WidthProperty);
                c.Measure(availableSize);
            }

            if (!double.IsNaN((double)c.GetValue(HeightProperty)))
            {
                if (double.IsNaN(GetFlexHeight(c)))
                {
                    SetFlexHeight(c, (double)c.GetValue(HeightProperty));
                }

                c.ClearValue(HeightProperty);
                c.Measure(availableSize);
            }

            if (double.IsNaN(GetFlexHeight(c)) || double.IsNaN(GetFlexWidth(c)))
            {

                c.Measure(availableSize);
                if (double.IsNaN(GetFlexWidth(c)))
                {
                    item.Width = (float)c.DesiredSize.Width;
                }
                else
                {
                    item.Width = (float)GetFlexWidth(c);
                    // To automatically stretch the content to the required size
                    c.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                }

                if (double.IsNaN(GetFlexHeight(c)))
                {
                    item.Height = (float)c.DesiredSize.Height;
                }
                else
                {
                    item.Height = (float)GetFlexHeight(c);
                    // To automatically stretch the content to the required size
                    c.SetValue(VerticalAlignmentProperty, VerticalAlignment.Stretch);
                }
            }
            else
            {
                item.Width = GetYogaValue((float)GetFlexWidth(c), GetWidthUnit(c));
                item.Height = GetYogaValue((float)GetFlexHeight(c), GetHeightUnit(c));
            }

            item.Data = c;
            return item;
        }


        protected YogaNode GetRoot(Size renderSize)
        {
            var root = new YogaNode();
            root.FlexDirection = this.Direction;
            root.AlignItems = this.AlignItems;
            root.JustifyContent = this.JustifyContent;
            root.AlignContent = this.AlignContent;
            root.Width = GetYogaValue(renderSize.Width);
            root.Height = GetYogaValue(renderSize.Height);
            root.Wrap = this.Wrap;

            return root;
        }

        public static Thickness GetMartgin(DependencyObject obj) => (Thickness)obj.GetValue(MarginProperty);
    }

    public enum RenderUnit
    {
        Point,
        Percentage
    }

    public enum Axis
    {
        Horizontal,
        Vertical,
    }
}
