
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class SaleDetailService : ISaleDetailService
{
    private readonly ISaleDetailRepository _saleDetailRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;

    public SaleDetailService(
        ISaleDetailRepository saleDetailRepository,
        ISaleRepository saleRepository,
        IProductRepository productRepository)
    {
        _saleDetailRepository = saleDetailRepository;
        _saleRepository = saleRepository;
        _productRepository = productRepository;
    }

    public async Task<SaleDetail?> GetSaleDetailByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del detalle no es válido.");

        return await _saleDetailRepository.GetSaleDetailByIdAsync(id);
    }

    public async Task<IEnumerable<SaleDetail>> GetAllSaleDetailsAsync()
    {
        return await _saleDetailRepository.GetAllSaleDetailsAsync();
    }

    public async Task AddSaleDetailAsync(SaleDetail saleDetail)
    {
        if (saleDetail == null)
            throw new ArgumentNullException(nameof(saleDetail));

        if (saleDetail.Quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero.");

        var sale =
            await _saleRepository.GetSaleByIdAsync(saleDetail.IdSale);

        if (sale == null)
            throw new KeyNotFoundException("No se encontró la venta.");

        var product =
            await _productRepository.GetProductByIdAsync(saleDetail.ProductId);

        if (product == null || !product.IsActive)
            throw new KeyNotFoundException(
                "El producto no existe o está inactivo.");

        saleDetail.Sale = sale;
        saleDetail.Product = product;
        saleDetail.UnitPrice = product.SalePrice;
        saleDetail.Subtotal =
            (decimal)saleDetail.Quantity * saleDetail.UnitPrice;

        await _saleDetailRepository.AddSaleDetailAsync(saleDetail);
    }

    public async Task UpdateSaleDetailAsync(SaleDetail saleDetail)
    {
        if (saleDetail == null)
            throw new ArgumentNullException(nameof(saleDetail));

        if (saleDetail.Id <= 0)
            throw new ArgumentException("El ID del detalle no es válido.");

        if (saleDetail.Quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero.");

        var existingDetail =
            await _saleDetailRepository.GetSaleDetailByIdAsync(saleDetail.Id);

        if (existingDetail == null)
            throw new KeyNotFoundException("No se encontró el detalle.");

        var product =
            await _productRepository.GetProductByIdAsync(saleDetail.ProductId);

        if (product == null || !product.IsActive)
            throw new KeyNotFoundException(
                "El producto no existe o está inactivo.");

        existingDetail.Quantity = saleDetail.Quantity;
        existingDetail.ProductId = product.Id;
        existingDetail.Product = product;
        existingDetail.UnitPrice = product.SalePrice;
        existingDetail.Subtotal =
            (decimal)saleDetail.Quantity * product.SalePrice;

        await _saleDetailRepository.UpdateSaleDetailAsync(existingDetail);
    }

    public async Task DeleteSaleDetailAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del detalle no es válido.");

        var detail =
            await _saleDetailRepository.GetSaleDetailByIdAsync(id);

        if (detail == null)
            throw new KeyNotFoundException("No se encontró el detalle.");

        await _saleDetailRepository.DeleteSaleDetailAsync(id);
    }
}
