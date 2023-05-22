using admin_web_sell_phone.DataAccess;
using admin_web_sell_phone.Entity;
using admin_web_sell_phone.Models;
using admin_web_sell_phone.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace admin_web_sell_phone.Controllers
{
    public class PhieunhapController : Controller
    {
        private IPhieunhapService _phieunhapService;
        private IWebHostEnvironment _hostingEnvironment;
        private INhacungcapService _nhacungcapService;
        private IChitietphieunhapService _chitietphieunhapService;
        private ISanphamService _sanphamService;
        private ApplicationDbContext _db;

        public PhieunhapController(IPhieunhapService phieunhapService, IWebHostEnvironment hostingEnvironment,
            INhacungcapService nhacungcapservice,IChitietphieunhapService chitietphieunhapService,
            ISanphamService sanphamService, ApplicationDbContext _context)
        {
            _phieunhapService = phieunhapService;
            _hostingEnvironment = hostingEnvironment;
            _nhacungcapService = nhacungcapservice;
            _chitietphieunhapService = chitietphieunhapService;
            _sanphamService = sanphamService;
            _db = _context;
        }
        public IActionResult Index()
        {
            var model = _phieunhapService.GetAll().Select(pn => new PhieuNhapModel
            {
                ID_Phieunhap = pn.ID_Phieunhap,
                Ngaynhap = pn.Ngay_nhap,
                Tonggia = pn.Tong_gia,
                ID_nhacungcap = pn.ID_NhaCungCap
            }).ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Nhacungung = _nhacungcapService.GetAll();
            ViewBag.Sanpham = _sanphamService.GetAll();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            var tennhacungcap = collection["NhaCungCap"];
            var ngaynhap = collection["ngaynhap"];
            string tongphieunhap = collection["TongGia"];
            var listctphieunhapstring = collection["listCtpn"];
            var listctph = JsonConvert.DeserializeObject<List<dataphieunhap>>(listctphieunhapstring);
            System.Diagnostics.Debug.WriteLine(listctph);
            int nhacungcap = 0;
            if (tennhacungcap == "newprovice")
            {
                var ncc = new tb_Nhacungcap
                {
                    Tennhacungcap = "",
                    Diachi = "",
                    sdt = 0
                };
                await _nhacungcapService.CreateAsSync(ncc);
                nhacungcap = ncc.ID_Nhacungcap;
            }
            tongphieunhap = tongphieunhap.Replace(",", "");
            var phieunhap = new tb_Phieunhap
            {
                Ngay_nhap = DateTime.Parse(ngaynhap),
                Tong_gia = int.Parse(tongphieunhap),
                ID_NhaCungCap = nhacungcap == 0 ? int.Parse(tennhacungcap):nhacungcap
            };
            if(phieunhap != null)
            {
                await _phieunhapService.CreateAsSync(phieunhap);
            }
            foreach(var ctpn in listctph)
            {
                var chitietphieunhap = new tb_Chitietphieunhap
                {
                    ID_PhieuNhap = phieunhap.ID_Phieunhap,
                    Soluongnhap = ctpn.sl,
                    Gianhap = ctpn.gn,
                    ID_SanPham = ctpn.id
                    
                };
                _db.Add(chitietphieunhap);
                _db.SaveChanges();
                var sp = _sanphamService.GetById(chitietphieunhap.ID_SanPham);
                sp.Soluong += chitietphieunhap.Soluongnhap;
                await _sanphamService.UpdateAsSync(sp);
            }
            if(nhacungcap != 0)
            {
                return RedirectToAction("Edit", "Nhacungcap", new { id = nhacungcap });
            }
            return RedirectToAction("Index", "Phieunhap");
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var phieunhap = _phieunhapService.GetById(id);

            var model = new PhieuNhapModel
            {
                ID_Phieunhap = phieunhap.ID_Phieunhap,
                Ngaynhap = phieunhap.Ngay_nhap,
                Tonggia = phieunhap.Tong_gia,
                ID_nhacungcap = phieunhap.ID_NhaCungCap
            };
            ViewBag.Tennhacungcap = _nhacungcapService.GetById(model.ID_nhacungcap).Tennhacungcap;
            var ctpn = _chitietphieunhapService.GetById(model.ID_Phieunhap);
            var kqtvList = new List<ChitietphieunhapModel>();
            foreach (var ct in ctpn)
            {
                var kqtv = new ChitietphieunhapModel
                {
                    soluongnhap = ct.Soluongnhap,
                    ID_phieunhap = ct.ID_PhieuNhap,
                    ID_sanpham = ct.ID_SanPham,
                    gianhap = ct.Gianhap
                };
                kqtvList.Add(kqtv);
            }

            ViewBag.ChiTietPhieuNhap = kqtvList;
            var sanpham = new List<SanphamDetail_EditModel>();
            foreach(var ma in kqtvList)
            {
                var laysp = _sanphamService.GetById(ma.ID_sanpham);
                var kq = new SanphamDetail_EditModel
                {
                    ID_sanpham = laysp.ID_sanpham,
                    HinhAnh = laysp.HinhAnh,
                    Ten_Dienthoai = laysp.Ten_Dienthoai
                };
                sanpham.Add(kq);
            }
            ViewBag.Sanpham = sanpham;
            return View(model);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.Phieunhap == null)
            {
                return NotFound();
            }

            var phieunhap = await _db.Phieunhap
                .FirstOrDefaultAsync(m => m.ID_Phieunhap == id);
            if (phieunhap == null)
            {
                return NotFound();
            }

            return View(phieunhap);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.Phieunhap == null)
            {
                return Problem("Entity set 'AppDbContext.Hoadon'  is null.");
            }
            var phieunhap = await _db.Phieunhap.FindAsync(id);
            if (phieunhap != null)
            {
                var ctpn = (from c in _db.Chitietphieunhap
                            where c.ID_PhieuNhap == id
                            select c).ToList();
                _db.Phieunhap.Remove(phieunhap);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
