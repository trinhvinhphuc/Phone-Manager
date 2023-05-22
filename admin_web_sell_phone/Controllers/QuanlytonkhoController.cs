using admin_web_sell_phone.DataAccess;
using admin_web_sell_phone.Service;
using Microsoft.AspNetCore.Mvc;
using admin_web_sell_phone.Models;
using admin_web_sell_phone.Entity;

namespace admin_web_sell_phone.Controllers
{
    public class QuanlytonkhoController : Controller
    {
        private ApplicationDbContext _context;
        private ISanphamService _sanphamService;
        private IChitietphieunhapService _chitietphieunhapservice;
        private IPhieunhapService _phieunhapservice;
        public QuanlytonkhoController(ApplicationDbContext context, ISanphamService sanphamService, IChitietphieunhapService chitietphieunhapservice, IPhieunhapService phieunhapservice)
        {
            _context = context;
            _sanphamService = sanphamService;
            _chitietphieunhapservice = chitietphieunhapservice;
            _phieunhapservice = phieunhapservice;
        }

        public IActionResult Index()
        {
            List<QuanlytonkhoModel> dssp = new List<QuanlytonkhoModel>();
            var getsp = _sanphamService.GetAll();
            foreach(var item in getsp)
            {
                var kqtv = _chitietphieunhapservice.GetByIdlast(item.ID_sanpham);
                if(kqtv != null)
                {
                    var kq = new QuanlytonkhoModel
                    {
                        masp = kqtv.ID_SanPham,
                        tensp = item.Ten_Dienthoai,
                        anh = item.HinhAnh,
                        gianhap = kqtv.Gianhap,
                        giaban = item.GiaBan,
                        soluongton = item.Soluong
                    };
                    dssp.Add(kq);
                }
            }
            return View(dssp);
        }
    }
}
