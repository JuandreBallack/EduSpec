using System.Net.Http;
using System.Threading.Tasks;
using EduSpecDataService.Models;

namespace EduSpecDataService.Code
{
    public interface IRestService
    {
        Task<HttpResponseMessage> UpdateLearners(Learner item);
    }
}
