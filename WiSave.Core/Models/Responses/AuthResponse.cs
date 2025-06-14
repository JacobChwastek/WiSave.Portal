namespace WiSave.Core.Models.Responses;

public record AuthResponse(
    string Token,
    string RefreshToken,
    DateTime ExpiresAt,
    UserInfo User
);