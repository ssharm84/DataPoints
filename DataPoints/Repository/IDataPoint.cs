using DataPoints.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataPoints.Repository
{
    public interface IDataPoint
    {
        Task<List<DataPointsModel>> GetDataPoints();
    }
}
