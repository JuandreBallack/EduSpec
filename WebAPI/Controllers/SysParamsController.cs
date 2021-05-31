using System.Linq;
using System.Web.Http;
using EduSpecWebAPI.Models;

namespace EduSpecWebAPI.Controllers
{
    public class SysParamsController : ApiController
    {
        //public static SysParams sysParams = new SysParams();
        // GET: api/SysParams
        [HttpGet]
        [Route("api/SysParams/{InstID}")]
        public SysParams Get(int InstID)
        {
            using (var Context = new EduSpecWebAPIDataContext())
            {
                var Params = Context.Get_WebAPI_SysParams(InstID).FirstOrDefault();
                SysParams SysParam = new SysParams()
                {
                    InstID = Params.InstID,
                    AccessDatabaseProvider = Params.AccessDatabaseProvider,
                    SASAMS_FilePath = Params.SASAMS_FilePath,
                    SASAMS_Password = Params.SASAMS_Password
                };                
                return SysParam;
            }
        }
        
        // POST: api/SysParams
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/SysParams/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SysParams/5
        public void Delete(int id)
        {
        }
    }
}
