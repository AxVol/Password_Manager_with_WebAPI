using WebApi.DAL.Interfaces;
using WebApi.Domain.Entity;

namespace WebApi.DAL.Repositories
{
    public class PasswordsRepository : IRepository<Password>
    {
        private readonly AppDbContext context;

        public PasswordsRepository(AppDbContext db)
        {
            context = db;
        }

        public async Task Create(Password entity)
        {
            context.Passwords.Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Password entity)
        {
            context.Passwords.Remove(entity);
            await context.SaveChangesAsync();
        }

        public IEnumerable<Password> GetAll()
        {
            return context.Passwords;
        }

        public async Task Update(Password entity)
        {
            context.Passwords.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
