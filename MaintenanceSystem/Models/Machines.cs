﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MaintenanceSystem.Models;

public partial class Machines
{
    public int Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string SerialNumber { get; set; }

    public int? StationId { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}