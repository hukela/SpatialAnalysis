using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Custom
{
    public class ImageButton : Button
    {
        private const double CONTENT_MARGIN = 2.0;

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }
        public ImageButton(){}
        #region Properties
        public static readonly DependencyProperty NormalImageProperty = DependencyProperty.Register("NormalImage", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));
        public ImageSource NormalImage
        {
            get { return (ImageSource)GetValue(NormalImageProperty); }
            set { SetValue(NormalImageProperty, value); }
        }
        public static readonly DependencyProperty HoverImageProperty = DependencyProperty.Register("HoverImage", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));
        public ImageSource HoverImage
        {
            get { return (ImageSource)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }
        public static readonly DependencyProperty PressedImageProperty = DependencyProperty.Register("PressedImage", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));
        public ImageSource PressedImage
        {
            get { return (ImageSource)GetValue(PressedImageProperty); }
            set { SetValue(PressedImageProperty, value); }
        }
        public static readonly DependencyProperty DisabledImageProperty = DependencyProperty.Register("DisabledImage", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));
        public ImageSource DisabledImage
        {
            get { return (ImageSource)GetValue(DisabledImageProperty); }
            set { SetValue(DisabledImageProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ImageButton), new PropertyMetadata(new CornerRadius(0, 0, 0, 0), OnCornerRadiusChanged));
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }
        private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageButton thisButton = d as ImageButton;

            CornerRadius cornerRadius = new CornerRadius();
            cornerRadius = (CornerRadius)e.NewValue;

            if (LayoutModel.LeftToRight == thisButton.LayoutModel)
            {
                d.SetValue(ContentMarginProperty, new Thickness(cornerRadius.TopLeft / 2.0, CONTENT_MARGIN, cornerRadius.TopLeft / 2.0, CONTENT_MARGIN));
            }
            else if (LayoutModel.TopToBottom == thisButton.LayoutModel)
            {
                d.SetValue(ContentMarginProperty, new Thickness(cornerRadius.TopLeft / 3.0, CONTENT_MARGIN, cornerRadius.TopLeft / 3.0, CONTENT_MARGIN));
            }
            else if (LayoutModel.OutSideToInSide == thisButton.LayoutModel)
            {
                d.SetValue(ContentMarginProperty, new Thickness(cornerRadius.TopLeft / 3.0));
            }
            else
            {
                // Nothing to do
            }
        }
        public static readonly DependencyProperty LayoutModelProperty = DependencyProperty.Register("LayoutModel", typeof(LayoutModel), typeof(ImageButton), new PropertyMetadata(LayoutModel.LeftToRight));
        public LayoutModel LayoutModel
        {
            get { return (LayoutModel)GetValue(LayoutModelProperty); }
            set { SetValue(LayoutModelProperty, value); }
        }
        public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(ImageButton), new PropertyMetadata(new Thickness(2, 2, 2, 2)));
        Thickness contentMargin = new Thickness(0, 0, 0, 0);
        public Thickness ContentMargin
        {
            get { return (Thickness)GetValue(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }
        public static readonly DependencyProperty TextLeftMarginProperty = DependencyProperty.Register("TextLeftMargin", typeof(Thickness), typeof(ImageButton), new PropertyMetadata(new Thickness(3, 0, 3, 0)));
        public Thickness TextLeftMargin
        {
            get { return (Thickness)GetValue(TextLeftMarginProperty); }
            set { SetValue(TextLeftMarginProperty, value); }
        }
        #endregion
    }
    public enum LayoutModel
    {
        LeftToRight,
        TopToBottom,
        OutSideToInSide
    }
}
