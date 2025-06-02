using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyApiNetCore6.Data;
using MyApiNetCore6.Helpers;
using MyApiNetCore6.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyApiNetCore6.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInAsync;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager) 
        {
            this.userManager = userManager;
            this.signInAsync = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
        }
        public async Task<string> SignInAsync(SignInModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            var passwordCheck = await userManager.CheckPasswordAsync(user, model.Password);
            if (user == null || !passwordCheck)
            {
                return string.Empty;
            }
            var result = await signInAsync.PasswordSignInAsync(model.Email, model.Password, false, false);
            
            if(!result.Succeeded)
            {
                return string.Empty;
            }

            var authClaim = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(30),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if(!await roleManager.RoleExistsAsync(AppRole.Customer))
                {
                    await roleManager.CreateAsync(new IdentityRole(AppRole.Customer));
                }

                await userManager.AddToRoleAsync(user, AppRole.Customer);
            }
            return result;
        }
    }
}
