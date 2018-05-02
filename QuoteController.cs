using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYPHomecare.Models;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYPHomecare.Controllers
{
    public class QuoteController : Controller
    {
        private AppDbContext _dbContext;

        public QuoteController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            DbSet<Quote> dbs = _dbContext.Quote;
            List<Quote> model = dbs.ToList();

            return View(model);
        }

        public IActionResult Create()
        {
            DbSet<ServiceProvider> dbs = _dbContext.ServiceProvider;
            var lstPokes =
                dbs.ToList<ServiceProvider>()
                   .OrderBy(p => p.ServiceProviderId)
                   .Select(
                       p =>
                       {
                           dynamic d = new ExpandoObject();
                           d.value = p.ServiceProviderId;
                           d.text = p.Name;
                           return d;
                       }
                   )
                   .ToList<dynamic>();
            ViewData["pokes"] = lstPokes;

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Quote newquote)
        {
            ServiceRequest sq = (ServiceRequest)TempData["Info"];
            DbSet<ServiceProvider> dbS = _dbContext.ServiceProvider;
            DbSet<Patient> patient = _dbContext.Patient;
            DbSet<Users> user = _dbContext.Users;
            Quote model = newquote;

            if (ModelState.IsValid)
            {
                
                DbSet<Quote> dbs = _dbContext.Quote;
                var serviceproviderid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ServiceProvider sp = dbS.Where(s => s.ServiceProviderId == Int32.Parse(serviceproviderid)).FirstOrDefault();
                newquote.ServiceProviderId = sp.ServiceProviderId;
                
                Patient test = patient.Where(s => s.PatientId == sq.PatientId).FirstOrDefault();
                Users usertest = user.Where(s => s.UsersId == test.UsersId).FirstOrDefault();

                //DbSet<ServiceRequest> servicerequest = _dbContext.ServiceRequest;
                newquote.UsersId = usertest.UsersId;
        
                dbs.Add(newquote);
                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "New Quote Created!";
                else
                    TempData["Msg"] = "Failed to update database!";
            }
            else
            {
                TempData["Msg"] = "Invalid information entered";
            }
            return RedirectToAction("Index");
        }


    }
}
