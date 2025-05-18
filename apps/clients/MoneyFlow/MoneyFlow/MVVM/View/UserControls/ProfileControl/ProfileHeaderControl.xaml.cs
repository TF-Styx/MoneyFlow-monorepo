using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using MoneyFlow.MVVM.Models.DB_MSSQL;

namespace MoneyFlow.MVVM.View.UserControls.ProfileControl
{
    /// <summary>
    /// Логика взаимодействия для ProfileHeaderControl.xaml
    /// </summary>
    public partial class ProfileHeaderControl : UserControl
    {
        public ProfileHeaderControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("User", typeof(User), typeof(ProfileHeaderControl), new PropertyMetadata(null, OnUserChanged));

        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        private static void OnUserChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProfileHeaderControl control)
            {
                control.UpdateDisplay();
            }
        }
        private void UpdateDisplay()
        {
            if (User == null)
            {
                Avatar.Source = null;
                return;
            }

            Login.Text = string.IsNullOrEmpty(User.Login) ? "Отсутствует" : User.Login;
            Password.Text = string.IsNullOrEmpty(User.Password) ? "Отсутствует" : User.Password;
            UserName.Text = string.IsNullOrEmpty(User.UserName) ? "Отсутствует" : User.UserName;
            Gender.Text = User.IdGender == null ? "Отсутствует" : User.IdGenderNavigation.GenderName;
            if (User.Avatar != null)
            {
                try
                {
                    using (MemoryStream stream = new MemoryStream(User.Avatar))
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = stream;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();

                        Avatar.Source = bitmapImage;
                    }
                }
                catch (Exception ex)
                {
                    // Обработка ошибок при конвертации изображения
                    Console.WriteLine($"Error loading image: {ex.Message}");
                    Avatar.Source = null; // Если не удалось загрузить картинку, обнуляем
                }

            }
            else
            {
                Avatar.Source = null;
            }
        }
    }
}
