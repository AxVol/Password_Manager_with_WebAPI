using Microsoft.EntityFrameworkCore;
using System.Collections;
using WebApi.DAL.Interfaces;
using WebApi.Domain.Entity;

namespace WebApi.DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly AppDbContext context;

        public UserRepository(AppDbContext dbContext)
        {
            context = dbContext;
        }

        public async Task Create(User entity)
        {
            await context.Users.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(User entity)
        {
            context.Users.Remove(entity);
            await context.SaveChangesAsync();
        }

        public IEnumerable<User> GetAll()
        {
            return context.Users;
        }

        public async void Update(User entity)
        {
            context.Users.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
