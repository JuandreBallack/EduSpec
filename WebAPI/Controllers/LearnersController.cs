using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EduSpecWebAPI.Models;

namespace EduSpecWebAPI.Controllers
{
    public class LearnersController : ApiController
    {
        public static IList<Learner> Learners = new List<Learner>();
        // GET: api/Learners
        [HttpGet]
        [Route("api/Learners/{InstID}")]
        //[BasicAuthentication(RequireSsl = false)]
        public IEnumerable<Learner> Get(int InstID)
        {
            using (var Context = new EduSpecWebAPIDataContext())
            {
                var LearnerList = Context.Get_WebAPI_Learners(InstID).ToList().AsEnumerable();
                Learners = (from l in LearnerList
                            select new Learner
                            {
                                InstID = l.InstID,
                                ID = l.ID,
                                LearnerID = l.LearnerID,
                                AccessionNo = l.AccessionNo,
                                EntryDate = l.EntryDate,
                                DateLeft = l.DateLeft,
                                Title = l.Title,
                                Initials = l.Initials,
                                FullName = l.FullName,
                                Surname = l.Surname,
                                BirthDate = l.BirthDate,
                                IDNumber = l.IDNumber,
                                Gender = l.Gender,
                                Language = l.Language,
                                TelHome = l.TelHome,
                                TelCell = l.TelCell,
                                Email = l.Email,
                                GradeID = l.GradeID,
                                ClassID = l.ClassID,
                                ClassName = l.ClassName,
                                Race = l.Race,
                                LuritsNumber = l.LuritsNumber ?? 0,
                                Status = l.Status
                            }).ToList();
                return Learners;
            }
        }
        // POST: api/Learners
        [HttpPost]
        //[BasicAuthentication(RequireSsl = false)]
        [Route("api/Learners/{InstID}")]
        public void Post(Learner[] Learners, int InstID)
        {
            using (var Context = new EduSpecWebAPIDataContext())
            {
                Context.Set_WebAPI_LearnersClear(InstID);

                foreach (var Learner in Learners)
                {
                    Context.Set_WebAPI_Learners(
                        InstID,
                        Learner.ID,
                        Learner.LearnerID,
                        Learner.AccessionNo,
                        Learner.EntryDate,
                        Learner.DateLeft,
                        Learner.Title,
                        Learner.Initials,
                        Learner.FullName,
                        Learner.Surname,
                        Learner.BirthDate,
                        Learner.IDNumber,
                        Learner.Gender,
                        Learner.Language,
                        Learner.TelHome,
                        Learner.TelCell,
                        Learner.Email,
                        Learner.GradeID,
                        Learner.ClassID,
                        Learner.ClassName,
                        Learner.Race,
                        Learner.LuritsNumber,
                        Learner.Status);
                }
            }
        }

        // PUT: api/Learners/5
        [HttpPut]
        //[BasicAuthentication(RequireSsl = false)]
        [Route("api/Learners/{InstID}")]
        public void Put(Learner[] Learners, int InstID)
        {
            using (var Context = new EduSpecWebAPIDataContext())
            {
                foreach (var Learner in Learners)
                {
                    Context.Set_WebAPI_Learners(
                        InstID,
                        Learner.ID,
                        Learner.LearnerID,
                        Learner.AccessionNo,
                        Learner.EntryDate,
                        Learner.DateLeft,
                        Learner.Title,
                        Learner.Initials,
                        Learner.FullName,
                        Learner.Surname,
                        Learner.BirthDate,
                        Learner.IDNumber,
                        Learner.Gender,
                        Learner.Language,
                        Learner.TelHome,
                        Learner.TelCell,
                        Learner.Email,
                        Learner.GradeID,
                        Learner.ClassID,
                        Learner.ClassName,
                        Learner.Race,
                        Learner.LuritsNumber,
                        Learner.Status);
                }
            }
        }

        // DELETE: api/Learners/5
        public void Delete(int id)
        {
        }
    }
}
