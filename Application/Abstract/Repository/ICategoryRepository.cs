using Domain.Entities;

namespace Application.Abstract.Repository;

public interface ICategoryRepository:IRepository<Category>
{
    Task<List<Category>> GetActiveCategoriesAsync();
    
}