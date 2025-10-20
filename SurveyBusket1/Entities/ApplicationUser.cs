﻿using Microsoft.AspNetCore.Identity;

namespace SurveyBusket8.Entities;

public class ApplicationUser  : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<RefreshToken> RefreshTokens { get; set; } = [];
}
