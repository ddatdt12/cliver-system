using CliverSystem.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs.User;
using CliverSystem.DTOs;
using AutoMapper;
using CliverSystem.Attributes;
using CliverSystem.Services.Payment;
using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;
using CliverSystem.Error;
using static CliverSystem.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace CliverSystem.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly string API_URL;
        private readonly DataContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration, ILogger<AccountController> logger, IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService, DataContext context)
        {
            API_URL = configuration["API_Url"];
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
            _context = context;
        }

        [HttpGet("info")]
        [Protect]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = HttpContext.Items["UserId"] as string;
            var user = await _unitOfWork.Users.GetUserDetails(userId!);
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(new ApiResponse<UserDto>(userDto, "Get User details"));
        }

        [HttpPut("info")]
        [Protect]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserDto updateUserDto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            await _unitOfWork.Users.UpdateInfo(userId!, updateUserDto);

            return NoContent();
        }

        [HttpGet("wallet")]
        [Protect]
        [Produces(typeof(ApiResponse<WalletDto>))]
        public async Task<IActionResult> GetWalletInfo()
        {
            var userId = HttpContext.Items["UserId"] as string;
            var wallet = await _context.Wallets.Where(w => w.User.Id == userId).FirstOrDefaultAsync();

            //foreach (var item in wallets)
            //{
            //    var earningMoney = await _context.Orders.Where(o => o.SellerId == item.User!.Id
            //    && o.Status == OrderStatus.Completed).SumAsync(o => o.Price);
            //    var pendingMoney = await _context.Orders.Where(o => o.SellerId == item.User!.Id
            //    && o.Status != OrderStatus.Completed && o.Status != OrderStatus.PendingPayment && o.Status == OrderStatus.Cancelled).SumAsync(o => o.Price);
            //    var spentMoney = await _context.Orders.Where(o => o.BuyerId == item.User!.Id
            //    && o.Status != OrderStatus.Cancelled&& o.Status != OrderStatus.PendingPayment).SumAsync(o => o.Price);
            //    var depositMoney = await _context.TransactionHistories.Where(h => h.WalletId == item.Id && h.Type == TransactionType.Deposit).SumAsync(o => o.Amount);
            //    item.AvailableForWithdrawal = earningMoney + depositMoney - spentMoney;
            //    item.Balance = pendingMoney + item.AvailableForWithdrawal;
            //}

            var walletDto = _mapper.Map<WalletDto>(wallet);
            return Ok(new ApiResponse<object>(walletDto, "Get User Wallet"));
        }

        [HttpGet("wallet/history")]
        [Protect]
        [Produces(typeof(ApiResponse<IEnumerable<TransactionHistory>>))]
        public async Task<IActionResult> GetWalletHistory()
        {
            var user = HttpContext.Items["User"] as User;
            var transactions = await _unitOfWork.TransactionHistories.GetTransactions(user!.WalletId);
            var transactionsDto = _mapper.Map<IEnumerable<TransactionHistory>>(transactions);
            return Ok(new ApiResponse<object>(transactionsDto, "Get User Wallet Histories"));
        }

        [HttpPost]
        [Route("verify-seller")]
        public async Task<IActionResult> VerifySeller([FromBody] VerifySellerDto dto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            await _unitOfWork.Users.VerifyAccount(userId!, dto);

            return NoContent();
        }


        [HttpPost("deposit")]
        [Protect]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(string))]
        public IActionResult GetPaymentUrlFromVnPay([FromBody] long amount)
        {
            var vnpayConfig = _configuration.GetSection(VnPayConfig.Name).Get<VnPayConfig>();
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            vnpayConfig.ReturnUrl = vnpayConfig.DepositReturnUrl;

            var user = HttpContext.Items["User"] as User;

            string paymentUrl = _paymentService.CreateVnpayDepositAndWithdrawUrl(vnpayConfig, ipAddress, user!, amount, "Nạp tiền vào tài khoản");
            return Ok(paymentUrl);
        }


        [HttpGet("deposit/confirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(IActionResult))]
        public async Task<IActionResult> PaymentConfirm([FromQuery] RequestDto vnpayData)
        {
            var vnpayConfig = _configuration.GetSection(VnPayConfig.Name).Get<VnPayConfig>();
            string hashSecret = vnpayConfig.HashSecret; //Chuỗi bí mật
            var parameters = Request.Query.ToDictionary(k => k.Key, v => Request.Query[v.Value]);
            vnpayConfig.ReturnUrl = vnpayConfig.DepositReturnUrl;
            foreach (var item in Request.Query)
            {
                if (!string.IsNullOrEmpty(item.Key) && item.Key.StartsWith("vnp"))
                {
                    _paymentService.AddResponseData(item.Key, item.Value);
                }
            }

            string? userId = vnpayData.TxnRef.Split("_").FirstOrDefault(); //user Id
            long vnpayTranId = Convert.ToInt64(vnpayData.TransactionNo); //mã giao dịch tại hệ thống VNPAY
            string vnp_ResponseCode = vnpayData.ResponseCode.ToString(); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
            string vnp_SecureHash = vnpayData.SecureHash; //hash của dữ liệu trả về

            bool checkSignature = _paymentService.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

            if (checkSignature && userId != null)
            {
                if (vnp_ResponseCode == "00")
                {
                    await _unitOfWork.Users.DepositMoneyIntoWallet(userId, vnpayData.Amount / 100);
                    //Thanh toán thành công
                    return Ok("Nạp tiền thành công !");
                }
                else
                {
                    //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                    return BadRequest("Có lỗi xảy ra trong quá trình nạp tiền");
                }
            }
            else
            {
                BadRequest("Có lỗi xảy ra trong quá trình xử lý");
            }

            return BadRequest();
        }


        [HttpPost("withdraw")]
        [Protect]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(string))]
        public async Task<IActionResult> GetWithdrawUrlFromVnPay([Required][FromBody]long amount)
        {
            var vnpayConfig = _configuration.GetSection(VnPayConfig.Name).Get<VnPayConfig>();
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            vnpayConfig.ReturnUrl = vnpayConfig.WithdrawReturnUrl;
            var user = HttpContext.Items["User"] as User;
            var wallet = await _context.Wallets.Where(w => w.User!.Id == user!.Id).FirstOrDefaultAsync();

            if (wallet == null)
            {
                return BadRequest();
            }

            if (wallet.Balance < amount)
            {
                throw new ApiException("Số dư không đủ", 400);
            }

            string paymentUrl = _paymentService.CreateVnpayDepositAndWithdrawUrl(vnpayConfig, ipAddress, user!, amount, "Rút tiền về VNPAY");
            return Ok(paymentUrl);
        }


        [HttpGet("withdraw/confirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(IActionResult))]
        public async Task<IActionResult> WithdrawConfirm([FromQuery] RequestDto vnpayData)
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

            string? userId = vnpayData.TxnRef.Split("_").FirstOrDefault(); //user Id
            long vnpayTranId = Convert.ToInt64(vnpayData.TransactionNo); //mã giao dịch tại hệ thống VNPAY
            string vnp_ResponseCode = vnpayData.ResponseCode.ToString(); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
            string vnp_SecureHash = vnpayData.SecureHash; //hash của dữ liệu trả về

            bool checkSignature = _paymentService.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

            if (checkSignature && userId != null)
            {
                if (vnp_ResponseCode == "00")
                {
                    await _unitOfWork.Users.Withdraw(userId, vnpayData.Amount / 100);
                    //Thanh toán thành công
                    return Ok("Rút tiền thành công !");
                }
                else
                {
                    //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                    return BadRequest("Có lỗi xảy ra trong quá trình nạp tiền");
                }
            }
            else
            {
                BadRequest("Có lỗi xảy ra trong quá trình xử lý");
            }

            return BadRequest();
        }
    }
}
