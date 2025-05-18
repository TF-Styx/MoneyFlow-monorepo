using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Interfaces;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Services
{
    internal class NavigationPages : INavigationPages
    {
        private Dictionary<FrameType, Frame> _frame = [];
        private Dictionary<PageType, Page> _page = [];
        private readonly Dictionary<string, IPageFactory> _pageFactories = [];

        public NavigationPages(IEnumerable<IPageFactory> pageFactories)
        {
            _pageFactories = pageFactories.ToDictionary(f => f.GetType().Name.Replace("Factory", ""), f => f);
        }

        public void RegisterFrame(FrameType frameName, Frame frame)
        {
            if (!_frame.TryAdd(frameName, frame))
            {
                throw new Exception($"Данный «{frameName}» уже занят!!");
            }
        }

        public void OpenPage(PageType pageName, FrameType frameName, object parameter = null, ParameterType parameterType = ParameterType.None)
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
                    else throw new Exception("Данный Frame уже зарегистрирован!");
                }
                else throw new Exception($"У ViewModel страницы {pageName}, не реализован интерфейс!");
            }

            Open(pageName, frameName, parameter, parameterType);
        }

        public void TransitObject(PageType pageName, FrameType frameName, object parameter = null, ParameterType parameterType = ParameterType.None)
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

        private void Open(PageType pageName, FrameType frameName, object parameter = null, ParameterType parameterType = ParameterType.None)
        {
            if (_pageFactories.TryGetValue(pageName.ToString(), out var factory))
            {
                if (_frame.TryGetValue(frameName, out var frame))
                {
                    var page = factory.CreatePage(parameter);
                    _page.TryAdd(pageName, page);
                    frame.Navigate(page);
                }
                else throw new Exception("Фрейм не найден!!");
            }
            else throw new Exception($"Страничка с таким именем «{pageName}» не зарегистрирована !!");
        }
    }
}
