using Desktop_client.Views;
using Desktop_client.Services.Interfaces;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Desktop_client.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IPageService pageService;

        [ObservableProperty]
        private Page currentView;

        public MainViewModel(IPageService page) 
        {
            pageService = page;

            pageService.OnPageChanged += (page) => CurrentView = page;
            pageService.ChangePage(new LoginPage());
        }
    }
}
