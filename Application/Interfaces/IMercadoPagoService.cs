using Application.DTO_s.MercadoPago;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IMercadoPagoService
    {
        Task<List<SucursalResponse>> GetAllSucursalesAsync();
        Task<SucursalResponse?> GetSucursalesByIdAsync(long Id);
        Task<SucursalResponse?> CreateSucursalesAsync(SucursalRequest sucursalRequest);
        Task<SucursalResponse?> UpdateSucursalesAsync(long Id, SucursalRequest sucursalRequest);
        Task DeleteSucursalesAsync(long Id);
        Task<List<CajaResponse>> GetAllCajasAsync();
        Task<CajaResponse?> GetCajasByIdAsync(long Id);
        Task<CajaResponse?> CreateCajasAsync(CajaRequest CajaRequest);
        Task<CajaResponse?> UpdateCajasAsync(long Id, CajaRequest CajaRequest);
        Task DeleteCajasAsync(long Id);
    }
}
