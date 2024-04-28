using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO.Payment
{
    public class FormViewModel 
    {

        public string ApartmentName { get; set; }
        public int Price { get; set; }
       

        public string apartment { get; set; }
        public string email { get; set; }
        public string floor { get; set; }
        public string first_name { get; set; }
        public string street { get; set; }
        public string building { get; set; }
        public string phone_number { get; set; }
        public string shipping_method { get; set; }
        public string postal_code { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string last_name { get; set; }
        public string state { get; set; }
    }
}
