namespace WiSave.Core.Models;

public record UserInfo(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    IList<string> Roles
);