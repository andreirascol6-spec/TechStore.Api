using System;
using System.Collections.Generic;
using System.Text;
using TechStore.Wpf.Connection;

namespace TechStore.Wpf.ViewModels
{
    public class MainViewModel:BaseViewModel
    {
        public ProdusViewModel ProdusVM { get;}
        public ComandaViewModel ComandaVM { get; }
        public MainViewModel(ApiService apiService)
        {
            ProdusVM = new ProdusViewModel(apiService);
            ComandaVM = new ComandaViewModel(apiService);
        }
    }
}
