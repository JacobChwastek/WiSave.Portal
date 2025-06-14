using System.ComponentModel.DataAnnotations;

namespace WiSave.Core.Models.Requests;

public record RefreshTokenRequest(
    [Required] string RefreshToken
);