using PhoneNumberApi.Data;
using PhoneNumberApi.Enums;
using PhoneNumberApi.Interfaces;
using PhoneNumberApi.Models;

namespace PhoneNumberApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Account CreateAccount(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
            return account;
        }

        public Account GetAccount(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found");
            }

            return account;
        }

        public List<Account> GetAccounts()
        {
            return _context.Accounts.ToList();
        }

        public void ToggleAccountStatus(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found");
            }

            account.Status = account.Status switch
            {
                AccountStatus.Active => AccountStatus.Suspended,
                AccountStatus.Suspended => AccountStatus.Active,
                _ => account.Status
            };

            _context.SaveChanges();
        }
    }
}
