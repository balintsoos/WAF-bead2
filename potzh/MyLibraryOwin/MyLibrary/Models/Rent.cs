//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyLibrary.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rent
    {
        public int RentId { get; set; }
        public int CopyId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
    
        public virtual Copy Copy { get; set; }
    }
}
