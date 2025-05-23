﻿using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.WPF.Interfaces;
using MoneyFlow.WPF.ViewModels.PageViewModels;
using MoneyFlow.WPF.Views.Pages;
using System.Windows;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Factory.PageFactories
{
    internal class FinancialRecordPageFactory : IPageFactory
    {
        private readonly Lazy<IServiceProvider> _serviceProvider;

        public FinancialRecordPageFactory(Lazy<IServiceProvider> serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Page CreatePage(object parameter = null)
        {
            var viewModel = _serviceProvider.Value.GetRequiredService<FinancialRecordPageVM>();
            viewModel.Update(parameter);
            var page = new FinancialRecordPage() { DataContext = viewModel, };
            //page.Loaded += (sender, args) =>
            //{
            //    //viewModel.Update(parameter);
            //};

            return page;
        }
    }
}
