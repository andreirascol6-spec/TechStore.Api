using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TechStore.Wpf.Connection;
using TechStore.Wpf.Models;

namespace TechStore.Wpf.ViewModels
{
    public partial class ProdusViewModel : ObservableValidator
    {
        private readonly ApiService _apiService;
        public ObservableCollection<Produs> ListaProduse { get; set; } = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SalveazaProdusCommand))]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Numele produsului este obligatoriu!")]
        [MinLength(3, ErrorMessage = "Numele produsului trebuie să aibă minim 3 caractere!")]
        private string _nume = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SalveazaProdusCommand))]
        [Range(0.01, 1000, ErrorMessage = "Prețul trebuie să fie mai mare ca 0!")]
        private decimal _pret;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SalveazaProdusCommand))]
        private int _stoc;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(StergeProdusCommand))]
        private Produs? _produsSelectat;


        
        private ICollectionView _produseView;


        [ObservableProperty]
        private string _searchText=string.Empty;

        partial void OnSearchTextChanged(string value)
        {
            _produseView.Refresh();
        }



        public ProdusViewModel(ApiService apiService)
        {
            _apiService = apiService;
            _ = IncarcaProduseAsync();
        }

        // --- CAN EXECUTE ---
        public bool CanIncarcaProduse() => true;
        public bool CanSalveazaProdus() => !string.IsNullOrWhiteSpace(Nume) && Pret > 0;
        public bool CanStergeProdus() => ProdusSelectat != null;

        // --- COMMANDS ---
        [RelayCommand(CanExecute = nameof(CanIncarcaProduse))]
        private async Task IncarcaProduseAsync()
        {
            var date = await _apiService.GetProdusesAsyns();
            if (date != null)
            {
                ListaProduse.Clear();
                foreach (var produs in date)
                {
                    ListaProduse.Add(produs);
                }
            }
            _produseView = CollectionViewSource.GetDefaultView(ListaProduse);
            _produseView.Filter = FiltreazaProduse;
        }

        private bool FiltreazaProduse(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
              return true;
            if(obj is Produs produs)
            {
                return produs.Nume.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;

        }



        [RelayCommand(CanExecute = nameof(CanSalveazaProdus))]
        private async Task SalveazaProdusAsync()
        {
            ValidateAllProperties();
            if (HasErrors) return;

            var nou = new Produs { Nume = Nume, Pret = Pret, Stoc = Stoc };
            if (await _apiService.AddProdusAsync(nou))
            {
                Nume = string.Empty;
                Pret = 0;
                Stoc = 0;
                await IncarcaProduseAsync();
            }
        }

        [RelayCommand(CanExecute = nameof(CanStergeProdus))]
        private async Task StergeProdusAsync()
        {
            if (ProdusSelectat == null) return;

            if (await _apiService.DeleteProdusAsync(ProdusSelectat.Id))
            {
                await IncarcaProduseAsync();
            }
        }
    }
}