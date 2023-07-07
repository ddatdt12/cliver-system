namespace CliverSystem.DTOs.User
{
    public class WalletDto
    {
        public WalletDto()
        {
        }
        public int Id { get; set; }
        public long Balance { get; set; }
        public long NetIncome { get; set; }
        public long Withdrawn { get; set; }
        public long UsedForPurchases { get; set; }
        public long PendingClearance { get; set; }
        public long AvailableForWithdrawal { get; set; }
        public long ExpectedEarnings { get; set; }
    }
}
