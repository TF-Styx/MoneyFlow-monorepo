using System.Windows;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Views.UserControls.AuthWindowControls
{
    /// <summary>
    /// Логика взаимодействия для AuthUserControl.xaml
    /// </summary>
    public partial class AuthUserControl : UserControl
    {
        public AuthUserControl()
        {
            InitializeComponent();
        }

        #region Login
        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register(
                nameof(Login),
                typeof(string),
                typeof(AuthUserControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Login
        {
            get => (string)GetValue(LoginProperty); 
            set => SetValue(LoginProperty, value);
        }
        #endregion


        #region Password
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                nameof(Password),
                typeof(string),
                typeof(AuthUserControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }
        #endregion


        #region IsChecked
        //public static readonly DependencyProperty IsCheckedProperty =
        //    DependencyProperty.Register(
        //        nameof(IsChecked),
        //        typeof(bool),
        //        typeof(AuthUserControl),
        //        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public bool IsChecked
        //{
        //    get => (bool)GetValue(IsCheckedProperty);
        //    set => SetValue(IsCheckedProperty, value);
        //}
        #endregion
    }
}
