using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using ToDo_List.BLL.Interfaces;
using ToDo_List.PL.ViewModels;

namespace ToDo_List.PL.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IWebHostEnvironment _env;

        public AuthController
            (UserManager<IdentityUser> userManager ,
            IMapper mapper ,
            IEmailSender  emailSender ,
            SignInManager<IdentityUser> signInManager ,
            IWebHostEnvironment env
            )
        {
           _userManager = userManager;
            _mapper = mapper;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _env = env;
        }

        #region SignUp

        public IActionResult SignUp()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel createdUser)
        {
            if (ModelState.IsValid)
            {
                // validate if user already exists
                var existingUser = await _userManager.FindByEmailAsync(createdUser.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "User with this email already exists");
                    return View(createdUser);
                }

                //create new user 
                var user = new IdentityUser
                {
                    UserName = createdUser.UserName,
                    Email = createdUser.Email
                };
                var result = await _userManager.CreateAsync(user, createdUser.Password);

                // make the link and send it 
                if (result.Succeeded)
                {
                   return await GenerateEmailConfirmationAsync(user);
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View(createdUser);
        }


        // Generate email confirmation link and send email with template
        public async Task<IActionResult> GenerateEmailConfirmationAsync(IdentityUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = Url.Action("ConfirmEmail", "Auth",
                new { userId = user.Id, token = token }, Request.Scheme);

            var templatePath = Path.Combine(_env.ContentRootPath, "Views", "Auth", "LinkEmailPage.cshtml");

            if (!System.IO.File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Email template not found: {templatePath}");
            }

            var emailHtml = await System.IO.File.ReadAllTextAsync(templatePath);

            emailHtml = emailHtml.Replace("{{UserName}}", user.UserName)
                                 .Replace("{{ConfirmationLink}}", confirmationLink)
                                 .Replace("{{Confirm}}", "Confirm Email");

            try
            {
                await _emailSender.SendEmailAsync(user.Email, "Confirm your email", emailHtml);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to send confirmation email. Please try again later.");
            }

            return RedirectToAction(nameof(CheckYourInbox), new { actionName = "ResendLink" });
        }

        // Confirm email action
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest("Invalid email confirmation request.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return View("EmailConfirmed");

            return BadRequest("Email confirmation failed.");
        }



        // Resend link view

        public IActionResult ResendLink()
        {
            return View(new ResendLinkViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendLink(ResendLinkViewModel model)
        {
            if (model is null)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email not found.");
                return View(nameof(ResendLink));
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Email already confirmed.");
                return View(model);
            }

            return await GenerateEmailConfirmationAsync(user);

        }

        #endregion

        #region CheckYourInbox
        public IActionResult CheckYourInbox(string actionName)
        {
            TempData["ActionName"] = actionName;    
            return View();
        }
        #endregion

        #region SignIn

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View(model);
                }

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError("", "Please confirm your email before signing in.");
                    return View(model);
                }

                var result = _signInManager.PasswordSignInAsync(user ,model.Password ,model.RememberMe , false);
                if (result.Result.Succeeded)
                {
                    return RedirectToAction(nameof(Index), "Tasks");
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }


        // Forget Password Methods
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //check if user exists
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return View();
                }

                //generate reset token
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Action("ResetPassword", "Auth",
                    new { email = user.Email, token = token }, Request.Scheme);

                var templatePath = Path.Combine(Directory.GetCurrentDirectory(),
                     "Views", "Auth", "LinkEmailPage.cshtml");

                var emailHtml = await System.IO.File.ReadAllTextAsync(templatePath);

                emailHtml = emailHtml.Replace("{{UserName}}", user.UserName)
                                 .Replace("{{ConfirmationLink}}", resetLink)
                                 .Replace("{{Confirm}}" , "Reset Password");

                // Send email
                try
                {
                    await _emailSender.SendEmailAsync(model.Email, "Reset your password", emailHtml);
                    ViewBag.Message = "If an account with that email exists, a password reset link has been sent.";

                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Failed to send reset email. Please try again later.");
                    return View(model);
                }

                return RedirectToAction(nameof(CheckYourInbox), new { actionName = "ForgetPassword" });

            }
            else
            {
                return View(model);
            }
            
        }


        // Reset Password Methods
        public IActionResult ResetPassword(string email, string token)
        {
                if (email == null || token == null)
            {
                return BadRequest("Invalid password reset request.");
            }
            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction(nameof(SignIn));
                }

                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                if (result.Succeeded)
                {
                    return View(nameof(SignIn));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        #endregion

        #region SignOut

        public async new Task<IActionResult> SignOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction(nameof(SignIn));
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
