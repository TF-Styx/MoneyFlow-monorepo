using System.Windows;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Views.UserControls.AuthWindowControls
{
    /// <summary>
    /// Логика взаимодействия для RegistrationUserControl.xaml
    /// </summary>
    public partial class RegistrationUserControl : UserControl
    {
        public RegistrationUserControl()
        {
            InitializeComponent();
        }

        #region UserName
        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register(
                nameof(UserName),
                typeof(string),
                typeof(RegistrationUserControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string UserName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }
        #endregion

        #region Login
        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register(
                nameof(Login),
                typeof(string),
                typeof(RegistrationUserControl),
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
                typeof(RegistrationUserControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }
        #endregion
    }
}
