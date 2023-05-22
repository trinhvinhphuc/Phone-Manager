using admin_web_sell_phone.Entity;
using admin_web_sell_phone.Models;
using admin_web_sell_phone.Service;
using Microsoft.AspNetCore.Mvc;
using admin_web_sell_phone.Service.implamentation;
using admin_web_sell_phone.DataAccess.Migrations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace admin_web_sell_phone.Controllers
{
    public class SanphamController : Controller
    {
        private ISanphamService _sanphamService;
        private IWebHostEnvironment _hostingEnvironment;
        private IThuonghieuService _thuonghieuService;
       
        public SanphamController(ISanphamService employeeService, IWebHostEnvironment hostingEnvironment,IThuonghieuService thuonghieuservice)
        {
            _sanphamService = employeeService;
            _hostingEnvironment = hostingEnvironment;
            _thuonghieuService = thuonghieuservice;
        }
        [HttpGet]
        public IActionResult Index(int pg = 1)
        {
            var model = _sanphamService.GetAll().Select(sp => new SanphamIndexModel
            {
                ID_sanpham = sp.ID_sanpham,
                Ten_Dienthoai = sp.Ten_Dienthoai,
                Hinhanh = sp.HinhAnh,
                Tenthuonghieu = _thuonghieuService.GetById(sp.ID_ThuongHieu).Tenthuonghieu
            }).ToList();
            int pageSize = 3;
            if (pg < 1) pg = 1;
            int recsCount = model.Count();
            var pager = new Pager(recsCount,pg,pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = model.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager; 

            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Brand = _thuonghieuService.GetAll();
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanphamCreateModel model)
        {

           
                var sanpham = new tb_Sanpham
                {
                    ID_sanpham = model.ID_sanpham,
                    Ten_Dienthoai = model.Ten_Dienthoai,
                    GiaBan = model.GiaBan,
                    Soluong = model.Soluong,
                    Kichthuoc = model.Kichthuoc,
                    Camera = model.Camera,
                    Mausac = model.Mausac,
                    Manhinh = model.Manhinh,
                    Pin = model.Pin,
                    BaoHanh = model.BaoHanh,
                    ID_ThuongHieu = model.ID_ThuongHieu
                };
            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Content/img");
                string uniqueFileName =  model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
                sanpham.HinhAnh = "Content/img/" + uniqueFileName;
            }
                
                await _sanphamService.UpdateAsSync(sanpham);
                return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var sanPham = _sanphamService.GetById(id);
            
            var model = new SanphamDetail_EditModel
            {
                ID_sanpham = sanPham.ID_sanpham,
                Ten_Dienthoai = sanPham.Ten_Dienthoai,
                GiaBan = sanPham.GiaBan,
                Soluong = sanPham.Soluong,
                Kichthuoc = sanPham.Kichthuoc,
                Camera = sanPham.Camera,
                Mausac = sanPham.Mausac,
                Pin = sanPham.Pin,
                Manhinh = sanPham.Manhinh,
                BaoHanh = sanPham.BaoHanh,
                HinhAnh = sanPham.HinhAnh,
                ID_ThuongHieu = sanPham.ID_ThuongHieu
            };
            ViewBag.thuonghieu = _thuonghieuService.GetById(sanPham.ID_ThuongHieu).Tenthuonghieu;
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit (int id)
        {
            var sanPham = _sanphamService.GetById(id);
            var model = new SanphamDetail_EditModel
            {
                ID_sanpham = sanPham.ID_sanpham,
                Ten_Dienthoai = sanPham.Ten_Dienthoai,
                GiaBan = sanPham.GiaBan,
                Soluong = sanPham.Soluong,
                Kichthuoc = sanPham.Kichthuoc,
                Camera = sanPham.Camera,
                Mausac = sanPham.Mausac,
                Pin = sanPham.Pin,
                Manhinh = sanPham.Manhinh,
                BaoHanh = sanPham.BaoHanh,
                HinhAnh = sanPham.HinhAnh,
                ID_ThuongHieu = sanPham.ID_ThuongHieu
            };
            ViewBag.thuonghieu = _thuonghieuService.GetAll();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SanphamDetail_EditModel model)
        {
            
                var sanpham = _sanphamService.GetById(model.ID_sanpham);
                sanpham.Ten_Dienthoai = model.Ten_Dienthoai;
                sanpham.Manhinh = model.Manhinh;
                sanpham.BaoHanh = model.BaoHanh;
                sanpham.Kichthuoc = model.Kichthuoc;
                sanpham.Mausac = model.Mausac;
                sanpham.Pin = model.Pin;
                sanpham.Camera= model.Camera;
                sanpham.GiaBan = model.GiaBan;
                sanpham.ID_ThuongHieu = model.ID_ThuongHieu;
                if (model.Image != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Content/img");
                    string uniqueFileName = model.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Image.CopyTo(fileStream);
                    }
                    sanpham.HinhAnh = "Content/img/" + uniqueFileName;
                }
            await _sanphamService.UpdateAsSync(sanpham);
                return RedirectToAction("Index");
        }
        public void Uploadfile (IFormFile file,string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
        }
        
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var sanpham = _sanphamService.GetById(id);
            if (sanpham == null)
            {
                return NotFound();
            }
            var model = new SanphamDeleteModel
            {
                ID_sanpham = sanpham.ID_sanpham,
                
            };
            ViewBag.thuonghieu = _thuonghieuService.GetById(sanpham.ID_ThuongHieu).Tenthuonghieu;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(SanphamDeleteModel model)
        {
            var sanpham = _sanphamService.GetById(model.ID_sanpham);
            if (sanpham == null)
            {
                return NotFound();
            }
            await _sanphamService.DeleteById(sanpham.ID_sanpham);

            return  RedirectToAction("Index");
        }
    }
}


