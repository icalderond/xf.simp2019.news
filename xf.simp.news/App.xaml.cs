using Xamarin.Forms;
using xf.simp.news.Views;

namespace xf.simp.news
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new ReportsPage());
        }
    }
}