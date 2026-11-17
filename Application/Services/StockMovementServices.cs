using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services;

public class StockMovementServices : IStockMovementService
{
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    public StockMovementServices(
        IStockMovementRepository stockMovementRepository,
        IProductRepository productRepository,
        IUserRepository userRepository)
    {
        _stockMovementRepository = stockMovementRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    public async Task<StockMovementDto?> GetStockMovementByIdAsync(int id)
    {
        var sm = await _stockMovementRepository.GetStockMovementByIdAsync(id);
        return sm == null ? null : ToDto(sm);
    }

    public async Task<IEnumerable<StockMovementDto>> GetAllStockMovementsAsync()
    {
        var list = await _stockMovementRepository.GetAllStockMovementsAsync();
        return list.Select(ToDto).ToList();
    }

    public async Task RegisterMovementAsync(
        int productId,
        float quantity,
        StockMovementType type,
        string reason,
        int userId,
        bool allowNegativeStock = false)
    {
        if (quantity < 0 || (quantity == 0 && type != StockMovementType.Adjustment))
            throw new ArgumentException("La cantidad del movimiento debe ser mayor a cero.");

        var product = await _productRepository.GetProductByIdAsync(productId)
            ?? throw new KeyNotFoundException($"No se encontró el producto con ID {productId}.");

        var user = await _userRepository.GetUserByIdAsync(userId)
            ?? throw new KeyNotFoundException($"No se encontró el usuario con ID {userId}.");

        switch (type)
        {
            case StockMovementType.Entry:
                product.CurrentStock += quantity;
                break;

            case StockMovementType.Exit:
                if (!allowNegativeStock && product.CurrentStock < quantity)
                    throw new InvalidOperationException(
                        $"Stock insuficiente para '{product.Name}'. Disponible: {product.CurrentStock}, solicitado: {quantity}.");
                product.CurrentStock -= quantity;
                break;

            case StockMovementType.Adjustment:
                
                product.CurrentStock = quantity;
                break;
        }

        await _productRepository.UpdateProductAsync(product);

        await _stockMovementRepository.AddStockMovementAsync(new StockMovement
        {
            Date = DateTime.Now,
            Quantity = quantity,
            Type = type,
            Reason = reason,
            ProductId = product.Id,
            Product = product,
            UserId = user.Id,
            User = user
        });
    }

    private static StockMovementDto ToDto(StockMovement sm) => new()
    {
        Id = sm.Id,
        Date = sm.Date,
        Quantity = sm.Quantity,
        Type = sm.Type,
        Reason = sm.Reason,
        ProductId = sm.ProductId,
        UserId = sm.UserId
    };
}