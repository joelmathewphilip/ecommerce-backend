using Amazon.Runtime.Internal.Util;
using Ecommerce.Identity.API.Dto;
using Ecommerce.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ecommerce.Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IRepository _repository;
        public IdentityController(IRepository repository)
        {
            _repository = repository;
        }
        // GET api/<IdentityController>/5
        [HttpGet("refreshtoken")]
        public string RefreshToken(int id)
        {
            return "value";
        }

        // POST api/<IdentityController>
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] LoginDto loginDto)
        {
            ControllerError controllerError = new ControllerError();
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
                return CreatedAtAction(nameof(Register), user.username, user);
                //return Ok("User Registed Successfully");
            }
            catch (Exception ex)
            {
                controllerError.statusCode = 500;
                controllerError.message = ex.Message;
                Response.Headers["Content-Type"] = "application/json";
                return StatusCode(StatusCodes.Status500InternalServerError, controllerError);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            UserIdentity userIdentity = await _repository.FetchRegisteredUsers(loginDto.username);
            if (userIdentity != null)
            {
                byte[] passwordSaltBytes = userIdentity.passwordSalt;
                byte[] passwordHashBytes = userIdentity.passwordHash;
                if (VerifyPasswordHash(loginDto.password, passwordSaltBytes, passwordHashBytes))
                {
                    return Ok("Correct credentials");
                }
                else
                {
                    return Ok("Incorrect Password");
                }
            }
            else
            {
                return Ok("User does not exist");
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

    }
}
