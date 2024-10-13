using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using TicketMasterMVC.Models;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailSender _emailSender;

    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var otp = GenerateOTP(); // Implement this method to generate a random OTP
                await SendOtpEmail(model.Email, otp); // Implement this method to send OTP email

                // Store OTP in the user claims for verification
                await _userManager.AddClaimAsync(user, new Claim("OTP", otp));

                // Redirect to a page where user enters the OTP
                return RedirectToAction("VerifyOtp", new { email = model.Email });
            }
            AddErrors(result);
        }
        return View(model);
    }

    // Verification of OTP
    public async Task<IActionResult> VerifyOtp(string email, string otp)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var userOtp = await _userManager.GetClaimsAsync(user);
        var storedOtp = userOtp.FirstOrDefault(c => c.Type == "OTP")?.Value;

        if (storedOtp == otp)
        {
            await _userManager.RemoveClaimAsync(user, new Claim("OTP", storedOtp));
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError("", "Invalid OTP");
        return View();
    }

    private string GenerateOTP()
    {
        // Generate a random OTP code
        return new Random().Next(100000, 999999).ToString();
    }

    private async Task SendOtpEmail(string email, string otp)
    {
        // Example using SmtpClient for sending OTP email (Replace with your email service)
        var smtpClient = new SmtpClient("smtp.example.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("your-email@example.com", "your-password"),
            EnableSsl = true,
        };
        var mailMessage = new MailMessage
        {
            From = new MailAddress("your-email@example.com"),
            Subject = "Your OTP Code",
            Body = $"Your OTP code is {otp}",
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage);
    }
    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

}
