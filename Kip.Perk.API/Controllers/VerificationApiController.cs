using Kip.Perk.API.DataContext;
using Kip.Perk.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kip.Perk.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/verification")]
    public class VerificationApiController : ApiController
    {
        [HttpGet]
        [Route("getallclaims")]
        public List<ClaimsModel> GetAllClaims()
        {
            using (var db = new Entities())
            {
                var claims =
                    (from x in db.UserClaims.AsQueryable()
                     join au in db.AssociatesUsers.AsQueryable()
                     on x.UserId equals au.UserId
                     join u in db.AspNetUsers.AsQueryable()
                     on x.UserId equals u.Id
                     select new ClaimsModel()
                     {
                         ClaimDate = x.ClaimDate,
                         ClaimFor = x.TeamId,
                         ClaimId = x.ClaimId,
                         DateOfVerification = x.DateOfVerification,
                         Description = x.Description,
                         Email = u.Email,
                         EmpId = x.UserId,
                         FirstName = au.FirstName,
                         LastName = au.LastName,
                         ImageURL = au.ImageUrl,
                         PointsToClaim = x.PointsToClaim,
                         Remarks = x.Remark,
                         Status = x.Status
                     }).ToList();
                claims.ForEach(x =>
                {
                    x.Witnesses = (from au in db.AssociatesUsers.AsQueryable()
                                   join u in db.AspNetUsers.AsQueryable() on au.UserId equals u.Id
                                   join w in db.Witnesses.AsQueryable() on u.Id equals w.WitnessId
                                   where w.ClaimId == x.ClaimId
                                   select new UserModel()
                                   {
                                       Email = u.Email,
                                       EmpId = u.Id,
                                       FirstName = au.FirstName,
                                       LastName = au.LastName,
                                       ImageURL = au.ImageUrl,
                                       TotalPoints = au.TotalPoints,
                                   }).ToList();
                    x.Witnesses.ForEach(c => c.Claims = db.UserTeams.Where(u => u.UserId == c.EmpId).Select(u => u.TeamId).ToList());
                });
                return claims;
            }
        }

        [HttpPost]
        [Route("submitclaim")]
        public HttpResponseMessage UpdateClaimStatus(ClaimsModel model)
        {
            using (var db = new Entities())
            {
                var claim=db.UserClaims.Where(x => x.ClaimId == model.ClaimId).FirstOrDefault();
                if (claim!=null)
                {
                    claim.Status = model.Status;
                    claim.Remark = model.Remarks;
                    claim.DateOfVerification = model.DateOfVerification;
                    if (model.Status==(int)VerificationStatusEnum.Approved)
                    {
                        var user = db.AssociatesUsers.AsQueryable().Where(x => x.Id == claim.UserId).FirstOrDefault();
                        if (user!=null)
                        {
                            user.TotalPoints += claim.PointsToClaim;
                        }
                    }
                }
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
