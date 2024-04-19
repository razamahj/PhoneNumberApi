using PhoneNumberApi.Enums;

namespace PhoneNumberApi.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public AccountStatus Status { get; set; }
        public List <PhoneNumber>? PhoneNumbers { get; set; }


    }
}
