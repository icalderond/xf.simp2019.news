using System;
using System.Collections.Generic;

using Xamarin.Forms;
using xf.simp.news.ViewModels;

namespace xf.simp.news.Views
{
    public partial class ReportsPage : ContentPage
    {
        public ReportsPage()
        {
            InitializeComponent();
            this.BindingContext = new ReportsViewModel();
        }
    }
}
