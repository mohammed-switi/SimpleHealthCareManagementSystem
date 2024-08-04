using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HealthCare.Models
{
    public class AppUser : IdentityUser<int>
    {




        public string? firstName { get; set; }
        

        public string? lastName { get; set; }



        public  byte[]? PasswordHashed { get; set; }

        public byte[]? PasswordSalt { get; set; }


        public string? phone { get; set; }


    }
}
