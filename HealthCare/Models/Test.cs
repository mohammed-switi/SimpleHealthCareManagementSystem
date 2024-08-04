using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models;

public partial class Test
{
    [Key]

    public int TestId { get; set; }

    public int? PtId { get; set; }

    public string? TestType { get; set; }

    public int? DoctorId { get; set; }

    public int? VisitId { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Pt { get; set; }

    public virtual Visit? Visit { get; set; }
}
