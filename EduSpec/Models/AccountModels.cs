using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace EduSpec.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("EduSpecConnectionString")
        {
        }
        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public int ImpersonationUserID { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Required.")]
        [StringLength(30, ErrorMessage = "Password must be betreen 5 and 30 characters.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class SMSModel
    {
        [Required(ErrorMessage = "Required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Cell number")]
        public string CellNumber { get; set; }

        [Required(ErrorMessage = "Required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Message")]        
        public string SMSMessage { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }



        private bool? rememberMe;
        [Display(Name = "Remember me?")]
        public bool? RememberMe
        {
            get
            {
                return rememberMe ?? false;
            }
            set
            {
                rememberMe = value;
            }
        }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [RegularExpression("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessage = "Please enter a valid email address.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
