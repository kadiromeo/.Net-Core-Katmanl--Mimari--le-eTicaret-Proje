using Core_eTicaret.DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Others;
using System;
using System.Linq;

namespace Core_eTicaret.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LockUnLock([FromBody] string id)
        {
            var nesne = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (nesne==null)
            {
                return Json(new { success = false, message = "Hesap Açma/Kapatma Esnasında Hata" });

            }
            if (nesne.LockoutEnd!=null && nesne.LockoutEnd>DateTime.Now)
            {
                nesne.LockoutEnd = DateTime.Now;
            }
            else
            {
                nesne.LockoutEnd = DateTime.Now.AddYears(10);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Başarılı" });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _db.ApplicationUsers.ToList();
            var userRole=_db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach (var item in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == item.Id).RoleId;
                item.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }
            return Json(new {data = userList });
        }
    }
}
