using System.ComponentModel.DataAnnotations;
using Xunit.Abstractions;

namespace admin_web_sell_phone.Models
{
    public class PhieuNhapModel
    {
        public int ID_Phieunhap { get; set; }
        public DateTime Ngaynhap { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Bạn muốn nhập hàng phải thêm sản phẩm.")]
        public int Tonggia { get; set; }
        public int ID_nhacungcap { get; set; }

    }
}
