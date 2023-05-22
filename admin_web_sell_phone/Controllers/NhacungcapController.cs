using admin_web_sell_phone.DataAccess;
using admin_web_sell_phone.Entity;
using admin_web_sell_phone.Models;
using admin_web_sell_phone.Service;
using admin_web_sell_phone.Service.implamentation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace admin_web_sell_phone.Controllers
{
    public class NhacungcapController : Controller
    {
        private readonly ApplicationDbContext _context;
        private INhacungcapService _Nhacungcapservice;

        public NhacungcapController(ApplicationDbContext context, INhacungcapService nhacungcapservice)
        {
            _context = context;
            _Nhacungcapservice = nhacungcapservice;
        }
        public IActionResult Index()
        {
            var model = _Nhacungcapservice.GetAll().Select(ncc => new NhacungcapModel
            {
               ID_Nhacungcap = ncc.ID_Nhacungcap,
               Tennhacungcap= ncc.Tennhacungcap,
               sdt = ncc.sdt,
               Diachi = ncc.Diachi
            }).ToList();
            return View(model);
        }
        public ActionResult Create()
        {

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Nhacungcap,Tennhacungcap,Diachi,sdt")] tb_Nhacungcap nhacungcap)
        {

            if (ModelState.IsValid)
            {
                await _Nhacungcapservice.CreateAsSync(nhacungcap);

                return RedirectToAction(nameof(Index));
            };
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var nhacungcap = _Nhacungcapservice.GetById(id);
            if (nhacungcap == null)
            {
                return NotFound();
            }

            var model = new NhacungcapModel { 
                ID_Nhacungcap = nhacungcap.ID_Nhacungcap,
                Tennhacungcap = nhacungcap.Tennhacungcap,
                sdt = nhacungcap.sdt,
                Diachi = nhacungcap.Diachi
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Nhacungcap,Tennhacungcap,Diachi,sdt")] NhacungcapModel nhacungcap)
        {
            if (id != nhacungcap.ID_Nhacungcap)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var ncc = _Nhacungcapservice.GetById(id);
                    ncc.ID_Nhacungcap = nhacungcap.ID_Nhacungcap;
                    ncc.Tennhacungcap = nhacungcap.Tennhacungcap;
                    
                    ncc.Diachi = nhacungcap.Diachi;
                    ncc.sdt = nhacungcap.sdt;
                    

                    await _Nhacungcapservice.UpdateAsSync(ncc);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanvienExists(nhacungcap.ID_Nhacungcap))
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
            return View(nhacungcap);
        }
        private bool NhanvienExists(int iD_Nhacungcap)
        {
            return _context.Nhacungcap.Any(n => n.ID_Nhacungcap == iD_Nhacungcap);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhacungcap = await _context.Nhacungcap
                .Select(h => new NhacungcapModel
                {
                    ID_Nhacungcap = h.ID_Nhacungcap,
                    Tennhacungcap = h.Tennhacungcap,
                    Diachi = h.Diachi,
                    sdt = h.sdt
                })
                .FirstOrDefaultAsync(m => m.ID_Nhacungcap == id);

            if (nhacungcap == null)
            {
                return NotFound();
            }

            return View(nhacungcap);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(NhacungcapModel model)
        {
            var nhacungcap = await _context.Nhacungcap.FindAsync(model.ID_Nhacungcap);
            if (nhacungcap == null)
            {
                return NotFound();
            }

            _context.Nhacungcap.Remove(nhacungcap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
