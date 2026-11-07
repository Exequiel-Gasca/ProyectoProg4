using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;

    public SaleService(
        ISaleRepository saleRepository,
        IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
    }

    public async Task<Sale?> GetSaleByIdAsync(int id)
    {
        return await _saleRepository.GetSaleByIdAsync(id);
    }

    public async Task<IEnumerable<Sale>> GetAllSalesAsync()
    {
        return await _saleRepository.GetAllSalesAsync();
    }

    public async Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(
        DateTime startDate,
        DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException(
                "La fecha inicial no puede ser posterior a la fecha final.");

        return await _saleRepository.GetSalesByDateRangeAsync(
            startDate, endDate);
    }

    public async Task<Sale> CreateSaleAsync(Sale sale)
    {
        if (sale.Detail == null || !sale.Detail.Any())
            throw new ArgumentException(
                "La venta debe contener al menos un producto.");

        decimal total = 0;

        foreach (var detail in sale.Detail)
        {
            if (detail.Quantity <= 0)
                throw new ArgumentException(
                    "La cantidad debe ser mayor a cero.");

            var product = await _productRepository
                .GetProductByIdAsync(detail.ProductId);

            if (product == null || !product.IsActive)
                throw new KeyNotFoundException(
                    $"No se encontró el producto con ID {detail.ProductId}.");

            detail.Product = product;
            detail.Sale = sale;

            detail.UnitPrice = product.SalePrice;
            detail.Subtotal = detail.UnitPrice * (decimal)detail.Quantity;

            total += detail.Subtotal;
        }

        sale.Date = DateTime.Now;
        sale.Total = total;
        sale.Status = SaleStatus.Pending;
        sale.User = null!;

        await _saleRepository.AddSaleAsync(sale);

        return sale;
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
}