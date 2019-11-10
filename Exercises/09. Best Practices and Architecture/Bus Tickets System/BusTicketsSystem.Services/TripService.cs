using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper.QueryableExtensions;

using BusTicketsSystem.Data;
using BusTicketsSystem.Models;
using BusTicketsSystem.Models.Enums;
using BusTicketsSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusTicketsSystem.Services
{
    public class TripService : ITripService
    {
        private readonly BusTicketsSystemContext db;

        public TripService(BusTicketsSystemContext db)
        {
            this.db = db;
        }

        private IEnumerable<TModel> By<TModel>(Func<Trip, bool> predicate) =>
            this.db.Trips
                .Where(predicate)
                .AsQueryable()
                .ProjectTo<TModel>();

        public TModel ById<TModel>(int id)
            => this.By<TModel>(t => t.Id == id).FirstOrDefault();

        public bool Exists(int id) => this.ById<Trip>(id) != null;

        public void ChangeStatus(int id, string newStatusStr)
        {
            Trip trip = this.ById<Trip>(id);

            Status newStatus = Enum.Parse<Status>(newStatusStr);

            this.db.Entry(this.db.Trips.Find(trip.Id)).State = EntityState.Detached;

            trip.Status = newStatus;

            if (this.db.Entry(trip).State == EntityState.Detached)
            {
                this.db.Entry(trip).State = EntityState.Modified;
            }

            this.db.SaveChanges();
        }
    }
}