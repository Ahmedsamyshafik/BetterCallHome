using Infrastructure.DTO.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstracts
{
    public interface IPaymentService
    {
        Task<string> Index(FormViewModel model);
    }
}
