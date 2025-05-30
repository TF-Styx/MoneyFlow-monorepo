using MoneyFlow.WPF.Client.Enums;
using MoneyFlow.WPF.Client.Factories.Interfaces;
using MoneyFlow.WPF.Client.Services.Interfaces;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Client.Services.NavigationPages
{
    internal class NavigationPage : INavigationPage
    {
        private Dictionary<FrameName, Frame> _frame = [];
        private Dictionary<PageName, Page> _page = [];
        private readonly Dictionary<string, IPageFactory> _pageFactories = [];

        public NavigationPage(IEnumerable<IPageFactory> pageFactories)
        {
            _pageFactories = pageFactories.ToDictionary(f => f.GetType().Name.Replace("Factory", ""), f => f);
        }

        public void RegisterFrame(FrameName frameName, Frame frame)
        {
            if (!_frame.TryAdd(frameName, frame))
            {
                throw new Exception($"Данный «{frameName}» уже занят!!");
            }
        }

        public void OpenPage(PageName pageName, FrameName frameName)
        {
            if (_page.TryGetValue(pageName, out var pageExist))
            {
                if (pageExist.DataContext is IUpdatable viewModel)
                {
                    if (_frame.TryGetValue(frameName, out var frame))
                    {
                        frame.Navigate(pageExist);
                    }
                    else throw new Exception("Данный Frame уже зарегистрирован!");
                }
                else throw new Exception($"У ViewModel страницы {pageName}, не реализован интерфейс!");
            }

            Open(pageName, frameName);
        }

        public void TransitObject(PageName pageName, FrameName frameName, object parameter = null, ParameterType parameterType = ParameterType.None)
        {
            if (_page.TryGetValue(pageName, out var pageExist))
            {
                if (pageExist.DataContext is IUpdatable viewModel)
                {
                    if (_frame.TryGetValue(frameName, out var frame))
                    {
                        viewModel.Update(parameter, parameterType);
                        frame.Navigate(pageExist);
                    }
                }
            }
        }

        private void Open(PageName pageName, FrameName frameName)
        {
            if (_pageFactories.TryGetValue(pageName.ToString(), out var factory))
            {
                if (_frame.TryGetValue(frameName, out var frame))
                {
                    var page = factory.CreatePage();
                    _page.TryAdd(pageName, page);
                    frame.Navigate(page);
                }
                else throw new Exception("Фрейм не найден!!");
            }
            else throw new Exception($"Страничка с таким именем «{pageName}» не зарегистрирована !!");
        }
    }
}
