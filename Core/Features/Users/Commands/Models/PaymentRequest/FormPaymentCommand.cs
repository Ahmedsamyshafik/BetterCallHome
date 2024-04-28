using Core.Bases;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.Users.Commands.Models.PaymentRequest
{
    public class FormPaymentCommand :IRequest<Response<string>>
    {
        public string ApartmentName { get; set; }
        public int Price { get; set; }

        public string UserID { get; set; }



    }
}
