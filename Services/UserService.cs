using CRUDinNETCORE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRUDinNETCORE.Services
{
    public class UserService: IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly BrandContext db;

        public UserService(IOptions<AppSettings> appSettings, BrandContext _db)
        {
            _appSettings = appSettings.Value;
            db = _db;
        }

        public async Task<User?> AddandUpdateUser(User userObj)
        {
            bool isSuccess = false;
            if (userObj.Id > 0)
            {
                var obj = await db.Users.FirstOrDefaultAsync(c => c.Id == userObj.Id);
                if (obj != null)
                {
                    obj.FirstName = userObj.FirstName;
                    obj.LastName = userObj.LastName;
                    db.Users.Update(obj);
                    isSuccess = await db.SaveChangesAsync() > 0;
                    return obj; // Return the updated user object
                }
            }
            else
            {
                await db.Users.AddAsync(userObj);
                isSuccess = await db.SaveChangesAsync() > 0;
                return userObj; // Return the added user object
            }

            return null; // Return null if no action was taken
        }

        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
        {
            var user = await db.Users.SingleOrDefaultAsync(x => x.Username == model.Username && x.Password == model.Password);

            //return null if user not found
            if (user == null) return null;

            //authentication successful so generate jwt token
            var token = await generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        private async Task<string> generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("Id", user.Id.ToString()), 
                                                         new Claim("Firstname", user.FirstName.ToString()),
                                                         new Claim("Lastname", user.LastName.ToString()),
                                                         new Claim("Username", user.Username.ToString())
                                                       }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });

            return tokenHandler.WriteToken(token);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await db.Users.Where(x => x.IsActive == true).ToListAsync();
        }

        public async Task<User?> GetById(int Id)
        {
            return await  db.Users.FirstOrDefaultAsync(x => x.Id == Id);
        }
    }
}
