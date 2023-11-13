using BlogAPI.Repository.Interfaces;

namespace BlogAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext db;
        public UserRepository(AppDbContext _db)
        {
            db = _db;
        }

        public async Task<User> CreateAsync(User user)
        {
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return user;
        }

        public async Task<User> DeleteByIdAsync(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                return null;
            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = await db.Users.ToListAsync();
            return users;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var user = await db.Users.SingleOrDefaultAsync(u => u.UserId == id);
            return user;
        }

        public async Task<User?> UpdateByIdAsync(int id, User user)
        {
            var existingUser = await db.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (existingUser == null)
            {
                return null;
            }

            // Update the properties of the existingUser with values from userDto
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;

            await db.SaveChangesAsync();
            return existingUser;
        }
    }
}