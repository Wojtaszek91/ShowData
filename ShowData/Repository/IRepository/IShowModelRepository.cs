using ShowData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Repository.IRepository
{
    public interface IShowModelRepository
    {
        ICollection<ShowModel> GetShowModelList();
        ICollection<ShowModel> GetDataOverviewListWithSpecificShowData(int dataOverviewId);
        ShowModel GetShowModel(int id);
        bool IsShowModelExists(string id);
        bool CreateShowModel(ShowModel showModel);
        bool UpdateShowModel(ShowModel showModel);
        bool DeleteShowModel(ShowModel showModel);
        bool Save();
            
    }
}
