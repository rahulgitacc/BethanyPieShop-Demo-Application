using BethanyPieShop.ApplicationDbContext;
using BethanyPieShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BethanyPieShop.Interfaces
{
    public class PieRepository : IPieRepository
    {
        private readonly AppDbContext appDbContext;
        public PieRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public IEnumerable<Pie> Pies => appDbContext.Pies.Include(c => c.Category);

        public IEnumerable<Pie> PiesOfTheWeek => appDbContext.Pies.Include(c => c.Category).Where(p => p.IsPieOfTheWeek);

        public Pie GetPieById(int pieId) => appDbContext.Pies.FirstOrDefault(p => p.PieId == pieId);

        public void UpdatePie(Pie pie)
        {
            appDbContext.Pies.Update(pie);
            appDbContext.SaveChanges();
        }

        public void CreatePie(Pie pie)
        {
            appDbContext.Pies.Add(pie);
            appDbContext.SaveChanges();
        }
    }
}
