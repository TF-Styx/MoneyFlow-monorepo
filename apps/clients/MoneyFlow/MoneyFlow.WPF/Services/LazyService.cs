using Microsoft.Extensions.DependencyInjection;

namespace MoneyFlow.WPF.Services
{
    internal class LazyService<T> : Lazy<T> where T : class
    {
        public LazyService(IServiceProvider serviceProvider)
            : base(() => serviceProvider.GetRequiredService<T>())
        {
        }
    }
}
