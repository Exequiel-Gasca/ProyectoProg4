using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Domain.Interfaces;
namespace Application.Services
{
    public class StockMovementServices
    {
        private readonly IProductRepository _productRepository;
        public StockMovementServices(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task UpdateStock(StockMovement stockMovement)
        {
            Product? product = await _productRepository.GetProductByIdAsync(stockMovement.ProductId);

            if (stockMovement.Quantity > 0) {
                switch (stockMovement.Type)
                {
                    case Domain.Enums.StockMovementType.Entry:
                        product.CurrentStock +=  stockMovement.Quantity;
                        break;
                    case Domain.Enums.StockMovementType.Exit:

                        if (product.CurrentStock - stockMovement.Quantity < 0)
                        {
                            throw new ArgumentException("CurrentStock can't be negative");
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
                throw new ArgumentException("the stockMovement quantity must be greater than 0");
            }
            await _productRepository.UpdateProductAsync(product);

        }
    }
}
