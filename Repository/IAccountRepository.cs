using Microsoft.AspNetCore.Identity;
using Rest.Models;

namespace Rest.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAsync(SignUpModel signUpModel);
        Task<string> SignInAsync(SignInModel signInModel);
        Task<List<SignUpModel>> GetUsersAsync();
    }
}
