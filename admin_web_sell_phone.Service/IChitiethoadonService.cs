using admin_web_sell_phone.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace admin_web_sell_phone.Service
{
    public interface IChitiethoadonService
    {
        Task CreateAsSync(tb_Chitiethoadon cthd);
        Task UpdateById(int id);
        Task UpdateAsSync(tb_Chitiethoadon cthd);
        Task DeleteById(int id);
        IEnumerable<tb_Chitiethoadon> GetById(int id1);
        IEnumerable<tb_Chitiethoadon> GetAll();
    }
}
