
namespace JHipsterNetSampleApplication.Areas.Identity.v1.Controllers {
    using AutoMapper;
    using JHipsterNet.Mvc.Filters;
    using JHipsterNet.Mvc.Problems;
    using JHipsterNetSampleApplication.Areas.Identity.v1.Models;
    using JHipsterNetSampleApplication.Domain.Identity;
    using JHipsterNetSampleApplication.Service;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase {
        private readonly ILogger _log;
        private readonly IMailService _mailService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _userMapper;

        public AccountController(ILogger<AccountController> log, UserManager<User> userManager,
            IMapper userMapper, IMailService mailService)
        {
            _log = log;
            _userManager = userManager;
            _userMapper = userMapper;
            _mailService = mailService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ValidateModel]
        public async Task<IActionResult> RegisterAccount([FromBody] RegisterAccountModel model)
        {
            var user = new User {
                UserName = model.UserName,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) {
                return BadRequest(result.Errors);
            }

            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
            // Send an email with this link
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action("ActivateAccount", "Account", new { email = user.Email, token = token });

            await _mailService.SendActivationEmail(model.Email, "Confirm your account",
                "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");

            return CreatedAtAction(nameof(GetAccount), user);
        }

        [HttpGet("activate")]
        [AllowAnonymous]
        [ValidateModel]
        public async Task<IActionResult> ActivateAccount([FromQuery(Name = "email")] string email, [FromQuery(Name = "token")] string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) {
                return BadRequest();
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded) {
                return BadRequest();
            }

            return Redirect("~/");
        }

        [HttpGet()]
        [Authorize]
        public async Task<ActionResult<UserAccountModel>> GetAccount()
        {
            var user = await this._userManager.FindByNameAsync(User.Identity.Name);

            if(user == null) {
                this._log.LogWarning("Failed to get the account whild the user was not found.");
                return BadRequest();
            }

            return Ok(new UserAccountModel());
        }

        [HttpPost()]
        [ValidateModel]
        [Authorize]
        public async Task<ActionResult> SaveAccount([FromBody] UpdateAccountModel userDto)
        {
            var existingUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (existingUser == null) {
                this._log.LogWarning("Failed to get the account whild the user was not found.");
                return BadRequest();
            }

            var userByEmail = await this._userManager.FindByEmailAsync(userDto.Email);

            if (userByEmail != null &&
                !string.Equals(existingUser.Id, userByEmail.Id, StringComparison.InvariantCultureIgnoreCase))
                throw new EmailAlreadyUsedException();

            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.ImageUrl = userDto.ImageUrl;
            existingUser.LangKey = userDto.LangKey;
            existingUser.Email = userDto.Email;

            var result = await this._userManager.UpdateAsync(existingUser);

            if (!result.Succeeded) {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpPost("change-password")]
        [ValidateModel]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel passwordChangeDto)
        {
            var user = await this._userManager.FindByNameAsync(User.Identity.Name);

            if(user == null) {
                this._log.LogWarning("Failed to update the account password since the user was not found.");
                return BadRequest();
            }

            var result = await this._userManager.ChangePasswordAsync(user, passwordChangeDto.CurrentPassword, passwordChangeDto.NewPassword);

            if (!result.Succeeded) {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpPost("reset-password/init")]
        [AllowAnonymous]
        public async Task<ActionResult> RequestPasswordReset([FromBody] string email)
        {
            var user = await this._userManager.FindByEmailAsync(email);

            if(user == null) {
                this._log.LogWarning("Failed to reset the account password since the user was not found.");
                return Ok();
            }

            var token = await this._userManager.GeneratePasswordResetTokenAsync(user);

            await _mailService.SendPasswordResetMail(user, token);
            return Ok();
        }

        [HttpPost("reset-password/finish")]
        [AllowAnonymous]
        [ValidateModel]
        public async Task<IActionResult> RequestPasswordReset([FromForm(Name = "email")] string email, [FromForm(Name = "token")] string token, [FromForm] string newPassword)
        {
            var user = await this._userManager.FindByEmailAsync(email);

            if (user == null) {
                this._log.LogWarning("Failed to reset the account password since the user was not found.");
                return Ok();
            }

            var result = await this._userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded) {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
    }
}
