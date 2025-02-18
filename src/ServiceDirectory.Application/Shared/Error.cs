namespace ServiceDirectory.Application.Shared;

public readonly record struct Error(string Description, ErrorType Type);