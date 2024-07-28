using System;
using System.Collections.Generic;

namespace HealthCare.Models;

public partial class Visit
{
    public int VisitId { get; set; }

    public int? PtId { get; set; }

    public int? DoctorId { get; set; }

    public DateOnly? VisitDate { get; set; }

    public string? Purpose { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Pt { get; set; }

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
