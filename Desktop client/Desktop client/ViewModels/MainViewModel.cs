using TFT_TEAM_BUILDER.Core;

namespace Desktop_client.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private MainViewModel mainViewModel { get; set; }

        private object currentView;

        public object CurrentView
        {
            get => currentView;
            set 
            { 
                currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel() 
        {
            mainViewModel = this;
        }
    }
}
