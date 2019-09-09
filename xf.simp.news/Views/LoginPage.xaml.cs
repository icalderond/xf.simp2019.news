using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace xf.simp.news.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            btnLogin.Clicked += async (s, a) =>
            {
                await Navigation.PushAsync(new ReportsPage());
            };
        }
    }
}
