using static CliverSystem.Common.Enum;

namespace CliverSystem.Helper
{
    public static class Constants
    {
        public static List<PostStatus> HIDDEN_POST_STATUSES = new() { PostStatus.Draft, PostStatus.Paused, PostStatus.PendingApproval };
    }
}
