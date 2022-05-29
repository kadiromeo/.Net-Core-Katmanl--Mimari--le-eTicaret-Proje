﻿using Core_eTicaret.DataAccess.Data;
using Dapper;
using DataAccess.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class SP_Call : ISP_Call
    {
        private readonly ApplicationDbContext _db;
        private static string ConnectionString = "";
        public SP_Call(ApplicationDbContext db)
        {
            _db = db;
            ConnectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Execute(string prodecureName, DynamicParameters param = null)
        {
            using (SqlConnection baglanti = new SqlConnection(ConnectionString))
            {
                baglanti.Open();
                baglanti.Execute(prodecureName, param, commandType: System.Data.CommandType.StoredProcedure);

            }
        }

        public IEnumerable<T> List<T>(string prodecureName, DynamicParameters param = null)
        {
            using (SqlConnection baglanti = new SqlConnection(ConnectionString))
            {
                baglanti.Open();
                return baglanti.Query<T>(prodecureName, param, commandType: System.Data.CommandType.StoredProcedure);

            }
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string prodecureName, DynamicParameters param = null)
        {
            using (SqlConnection baglanti = new SqlConnection(ConnectionString))
            {
                baglanti.Open();
                var result = SqlMapper.QueryMultiple(baglanti, prodecureName, param, commandType: System.Data.CommandType.StoredProcedure);
                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();

                if (item1!=null && item2!=null)
                {
                    return new Tuple<IEnumerable<T1>,IEnumerable < T2 >> (item1, item2);
                }
            }
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(),new List<T2>());
        }

        public T oneRecord<T>(string prodecureName, DynamicParameters param = null)
        {
            using (SqlConnection baglanti = new SqlConnection(ConnectionString))
            {
                baglanti.Open();
                var value =baglanti.Query<T>(prodecureName, param, commandType: System.Data.CommandType.StoredProcedure);
                return (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T));
            }
        }

        public T Single<T>(string prodecureName, DynamicParameters param = null)
        {

            using (SqlConnection baglanti = new SqlConnection(ConnectionString))
            {
                baglanti.Open();
                return (T)Convert.ChangeType(baglanti.ExecuteScalar<T>(prodecureName, param, commandType: System.Data.CommandType.StoredProcedure), typeof(T));
            }
        }
    }
}
