using Application.DTO_s.MercadoPago;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.Services
{
    public class MercadoPagoServices : IMercadoPagoService
    {
        private readonly HttpClient _client;
        private readonly string _userId;
        public MercadoPagoServices(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient(HttpClientNames.MercadoPago);
            _userId = configuration["ExternalApis:MercadoPago:UserId"] ?? string.Empty;
        }
        public async Task<List<SucursalResponse>> GetAllSucursalesAsync()
        {
            string url = $"users/{_userId}/stores/search";
            var sucursales = await _client.GetFromJsonAsync<List<SucursalResponse>>(url);
            return sucursales ?? new List<SucursalResponse>();
        }
        public async Task<SucursalResponse?> GetSucursalesByIdAsync(long Id)
        {
            string url = $"stores/:{Id}";
            return await _client.GetFromJsonAsync<SucursalResponse>(url);
        }
        public async Task<SucursalResponse?> CreateSucursalesAsync(SucursalRequest sucursalRequest)
        {
            string url = $"users/:{_userId}/stores";
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, sucursalRequest);
            return await response.Content.ReadFromJsonAsync<SucursalResponse>();
        }

        public async Task<SucursalResponse?> UpdateSucursalesAsync(long Id, SucursalRequest sucursalRequest)
        {
            string url = $"users/:{_userId}/stores/:{Id}";
            HttpResponseMessage response= await _client.PutAsJsonAsync(url, sucursalRequest);
            return await response.Content.ReadFromJsonAsync<SucursalResponse>();
        }

        public async Task DeleteSucursalesAsync(long Id)
        {
            string url = $"users/:{_userId}/stores/:{Id}";
            HttpResponseMessage response = await _client.DeleteAsync(url);
        }
        public async Task<List<CajaResponse>> GetAllCajasAsync()
        {
            string url = "pos";
            var response = await _client.GetFromJsonAsync<List<CajaResponse>>(url);
            return response?? new List<CajaResponse>();
        }
        public async Task<CajaResponse?> GetCajasByIdAsync(long Id)
        {
            string url = $"pos/:{Id}";
            return await _client.GetFromJsonAsync<CajaResponse>(url);
            
        }
        public async Task<CajaResponse?> CreateCajasAsync(CajaRequest cajaRequest)
        {
            string url = "pos";
            var response = await _client.PostAsJsonAsync(url,cajaRequest);
            return await response.Content.ReadFromJsonAsync<CajaResponse>();
        }
        public async Task<CajaResponse?> UpdateCajasAsync(long Id, CajaRequest cajaRequest)
        {
            string url = $"pos/:{Id}";
            var response = await _client.PutAsJsonAsync(url,cajaRequest);
            return await response.Content.ReadFromJsonAsync<CajaResponse>(); 
        }
        public async Task DeleteCajasAsync(long Id)
        {
            string url = $"pos/:{Id}";
            var response = await _client.DeleteAsync(url);
        }

    }
}
