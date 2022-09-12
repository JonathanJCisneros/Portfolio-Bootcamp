using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;
namespace Portfolio.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class UserController : Controller
{
    private int? uid
    {
        get
        {
            return HttpContext.Session.GetInt32("UserId");
        }
    }

    private bool loggedIn
    {
        get
        {
            return uid != null;
        }
    }
    private PortfolioContext db;

    public UserController(PortfolioContext context)
    {
        db = context;
    }

    [HttpGet("/login")]
    public IActionResult UserLogin()
    {
        if(loggedIn)
        {
            return RedirectToAction("Dashboard");
        }
        return View("Login");
    }

    [HttpGet("/register")]
    public IActionResult UserRegister()
    {
        if(loggedIn)
        {
            return RedirectToAction("Dashboard");
        }
        return View("Register");
    }

    [HttpGet("/dashboard")]
    public IActionResult Dashboard()
    {
        if(!loggedIn)
        {
            return RedirectToAction("UserLogin");
        }
        List<Inquiry> InquiryList = db.Inquiries.ToList();
        return View("Dashboard", InquiryList);
    }

    [HttpPost("/user/login")]
    public IActionResult Login(UserLogin user)
    {
        if(ModelState.IsValid == false)
        {
            return UserLogin();
        }

        User? dbUser = db.Users.FirstOrDefault(a => a.Email == user.LoginEmail);

        if(dbUser == null)
        {
            ModelState.AddModelError("LoginEmail", "not found");
            return UserLogin();
        } 

        PasswordHasher<UserLogin> hashBrowns = new PasswordHasher<UserLogin>();
        PasswordVerificationResult pwCheck = hashBrowns.VerifyHashedPassword(user, dbUser.Password, user.LoginPassword);

        if(pwCheck == 0)
        {
            ModelState.AddModelError("LoginPassword", "is invalid");
            return UserLogin();
        }

        HttpContext.Session.SetInt32("UserId", dbUser.UserId);
        HttpContext.Session.SetString("Name", dbUser.FirstName);
        return RedirectToAction("Dashboard");
    }

    [HttpPost("/register")]
    public IActionResult Register(User newUser)
    {
        if(db.Users.Any(a => a.Email == newUser.Email))
        {
            ModelState.AddModelError("Email", "is taken");
        }

        if(newUser.AdminPass != 191982)
        {
            ModelState.AddModelError("AdminPass", "is invalid");
        }

        if(ModelState.IsValid == false)
        {
            return UserRegister();
        }


        PasswordHasher<User> hashBrowns = new PasswordHasher<User>();
        newUser.Password = hashBrowns.HashPassword(newUser, newUser.Password);

        db.Users.Add(newUser);
        db.SaveChanges();
        HttpContext.Session.SetInt32("UserId", newUser.UserId);
        HttpContext.Session.SetString("Name", newUser.FirstName);
        return RedirectToAction("Dashboard");
    }

    [HttpGet("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("UserLogin");
    }

    [HttpGet("/inquiry/{inquiryId}/resolve")]
    public IActionResult Resolve(int inquiryId)
    {
        Inquiry? inquiry = db.Inquiries.FirstOrDefault(i => i.InquiryId == inquiryId);
        if(inquiry == null)
        {
            return RedirectToAction("Dashboard");
        }

        inquiry.Status = "Resolved";
        db.Inquiries.Update(inquiry);
        db.SaveChanges();
        return RedirectToAction("Dashboard");
    }

    [HttpGet("/inquiry/{inquiryId}/delete")]
    public IActionResult Delete(int inquiryId)
    {
        Inquiry? inquiry = db.Inquiries.FirstOrDefault(i => i.InquiryId == inquiryId);
        if(inquiry == null)
        {
            return RedirectToAction("Dashboard");
        }

        db.Inquiries.Remove(inquiry);
        db.SaveChanges();
        return RedirectToAction("Dashboard");
    }
}