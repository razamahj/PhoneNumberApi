using PhoneNumberApi.Models;

namespace PhoneNumberApi.Interfaces
{
    public interface IPhoneNumberService
    {
        PhoneNumber AssignPhoneNumber(int accountId, PhoneNumber phoneNumber);
        PhoneNumber GetPhoneNumber(int id);
        void AssignPhoneNumberToAccount(int phoneNnumberid, int accountId);
        void DeletePhoneNumber(int phoneNumberId);
        List<PhoneNumber> GetPhoneNumbersForAccount(int accountId);
    }
}
