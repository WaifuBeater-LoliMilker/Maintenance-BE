﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MaintenanceSystem.Models;

public partial class Users
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Fullname { get; set; }

    /// <summary>
    /// 0 = manager, 1 = normal user
    /// </summary>
    public int? Role { get; set; }

    public bool? IsAdmin { get; set; }
}