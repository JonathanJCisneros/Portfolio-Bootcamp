using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;
namespace Portfolio.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class PortfolioController : Controller
{
    private PortfolioContext db;

    public PortfolioController(PortfolioContext context)
    {
        db = context;
    }

    [HttpGet("/")]
    public IActionResult Index()
    {
        return View("PortfolioHome");
    }

    [HttpPost("/inquire")]
    public IActionResult AddInquiry(Inquiry newInquiry)
    {
        if(ModelState.IsValid == false)
        {
            return RedirectToAction("Index");
        }
        db.Inquiries.Add(newInquiry);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
}
