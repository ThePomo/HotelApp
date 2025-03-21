namespace HotelPrenotazioni.Models
{
    public class UserWithRoles
    {
        public ApplicationUser User { get; set; }
        public IList<string> Roles { get; set; }
    }

}
