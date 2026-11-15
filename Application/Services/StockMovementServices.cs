using System;
using System.Collections.Generic;
using System.Text;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
namespace Application.Services
{
    public class StockMovementServices : IStockMovementService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockMovementRepository _stockMovementRepository;
        public StockMovementServices(IStockMovementRepository stockMovement ,IProductRepository product)
        {
            _stockMovementRepository = stockMovement;
            _productRepository = product;
        }
        public async Task<StockMovementDto?> GetStockMovementByIdAsync(int id)
        {
            var stockMovement = await _stockMovementRepository.GetStockMovementByIdAsync(id);
            if(stockMovement == null)
            {
                throw new KeyNotFoundException($"No se encontro movimiento de stock con el id {id}");
            }
            return new StockMovementDto
            {
                Id = stockMovement.Id,
                Date = stockMovement.Date,
                Quantity = stockMovement.Quantity,
                Type = stockMovement.Type,
                Reason = stockMovement.Reason,
                ProductId = stockMovement.ProductId,
                UserId = stockMovement.UserId
            };
        }
        public async Task <IEnumerable<StockMovementDto>> GetAllStockMovementsAsync()
        {
            var stockMovement = await _stockMovementRepository.GetAllStockMovementsAsync();
            var stockMovementDto = stockMovement.Select(sm => new StockMovementDto
            {
                Id = sm.Id,
                Date = sm.Date,
                Quantity = sm.Quantity,
                Type = sm.Type,
                Reason = sm.Reason,
                ProductId = sm.ProductId,
                UserId = sm.UserId
            })
            .ToList();
            return stockMovementDto;
        }
        public async Task<StockMovementDto?>AddStockMovementAsync(StockMovement stockMovement){
            throw new Exception("Tarea en curso");
        }
        public async Task<StockMovementDto> UpdateStockMovementAsync(int id, StockMovement stockMovement)
        {
            var _stockMovement = await _stockMovementRepository.GetStockMovementByIdAsync(id);
            _stockMovement.Date = stockMovement.Date;
            _stockMovement.Quantity = stockMovement.Quantity;
            _stockMovement.Type = stockMovement.Type;
            _stockMovement.Reason = stockMovement.Reason;
            _stockMovement.ProductId = stockMovement.ProductId;
            _stockMovement.Product = stockMovement.Product;
            _stockMovement.UserId = stockMovement.UserId;
            _stockMovement.User = stockMovement.User;
            await _stockMovementRepository.UpdateStockMovementAsync(_stockMovement);
            return new StockMovementDto
            {
                Id = id,
                Date = stockMovement.Date,
                Quantity = stockMovement.Quantity,
                Type = stockMovement.Type,
                Reason = stockMovement.Reason,
                ProductId = stockMovement.ProductId,
                UserId = stockMovement.UserId
            };            
        }

        public async Task DeleteStockMovementAsync(int id)
        {
            await _stockMovementRepository.DeleteStockMovementAsync(id);
        }
        public async Task UpdateStock(StockMovement stockMovement)
        {
            Product? product = await _productRepository.GetProductByIdAsync(stockMovement.ProductId);
            if (stockMovement.Quantity < 0) {
                switch (stockMovement.Type)
                {
                    case Domain.Enums.StockMovementType.Entry:
                        product.CurrentStock +=  stockMovement.Quantity;
                        break;
                    case Domain.Enums.StockMovementType.Exit:

                        if (product.CurrentStock - stockMovement.Quantity < 0)
                        {
                            throw new ArgumentException("El stock del producto no puede ser negativo");
                        }
                        else
                        {
                            product.CurrentStock -= product.CurrentStock;
                        }
                        break;
                    case Domain.Enums.StockMovementType.Adjustment:
                        product.CurrentStock = stockMovement.Quantity;
                        break;
                }
            }
            else
            {
                throw new ArgumentException("La cantidad de movimiento de stock debe ser mayor a 0");
            }
            await _productRepository.UpdateProductAsync(product);

        }
    }
}
