using admin_web_sell_phone.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace admin_web_sell_phone.Models
{
    public class HoaDonIndexModel
    {
        public int ID_Hoadon { get; set; }
        public int ID_KhachHang { get; set; }
        
        public int ID_NhanVien { get; set; }
        
        public DateTime Ngaytaophieu { get; set; }
        [Range(1,int.MaxValue,ErrorMessage = "Bạn cần thêm sản phẩm để tạo hóa đơn.")]
        public int TongDon { get; set; }
    }
}
