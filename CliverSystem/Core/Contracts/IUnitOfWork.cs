using CliverSystem.Core.Contracts;

namespace CliverSystem.Core.Contracts
{
  public interface IUnitOfWork
  {
    IUserRepository Users { get; }
    IAuthRepository Auth { get; }
    IPostRepository Posts { get; }
    ICategoryRepository Categories { get; }
    IOrderRepository Orders { get; }
    Task CompleteAsync();
  }
}
