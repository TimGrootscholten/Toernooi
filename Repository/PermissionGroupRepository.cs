using Microsoft.EntityFrameworkCore;
using Migrations;
using Models;

namespace Repositories
{
    public class PermissionGroupRepository : IPermissionGroupRepository
    {
        private readonly TournamentDbContext _dbContext;

        public PermissionGroupRepository(TournamentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PermissionGroup> GetPermissionGroupById(Guid id)
        {
            return await _dbContext.PermissionGroups.FindAsync(id);
        }
        public async Task<List<PermissionGroup>> GetPermissionGroupByIds(List<Guid> ids)
        {
            return await _dbContext.PermissionGroups.AsQueryable()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
        

        public async Task<List<PermissionGroup>> GetPermissionGroups()
        {
            return await _dbContext.PermissionGroups.AsQueryable().ToListAsync();
        }

        public async Task<PermissionGroup> CreatePermissionGroup(PermissionGroup permissionGroup)
        {
            await _dbContext.PermissionGroups.AddAsync(permissionGroup);
            await _dbContext.SaveChangesAsync();
            return permissionGroup;
        }

        public async Task<PermissionGroup> UpdatePermissionGroup(PermissionGroup permissionGroup)
        {
            _dbContext.PermissionGroups.Update(permissionGroup);
            await _dbContext.SaveChangesAsync();
            return permissionGroup;
        }

        public async Task<bool> DeletePermissionGroup(Guid id)
        {
            _dbContext.PermissionGroups.Remove(new PermissionGroup {Id = id});
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }

    public interface IPermissionGroupRepository
    {
        Task<PermissionGroup> GetPermissionGroupById(Guid id);
        Task<List<PermissionGroup>> GetPermissionGroupByIds(List<Guid> ids);
        Task<List<PermissionGroup>> GetPermissionGroups();
        Task<PermissionGroup> CreatePermissionGroup(PermissionGroup permissionGroup);
        Task<PermissionGroup> UpdatePermissionGroup(PermissionGroup permissionGroup);
        Task<bool> DeletePermissionGroup(Guid id);
    }
}