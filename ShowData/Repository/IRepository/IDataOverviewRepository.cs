using ShowData.Model;
using ShowData.Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Repository.IRepository
{

        public interface IDataOverviewRepository
        {
            ICollection<DataOverview> GetDataOverviewList();
            DataOverview GetDataOverview(int id);
            bool IsDataOverviewExists(string id);
            bool CreateDataOverview(DataOverview dataOverview);
            bool UpdateDataOverview(DataOverview dataOverview);
            bool DeleteDataOverview(DataOverview dataOverview);
            bool Save();

        }
}


