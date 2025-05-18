﻿using System;
using System.Collections.Generic;

namespace MoneyFlow.AuthenticationService.Infrastructure.Data.Entities;

public partial class User
{
    public int IdUser { get; set; }

    public string Login { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public DateTime DateRegistration { get; set; }

    public DateTime DateEntry { get; set; }

    public DateTime DateUpdate { get; set; }

    public int? IdGender { get; set; }

    public int IdRole { get; set; }

    public virtual Gender? IdGenderNavigation { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
