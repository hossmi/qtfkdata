﻿using QTFK.Models;
using QTFK.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTFK.Extensions.DBIO.QueryFactory
{
    public static class QueryFactoryDataExtension
    {
        public static IEnumerable<T> Select<T>(this IQueryFactory<T> factory, Action<IDBQuerySelect> queryBuild) where T : new()
        {
            var query = factory.NewSelect();
            queryBuild(query);
            return factory.DB.Get<T>(query);
        }

        public static int Insert(this IInsertQueryFactory factory, Action<IDBQueryInsert> queryBuild)
        {
            var query = factory.NewInsert();
            queryBuild(query);
            return factory.DB.Set(query);
        }

        public static int Update(this IUpdateQueryFactory factory, Action<IDBQueryUpdate> queryBuild)
        {
            var query = factory.NewUpdate();
            queryBuild(query);
            return factory.DB.Set(query);
        }

        public static int Delete(this IDeleteQueryFactory factory, Action<IDBQueryDelete> queryBuild)
        {
            var query = factory.NewDelete();
            queryBuild(query);
            return factory.DB.Set(query);
        }
    }
}
