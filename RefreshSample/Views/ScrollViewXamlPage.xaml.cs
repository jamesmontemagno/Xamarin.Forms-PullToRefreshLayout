using System;
using System.Collections.Generic;

using Xamarin.Forms;
using RefreshSample.ViewModels;

namespace RefreshSample.Views
{
    public partial class ScrollViewXamlPage : ContentPage
    {
        public ScrollViewXamlPage()
        {
            InitializeComponent();
            BindingContext = new TestViewModel(this);
        }
    }
}

