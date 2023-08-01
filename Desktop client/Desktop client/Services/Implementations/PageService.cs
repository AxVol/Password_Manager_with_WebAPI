using Desktop_client.Services.Interfaces;
using System;
using System.Windows.Controls;

namespace Desktop_client.Services.Implementations
{
    public class PageService : IPageService
    {
        public event Action<Page> OnPageChanged;

        public void ChangePage(Page page)
        {
            OnPageChanged?.Invoke(page);
        }
    }
}
