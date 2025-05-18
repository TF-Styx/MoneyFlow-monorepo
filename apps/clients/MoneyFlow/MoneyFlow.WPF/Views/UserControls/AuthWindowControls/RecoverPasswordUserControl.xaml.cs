using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoneyFlow.WPF.Views.UserControls.AuthWindowControls
{
    /// <summary>
    /// Логика взаимодействия для RecoverPasswordUserControl.xaml
    /// </summary>
    public partial class RecoverPasswordUserControl : UserControl
    {
        public RecoverPasswordUserControl()
        {
            InitializeComponent();
        }

        #region Login
        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register(
                nameof(Login),
                typeof(string),
                typeof(RecoverPasswordUserControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Login
        {
            get => (string)GetValue(LoginProperty);
            set => SetValue(LoginProperty, value);
        }
        #endregion


        #region CodeVerification
        public static readonly DependencyProperty CodeVerificationProperty =
            DependencyProperty.Register(
                nameof(CodeVerification),
                typeof(string),
                typeof(RecoverPasswordUserControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string CodeVerification
        {
            get => (string)GetValue(CodeVerificationProperty);
            set => SetValue(CodeVerificationProperty, value);
        }
        #endregion


        #region NewPassword
        public static readonly DependencyProperty NewPasswordProperty =
            DependencyProperty.Register(
                nameof(NewPassword),
                typeof(string),
                typeof(RecoverPasswordUserControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string NewPassword
        {
            get => (string)GetValue(NewPasswordProperty);
            set => SetValue(NewPasswordProperty, value);
        }
        #endregion
    }
}
