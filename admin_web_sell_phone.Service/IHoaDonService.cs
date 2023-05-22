using admin_web_sell_phone.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace admin_web_sell_phone.Service
{
    public interface IHoaDonService
    {
        Task CreateAsSync(tb_Hoadon hoadon);
        Task UpdateById(int id);
        Task UpdateAsSync(tb_Hoadon hoadon);
        Task DeleteById(int id);
        tb_Hoadon GetById(int id);
        IEnumerable<tb_Hoadon> GetAll();
    }
}
