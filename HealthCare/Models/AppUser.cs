using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models
{
    public class AppUser
    {


        [Key]
        public int userId { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public string phone { get; set; }


    }
}
