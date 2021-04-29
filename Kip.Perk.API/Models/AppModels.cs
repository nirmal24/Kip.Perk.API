using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kip.Perk.API.Models
{
    enum VerificationStatusEnum
    {
        Approved = 1,
        Rejected = 2,
        Pending = 3,
    }

    public class ClaimsModel
    {
        public string ClaimId { get; set; }
        public string EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int ClaimFor { get; set; }
        public string ImageURL { get; set; }
        public List<UserModel> Witnesses { get; set; }
        public int PointsToClaim { get; set; }
        public DateTime? ClaimDate { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public DateTime? DateOfVerification { get; set; }
        public string Description { get; set; }
    }

    public class UserModel
    {
        public string EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ImageURL { get; set; }
        public int TotalPoints { get; set; }
        public List<int> Claims { get; set; }
    }
}