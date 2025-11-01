using HRSystem.Csharp.Domain.Models.Common;

namespace HRSystem.Csharp.Domain.Models.Roles;

public class RoleListRequestModel : PaginationRequestModel
{
    public string? RoleName { get; set; }
}

public class RoleListResponseModel : PagedResult<RoleResponseModel>
{
}