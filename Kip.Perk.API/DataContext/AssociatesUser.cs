//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kip.Perk.API.DataContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class AssociatesUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public int TotalPoints { get; set; }
        public bool CanVerifyClaims { get; set; }
    }
}
