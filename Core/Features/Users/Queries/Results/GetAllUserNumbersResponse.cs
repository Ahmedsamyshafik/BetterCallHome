using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.Users.Queries.Results
{
    public class GetAllUserNumbersResponse
    {
        public int AllUsers {  get; set; }
        public int Students { get; set; }
        public int Owners { get; set; }
    }
}
