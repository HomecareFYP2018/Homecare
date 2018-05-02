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
    public class ServiceRequestController : Controller
    {
        private AppDbContext _dbContext;


        public ServiceRequestController(AppDbContext dbContext)

        {

            _dbContext = dbContext;

        }


        public IActionResult Index()
        {
            

            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            DbSet<Users> userdata = _dbContext.Users;
            Users user = userdata.Where(s => s.UsersId == Int32.Parse(userid)).FirstOrDefault();

            DbSet<Patient> dbS = _dbContext.Patient;
            Patient patient = dbS.Where(s => s.UsersId == Int32.Parse(userid)).FirstOrDefault();

            DbSet<ServiceRequest> dbs = _dbContext.ServiceRequest;
            List<ServiceRequest> model = null;

            if (User.IsInRole("ServiceProvider"))
            {
                model = dbs.ToList();
                //return RedirectToAction("Create", "Quote");
            }
            else
            {
                model = dbs.Where(m => m.PatientId == patient.PatientId && patient.UsersId == user.UsersId).ToList();
            }

            return View(model);

        }

        public IActionResult Create()
        {
            DbSet<Patient> dbs = _dbContext.Patient;
            var lstPokes =
                dbs.ToList<Patient>()
                   .OrderBy(p => p.PatientId)
                   .Select(
                       p =>
                       {
                           dynamic d = new ExpandoObject();
                           d.value = p.PatientId;
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
        public IActionResult Create(ServiceRequest sq)
        {
            DbSet<Users> userdata = _dbContext.Users;
            DbSet<Patient> patientdata = _dbContext.Patient;
            ServiceRequest model = sq;
            if (ModelState.IsValid)
            {
                DbSet<ServiceRequest> dbs = _dbContext.ServiceRequest;
                var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Users user = userdata.Where(s => s.UsersId == Int32.Parse(userid)).FirstOrDefault();
                Patient patient = patientdata.Where(s => s.UsersId == Int32.Parse(userid)).FirstOrDefault();

                sq.PatientId = patient.PatientId;

                dbs.Add(sq);
                if (_dbContext.SaveChanges() == 1)
                {
                    TempData["Msg"] = "New Service Request added!";
                    //TempData["Test"] = sq;
                }
                    
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
            DbSet<Users> userdata = _dbContext.Users;
            DbSet<Patient> patientdata = _dbContext.Patient;
            DbSet<ServiceRequest> dbs = _dbContext.ServiceRequest;
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Users user = userdata.Where(s => s.UsersId == Int32.Parse(userid)).FirstOrDefault();
            Patient patient = patientdata.Where(s => s.UsersId == Int32.Parse(userid)).FirstOrDefault();

            ServiceRequest tOrder = dbs.Where(mo => mo.PatientId == patient.PatientId && patient.UsersId == user.UsersId).FirstOrDefault();

            tOrder.ServiceRequestId = id;
            if (tOrder != null)
            {
                
                var lstPokes =
                    patientdata.ToList<Patient>()
                       .OrderBy(p => p.PatientId)
                       .Select(
                           p =>
                           {
                               dynamic d = new ExpandoObject();
                               d.value = p.PatientId;
                               d.text = p.Name;
                               return d;
                           }
                       )
                       .ToList<dynamic>();
                ViewData["pokes"] = lstPokes;

                return View();
            }
            else
            {
                TempData["Msg"] = "Service Request Not found!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Update(ServiceRequest sq)
        {
            DbSet<Patient> dbS = _dbContext.Patient;
            DbSet<Users> userdata = _dbContext.Users;
            if (ModelState.IsValid)
            {
                DbSet<ServiceRequest> dbs = _dbContext.ServiceRequest;

                var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Users user = userdata.Where(s => s.UsersId == Int32.Parse(userid)).FirstOrDefault();
                Patient patient = dbS.Where(s => s.UsersId == Int32.Parse(userid)).FirstOrDefault();
                ServiceRequest tOrder = dbs.Where(mo => mo.PatientId == patient.PatientId && patient.UsersId == user.UsersId).FirstOrDefault();

                sq.ServiceRequestId = tOrder.ServiceRequestId;

                if (tOrder != null)
                {
                    tOrder.Description = sq.Description;
                    tOrder.RequestDate = sq.RequestDate;
                    tOrder.AppointmentDatetime = sq.AppointmentDatetime;
                    tOrder.Status = sq.Status;


                    if (_dbContext.SaveChanges() == 1)
                        TempData["Msg"] = "Service Request updated!";
                    else
                        TempData["Msg"] = "Failed to update database!";
                }
                else
                {
                    TempData["Msg"] = "Service Request not found yet!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Msg"] = "Invalid information entered";
            }
            return RedirectToAction("Index");
        }

    }
}
