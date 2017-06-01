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

        /// <summary>
        /// Jelszó.
        /// </summary>
        [Required(ErrorMessage = "A jelszó megadása kötelező.")]
        [DataType(DataType.Password)]
        public String UserPassword { get; set; }

        /// <summary>
        /// Bejelentkezés megjegyzése.
        /// </summary>
        public Boolean RememberLogin { get; set; }
    }
}