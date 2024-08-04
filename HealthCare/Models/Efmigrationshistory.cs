using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models;

public partial class Efmigrationshistory
{

    [Key]
    public string MigrationId { get; set; } = null!;

    public string ProductVersion { get; set; } = null!;
}
