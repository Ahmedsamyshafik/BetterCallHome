﻿using Domin.Models;

namespace Services.Abstracts
{
    public interface IUsersApartmentsServices
    {
        Task<string> AddAsync(string userid, int apartmentId);
        Task<List<UsersApartments>> GetRecordsByApartmentdIds(List<int> ids);
        bool AnyStudnets(int apartmentId);
        int GetCountStudentsInApartment(int ApartmentID);
    }
}