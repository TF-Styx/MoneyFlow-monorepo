using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MoneyFlow.Utils.Services.NavigationServices.PageNavigationsService
{
    internal interface IPageNavigationService
    {
        void NavigateTo(string namePage, object parameter = null);
        void RefreshData(string namePage, object parameter = null);
        void InitializeFrame(Frame frame);
    }
}
