using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EduSpecWebAPI.Models;

namespace EduSpecWebAPI.Controllers
{
    public class ParentsController : ApiController
    {
        public static IList<Parent> Parents = new List<Parent>();
        
        // GET: api/Parents
        [HttpGet]
        [Route("api/Parents/{InstID}")]
        //[BasicAuthentication(RequireSsl = false)]
        public IEnumerable<Parent> Get(int InstID)
        {
            using (var Context = new EduSpecWebAPIDataContext())
            {
                var ParentsList = Context.Get_WebAPI_Parents(InstID).ToList().AsEnumerable();
                Parents = (from p in ParentsList
                           select new Parent
                            {
                                InstID = p.InstID,
                                ParentID = p.ParentID,
                                Title = p.Title,
                                Initials = p.Initials,
                                FullName = p.FullName,
                                Surname = p.Surname,
                                IDNumber = p.IDNumber,
                                Employer = p.Employer,
                                Occupation = p.Occupation,
                                StreetAddress1 = p.StreetAddress1,
                                StreetAddress2 = p.StreetAddress2,
                                StreetAddress3 = p.StreetAddress3,
                                StreetCode = p.StreetCode,
                                PostalAddress1 = p.PostalAddress1,
                                PostalAddress2 = p.PostalAddress2,
                                PostalAddress3 = p.PostalAddress3,
                                PostalCode = p.PostalCode,
                                TelHome = p.TelHome,
                                TelWork = p.TelWork,
                                TelCell = p.TelCell,
                                TelFax = p.TelFax,
                                EMail = p.EMail,
                                Gender = p.Gender,
                                Homelanguage = p.Homelanguage,
                                SpouseFullName = p.SpouseFullName,
                                SpouseSurname = p.SpouseSurname,
                                SpouseIDNumber = p.SpouseIDNumber,
                                SpouseOccupation = p.SpouseOccupation,
                                SpouseWorkTel = p.SpouseWorkTel,
                                SpouseGender = p.SpouseGender,
                                SpouseCell = p.SpouseCell,
                                SpouseEmail = p.SpouseEmail,
                                MaritalStatus = p.MaritalStatus,
                                Relationship = p.Relationship,
                                Status = p.Status

                            }).ToList();
                return Parents;
            }
        }

        // POST: api/Parents
        [HttpPost]
        //[BasicAuthentication(RequireSsl = false)]
        [Route("api/Parents/{InstID}")]
        public void Post(Parent[] Parents, int InstID)
        {
            using (var Context = new EduSpecWebAPIDataContext())
            {
                Context.Set_WebAPI_ParentsClear(InstID);

                foreach (var Parent in Parents)
                {
                    Context.Set_WebAPI_Parents(
                        InstID,
                        Parent.ParentID,
                        Parent.Title,
                        Parent.Initials,
                        Parent.FullName,
                        Parent.Surname,
                        Parent.IDNumber,
                        Parent.Employer,
                        Parent.Occupation,
                        Parent.StreetAddress1,
                        Parent.StreetAddress2,
                        Parent.StreetAddress3,
                        Parent.StreetCode,
                        Parent.PostalAddress1,
                        Parent.PostalAddress2,
                        Parent.PostalAddress3,
                        Parent.PostalCode,
                        Parent.TelHome,
                        Parent.TelWork,
                        Parent.TelCell,
                        Parent.TelFax,
                        Parent.EMail,
                        Parent.Gender,
                        Parent.Homelanguage,
                        Parent.SpouseFullName,
                        Parent.SpouseSurname,
                        Parent.SpouseIDNumber,
                        Parent.SpouseOccupation,
                        Parent.SpouseWorkTel,
                        Parent.SpouseGender,
                        Parent.SpouseCell,
                        Parent.SpouseEmail,
                        Parent.MaritalStatus,
                        Parent.Relationship,
                        Parent.Status
                        );
                }
            }
        }

        // PUT: api/Parents/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Parents/5
        public void Delete(int id)
        {
        }
    }
}
