using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EduSpecWebAPI.Models;

namespace EduSpecWebAPI.Controllers
{
    public class Parent_ChildController : ApiController
    {
        public static IList<Parent_Child> Parent_Child = new List<Parent_Child>();
        // GET: api/Parent_Child
        [HttpGet]
        [Route("api/Parent_Child/{InstID}")]
        public IEnumerable<Parent_Child> Get(int InstID)
        {
            using (var Context = new EduSpecWebAPIDataContext())
            {
                var Parent_ChildList = Context.Get_WebAPI_ParentChild(InstID).ToList().AsEnumerable();
                Parent_Child = (from l in Parent_ChildList
                                select new Parent_Child
                                {
                                    InstID = InstID,
                                    ParentID = l.ParentId,
                                    ChildID = l.ChildId,
                                    LearnerID = l.Learnerid,
                                    Status = l.Status,
                                    FamilyCode = l.FamilyCode
                                }).ToList();
                return Parent_Child;
            }
        }

        // POST: api/Parent_Child
        [HttpPost]
        //[BasicAuthentication(RequireSsl = false)]
        [Route("api/Parent_Child/{InstID}")]
        public void Post(Parent_Child[] Parent_Child, int InstID)
        {
            using (var Context = new EduSpecWebAPIDataContext())
            {
                Context.Set_WebAPI_ParentChildClear(InstID);

                foreach (var ParentChild in Parent_Child)
                {
                    Context.Set_WebAPI_ParentChild(
                        InstID,
                        ParentChild.ParentID,
                        ParentChild.ChildID,
                        ParentChild.LearnerID,
                        ParentChild.Status,
                        ParentChild.FamilyCode
                        );
                }
            }
        }

        // PUT: api/Parent_Child/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Parent_Child/5
        public void Delete(int id)
        {
        }
    }
}
