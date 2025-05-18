using MoneyFlow.MVVM.View.Pages;
using MoneyFlow.MVVM.ViewModels.PageVM;
using System.Windows.Controls;

namespace MoneyFlow.Utils.Services.NavigationServices.PageNavigationsService
{
    internal class PageNavigationService(IServiceProvider serviceProvider) : IPageNavigationService
    {
        private Dictionary<string, Page> _pages = [];
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private Frame _frame;

        public void InitializeFrame(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo(string namePage, object parameter = null)
        {
            if (_pages.TryGetValue(namePage, out var page))
            {
                if (page.DataContext is IUpdatable viewModel)
                {
                    _frame.Navigate(page);
                    viewModel.Update(parameter);

                    return;
                }
            }
            OpenPage(namePage, parameter);
        }

        public void RefreshData(string namePage, object parameter = null)
        {
            if (_pages.TryGetValue(namePage, out var page))
            {
                if (page.DataContext is IUpdatable viewModel)
                {
                    viewModel.Update(parameter);

                    return;
                }
            }
        }

        private void OpenPage(string namePage, object parameter)
        {
            Action action = namePage switch
            {
                "FinanceJournal" => () =>
                {
                    FinancialJournalPageVM financialJournalPageVM = new FinancialJournalPageVM(_serviceProvider);
                    FinancialJournalPage financialJournalPage = new FinancialJournalPage { DataContext = financialJournalPageVM };

                    _pages.TryAdd(namePage, financialJournalPage);
                    _frame.Navigate(financialJournalPage);

                    financialJournalPageVM.Update(parameter);
                },

                "Profile" => () =>
                {
                    ProfilePageVM profilePageVM = new ProfilePageVM(_serviceProvider);
                    ProfilePage profilePage = new ProfilePage { DataContext = profilePageVM };

                    _pages.TryAdd(namePage, profilePage);
                    _frame.Navigate(profilePage);

                    profilePageVM.Update(parameter);
                },

                _ => () =>
                {

                }
            };
            action?.Invoke();
        }
    }
}
