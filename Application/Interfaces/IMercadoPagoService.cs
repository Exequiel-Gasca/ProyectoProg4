using Application.DTO_s.MercadoPago;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IMercadoPagoService
    {
        Task<List<SucursalResponse>> GetAllSucursalesAsync();
        Task<SucursalResponse?> GetSucursalesByIdAsync(int Id);
        Task<SucursalResponse?> CreateSucursalesAsync(SucursalRequest sucursalRequest);
        Task<SucursalResponse?> UpdateSucursalesAsync(int Id, SucursalRequest sucursalRequest);
        Task<SucursalResponse?> DeleteSucursalesAsync(int Id);
        Task<List<CajaResponse>> GetAllCajasAsync();
        Task<CajaResponse?> GetCajasByIdAsync(int Id);
        Task<CajaResponse?> CreateCajasAsync(CajaRequest CajaRequest);
        Task<CajaResponse?> UpdateCajasAsync(int Id, CajaRequest CajaRequest);
        Task<CajaResponse?> DeleteCajasAsync(int Id);
    }
}
