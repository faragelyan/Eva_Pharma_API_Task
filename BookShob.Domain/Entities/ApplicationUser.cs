﻿using Microsoft.AspNetCore.Identity;

public class ApplicationUser :IdentityUser<Guid>
{
    public string FullName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}