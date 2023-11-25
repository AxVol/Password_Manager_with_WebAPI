using Desktop_client.Models;
using Desktop_client.Services.Interfaces;
using System;
using System.Windows.Controls;

namespace Desktop_client.Services.Implementations
{
    public class PageService : IPageService
    {
        public string PasswordPageStatus { get; set; }
        public Password Password { get; set; }

        public event Action<Page> OnPageChanged;

        public void ChangePage(Page page)
        {
            OnPageChanged?.Invoke(page);
        }
    }
}
