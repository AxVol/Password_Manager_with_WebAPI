using System;
using System.Windows.Controls;

namespace Desktop_client.Services.Interfaces
{
    public interface IPageService
    {
        event Action<Page> OnPageChanged;

        void ChangePage(Page page);
    }
}
