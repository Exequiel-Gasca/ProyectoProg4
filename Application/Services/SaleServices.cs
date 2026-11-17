
using Application.DTOs;
using Application.DTOs.request;
using Application.DTOs.Responses.Sales;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    public SaleService(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        IUserRepository userRepository)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    public async Task<SaleResponseDto?> GetSaleByIdAsync(int id)
    {
        var sale = await _saleRepository.GetSaleByIdAsync(id);

        if (sale == null)
            return null;

        return MapToResponse(sale);
    }

    public async Task<IEnumerable<SaleResponseDto>> GetAllSalesAsync()
    {
        var sales = await _saleRepository.GetAllSalesAsync();

        return sales.Select(MapToResponse);
    }

    public async Task<IEnumerable<SaleResponseDto>> GetSalesByDateRangeAsync(
        DateTime startDate,
        DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException(
                "La fecha inicial no puede ser posterior a la fecha final.");

        var sales = await _saleRepository.GetSalesByDateRangeAsync(
            startDate, endDate);

        return sales.Select(MapToResponse);
    }

    public async Task<SaleResponseDto> CreateSaleAsync(
        SaleRequestDto saleRequest)
    {
        if (saleRequest == null)
            throw new ArgumentNullException(nameof(saleRequest));

        if (saleRequest.Details == null || !saleRequest.Details.Any())
            throw new ArgumentException(
                "La venta debe contener al menos un producto.");

        var user = await _userRepository.GetUserByIdAsync(
            saleRequest.IdUser);

        if (user == null)
            throw new KeyNotFoundException(
                $"No se encontró el usuario con ID {saleRequest.IdUser}.");

        var sale = new Sale
        {
            UserId = user.Id,
            User = user,
            CustomerId = saleRequest.IdCustomer,
            Date = DateTime.Now,
            Status = SaleStatus.Pending
        };

        decimal total = 0;

        foreach (var detailRequest in saleRequest.Details)
        {
            if (detailRequest.Quantity <= 0)
                throw new ArgumentException(
                    "La cantidad debe ser mayor a cero.");

            var product = await _productRepository.GetProductByIdAsync(
                detailRequest.IdProduct);

            if (product == null || !product.IsActive)
                throw new KeyNotFoundException(
                    $"No se encontró el producto con ID {detailRequest.IdProduct}.");

            var detail = new SaleDetail
            {
                Quantity = detailRequest.Quantity,
                ProductId = product.Id,
                Product = product,
                Sale = sale,
                UnitPrice = product.SalePrice,
                Subtotal = product.SalePrice * (decimal)detailRequest.Quantity
            };

            total += detail.Subtotal;

            sale.Detail.Add(detail);
        }

        sale.Total = total;

        await _saleRepository.AddSaleAsync(sale);

        return MapToResponse(sale);
    }

    public async Task CancelSaleAsync(int id)
    {
        var sale = await _saleRepository.GetSaleByIdAsync(id);

        if (sale == null)
            throw new KeyNotFoundException(
                $"No se encontró la venta con ID {id}.");

        if (sale.Status != SaleStatus.Pending)
            throw new InvalidOperationException(
                "Solo se pueden cancelar ventas pendientes.");

        sale.Status = SaleStatus.Cancelled;

        await _saleRepository.UpdateSaleAsync(sale);
    }

    private static SaleResponseDto MapToResponse(Sale sale)
    {
        return new SaleResponseDto
        {
            Id = sale.Id,
            Date = sale.Date,
            Total = sale.Total,
            Status = sale.Status,
            IdUser = sale.UserId,
            IdCustomer = sale.CustomerId,

            Details = sale.Detail.Select(detail =>
                new SaleDetailResponseDto
                {
                    Id = detail.Id,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice,
                    Subtotal = detail.Subtotal,
                    IdProduct = detail.ProductId
                }).ToList(),

            Payments = sale.Payment.Select(payment =>
                new PaymentDto
                {
                    Id = payment.Id,
                    Date = payment.Date,
                    Amount = payment.Amount,
                    Status = payment.Status,
                    PaymentMethod = payment.PaymentMethod,
                    IdSale = payment.SaleId
                }).ToList()
        };
    }
}
