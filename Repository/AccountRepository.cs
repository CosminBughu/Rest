using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rest.Data;
using Rest.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rest.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        //private readonly ApplicationContext context;
        public AccountRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel signUpModel)
        {
            var user = new IdentityUser()
            {
                UserName = signUpModel.UserName,
                Email = signUpModel.Email,
            };

           return await _userManager.CreateAsync(user, signUpModel.Password);
        }

        public async Task<string> SignInAsync(SignInModel signInModel)
        {
            var user = _userManager.FindByEmailAsync(signInModel.Email).Result;
            var result = await _signInManager.PasswordSignInAsync(user.UserName, signInModel.Password, false, false);
            if (!result.Succeeded)
            {
                return null;
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, signInModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<List<SignUpModel>> GetUsersAsync()
        {
            var users = await _userManager.Users.Select(x => new SignUpModel()
            {
                UserName = x.UserName,
                Email = x.Email,
                Password = x.PasswordHash,
                ConfirmPassword = x.PasswordHash
            }).ToListAsync();

            return users;
        }

    }
}
