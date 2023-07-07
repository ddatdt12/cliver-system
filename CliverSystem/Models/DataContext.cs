using CliverSystem.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CliverSystem.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
            modelBuilder.ApplyConfiguration(new PackageConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());

            modelBuilder.ApplyConfiguration(new RecentPostConfiguration());
            modelBuilder.ApplyConfiguration(new SavedSellerConfiguration());
            modelBuilder.ApplyConfiguration(new SavedServiceConfiguration());
            modelBuilder.ApplyConfiguration(new ParametersConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Subcategory> Subcategories => Set<Subcategory>();
        public DbSet<Package> Packages => Set<Package>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderHistory> OrderHistories => Set<OrderHistory>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<RoomMember> RoomMembers => Set<RoomMember>();
        public DbSet<RecentPost> RecentPosts => Set<RecentPost>();
        public DbSet<SavedList> SavedLists => Set<SavedList>();
        public DbSet<SavedSeller> SavedSellers => Set<SavedSeller>();
        public DbSet<SavedService> SavedServices => Set<SavedService>();
        public DbSet<Wallet> Wallets => Set<Wallet>();
        public DbSet<Parameters> Parameters  => Set<Parameters>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();
        public DbSet<TransactionHistory> TransactionHistories => Set<TransactionHistory>();
        public DbSet<Resource> Resources => Set<Resource>();

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is AuditEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow; // current datetime

                if (entity.State == EntityState.Added)
                {
                    ((AuditEntity)entity.Entity).CreatedAt = now;
                }
                ((AuditEntity)entity.Entity).UpdatedAt = now;
            }
        }
    }
}
