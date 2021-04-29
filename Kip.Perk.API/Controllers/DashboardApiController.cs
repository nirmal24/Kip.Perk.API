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
    [RoutePrefix("api/dashboard")]
    [Authorize]
    public class DashboardApiController : ApiController
    {
        [HttpGet]
        [Route("getapprovedclaims")]
        public List<ClaimsModel> GetApprovedClaims()
        {
            using (var db = new Entities())
            {
                var claims =
                    (from x in db.UserClaims.AsQueryable()
                     join au in db.AssociatesUsers.AsQueryable()
                     on x.UserId equals au.UserId
                     join u in db.AspNetUsers.AsQueryable()
                     on x.UserId equals u.Id
                     where x.Status == (int)VerificationStatusEnum.Approved
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

        [HttpGet]
        [Route("getuserinfo")]
        public UserModel GetUserInfo(string id)
        {
            using (var db = new Entities())
            {
                var user = (from au in db.AssociatesUsers.AsQueryable()
                               join u in db.AspNetUsers.AsQueryable() on au.UserId equals u.Id
                               where u.Id== id
                               select new UserModel()
                               {
                                   Email = u.Email,
                                   EmpId = u.Id,
                                   FirstName = au.FirstName,
                                   LastName = au.LastName,
                                   ImageURL = au.ImageUrl,
                                   TotalPoints = au.TotalPoints,
                               }).FirstOrDefault();
                if (user!=null)
                {
                    user.Claims = db.UserTeams.Where(u => u.UserId == id).Select(u => u.TeamId).ToList());
                }
                return user;
            }
        }

        [HttpGet]
        [Route("getallteamsMembers")]
        public List<UserModel> GetAllTeamsMembers(List<int> claims,string id)
        {
            using (var db=new Entities())
            {
                var user = (from au in db.AssociatesUsers.AsQueryable()
                            join u in db.AspNetUsers.AsQueryable() on au.UserId equals u.Id
                            join t in db.UserTeams.AsQueryable() on u.Id equals t.UserId
                            where u.Id != id &&  claims.Contains(t.TeamId)
                            select new UserModel()
                            {
                                Email = u.Email,
                                EmpId = u.Id,
                                FirstName = au.FirstName,
                                LastName = au.LastName,
                                ImageURL = au.ImageUrl,
                                TotalPoints = au.TotalPoints,
                            }).ToList();
                
                    user.ForEach(c=>c.Claims = db.UserTeams.Where(u => u.UserId == c.EmpId).Select(u => u.TeamId).ToList());
                
                return user;

            }
        }
    }
}
