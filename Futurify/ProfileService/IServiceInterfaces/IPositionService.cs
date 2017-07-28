using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProfileService.Model;

namespace ProfileService.IServiceInterfaces
{
    public interface IPositionService
    {
        Task<Position> GetAsync(int Id);
        Task<IEnumerable<Position>> GetAllAsync();
        Task<int> CreateAsync(Position position);
        Task<int> UpdateAsync(Position position);
        Task<int> RemovePositionAsync(int PositionId);
    }
}
