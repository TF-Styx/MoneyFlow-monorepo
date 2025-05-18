using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Interfaces;
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
using System.Windows.Shapes;

namespace MoneyFlow.WPF.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для InteractionWithDataWindow.xaml
    /// </summary>
    public partial class InteractionWithDataWindow : Window
    {
        internal InteractionWithDataWindow(INavigationPages navigationPages)
        {
            InitializeComponent();

            navigationPages.RegisterFrame(FrameType.InteractionWithDataFrame, InteractionWithDataFrame);
        }
    }
}
