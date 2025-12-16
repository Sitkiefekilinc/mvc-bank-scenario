using EEZBankServer.Models;
using EEZBankServer.EfCore;
using Microsoft.IdentityModel.Tokens;

namespace EEZBankServer.Services
{
    public interface IEEZBankUserService
    {
        List<UserAccountInfos> GetAllUsers();
        void UpdateUsers(UserAccountInfos? user,List<UserAccountInfos>? users);
        void AddUsers(UserAccountInfos? user, List<UserAccountInfos>? users);
        void DeleteUsers(UserAccountInfos? user, List<UserAccountInfos>? users);
        void Save();

    }
    public class EEZBankUserService : IEEZBankUserService
    {
        private readonly UserDbContext _context;

        public EEZBankUserService(UserDbContext context)
        {
            _context = context;
        }

        public void AddUsers(UserAccountInfos? user, List<UserAccountInfos>? users)
        {
            if (!users.IsNullOrEmpty())
            {
                _context.AddRange(users);
            }
            else
            {
                _context.Users.Add(user);
            }
            _context.SaveChanges();
        }

        public void DeleteUsers(UserAccountInfos? user, List<UserAccountInfos>? users)
        {

        }

        public List<UserAccountInfos> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {

        }

        public void UpdateUsers(UserAccountInfos? user, List<UserAccountInfos>? users)
        {

        }
    }
}
