using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TechStore.Wpf.Connection;
using TechStore.Wpf.Models;

namespace TechStore.Wpf.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isBusy;

        public LoginViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [RelayCommand]
        private async Task LoginAsync(object? parameter)
        {
            if (IsBusy) return;

            // Extragem parola din PasswordBox transmis ca parametru
            if (parameter is not PasswordBox passwordBox || string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                ErrorMessage = "Te rugăm să introduci parola!";
                return;
            }

            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Te rugăm să introduci username-ul!";
                return;
            }

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                // 1. Instanțiem DTO-ul local din frontend
                var loginDto = new UserDto
                {
                    Username = Username,
                    Password = passwordBox.Password
                };

                // 2. Apelăm serviciul cu numele corect al variabilei
                var result = await _apiService.LoginAsync(loginDto);

                if (result != null)
                {
                    // 3. Pasăm dependențele necesare pentru MainViewModel și deschidem MainWindow
                    var mainViewModel = new MainViewModel(_apiService);
                    var mainWindow = new MainWindow(mainViewModel);
                    mainWindow.Show();

                    // Închidem fereastra curentă de Login
                    Application.Current.Windows.OfType<LoginWindow>().FirstOrDefault()?.Close();
                }
                else
                {
                    ErrorMessage = "Username sau parolă incorectă!";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la conectare: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task RegisterAsync(object? parameter)
        {
            if (IsBusy) return;

            if (parameter is not PasswordBox passwordBox || string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                ErrorMessage = "Introduceți o parolă pentru înregistrare!";
                return;
            }

            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Introduceți un username pentru înregistrare!";
                return;
            }

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var registerDto = new UserDto
                {
                    Username = Username,
                    Password = passwordBox.Password
                };

                var success = await _apiService.RegisterAsync(registerDto);

                if (success)
                {
                    ErrorMessage = "Cont creat cu succes! Te poți autentifica.";
                }
                else
                {
                    ErrorMessage = "Numele de utilizator este deja folosit.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la înregistrare: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}