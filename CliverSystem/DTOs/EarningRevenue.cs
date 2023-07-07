namespace CliverSystem.DTOs
{
    public class EarningRevenue
    {
        public double CancelledOrders { get; set; }
        public double PendingClearance { get; set; }
        public double Withdrawn { get; set; }
        public double UsedForPurchases { get; set; }
        public double Cleared { get; set; }
    }
}
