using System.ComponentModel.DataAnnotations;

namespace WiSave.Core.Models.Requests;

public record RegisterRequest(
    [Required] string Email,
    [Required] string Password,
    [Required] string FirstName,
    [Required] string LastName
);