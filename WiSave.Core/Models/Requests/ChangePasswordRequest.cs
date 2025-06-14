using System.ComponentModel.DataAnnotations;

namespace WiSave.Core.Models.Requests;

public record ChangePasswordRequest(
    [Required] string CurrentPassword,
    [Required] string NewPassword
);