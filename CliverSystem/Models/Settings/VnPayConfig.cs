namespace CliverSystem.Models.Settings
{
    public class VnPayConfig
    {
        public static readonly string Name = "VnPayConfig";
        public VnPayConfig()
        {
            Url = "";
            ReturnUrl = "";
            DepositReturnUrl = "";
            TmnCode = "";
            HashSecret = "";
        }
        public string Url { get; set; }
        public string ReturnUrl { get; set; }
        public string DepositReturnUrl { get; set; }
        public string WithdrawReturnUrl { get; set; }
        public string TmnCode { get; set; }
        public string HashSecret { get; set; }

    }
}
