using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using VnvcStaffAdmin.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VnvcStaffAdmin.Domain.Model;
using VnvcStaffAdmin.Identity.Constants;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using VnvcStaffAdmin.Identity.Dtos.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace VnvcStaffAdmin.Authen.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IOptions<JwtSetting> _jwtSetting;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JwtSetting> jwtSetting)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _signInManager = signInManager;
            _jwtSetting = jwtSetting;
        }

        [HttpPost]
        [Route("roles/add")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            var appRole = new ApplicationRole { Name = request.Role };
            var createRole = await _roleManager.CreateAsync(appRole);
            var response = ResponseModel.Successed("role created succesfully", createRole);
            return Ok(response);
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ResponseModel.BadRequest(ModelState));

                if (!await _roleManager.RoleExistsAsync(RoleConst.ADMIN))
                {
                    var appRole = new ApplicationRole { Name = RoleConst.ADMIN };
                    await _roleManager.CreateAsync(appRole);
                }

                var result = await RegisterAsync(request, RoleConst.ADMIN);

                return result.Success ? Ok(ResponseModel.Successed()) : BadRequest(ResponseModel.Failed(result.Message));
            }
            catch (Exception ex)
            {
                return Ok(ResponseModel.Exception(ex.Message));
            }
        }

        [HttpPost]
        [Route("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ResponseModel.BadRequest(ModelState));

                if (!await _roleManager.RoleExistsAsync(RoleConst.USER))
                {
                    var appRole = new ApplicationRole { Name = RoleConst.USER };
                    await _roleManager.CreateAsync(appRole);
                }

                var result = await RegisterAsync(request, RoleConst.USER);

                return result.Success ? Ok(ResponseModel.Successed()) : BadRequest(ResponseModel.Failed(result.Message));
            }
            catch (Exception ex)
            {
                return Ok(ResponseModel.Exception(ex.Message));
            }
        }

        private async Task<ResponseModel> RegisterAsync(RegisterRequest request, string role)
        {
            try
            {
                role = role.Normalize();

                var userExists = await _userManager.FindByEmailAsync(request.Email);
                if (userExists != null)
                    return ResponseModel.Failed("User already exists");

                userExists = new ApplicationUser
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    UserName = request.Email,
                };
                var createUserResult = await _userManager.CreateAsync(userExists, request.Password);
                if (!createUserResult.Succeeded)
                    return ResponseModel.Failed($"Create user failed {createUserResult?.Errors?.First()?.Description}");

                var addUserToRoleResult = await _userManager.AddToRoleAsync(userExists, role);
                if (!addUserToRoleResult.Succeeded)
                    return ResponseModel.Failed($"Create user succeeded but could not add user to role {addUserToRoleResult?.Errors?.First()?.Description}");

                return ResponseModel.Successed("User registered successfully");
            }
            catch (Exception ex)
            {
                return ResponseModel.Exception(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResponseModel<LoginResponse>))]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await LoginAsync(request);

            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        private async Task<ResponseModel> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null)
                    return ResponseModel.Failed("Invalid email/password");
                var isMatchPassword = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!isMatchPassword)
                    return ResponseModel.Failed("Invalid email/password");

                var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
                var roles = await _userManager.GetRolesAsync(user);
                var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
                claims.AddRange(roleClaims);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a_very_long_secret_key_with_at_least_32_chars"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddHours(1);

                var token = new JwtSecurityToken(
                    issuer: "https://your-identity-service-url",
                    audience: "https://your-application-url",
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds

                    );

                return ResponseModel.Successed("Login Successful", new LoginResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Message = "Login Successful",
                    Email = user?.Email,
                    Success = true,
                    UserId = user?.Id.ToString()
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi hệ thống");
                return ResponseModel.Exception(e.Message);
            }
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            await _signInManager.SignOutAsync();

            return Ok(ResponseModel.Successed("Logout successfully"));
        }
    }
}