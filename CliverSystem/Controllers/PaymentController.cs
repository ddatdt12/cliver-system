using AutoMapper;
using CliverSystem.Attributes;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.Error;
using CliverSystem.Helper;
using CliverSystem.Models;
using CliverSystem.Models.Settings;
using CliverSystem.Services.Payment;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Net;
using System.Reflection;

namespace CliverSystem.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public PaymentController(IConfiguration configuration, IPaymentService paymentService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _configuration = configuration;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Protect]
        [ProducesResponseType(StatusCodes.Status308PermanentRedirect)]
        [Produces(typeof(string))]
        public async Task<IActionResult> Payment(CreateOrderDto createOrderDto)
        {
            var vnpayConfig = _configuration.GetSection(VnPayConfig.Name).Get<VnPayConfig>();
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var user = HttpContext.Items["User"] as User;
            var newOrder = _mapper.Map<Order>(createOrderDto);
            newOrder.BuyerId = user!.Id;
            await _unitOfWork.Orders.InsertOrder(newOrder, Common.Enum.PaymentMethod.VnPay);


            string paymentUrl = _paymentService.CreateOrderPaymentUrl(vnpayConfig, ipAddress, newOrder);
            return Ok(paymentUrl);
        }


        //[Protect]
        [HttpGet("confirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(IActionResult))]
        public async Task<IActionResult> PaymentConfirm([FromQuery] RequestDto vnpayData)
        {
            var vnpayConfig = _configuration.GetSection(VnPayConfig.Name).Get<VnPayConfig>();
            string hashSecret = vnpayConfig.HashSecret; //Chuỗi bí mật
            var parameters = Request.Query.ToDictionary(k => k.Key, v => Request.Query[v.Value]);

            foreach (var item in Request.Query)
            {
                if (!string.IsNullOrEmpty(item.Key) && item.Key.StartsWith("vnp"))
                {
                    _paymentService.AddResponseData(item.Key, item.Value);
                }
            }

            int orderId = Convert.ToInt16(vnpayData.TxnRef.Split("_").FirstOrDefault()); //mã hóa đơn
            long vnpayTranId = Convert.ToInt64(vnpayData.TransactionNo); //mã giao dịch tại hệ thống VNPAY
            string vnp_ResponseCode = vnpayData.ResponseCode.ToString(); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
            string vnp_SecureHash = vnpayData.SecureHash; //hash của dữ liệu trả về

            bool checkSignature = _paymentService.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

            if (checkSignature)
            {
                if (vnp_ResponseCode == "00")
                {
                    await _unitOfWork.Orders.UpdateOrderPayment(orderId);
                    //Thanh toán thành công
                    return Ok("Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId);
                }
                else
                {
                    //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                    return BadRequest("Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode);
                }
            }
            else
            {
                BadRequest("Có lỗi xảy ra trong quá trình xử lý");
            }

            return BadRequest();
        }
    }

    public class RequestDto
    {

        [FromQuery(Name = "vnp_TmnCode")]
        public string TmnCode { get; set; } = string.Empty;

        [FromQuery(Name = "vnp_Amount")]
        public int Amount { get; set; }
        [FromQuery(Name = "vnp_BankCode")]
        public string BankCode { get; set; } = string.Empty;

        [FromQuery(Name = "vnp_BankTranNo")]
        public string BankTranNo { get; set; } = string.Empty;

        [FromQuery(Name = "vnp_CardType")]
        public string CardType { get; set; } = string.Empty;

        [FromQuery(Name = "vnp_PayDate")]
        public string PayDate { get; set; } = string.Empty;

        [FromQuery(Name = "vnp_OrderInfo")]
        public string OrderInfo { get; set; } = string.Empty;

        [FromQuery(Name = "vnp_TransactionNo")]
        public string TransactionNo { get; set; } = string.Empty;

        [FromQuery(Name = "vnp_ResponseCode")]
        public string ResponseCode { get; set; } = string.Empty;

        [FromQuery(Name = "vnp_TransactionStatus")]
        public string TransactionStatus { get; set; } = string.Empty;

        [FromQuery(Name = "vnp_TxnRef")]
        public string TxnRef { get; set; } = string.Empty;

        [FromQuery(Name = "vnp_SecureHashType")]
        public string? SecureHashType { get; set; }

        [FromQuery(Name = "vnp_SecureHash")]
        public string SecureHash { get; set; } = string.Empty;
    }

}
