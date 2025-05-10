namespace EFCORE.Identity.Dtos;

public sealed record LoginDto(
    string UserNameOrEmail,
    string Password);
