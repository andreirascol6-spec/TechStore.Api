using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TechStore.Api.Models;
using TechStore.Wpf.Models;

namespace TechStore.Wpf.Connection
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        // Constructorul care primește HttpClient din exterior (Dependency Injection)
        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// OPERATII PRODUSE

        public async Task<List<Produs>?> GetProdusesAsyns()
        {
            return await _httpClient.GetFromJsonAsync<List<Produs>>("TechStore");
        }

        public async Task<Produs?> GetProdusAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Produs>($"TechStore/{id}");
        }

        public async Task<bool> AddProdusAsync(Produs produs)
        {
            var response = await _httpClient.PostAsJsonAsync("TechStore", produs);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProdusAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"TechStore/{id}");
            return response.IsSuccessStatusCode;
        }

        /// OPERATII COMENZI

        public async Task<List<Comanda>?> GetOrdersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Comanda>>("TechStoreOrders");
        }

        public async Task<Comanda?> GetOrderById(int id)
        {
            return await _httpClient.GetFromJsonAsync<Comanda>($"TechStoreOrders/{id}");
        }

        public async Task<(bool Succes, string Mesaj)> AddComandaAsync(Comanda comanda)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("TechStoreOrders", comanda);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Comanda a fost plasată cu succes!");
                }
                string mesajEroare = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(mesajEroare))
                {
                    mesajEroare = "Nu s-a putut plasa comanda";
                }
                return (false, mesajEroare);
            }
            catch (Exception)
            {
                return (false, "Nu s-a putut conecta la serverul API");
            }
        }

        public async Task<bool> DeleteComandaAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"TechStoreOrders/{id}");
            return response.IsSuccessStatusCode;
        }

        /// AUTENTIFICARE (Rutele folosesc "Auth/login" deoarece BaseAddress include deja "api/")

        public async Task<TokenResponseDto?> LoginAsync(UserDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("Auth/login", dto);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<TokenResponseDto>();
        }

        public async Task<bool> RegisterAsync(UserDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("Auth/register", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("Auth/refresh-tokens", dto);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<TokenResponseDto>();
        }
    }
}