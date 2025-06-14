using System.ComponentModel.DataAnnotations;

namespace WiSave.Core.Models.Requests;

public record LoginRequest(
    [Required] string Email,
    [Required] string Password
);