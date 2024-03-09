using Microsoft.AspNetCore.Identity;
namespace Services.Abstracts
{

    public interface IGlobalErrorResponse
    {
        string ErrorCode(IdentityResult identityResult);
    }

}
