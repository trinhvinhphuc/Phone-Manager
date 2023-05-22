using admin_web_sell_phone.DataAccess;
using admin_web_sell_phone.DataAccess.Migrations;
using admin_web_sell_phone.Entity;
using admin_web_sell_phone.Models;
using admin_web_sell_phone.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Security.Permissions;
namespace admin_web_sell_phone.Controllers
{
    public class NhanvienController : Controller
    {
        private readonly ApplicationDbContext _context;
        private INhanvienService _Nhanvienservice;
        private IWebHostEnvironment _hostingEnvironment;

        public NhanvienController(ApplicationDbContext context, INhanvienService nhanvienservice, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _Nhanvienservice = nhanvienservice;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var model = _Nhanvienservice.GetAll().Select(nv => new NhanvienIndexModel
            {
                ID_Nhanvien = nv.ID_Nhanvien,
                Tennhanvien = nv.Tennhanvien,
                Email = nv.Email,
                sdt = nv.sdt
            }).ToList();
            return View(model);
        }
        public ActionResult Detail(int id)
        {
            var nhanvien = _Nhanvienservice.GetById(id);
            if (nhanvien == null)
            {
                return NotFound();
            }

            var model = new NhanvienIndexModel
            {
                ID_Nhanvien = nhanvien.ID_Nhanvien,
                Tennhanvien = nhanvien.Tennhanvien,
                Ngaysinh = (DateTime)nhanvien.Ngaysinh,
                Diachi = nhanvien.Diachi,
                sdt = nhanvien.sdt,
                Email = nhanvien.Email,
                Hinhanh = nhanvien.Hinhanh,
                username = nhanvien.username,
                password = nhanvien.password,
            };

            return View(model);
        }

        public ActionResult Create()
        {

            return View();

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Nhanvien,Tennhanvien,Ngaysinh,Diachi,sdt,Email,Hinhanh,Image,username,password")] NhanvienIndexModel nhanvien)
        {

            var nhanvien_tb = new tb_Nhanvien
            {
                ID_Nhanvien = nhanvien.ID_Nhanvien,
                Tennhanvien = nhanvien.Tennhanvien,
                Ngaysinh = nhanvien.Ngaysinh,
                Diachi = nhanvien.Diachi,
                sdt = nhanvien.sdt,
                Email = nhanvien.Email,
                username = nhanvien.username,
                password = nhanvien.password,
            };
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Content/img");
            string uniqueFileName = nhanvien.Image.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                nhanvien.Image.CopyTo(fileStream);
            }
            nhanvien_tb.Hinhanh = "Content/img/" + uniqueFileName;
            await  _Nhanvienservice.CreateAsSync(nhanvien_tb);
               
                return RedirectToAction(nameof(Index));
            
           
        }
        //Delete 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanvien = await _context.Nhanvien
                .Select(h => new NhanvienIndexModel
                {
                    ID_Nhanvien = h.ID_Nhanvien,
                    Tennhanvien = h.Tennhanvien,
                    Ngaysinh = (DateTime)h.Ngaysinh,
                    Diachi = h.Diachi,
                    sdt = h.sdt,
                    Email = h.Email,
                    Hinhanh = h.Hinhanh,
                    username = h.username,
                    password = h.password
                })
                .FirstOrDefaultAsync(m => m.ID_Nhanvien == id);

            if (nhanvien == null)
            {
                return NotFound();
            }

            return View(nhanvien);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(NhanvienIndexModel model)
        {
            var nhanvien = await _context.Nhanvien.FindAsync(model.ID_Nhanvien);
            if (nhanvien == null)
            {
                return NotFound();
            }

            _context.Nhanvien.Remove(nhanvien);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var nhanvien = _Nhanvienservice.GetById(id);
            if (nhanvien == null)
            {
                return NotFound();
            }

            var model = new NhanvienIndexModel
            {
                ID_Nhanvien = nhanvien.ID_Nhanvien,
                Tennhanvien = nhanvien.Tennhanvien,
                Ngaysinh = (DateTime)nhanvien.Ngaysinh,
                Diachi = nhanvien.Diachi,
                sdt = nhanvien.sdt,
                Email = nhanvien.Email,
                Hinhanh = nhanvien.Hinhanh,
                username = nhanvien.username,
                password = nhanvien.password,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Nhanvien,Tennhanvien,Ngaysinh,Diachi,sdt,Email,Hinhanh,username,password,Image")] NhanvienIndexModel nhanvienModel)
        {
            if (id != nhanvienModel.ID_Nhanvien)
            {
                return NotFound();
            }

                
                    var nhanvien = _Nhanvienservice.GetById(id);
                    nhanvien.ID_Nhanvien = nhanvienModel.ID_Nhanvien;
                    nhanvien.Tennhanvien = nhanvienModel.Tennhanvien;
                    nhanvien.Ngaysinh = nhanvienModel.Ngaysinh;
                    nhanvien.Diachi = nhanvienModel.Diachi;
                    nhanvien.sdt = nhanvienModel.sdt;
                    nhanvien.Email = nhanvienModel.Email;
                    nhanvien.username = nhanvienModel.username;
                    nhanvien.password = nhanvienModel.password;

            //xu ly them anh nhanvien
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Content/img");
            string uniqueFileName = nhanvienModel.Image.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                nhanvienModel.Image.CopyTo(fileStream);
            }
            nhanvien.Hinhanh = "Content/img/" + uniqueFileName;
            await _Nhanvienservice.UpdateAsSync(nhanvien);
                
                
                return RedirectToAction(nameof(Index));
        }
        private bool NhanvienExists(int iD_Nhanvien)
        {
            return _context.Nhanvien.Any(n => n.ID_Nhanvien == iD_Nhanvien);
        }
        // GET: /Account/Index
        public IActionResult Login()
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Sanpham");

            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(IFormCollection collection)
        {
            var username = collection["username"];
            var password = collection["password"];
            var nhanvien = (NhanvienIndexModel)null;

            foreach (var user in _Nhanvienservice.GetAll())
            {
                if (user != null && user.username == username && user.password == password)
                {
                    nhanvien = new NhanvienIndexModel()
                    {
                        ID_Nhanvien = user.ID_Nhanvien,
                        Tennhanvien = user.Tennhanvien,
                        Ngaysinh = (DateTime)user.Ngaysinh,
                        Diachi = user.Diachi,
                        sdt = user.sdt,
                        Email = user.Email,
                        Hinhanh = user.Hinhanh,
                        username = user.username,
                        password = user.password,
                    };
                    break;
                }
            }

            if (nhanvien != null)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim("id", nhanvien.ID_Nhanvien.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, nhanvien.Tennhanvien),
                    new Claim("email", nhanvien.Email),
                    new Claim("img", nhanvien.Hinhanh),
                    //new Claim(ClaimTypes.NameIdentifier, nhanvien.ID_Nhanvien.ToString())
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Sanpham");
            }
            else
            {
                // Login failed
                ViewBag.ErrorMessage = "Invalid username or password";
                return View("Login");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Nhanvien");
        }
    }
}
