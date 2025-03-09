using Entities.DTO;
using Entities.Models;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        public AccountController(UserManager<ApplicationUser> userManager,IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;   
        }
        [HttpPost("register")]
        public async Task<IActionResult> Registration(RegisterUserDto UserDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = UserDto.UserName;
                user.Email = UserDto.Email;
                IdentityResult result = await userManager.CreateAsync(user, UserDto.Password);
                if (result.Succeeded)
                {
                  var roleResult = await userManager.AddToRoleAsync(user, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok("Account Created Successfully");
                    }
                }
                return BadRequest(result.Errors.FirstOrDefault());
            }            
            return BadRequest(ModelState);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto UserDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(UserDto.UserName);
                if (user != null)
                {
                    bool found = await userManager.CheckPasswordAsync(user, UserDto.Password);
                    if (found)
                    {
                        //var claims = new List<Claim>();
                        ////Claims
                        // claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        // claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        // claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        ////get role
                        //var roles = await userManager.GetRolesAsync(user);
                        //foreach (var Roleitem in roles)
                        //{
                        //    claims.Add(new Claim(ClaimTypes.Role, Roleitem));
                        //}
                        var claims = new[]
                        {
                             new Claim(ClaimTypes.Name, user.UserName),
                             new Claim(ClaimTypes.NameIdentifier, user.Id),
                             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));
                        SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        //Token
                        JwtSecurityToken myToken = new JwtSecurityToken(
                            issuer: config["JWT:ValidIssuer"],
                            audience: config["JWT:ValidVudience"],
                            claims: claims,
                            expires: DateTime.Now.AddHours(2),
                            signingCredentials: signingCred
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(myToken)
                        });
                    }
                }
            }
            return Unauthorized();
        }
    }
}
