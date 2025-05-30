using System.Windows;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Client.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для RecoveryAccessUserControl.xaml
    /// </summary>
    public partial class RecoveryAccessUserControl : UserControl
    {
        public RecoveryAccessUserControl()
        {
            InitializeComponent();
        }

        #region Email
        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register
                (
                    nameof(Email),
                    typeof(string),
                    typeof(RecoveryAccessUserControl),
                    new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
                );
        public string Email
        {
            get => (string)GetValue(EmailProperty);
            set => SetValue(EmailProperty, value);
        }
        #endregion

        #region Login
        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register
                (
                    nameof(Login),
                    typeof(string),
                    typeof(RecoveryAccessUserControl),
                    new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
                );
        public string Login
        {
            get => (string)GetValue(LoginProperty);
            set => SetValue(LoginProperty, value);
        }
        #endregion

        #region NewPassword
        public static readonly DependencyProperty NewPasswordProperty =
            DependencyProperty.Register
                (
                    nameof(NewPassword),
                    typeof(string),
                    typeof(RecoveryAccessUserControl),
                    new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
                );
        public string NewPassword
        {
            get => (string)GetValue(NewPasswordProperty);
            set => SetValue(NewPasswordProperty, value);
        }
        #endregion
    }
}
