using CliverSystem.Helper;
using CliverSystem.Models;
using CliverSystem.Models.Settings;
using System.Net;
using System.Text;

namespace CliverSystem.Services.Payment
{


    public class PaymentService : IPaymentService
    {

        private SortedList<string, string> _requestData = new SortedList<string, string>(new PaymentCompare());
        private SortedList<string, string> _responseData = new SortedList<string, string>(new PaymentCompare());

        //------------------------REQUEST DATA----------------------------------------
        public void AddRequestData(string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            string queryString = data.ToString();

            baseUrl += "?" + queryString;
            string signData = queryString;
            if (signData.Length > 0)
            {
                signData = signData.Remove(data.Length - 1, 1);
            }
            string vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

            return baseUrl;
        }




        //------------------------RESPONSE DATA----------------------------------------
        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            string? retValue;
            if (_responseData.TryGetValue(key, out retValue))
            {
                return retValue;
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetResponseData()
        {

            StringBuilder data = new StringBuilder();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }
            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }
            foreach (KeyValuePair<string, string> kv in _responseData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            string rspRaw = GetResponseData();
            string myChecksum = Utils.HmacSHA512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        public string CreateOrderPaymentUrl(VnPayConfig vnPayConfig, string? ipAddress, Order order)
        {
            string url = vnPayConfig.Url;
            string returnUrl = vnPayConfig.ReturnUrl;
            string tmnCode = vnPayConfig.TmnCode;
            string hashSecret = vnPayConfig.HashSecret;

            //Order
            if (ipAddress == "::1")
            {
                ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            }


            string DateStr = DateTime.Now.ToString("yyyyMMddHHmmss");

            AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            AddRequestData("vnp_Amount", (order.Price * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            AddRequestData("vnp_CreateDate", DateStr); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            AddRequestData("vnp_IpAddr", ipAddress ?? ""); //Địa chỉ IP của khách hàng thực hiện giao dịch
            AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            AddRequestData("vnp_TxnRef", order.Id.ToString() + "_" + DateStr); //mã hóa đơn

            return CreateRequestUrl(url, hashSecret);
        }
        public string CreateVnpayDepositAndWithdrawUrl(VnPayConfig vnPayConfig, string? ipAddress, User user, long money, string description)
        {
            string url = vnPayConfig.Url;
            string returnUrl = vnPayConfig.ReturnUrl;
            string tmnCode = vnPayConfig.TmnCode;
            string hashSecret = vnPayConfig.HashSecret;

            //Order
            if (ipAddress == "::1")
            {
                ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            }


            string DateStr = DateTime.Now.ToString("yyyyMMddHHmmss");

            AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            AddRequestData("vnp_Amount", (money * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            AddRequestData("vnp_CreateDate", DateStr); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            AddRequestData("vnp_IpAddr", ipAddress ?? ""); //Địa chỉ IP của khách hàng thực hiện giao dịch
            AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            AddRequestData("vnp_OrderInfo", description); //Thông tin mô tả nội dung thanh toán
            AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            AddRequestData("vnp_TxnRef", user.Id.ToString() + "_" + DateStr); //mã hóa đơn

            return CreateRequestUrl(url, hashSecret);
        }
    }
}
