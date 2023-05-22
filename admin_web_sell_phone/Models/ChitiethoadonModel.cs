using admin_web_sell_phone.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace admin_web_sell_phone.Models
{
    public class ChitiethoadonModel
    {
        public int Soluongmua { get; set; }
        public int ID_HoaDon { get; set; }
        public int ID_SanPham { get; set; }
    }
}
