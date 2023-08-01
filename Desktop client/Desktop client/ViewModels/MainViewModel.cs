using Desktop_client.Core;
using Desktop_client.Views;
using Desktop_client.Services.Interfaces;
using System.Windows.Controls;

namespace Desktop_client.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly IPageService pageService;

        public Page CurrentView { get; set; }

        public MainViewModel(IPageService page) 
        {
            pageService = page;

            pageService.OnPageChanged += (page) => CurrentView = page;
            pageService.ChangePage(new LoginPage());
        }
    }
}
