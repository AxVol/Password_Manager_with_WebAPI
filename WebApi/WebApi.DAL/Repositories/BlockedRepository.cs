using Microsoft.EntityFrameworkCore;
using WebApi.DAL.Interfaces;
using WebApi.Domain.Entity;

namespace WebApi.DAL.Repositories
{
    public class BlockedRepository : IRepository<BlockedUser>
    {
        private readonly AppDbContext context;

        public BlockedRepository(AppDbContext dbContext)
        {
            context = dbContext;
        }

        public async Task Create(BlockedUser entity)
        {
            await context.BlockedUsers.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(BlockedUser entity)
        {
            context.BlockedUsers.Remove(entity);
            await context.SaveChangesAsync();
        }

        public IEnumerable<BlockedUser> GetAll()
        {
            return context.BlockedUsers.Include(u => u.user);
        }

        public Task Update(BlockedUser entity)
        {
            throw new NotSupportedException();
        }
    }
}
