using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;

        public RegisterModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Validating Email address (Optional)

            // Create the user 
            var user = new IdentityUser
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email
            };

            var result = await this.userManager.CreateAsync(user, RegisterViewModel.Password);
            if (result.Succeeded)
            {
                var confirmationToken = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.PageLink(pageName: "/Account/ConfirmEmail",
                    values: new { userId = user.Id, token = confirmationToken });

                var message = new MailMessage("shreyaspoojary240@gmail.com",
                    user.Email,
                    "Please confirm your email",
                    $"Please click on this link to confirm your email address: {confirmationLink}");

                using (var emailClient = new SmtpClient("smtp-relay.sendinblue.com", 587))
                {
                    emailClient.Credentials = new NetworkCredential(
                        "shreyaspoojary240@gmail.com",
                        "wTjnFpYtJkarW9Ey");

                    await emailClient.SendMailAsync(message);
                }

                return RedirectToPage("/Account/Login");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }

                return Page();
            }
        }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage ="Invalid email address.")]
        public string Email { get; set; }

        [Required]
        [DataType(dataType:DataType.Password)]
        public string Password { get; set; }
    }
}
