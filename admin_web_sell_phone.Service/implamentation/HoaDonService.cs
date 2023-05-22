using admin_web_sell_phone.DataAccess;
using admin_web_sell_phone.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace admin_web_sell_phone.Service.implamentation
{
    public class HoaDonService : IHoaDonService
    {
        public ApplicationDbContext _context;
        public HoaDonService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsSync(tb_Hoadon hoadon)
        {
            _context.Hoadon.Update(hoadon);
            await _context.SaveChangesAsync();
        }

        public Task DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tb_Hoadon> GetAll()
        {
            return _context.Hoadon.ToList();
        }

        public tb_Hoadon GetById(int id)
        {
            return _context.Hoadon.Where(x => x.ID_Hoadon == id).FirstOrDefault();
        }

        public Task UpdateAsSync(tb_Hoadon hoadon)
        {
            throw new NotImplementedException();
        }

        public Task UpdateById(int id)
        {
            throw new NotImplementedException();
        }
    }

    
}
