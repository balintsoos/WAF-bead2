using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace MyExam.Models
{
    /// <summary>
    /// Vendég.
    /// </summary>
    public class IdentityTeacher : IdentityUser
    {
        /// <summary>
        /// Teljes név.
        /// </summary>
        public String UserId { get; set; }
    }
}