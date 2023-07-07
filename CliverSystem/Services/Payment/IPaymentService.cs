using CliverSystem.Models;
using CliverSystem.Models.Settings;

namespace CliverSystem.Services.Payment
{
    public interface IPaymentService
    {
        void AddRequestData(string key, string value);
        void AddResponseData(string key, string value);
        string CreateRequestUrl(string baseUrl, string vnp_HashSecret);
        string GetResponseData(string key);
        bool ValidateSignature(string inputHash, string secretKey);
        string CreateOrderPaymentUrl(VnPayConfig vnPayConfig, string? ipAddress, Order order);
        string CreateVnpayDepositAndWithdrawUrl(VnPayConfig vnPayConfig, string? ipAddress, User user, long money, string description);
    }
}
