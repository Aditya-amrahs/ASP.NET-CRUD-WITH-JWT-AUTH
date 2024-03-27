namespace CRUDinNETCORE.Models
{
    public class AuthenticateResponse
    {
        public int  Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }

        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            Firstname = user.FirstName;
            Lastname = user.LastName;
            Username = user.Username;
            Token = token;
        }
    }
}
