using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyExam.Models
{
    public class IdentityTeacher : IdentityUser
    {
        /// <summary>
        /// Teljes név.
        /// </summary>
        public String UserId { get; set; }
    }
}