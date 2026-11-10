using Application.DTO_s.MercadoPago;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.Services
{
    public class MercadoPagoServices : IMercadoPagoService
    {

        public async Task<List<SucursalResponse>> GetAllSucursalesAsync()
        {

        }
        GetSucursalesByIdAsync(int Id)
        {

        }
        CreateSucursalesAsync(SucursalRequest sucursalRequest)
        {

        }

        UpdateSucursalesAsync(int Id, SucursalRequest sucursalRequest)
        {

        }

        DeleteSucursalesAsync(int Id)
        {

        }
        GetAllCajasAsync()
        {

        }
        GetCajasByIdAsync(int Id)
        {

        }

    }
}
