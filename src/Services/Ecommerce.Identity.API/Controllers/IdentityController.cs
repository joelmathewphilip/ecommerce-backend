using Ecommerce.Identity.API.Dto;
using Ecommerce.Shared;
using Ecommerce.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Ecommerce.Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IConfiguration _config;
        private readonly ILogger<IdentityController> _logger;
        private ControllerError _controllerError;
        public IdentityController(IRepository repository, IConfiguration config, ILogger<IdentityController> logger)
        {
            _repository = repository;
            _config = config;
            _logger = logger;
            _controllerError = new ControllerError();
        }
        // GET api/<IdentityController>/5
        [HttpGet("refreshtoken")]
        public string RefreshToken(int id)
        {
            return "value";
        }

        // POST api/<IdentityController>
        [HttpPost("register")]
        public async Task<ActionResult> UserRegister([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation("Started executing " + nameof(UserRegister) + " method");
            try
            {
                byte[] passwordKey;
                byte[] passwordHash;
                CreatePasswordHash(loginDto.password, out passwordKey, out passwordHash);
                UserIdentity user = new UserIdentity()
                {
                    username = loginDto.username,
                    userid = Guid.NewGuid(),
                    passwordSalt = passwordKey,
                    passwordHash = passwordHash
                };
                await _repository.AddData(user);
                _logger.LogInformation("Finished executing" + nameof(UserRegister));
                return CreatedAtAction(nameof(UserRegister), user.username, user);
                //return Ok("User Registed Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occured while executing:" + nameof(UserRegister));
                _controllerError.statusCode = 500;
                _controllerError.message = ex.Message;
                Response.Headers["Content-Type"] = "application/json";
                return StatusCode(StatusCodes.Status500InternalServerError, _controllerError);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            ControllerError controllerError = new ControllerError();
            DateTime validTill;
            string token = string.Empty;
            try
            {
                _logger.LogDebug("Started executing method:" + nameof(Login));
                UserIdentity userIdentity = await _repository.FetchRegisteredUsers(loginDto.username);
                if (userIdentity != null)
                {
                    byte[] passwordSaltBytes = userIdentity.passwordSalt;
                    byte[] passwordHashBytes = userIdentity.passwordHash;
                    if (VerifyPasswordHash(loginDto.password, passwordSaltBytes, passwordHashBytes))
                    {
                        if (loginDto.username.Equals("admin"))
                        {
                            token = CreateToken(loginDto.username, out validTill, isAdmin: true);
                        }
                        else
                        {
                            token = CreateToken(loginDto.username, out validTill);
                        }
                        _logger.LogDebug("Finished executing method:" + nameof(Login));
                        var httpCookie = new Microsoft.AspNetCore.Http.CookieOptions
                        {
                            HttpOnly = true
                        };
                        Response.Cookies.Append("AuthCookie", token, httpCookie);
                        return Ok(new TokenDto() { jwtToken = token, ValidTill = validTill });
                    }
                    else
                    {
                        controllerError.message = "Incorrect Password";
                        controllerError.statusCode = 401;
                        _logger.LogDebug("Finished executing method:" + nameof(Login));
                        return StatusCode(StatusCodes.Status401Unauthorized, controllerError);
                    }
                }
                else
                {
                    controllerError.message = "User does not exist";
                    controllerError.statusCode = 404;
                    _logger.LogDebug("Finished executing method:" + nameof(Login));
                    return StatusCode(StatusCodes.Status404NotFound, controllerError);
                }

            }
            catch (Exception exception)
            {
                _controllerError.statusCode = 500;
                _controllerError.message = exception.Message;
                _logger.LogError("An error occured while running the method:" + nameof(Login));
                return StatusCode(StatusCodes.Status500InternalServerError, _controllerError);
            }
        }


        private void CreatePasswordHash(string password, out byte[] passwordKey, out byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                byte[] generatedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return generatedHash.SequenceEqual(passwordHash);
            }
        }


        private string CreateToken(string username, out DateTime validTill, bool isAdmin = false)
        {
            try
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,username),
                    new Claim(ClaimTypes.Role, isAdmin ? "admin" : "nonadmin"),
                    //new Claim(JwtRegisteredClaimNames.Aud,"ecommerce.catalog.api")
                };
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config[Constants.IdentitySecretKeySettingName]));

                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                validTill = DateTime.UtcNow.AddHours(3);

                var token = new JwtSecurityToken(
                    claims: claims,
                    issuer: _config[Constants.IdentityIssuerSettingName],
                    expires: validTill,
                    signingCredentials: cred
                    );
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return jwt;
            }
            catch (Exception)
            {
                _logger.LogError("An error occured while running" + nameof(CreateToken));
                throw;
            }

        }

    }
}
