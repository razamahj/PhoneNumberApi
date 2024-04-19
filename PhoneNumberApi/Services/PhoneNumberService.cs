using PhoneNumberApi.Data;
using PhoneNumberApi.Enums;
using PhoneNumberApi.Interfaces;
using PhoneNumberApi.Models;

namespace PhoneNumberApi.Services
{
    public class PhoneNumberService : IPhoneNumberService
    {
        private readonly ApplicationDbContext _context;
        public PhoneNumberService(ApplicationDbContext context)
        {
            _context = context;
        }
        public PhoneNumber AssignPhoneNumber(int accountId, PhoneNumber phoneNumber)
        {
            var account = _context.Accounts.Find(accountId);
            if (account == null)
            {
                throw new ArgumentException("Account not found");
            }

            var existingPhoneNumber = _context.PhoneNumbers.FirstOrDefault(p => p.Number == phoneNumber.Number);
            if (existingPhoneNumber != null) 
            {
                throw new InvalidOperationException("Phone number is already assigned to another account");
            }

            if(phoneNumber.Number?.Length > 11)
            {
                throw new ArgumentException("Phone number must be no more than 11 characters long");
            }

            phoneNumber.AccountId = accountId;
            _context.PhoneNumbers.Add(phoneNumber);
            _context.SaveChanges();

            return phoneNumber;
        }

        public void AssignPhoneNumberToAccount(int id, int accountId)
        {
            var phoneNumber = _context.PhoneNumbers.Find(id);
            if (phoneNumber == null)
            {
                throw new KeyNotFoundException("Phone Number not found");
            }

            var account = _context.Accounts.Find(accountId);
            if (account == null)
            {
                throw new ArgumentException("Account not found");
            }

            //Check if account is suspended
            if (IsAccountSuspended(accountId))
            {
                throw new InvalidOperationException("Cannot assign phone number to a suspended account");
            }

            phoneNumber.AccountId = accountId;
            _context.SaveChanges();
        }

        public void DeletePhoneNumber(int phoneNumberId)
        {
            var phoneNumber = _context.PhoneNumbers.Find(phoneNumberId);
            if (phoneNumber != null) 
            {
                _context.PhoneNumbers.Remove(phoneNumber);
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException("Phone Number not Found");
            }
        }

        public PhoneNumber GetPhoneNumber(int id)
        {
            var phoneNumber = _context.PhoneNumbers.Find(id);
            if (phoneNumber == null)
            {
                throw new KeyNotFoundException("Phone number not found");
            }

            return phoneNumber;
        }

        public List<PhoneNumber> GetPhoneNumbersForAccount(int accountId)
        {
            var account = _context.Accounts.Find(accountId);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found");
            }

            return _context.PhoneNumbers.Where(p => p.AccountId == accountId).ToList();
        }

        public bool IsAccountSuspended(int accountId)
        {
            var account = _context.Accounts.Find(accountId);
            if (account == null)
            {
                throw new KeyNotFoundException($"Account not found.");
            }

            return account.Status == AccountStatus.Suspended;
        }
    }
}
