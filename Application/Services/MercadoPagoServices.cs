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
            var url = $"users/{_userId}/stores/search";
            var sucursales = await _client.GetFromJsonAsync<List<SucursalResponse>>(url);
            return sucursales ?? new List<SucursalResponse>();
        }
        public async Task<SucursalResponse?> GetSucursalesByIdAsync(long Id)
        {
            var url = $"stores/:{Id}";
            return await _client.GetFromJsonAsync<SucursalResponse>(url);
        }
        public async Task<SucursalResponse?> CreateSucursalesAsync(SucursalRequest sucursalRequest)
        {
            throw new Exception("");
        }

        public async Task<SucursalResponse?> UpdateSucursalesAsync(int Id, SucursalRequest sucursalRequest)
        {
            throw new Exception("");
        }

        public async Task DeleteSucursalesAsync(int Id)
        {
            throw new Exception("");
        }
        public async Task<List<CajaResponse>> GetAllCajasAsync()
        {
            throw new Exception("");
        }
        public async Task<CajaResponse?> GetCajasByIdAsync(int Id)
        {
            throw new Exception("");
        }
        public async Task<CajaResponse?> CreateCajasAsync(CajaRequest cajaRequest)
        {
            throw new Exception("");
        }
        public async Task<CajaResponse?> UpdateCajasAsync(int Id, CajaRequest cajaRequest)
        {
            throw new Exception("");
        }
        public async Task DeleteCajasAsync(int Id)
        {
            throw new Exception("");
        }

    }
}
