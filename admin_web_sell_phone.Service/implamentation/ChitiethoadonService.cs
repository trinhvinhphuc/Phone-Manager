using admin_web_sell_phone.DataAccess;
using admin_web_sell_phone.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace admin_web_sell_phone.Service.implamentation
{
    public class ChitiethoadonService : IChitiethoadonService
    {
        public ApplicationDbContext _context;
        public ChitiethoadonService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsSync(tb_Chitiethoadon cthd)
        {
            _context.Chitiethoadon.Update(cthd);
            await _context.SaveChangesAsync();
        }

        public Task DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tb_Chitiethoadon> GetAll()
        {
            return _context.Chitiethoadon.ToList();
        }

        public IEnumerable<tb_Chitiethoadon> GetById(int id1)
        {
           return _context.Chitiethoadon.Where(cthd => cthd.ID_HoaDon == id1).ToList();
        }

        public Task UpdateAsSync(tb_Chitiethoadon cthn)
        {
            throw new NotImplementedException();
        }

        public Task UpdateById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
