namespace BlogAPI.Repository.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllAsync();
        public Task<User?> GetByIdAsync(int id);
        public Task<User?> UpdateByIdAsync(int id, User user);
        public Task<User> DeleteByIdAsync(int id);
        public Task<User> CreateAsync(User user);
        public Task<User> AuthenticateUser(UserLoginDto userLogin);
    }
}