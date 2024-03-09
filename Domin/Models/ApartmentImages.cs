﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.Models
{
    public class ApartmentImages
    {
        [Key]
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }

        [ForeignKey("Apartment")]
        public int ApartmentID { get; set; }
        public Apartment Apartment { get; set; }
    }
}
