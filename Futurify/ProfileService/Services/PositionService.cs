using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProfileService.IServiceInterfaces;
using ProfileService.Model;

namespace ProfileService.Services
{
    public class PositionService : IPositionService
    {
        private ProfileContext _ProfileContext;
        public PositionService(ProfileContext context)
        {
            _ProfileContext = context;
        }

        public async Task<int> CreateAsync(Position position)
        {
            if (position == null)
                throw new ArgumentNullException("position");
            await _ProfileContext.Positions.AddAsync(position);
            return  await _ProfileContext.SaveChangesAsync();
        }
        public async Task<int> UpdateAsync(Position position)
        {
            if (position == null)
                throw new ArgumentNullException("position");
            var existingPosition = await this.GetAsync(position.PositionId);
            if (existingPosition == null)
                throw new NullReferenceException("existing position");
            existingPosition.PositionName = position.PositionName;
            return await _ProfileContext.SaveChangesAsync();
        }
        public async Task<int> RemovePositionAsync(int PositionId)
        {
            var existingPosition = await this.GetAsync(PositionId);
            if (existingPosition == null)
                throw new NullReferenceException("existing position");
            _ProfileContext.Positions.Remove(existingPosition);
            return await _ProfileContext.SaveChangesAsync();
        }
        public async Task<Position> GetAsync(int Id)
        {
            return await _ProfileContext.Positions.FirstOrDefaultAsync(s => s.PositionId == Id);
        }

        public async Task<IEnumerable<Position>> GetAllAsync()
        {
            return await _ProfileContext.Positions.ToListAsync();
        }
    }
}
