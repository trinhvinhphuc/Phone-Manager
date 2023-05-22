using admin_web_sell_phone.DataAccess;
using admin_web_sell_phone.Entity;
using admin_web_sell_phone.Models;
using admin_web_sell_phone.Service;
using admin_web_sell_phone.Service.implamentation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace admin_web_sell_phone.Controllers
{
    public class ThuonghieuController : Controller
    {
        public ApplicationDbContext _context;
        public IThuonghieuService _thuonghieuservice;
        public ThuonghieuController(ApplicationDbContext context, IThuonghieuService thuonghieuservice)
        {
            _context = context;
            _thuonghieuservice = thuonghieuservice;
        }

        public IActionResult Index()
        {
            var model = _thuonghieuservice.GetAll().Select(th => new ThuonghieuModel
            {
                ID_thuonghieu = th.Id_Thuonghieu,
                Tenthuonghieu = th.Tenthuonghieu
            }).ToList();

            return View(model);
        }
        public ActionResult Create()
        {

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Thuonghieu,Tenthuonghieu")] tb_Thuonghieu thuonghieu)
        {

            if (ModelState.IsValid)
            {
                await _thuonghieuservice.CreateAsSync(thuonghieu);

                return RedirectToAction(nameof(Index));
            };
            return View();
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuonghieu = await _context.Thuonghieu
                .Select(h => new ThuonghieuModel
                {
                    ID_thuonghieu = h.Id_Thuonghieu,
                    Tenthuonghieu = h.Tenthuonghieu
                    
                })
                .FirstOrDefaultAsync(m => m.ID_thuonghieu == id);

            if (thuonghieu == null)
            {
                return NotFound();
            }

            return View(thuonghieu);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ThuonghieuModel model)
        {
            var thuonghieu = await _context.Thuonghieu.FindAsync(model.ID_thuonghieu);
            if (thuonghieu == null)
            {
                return NotFound();
            }

            _context.Thuonghieu.Remove(thuonghieu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
