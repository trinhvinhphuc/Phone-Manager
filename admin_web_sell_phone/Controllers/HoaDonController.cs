using admin_web_sell_phone.DataAccess;
using admin_web_sell_phone.Entity;
using admin_web_sell_phone.Models;
using admin_web_sell_phone.Service;
using admin_web_sell_phone.Service.implamentation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace admin_web_sell_phone.Controllers
{
    public class HoaDonController : Controller
    {
        private ApplicationDbContext _context;
        private IHoaDonService _hoadonservice;
        private INhanvienService _nhanvienservice;
        private IKhachhangService _khachhangservice;
        private IChitiethoadonService _chitiethoadonservice;
        private ISanphamService _Sanphamservice;
        public HoaDonController(ApplicationDbContext context,IHoaDonService hoadonsv,
            INhanvienService nhanviensv, IKhachhangService khachhangsv,
            IChitiethoadonService chitiethoadon, ISanphamService sanphamservice)
        {
            _context = context;
            _hoadonservice = hoadonsv;
            _nhanvienservice = nhanviensv;
            _khachhangservice = khachhangsv;
            _chitiethoadonservice = chitiethoadon;
            _Sanphamservice = sanphamservice;
        }
        public IActionResult Index(string SearchString)
        {
            ViewBag.nhanvien = _nhanvienservice.GetAll();
            
           
            ViewBag.Khachhang = _khachhangservice.GetAll();
            var hd = _hoadonservice.GetAll();
            var listhd = new List<HoaDonIndexModel>();
            foreach(var item in hd)
            {
                
                var kq = new HoaDonIndexModel
                {
                    ID_Hoadon = item.ID_Hoadon,
                    ID_KhachHang = item.ID_KhachHang,
                    ID_NhanVien = item.ID_NhanVien,
                    TongDon = item.TongDon,
                    Ngaytaophieu = item.Ngaytaophieu
                };
                listhd.Add(kq);
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                listhd.Clear();
                foreach (var item in hd)
                {
                    if(_nhanvienservice.GetById(item.ID_NhanVien).Tennhanvien == SearchString || _khachhangservice.GetById(item.ID_KhachHang).TenKhachhang == SearchString )
                    {
                        var kq = new HoaDonIndexModel
                        {
                            ID_Hoadon = item.ID_Hoadon,
                            ID_KhachHang = item.ID_KhachHang,
                            ID_NhanVien = item.ID_NhanVien,
                            TongDon = item.TongDon,
                            Ngaytaophieu = item.Ngaytaophieu
                        };
                        listhd.Add(kq);
                    }
                }
            }
            ViewBag.danhsachhoadon = listhd;
            return View();
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var hoadon = _hoadonservice.GetById(id);
            var model = new HoaDonIndexModel
            {
                ID_Hoadon = hoadon.ID_Hoadon,
                ID_KhachHang = hoadon.ID_KhachHang,
                ID_NhanVien = hoadon.ID_NhanVien,
                Ngaytaophieu = hoadon.Ngaytaophieu,
                TongDon = hoadon.TongDon
            };
            ViewBag.Tennhanvien = _nhanvienservice.GetById(model.ID_NhanVien);
            var cthd = _chitiethoadonservice.GetById(model.ID_Hoadon);
            var kqtvList = new List<ChitiethoadonModel>();
            foreach (var ct in cthd)
            {
                var kqtv = new ChitiethoadonModel
                {
                   Soluongmua= ct.Soluongmua,
                   ID_HoaDon= ct.ID_HoaDon,
                   ID_SanPham = ct.ID_SanPham
                };
                kqtvList.Add(kqtv);
            }

            ViewBag.ChiTiethoadon = kqtvList;
            var sanpham = new List<SanphamDetail_EditModel>();
            foreach (var ma in kqtvList)
            {
                var laysp = _Sanphamservice.GetById(ma.ID_SanPham);
                var kq = new SanphamDetail_EditModel
                {
                    ID_sanpham = laysp.ID_sanpham,
                    HinhAnh = laysp.HinhAnh,
                    Ten_Dienthoai = laysp.Ten_Dienthoai,
                    GiaBan = laysp.GiaBan
                };
                sanpham.Add(kq);
            }
            ViewBag.Khachhang = _khachhangservice.GetById(model.ID_KhachHang);
            ViewBag.Sanpham = sanpham;
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var kqhd = _hoadonservice.GetById(id);
            var hd = new HoaDonIndexModel
            {
                ID_Hoadon = kqhd.ID_Hoadon,
                ID_KhachHang = kqhd.ID_KhachHang,
                ID_NhanVien = kqhd.ID_NhanVien,
                Ngaytaophieu = kqhd.Ngaytaophieu,
                TongDon = kqhd.TongDon
            };
            ViewData["hoadon"] = hd;

           // var kqkh = _khachhangservice.GetById(hd.ID_KhachHang);
            
            
            ViewBag.khachhanglist = _khachhangservice.GetAll();
            ViewBag.nhanvienlist = _nhanvienservice.GetAll();
            var cthd = _chitiethoadonservice.GetById(hd.ID_Hoadon);
            var kqtvList = new List<ChitiethoadonEditModel>();
            tb_Sanpham sanpham = new tb_Sanpham();
            foreach (var ct in cthd)
            {
                sanpham = _Sanphamservice.GetById(ct.ID_SanPham);
                var kqtv = new ChitiethoadonEditModel
                {
                    Soluongmua = ct.Soluongmua,
                    Giaban = sanpham.GiaBan,
                    ID_HoaDon = ct.ID_HoaDon,
                    ID_SanPham = ct.ID_SanPham,
                    tensanpham = sanpham.Ten_Dienthoai
                };
                kqtvList.Add(kqtv);
            }

            ViewData["cthd"] = kqtvList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Hoadon,ID_KhachHang,ID_NhanVien,Ngaytaophieu,TongDon")]HoaDonIndexModel hoadon)
        {
            if (id != hoadon.ID_Hoadon)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var hdupdate = new tb_Hoadon
                    {
                        ID_Hoadon = hoadon.ID_Hoadon,
                        ID_KhachHang = hoadon.ID_KhachHang,
                        ID_NhanVien = hoadon.ID_NhanVien,
                        TongDon = hoadon.TongDon,
                        Ngaytaophieu = hoadon.Ngaytaophieu
                    };
                    _context.Update(hdupdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoadonExists(hoadon.ID_Hoadon))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hoadon);
        }
        private bool HoadonExists(int id)
        {
            return (_context.Hoadon?.Any(e => e.ID_Hoadon == id)).GetValueOrDefault();
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["nhanvien"] = _nhanvienservice.GetAll();
            ViewData["dssp"] = _Sanphamservice.GetAll();
            ViewData["kh"] = _khachhangservice.GetAll();
            return View();
        }
        private class datahd
        {
            public int id { get; set; }
            public int sl { get; set; }
            public int gb { get; set; }
            public string ten { get; set; }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
        {
            var tennguoimua = collection["ttkhach"];
            var manhanvien = collection["idnv"];
            var ngaytao = collection["Ngaytaophieu"];
            string tongdon = collection["TongDon"];

            var listctdhstring = collection["listcthd"];

            var listctdh = JsonConvert.DeserializeObject<List<datahd>>(listctdhstring);

            int nguoimua = 0;

            System.Diagnostics.Debug.WriteLine(listctdh);
            if (tennguoimua == "nm")
            {
                var kh = new tb_Khachhang
                {
                    TenKhachhang = "",
                    sdt = 0
                };
                _context.Add(kh);
                _context.SaveChanges();

                nguoimua = kh.ID_Khachhang;
            }

            tongdon = tongdon.Replace(",", "");
            var hoadon = new tb_Hoadon
            {
                ID_KhachHang = nguoimua == 0 ? int.Parse(tennguoimua) : nguoimua,
                ID_NhanVien = int.Parse(manhanvien),
                Ngaytaophieu = DateTime.Parse(ngaytao),
                TongDon = int.Parse(tongdon)
            };

            if (hoadon != null)
            {
                _context.Add(hoadon);
                _context.SaveChanges();
            }

            foreach (var cthd in listctdh)
            {
                var chitiethoadon = new tb_Chitiethoadon
                {
                    ID_HoaDon = hoadon.ID_Hoadon,
                    ID_SanPham = cthd.id,
                    Soluongmua = cthd.sl
                };
                _context.Add(chitiethoadon);
                _context.SaveChanges();

                var sp = _context.Sanpham.Find(cthd.id);
                sp.Soluong -= chitiethoadon.Soluongmua;
                _context.SaveChanges();
            }

            if (nguoimua != 0)
            {
                return RedirectToAction("Edit", "Khachhang", new { id = nguoimua });
            }

            return RedirectToAction("Index", "hoadon");
        }
        // GET: Hoadon/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hoadon == null)
            {
                return NotFound();
            }

            var hoadon = await _context.Hoadon
                .FirstOrDefaultAsync(m => m.ID_Hoadon == id);
            if (hoadon == null)
            {
                return NotFound();
            }

            return View(hoadon);
        }

        // POST: Hoadon/Delete/5
       [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hoadon == null)
            {
                return Problem("Entity set 'AppDbContext.Hoadon'  is null.");
            }
            var hoadon = await _context.Hoadon.FindAsync(id);
            if (hoadon != null)
            {
                var cthd = (from c in _context.Chitiethoadon
                            where c.ID_HoaDon == id
                            select c).ToList();
                foreach(var item in cthd)
                {
                    var sp = _context.Sanpham.Find(item.ID_SanPham);
                    sp.Soluong += item.Soluongmua;
                    _context.SaveChanges();
                }
                _context.Hoadon.Remove(hoadon);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
