using System.ComponentModel.DataAnnotations;

namespace WebApp_UnderTheHood.Authorization
{
    public class Credential
    {
        
        
            [Required]
            [Display(Name = "User Name")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remeber Me")]
            public bool RememberMe { get; set; }
        
    }
}
