using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper.QueryableExtensions;

using BusTicketsSystem.Data;
using BusTicketsSystem.Models;
using BusTicketsSystem.Services.Interfaces;

namespace BusTicketsSystem.Services
{
    public class BusCompanyService : IBusCompanyService
    {
        private readonly BusTicketsSystemContext db;

        public BusCompanyService(BusTicketsSystemContext db)
        {
            this.db = db;
        }

        private IEnumerable<TModel> By<TModel>(Func<BusCompany, bool> predicate) =>
            this.db.BusCompanies
                .Where(predicate)
                .AsQueryable()
                .ProjectTo<TModel>();

        public TModel ById<TModel>(int id)
            => this.By<TModel>(bs => bs.Id == id).FirstOrDefault();

        public TModel ByName<TModel>(string name) 
            => this.By<TModel>(bc => bc.Name == name).FirstOrDefault();

        public bool Exists(string name)
             => this.ByName<BusCompany>(name) != null;

        public bool Exists(int id) => this.ById<BusCompany>(id) != null;
    }
}