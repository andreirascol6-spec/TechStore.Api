using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TechStore.Wpf.Connection;
using TechStore.Wpf.Models;

namespace TechStore.Wpf.ViewModels
{
    public partial class ComandaViewModel : ObservableValidator
    {
        private readonly ApiService _apiservice;

        public ObservableCollection<Produs> ListaProduse { get; set; } = new();
        public ObservableCollection<Comanda> ListaComenzi { get; set; } = new();
        public ObservableCollection<Produs> ProduseDisponibile { get; set; } = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveOrderCommand))]
        private Produs? _produsSelectat;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(SaveOrderCommand))]
        [Required(ErrorMessage = "Cantitatea este obligatorie!")]
        [Range(1, 100, ErrorMessage = "Cantitatea selectata trebuie sa fie >0 si <=100")]
        private int _cantitate;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteOrderCommand))]
        private Comanda? _selectedOrder;

        [ObservableProperty]
        private int _produsId;

        [ObservableProperty]
            
        [NotifyCanExecuteChangedFor(nameof(LoadOrdersCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteOrderCommand))]
        private bool _isBusy;


        private ICollectionView _produsView;

        [ObservableProperty]
        private string _searchText = string.Empty;

        partial void OnSearchTextChanged(string value)
        {
           _produsView?.Refresh();
        }


        

        public ComandaViewModel(ApiService apiservice)
        {
            _apiservice = apiservice;
            _ = LoadOrdersAsync();
            _ = LoadProduseAsync();
        }

        public async Task LoadProduseAsync()
        {
            IsBusy = true;
            try
            {
                var produse = await _apiservice.GetProdusesAsyns();
                if (produse == null) return;

                ListaProduse.Clear();
                foreach (var produs in produse)
                {
                    ListaProduse.Add(produs);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        // --- CAN EXECUTE ---
        public bool CanLoadOrder() => !IsBusy;
        public bool CanSaveOrder() => ProdusSelectat != null && Cantitate > 0 && !IsBusy;
        public bool CanDeleteOrder() => SelectedOrder != null && !IsBusy;

        // --- COMMANDS ---
        [RelayCommand(CanExecute = nameof(CanLoadOrder))]
        private async Task LoadOrdersAsync()
        {
            IsBusy = true;
            try
            {
                var orders = await _apiservice.GetOrdersAsync();
                if (orders != null)
                {
                    ListaComenzi.Clear();
                    foreach (var order in orders)
                    {
                        ListaComenzi.Add(order);
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
            _produsView = CollectionViewSource.GetDefaultView(ListaComenzi);
            _produsView.Filter = FiltreazaComenzi;
        }   

        private bool FiltreazaComenzi(object obj)
        {
            if(string.IsNullOrWhiteSpace(SearchText))
            {
                return true;
            }
            if(obj is Comanda comanda)
            {
                return comanda.Produs.Nume!=null && comanda.Produs.Nume.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        [RelayCommand(CanExecute = nameof(CanSaveOrder))]
        private async Task SaveOrderAsync()
        {
            ValidateAllProperties();
            if (HasErrors || ProdusSelectat == null) return;

            IsBusy = true;
            try
            {
                var comandaNoua = new Comanda
                {
                    ProdusId = ProdusSelectat.Id,
                    Cantitate = Cantitate,
                    DataComanda = DateTime.Now
                };

                var(succes,mesaj) = await _apiservice.AddComandaAsync(comandaNoua);

                if (succes)
                {
                    ProdusId = 0;
                    Cantitate = 0;
                    ProdusSelectat = null;

                    await LoadOrdersAsync();
                    await LoadProduseAsync();
                }
                else
                {
                    MessageBox.Show(mesaj, "Eroare comanda", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand(CanExecute = nameof(CanDeleteOrder))]
        private async Task DeleteOrderAsync()
        {
            if (SelectedOrder == null) return;

            IsBusy = true;
            try
            {
                if (await _apiservice.DeleteComandaAsync(SelectedOrder.Id))
                {
                    await LoadOrdersAsync();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [ObservableProperty]
        
        public decimal _totalPlata;




        private void RecalculeazaTotal()
        {
            if(ProdusSelectat!=null && Cantitate>0)
            {
                TotalPlata = ProdusSelectat.Pret * Cantitate;
            }
            else
            {
                TotalPlata = 0;
            }
        }

        partial void OnCantitateChanged(int value)
        {
            RecalculeazaTotal();
        }

        partial void OnProdusSelectatChanged(Produs? value)
        {
            RecalculeazaTotal();
        }
    }
}