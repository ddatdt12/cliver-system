namespace CliverSystem.DTOs.RequestFeatures
{
    public abstract class PaginationOptions
    {
        const int MAX_LIMIT = 50;
        public int Offset { get; set; } = 0;
        private int _limit = 10;
        public int Limit
        {
            get
            {
                return _limit;
            }
            set
            {
                _limit = (value > MAX_LIMIT) ? MAX_LIMIT : value;
            }
        }
    }
}
