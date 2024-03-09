
using Microsoft.AspNetCore.Identity;
using Services.Abstracts;

namespace Services.Implementations
{
    public class GlobalErrorResponse : IGlobalErrorResponse
    {
        public string ErrorCode(IdentityResult identityResult)
        {
            var Es = string.Empty;
            foreach (var error in identityResult.Errors)
            {
                Es += $"{error.Description} ,";
            }
            return ($"Faild.. {Es}");

        }


    }
}
