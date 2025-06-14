using System.ComponentModel.DataAnnotations;

namespace WiSave.Core.Models.Requests;

public record AssignRoleRequest(
    [Required] string UserId,
    [Required] string RoleName
);