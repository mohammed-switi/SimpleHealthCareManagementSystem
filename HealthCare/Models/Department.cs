using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models;

public partial class Department
{
    [Key]

    public int DpId { get; set; }

    public string? DpName { get; set; }

    public string? Location { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
