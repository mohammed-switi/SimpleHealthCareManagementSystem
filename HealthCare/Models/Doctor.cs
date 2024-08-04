using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models;

public partial class Doctor
{
    [Key]

    public int DoctorId { get; set; }

    public int? DpId { get; set; }

    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public virtual Department? Dp { get; set; }

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
