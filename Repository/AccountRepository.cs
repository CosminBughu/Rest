using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rest.Data;
using Rest.Models;

namespace Rest.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly ApplicationContext context;
        public AccountRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
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
