using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper.QueryableExtensions;

using BusTicketsSystem.Data;
using BusTicketsSystem.Models;
using BusTicketsSystem.Services.Interfaces;

namespace BusTicketsSystem.Services
{
    public class BusStationService : IBusStationService
    {
        private BusTicketsSystemContext db;

        public BusStationService(BusTicketsSystemContext db)
        {
            this.db = db;
        }

        private IEnumerable<TModel> By<TModel>(Func<BusStation, bool> predicate) =>
            this.db.BusStations
                .Where(predicate)
                .AsQueryable()
                .ProjectTo<TModel>();

        public TModel ById<TModel>(int id)
            => this.By<TModel>(bs => bs.Id == id).FirstOrDefault();

        public bool Exists(int id) => this.ById<BusStation>(id) != null;
    }
}