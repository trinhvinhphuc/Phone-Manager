﻿using admin_web_sell_phone.DataAccess;
using admin_web_sell_phone.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace admin_web_sell_phone.Service.implamentation
{
    public class ThuonghieuService : IThuonghieuService
    {
        public ApplicationDbContext _context;
        public ThuonghieuService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsSync(tb_Thuonghieu newthuonghieu)
        {
            _context.Thuonghieu.Update(newthuonghieu);
            await _context.SaveChangesAsync();
        }

        public Task DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tb_Thuonghieu> GetAll()
        {
            return _context.Thuonghieu.ToList();
        }

        

        public tb_Thuonghieu GetById(int id)
        {
            return _context.Thuonghieu.Where(x => x.Id_Thuonghieu == id).FirstOrDefault();
        }

        public async Task UpdateAsSync(tb_Thuonghieu thuonghieu)
        {
            _context.Thuonghieu.Update(thuonghieu);
            await _context.SaveChangesAsync();
        }

        public Task UpdateById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
