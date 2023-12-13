#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        readonly SignInManager<AppUser> signInManager;
        readonly UserManager<AppUser> userManager;
        readonly ILogger<RegisterModel> logger;
        readonly IEmailSender emailSender;

        public RegisterModel(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.logger = logger;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "Длина имени должна быть не меньше {2} и не больше {1} символов.")]
            [Display(Name = "Имя")]
            public string Name { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "Длина фамилии должна быть не меньше {2} и не больше {1} символов.")]
            [Display(Name = "Фамилия")]
            public string Surname { get; set; }

            [StringLength(100, MinimumLength = 2, ErrorMessage = "Длина отчества должна быть не меньше {2} и не больше {1} символов.")]
            [Display(Name = "Отчество")]
            public string MiddleName { get; set; }

            [Required]
            [StringLength(128, MinimumLength = 8, ErrorMessage = "Длина пароля должна быть не меньше {2} и не больше {1} символов.")]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Повторите пароль")]
            [Compare("Password", ErrorMessage = "Введённые пароли не совпадают.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Name = Input.Name,
                    Surname = Input.Surname,
                };

                var result = await userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    logger.LogInformation("Создан новый пользователь.");

                    await userManager.AddToRoleAsync(user, "User");

                    var userId = await userManager.GetUserIdAsync(user);
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await emailSender.SendEmailAsync(Input.Email, "Подтвердите свой адрес электронной почты.",
                        $"Для подтверждения адреса электронной почты нажмите <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>здесь</a>.");

                    if (userManager.Options.SignIn.RequireConfirmedAccount)
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    else
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);

                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}