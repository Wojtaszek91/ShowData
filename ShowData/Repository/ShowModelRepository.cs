using ShowData.Data;
using ShowData.Model;
using ShowData.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShowData.Repository
{
    public class ShowModelRepository : IShowModelRepository
    {
        private readonly ApplicationDbContext _context;

        public ShowModelRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateShowModel(ShowModel showModel)
        {
            _context.ShowModels.Add(showModel);
            return Save();
        }

        public bool DeleteShowModel(ShowModel showModel)
        {
            _context.ShowModels.Remove(showModel);
            return Save();
        }

        public ShowModel GetShowModel(int id)
        {
          return  _context.ShowModels.FirstOrDefault(x => x.Id == id);
        }

        public ICollection<ShowModel> GetShowModelList()
        {
            return _context.ShowModels.OrderBy(a => a.DisplayName).ToList();
        }

        public bool IsShowModelExists(string displayName)
        {
          return  _context.ShowModels.Any(n => n.DisplayName.ToLower().Trim() == displayName.ToLower().Trim());
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
            
        }

        public bool UpdateShowModel(ShowModel showModel)
        {
            _context.ShowModels.Update(showModel);
            return Save();

        }
    }
}
