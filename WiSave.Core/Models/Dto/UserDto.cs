namespace WiSave.Core.Models.Dto;

public record UserDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    IList<string> Roles
);