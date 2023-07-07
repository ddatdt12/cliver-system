using System.Text.Json.Serialization;

namespace CliverSystem.Common
{
    public static class Enum
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum UserType
        {
            Admin,
            User
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Mode
        {
            Buyer,
            Seller
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum PackageType
        {
            Basic,
            Standard,
            Premium,
            Custom
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum PostStatus
        {
            Draft,
            PendingApproval,
            Active,
            Paused,
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum PaymentMethod
        {
            MyWallet,
            VnPay,
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum OrderStatus
        {
            PendingPayment,
            Created,
            Doing,
            Delivered,
            Completed,
            Cancelled,
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum PostFilter
        {
            Relevance,
            BestSelling,
            NewArrivals,
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum OrderActionType
        {
            Start,
            Cancel,
            Delivery,
            Receive,
            Revision,
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ReviewType
        {
            FromBuyer,
            FromSeller,
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum TransactionType
        {
            Deposit,
            Payment,
            Refund,
            Withdrawal,
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum NotificationType
        {
            OrderInfo,
            CustomPackage,
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum PackageStatus
        {
            Available,
            Closed,
            Declined,
            Ordered,
        }
    }
}
