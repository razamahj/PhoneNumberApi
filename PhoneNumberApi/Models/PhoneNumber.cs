namespace PhoneNumberApi.Models
{
    public class PhoneNumber
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public int AccountId { get; set; }
    }
}
