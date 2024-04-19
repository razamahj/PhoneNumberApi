using PhoneNumberApi.Models;

namespace PhoneNumberApi.Interfaces
{
    public interface IAccountService
    {
        List<Account> GetAccounts();
        Account CreateAccount(Account account);
        void ToggleAccountStatus(int id);
        Account GetAccount(int id);
    }
}
