namespace RanljivostiSpletneStrani.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Ranljivo v string
        public string Email { get; set; }
        public string CreditCard { get; set; } 

    }
}
