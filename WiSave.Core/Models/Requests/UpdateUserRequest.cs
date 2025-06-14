namespace WiSave.Core.Models.Requests;

public record UpdateUserRequest(
    string FirstName,
    string LastName
);