using System;
using System.ComponentModel.DataAnnotations;

namespace MyExam.Models
{
    /// <summary>
    /// Felhasználóval kapcsolatos információk.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Felhasználónév.
        /// </summary>
        [Required(ErrorMessage = "A UserId megadása kötelező.")]
        public String UserId { get; set; }
    }
}