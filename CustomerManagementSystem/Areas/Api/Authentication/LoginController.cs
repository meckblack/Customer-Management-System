using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CustomerManagementSystem.Helpers;
using CustomerManagementSystem.Models.Entities;
using CustomerManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CustomerManagementSystem.Areas.Api.Authentication
{
    public class LoginController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        /// <summary>
        /// LOGIN
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginPostModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.EmailAddress, 
                model.Password, true, true);

            if (result.IsNotAllowed)
            {
                return BadRequest("User is not allowed to login");
            }

            if (result.Succeeded)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(model.EmailAddress);
                
                LoginViewModel loginViewModel = new LoginViewModel
                {
                    Address = user.Address,
                    Token = await GenerateJsonWebTokenString(user),
                    EmailAddress = user.Email,
                    PhoneNumber = user.PhoneNumber
                };

                return Ok(loginViewModel);
            }

            return BadRequest("Opz something went wrong");
        }

        private async Task<string> GenerateJsonWebTokenString(ApplicationUser user)
        {
            string role = (await _userManager.GetRolesAsync(user))[0];
            byte[] secret = Encoding.ASCII.GetBytes(new AppConfig().TokenSecret);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("user_id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), 
                    SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

    }
}