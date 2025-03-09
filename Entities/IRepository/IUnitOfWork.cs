namespace Entities.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IApplicationUserRepository ApplicationUser { get; } 
        int Complete();
    }
}
