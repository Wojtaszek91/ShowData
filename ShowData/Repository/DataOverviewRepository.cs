using Microsoft.EntityFrameworkCore;
using ShowData.Data;
using ShowData.Model;
using ShowData.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Repository
{
    public class DataOverviewRepository : IDataOverviewRepository
    {

            private readonly ApplicationDbContext _context;

            public DataOverviewRepository(ApplicationDbContext context)
            {
                _context = context;
            }
            public bool CreateDataOverview(DataOverview dataOverview)
            {
                _context.DataOverviews.Add(dataOverview);
                return Save();
            }

            public bool DeleteDataOverview(DataOverview dataOverview)
            {
                _context.DataOverviews.Remove(dataOverview);
                return Save();
            }

            public DataOverview GetDataOverview(int id)
            {
                return _context.DataOverviews.FirstOrDefault(x => x.Id == id);
            }

            public ICollection<DataOverview> GetDataOverviewList()
            {
                return _context.DataOverviews.OrderBy(a => a.Title).ToList();
            }

            public bool IsDataOverviewExists(string displayName)
            {
                return _context.DataOverviews.Any(n => n.Title.ToLower().Trim() == displayName.ToLower().Trim());
            }
            public bool IsDataOverviewExists(int id)
            {
                return _context.DataOverviews.Any(n => n.Id == id);
            }

            public bool Save()
            {
                return _context.SaveChanges() >= 0 ? true : false;

            }

            public bool UpdateDataOverview(DataOverview dataOverview)
            {
                _context.DataOverviews.Update(dataOverview);
                return Save();
            }
}
}
