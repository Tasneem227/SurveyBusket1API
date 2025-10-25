﻿namespace SurveyBusket1.Abstractions;

public record Error(
    string Code,
    string Description
    )
{
    public static readonly Error None = new Error(string.Empty , string.Empty);
}
    
