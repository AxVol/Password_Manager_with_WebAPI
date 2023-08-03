using Desktop_client.Models;
using System;
using System.Windows.Controls;

namespace Desktop_client.Services.Interfaces
{
    public interface IPageService
    {
        event Action<Page> OnPageChanged;
        Password password { get; set; }
        string PasswordPageStatus { get; set; }

        void ChangePage(Page page);
    }
}
