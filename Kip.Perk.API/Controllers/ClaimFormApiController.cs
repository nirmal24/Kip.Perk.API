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
    [RoutePrefix("api/claimform")]
    [Authorize]
    public class ClaimFormApiController : ApiController
    {
        [HttpPost]
        [Route("submitclaim")]
        public HttpResponseMessage SubmitClaim(ClaimsModel model)
        {
            using (var db = new Entities())
            {
                var claimId = Guid.NewGuid().ToString();
                db.UserClaims.Add(
                    new UserClaim()
                    {
                        ClaimDate = model.ClaimDate,
                        ClaimId = claimId,
                        PointsToClaim = model.PointsToClaim,
                        Status = (int)VerificationStatusEnum.Pending,
                        TeamId = model.ClaimFor,
                        UserId = model.EmpId,
                        Description=model.Description,
                    });
                model.Witnesses.ForEach(x => {
                    db.Witnesses.Add(new Witness() {
                        ClaimId = claimId,
                        WitnessId = x.EmpId
                    });
                });

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
        }
    }
}
