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
    public class PatientController : Controller
    {
        private AppDbContext _dbContext;


        public PatientController(AppDbContext dbContext)

        {

            _dbContext = dbContext;

        }
        private Users curUser()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            DbSet<Users> dbs = _dbContext.Users;
            Users user = dbs.Where(s => s.UsersId == Int32.Parse(userid)).FirstOrDefault();
            return user;
        }

        public IActionResult Index()
        {
            DbSet<Patient> dbs = _dbContext.Patient;

            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            DbSet<Users> dbS = _dbContext.Users;
            Users user = dbS.Where(s => s.UsersId == Int32.Parse(userid)).FirstOrDefault();
            List<Patient> model = null;

            model = dbs.Where(m => m.UsersId == user.UsersId).ToList();
            

            return View(model);
        }





        public IActionResult Create()
        {
            DbSet<Users> dbs = _dbContext.Users;
            var lstPokes =
                dbs.ToList<Users>()
                   .OrderBy(p => p.UsersId)
                   .Select(
                       p =>
                       {
                           dynamic d = new ExpandoObject();
                           d.value = p.UsersId;
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
        public IActionResult Create(Patient newpatient)
        {
            DbSet<Users> dbS = _dbContext.Users;
            Patient model = newpatient;
            if (ModelState.IsValid)
            {
                DbSet<Patient> dbs = _dbContext.Patient;
                var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Users user = dbS.Where(s => s.UsersId == Int32.Parse(userid)).FirstOrDefault();
                newpatient.UsersId = user.UsersId;

                dbs.Add(newpatient);
                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "New Patient added!";
                else
                    TempData["Msg"] = "Failed to update database!";
            }
            else
            {
                TempData["Msg"] = "Invalid information entered";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            DbSet<Patient> dbs = _dbContext.Patient;
            Patient tOrder = dbs.Where(mo => mo.PatientId == id).FirstOrDefault();

            if (tOrder != null)
            {
                
                DbSet<Users> dbS = _dbContext.Users;
                var lstPokes =
                        dbS.ToList<Users>()
                                .OrderBy(p => p.Name)
                                .Select(
                                    p =>
                                    {
                                        dynamic d = new ExpandoObject();
                                        d.value = p.UsersId;
                                        d.text = p.Name;
                                        return d;
                                    }
                                )
                                .ToList<dynamic>();
                ViewData["pokes"] = lstPokes;

                return View(tOrder);
            }
            else
            {
                TempData["Msg"] = "Patient not found!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Update(Patient patient)
        {
            DbSet<Users> dbS = _dbContext.Users;
            if (ModelState.IsValid)
            {
                DbSet<Patient> dbs = _dbContext.Patient;
                Patient tOrder = dbs.Where(mo => mo.PatientId == patient.PatientId)
                                     .FirstOrDefault();

                var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Users user = dbS.Where(s => s.UsersId == Int32.Parse(userid)).FirstOrDefault();
                patient.UsersId = user.UsersId;
                if (tOrder != null)
                {
                    tOrder.Name = patient.Name;
                    tOrder.DateOfBirth = patient.DateOfBirth;
                    tOrder.Nric = patient.Nric;
                    tOrder.MedicalHistory = patient.MedicalHistory;
                    tOrder.AnnualIncome = patient.AnnualIncome;
                    tOrder.Address = patient.Address;
                    tOrder.RelationshipToUser = patient.RelationshipToUser;
                    tOrder.Postalcode = patient.Postalcode;
                    tOrder.Phone = patient.Phone;
                    tOrder.Gender = patient.Gender;


                    if (_dbContext.SaveChanges() == 1)
                        TempData["Msg"] = "Patient Information updated!";
                    else
                        TempData["Msg"] = "Failed to update database!";
                }
                else
                {
                    TempData["Msg"] = "Patient not found yet!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Msg"] = "Invalid information entered";
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            DbSet<Patient> dbs = _dbContext.Patient;

            Patient tOrder = dbs.Where(mo => mo.PatientId == id).FirstOrDefault();


            //MugOrder tOrder = null; // this line needs to be modified to achieve Task 2-2

            if (tOrder != null)
            {
                // TODO P10 Task 2-3 Use Remove method to remove the found MugOrder object from DbSet
                dbs.Remove(tOrder);

                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "Patient Information deleted!";
                else
                    TempData["Msg"] = "Failed to update database!";
            }
            else
            {
                TempData["Msg"] = "Patient Information not found!";
            }
            return RedirectToAction("Index");
        }

    }
}
