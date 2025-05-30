using MoneyFlow.Application.DTOs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Client.Views.UserControls
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
            DependencyProperty.Register
                (
                    nameof(UserName),
                    typeof(string),
                    typeof(RegistrationUserControl),
                    new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
                );
        public string UserName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }
        #endregion

        #region Email
        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register
                (
                    nameof(Email),
                    typeof(string),
                    typeof(RegistrationUserControl),
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
                    typeof(RegistrationUserControl),
                    new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
                );
        public string Login
        {
            get => (string)GetValue(LoginProperty);
            set => SetValue(LoginProperty, value);
        }
        #endregion

        #region Password
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register
                (
                    nameof(Password),
                    typeof(string),
                    typeof(RegistrationUserControl),
                    new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
                );
        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }
        #endregion

        #region Phone
        public static readonly DependencyProperty PhoneProperty =
            DependencyProperty.Register
                (
                    nameof(Phone),
                    typeof(string),
                    typeof(RegistrationUserControl),
                    new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
                );
        public string Phone
        {
            get => (string)GetValue(PhoneProperty);
            set => SetValue(PhoneProperty, value);
        }
        #endregion

        #region Genders
        public static readonly DependencyProperty GendersProperty =
            DependencyProperty.Register
                (
                    nameof(Genders),
                    typeof(ObservableCollection<GenderDTO>),
                    typeof(RegistrationUserControl),
                    new FrameworkPropertyMetadata(new ObservableCollection<GenderDTO>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
                );
        public ObservableCollection<GenderDTO> Genders
        {
            get => (ObservableCollection<GenderDTO>)GetValue(GendersProperty);
            set => SetValue(GendersProperty, value);
        }

        public static readonly DependencyProperty SelectedGenderProperty =
            DependencyProperty.Register
                (
                    nameof(SelectedGender),
                    typeof(GenderDTO),
                    typeof(RegistrationUserControl),
                    new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
                );
        public GenderDTO SelectedGender
        {
            get => (GenderDTO)GetValue(SelectedGenderProperty);
            set => SetValue(SelectedGenderProperty, value);
        }
        #endregion
    }
}
