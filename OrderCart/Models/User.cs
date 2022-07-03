using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace OrderCart.Models
{
    public class User
    {
        [Display(Name = "User ID")]    
        [Required(ErrorMessage = "Please enter Client Name.")]
        [StringLength(20, ErrorMessage = "Cannot Exceed more than 20 characters")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string UserId { get; set; }

        
    }

}
