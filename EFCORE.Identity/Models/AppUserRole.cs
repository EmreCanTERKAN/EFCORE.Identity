﻿namespace EFCORE.Identity.Models;

public sealed class AppUserRole 
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
