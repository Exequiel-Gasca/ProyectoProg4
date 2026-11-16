using Application.DTO_s.MercadoPago;

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
        Task<OrderResponse?> CreateOrderAsync(OrderRequest request);
        Task<OrderResponse?> GetOrderByIdAsync(string orderId);
        Task<OrderResponse?> CancelOrderAsync(string orderId);
        Task<OrderResponse?> RefundOrderTotalAsync(string orderId);
        Task<OrderResponse?> RefundOrderPartialAsync(string orderId, PartialRefundRequest request);
        bool ValidateWebhookSignature(string xSignature, string xRequestId, string dataId);
        Task ProcessWebhookAsync(WebhookNotificationDto notification);
    }
}