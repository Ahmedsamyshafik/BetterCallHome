﻿using Microsoft.AspNetCore.Http;

namespace Core.Features.Apartments.Commands.Models
{
    public class EditApartmentDTO
    {
        public int ApartmentId { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }
        public int numberOfUsers { get; set; }
        public decimal price { get; set; }

        public IFormFile CoverImage { get; set; }

        public List<IFormFile> Pics { get; set; }

        public IFormFile? Video { get; set; }

    }
}