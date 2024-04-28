using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO.Payment
{
    public class SecondStepRequestBody
    {
        public string auth_token { get; set; }
        public string delivery_needed { get; set; }
        public string amount_cents { get; set; }
        public string currency { get; set; }
        public List<Item> items { get; set; }


    }

    public class Item
    {
        public string name { get; set; }
        public string amount_cents { get; set; }
        public string description { get; set; }
        public string quantity { get; set; }
    }
}
