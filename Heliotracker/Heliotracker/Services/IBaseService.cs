namespace Heliotracker.Services;

public interface IBaseService<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
}