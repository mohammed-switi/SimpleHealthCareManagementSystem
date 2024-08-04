using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models;

public partial class Patient
{
    [Key]

    public int PtId { get; set; }

    public string? PtName { get; set; }

    public string? Gender { get; set; }

    public DateOnly? Dob { get; set; }

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
