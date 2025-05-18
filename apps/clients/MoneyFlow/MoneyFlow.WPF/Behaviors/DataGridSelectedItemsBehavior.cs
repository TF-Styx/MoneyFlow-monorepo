﻿using Microsoft.Xaml.Behaviors;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Behaviors
{
    internal class DataGridSelectedItemsBehavior : Behavior<DataGrid>
    {
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
        "SelectedItems",
        typeof(IList),
        typeof(DataGridSelectedItemsBehavior),
        new PropertyMetadata(null));

        public IList SelectedItems
        {
            get => (IList)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.SelectionChanged += OnSelectionChanged;
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.SelectionChanged -= OnSelectionChanged;
            }
            base.OnDetaching();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedItems == null)
            {
                return;
            }

            SelectedItems.Clear();
            foreach (var item in AssociatedObject.SelectedItems)
            {
                SelectedItems.Add(item);
            }
        }
    }
}