using DataSource.Model;

namespace DataSource
{
    public interface IUserRepository
    {
        private string _connectionString { get { return null; } }

        public Task Insert(User model);

        public Task Delete(User model);

        public Task<bool> SearchByLoginAndValues(User model);

        public Task<User> SearchByLogin(string login);

    }
}
