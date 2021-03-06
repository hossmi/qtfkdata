﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QTFK.Data.Tests.Models;
using QTFK.Data.Tests.Services;
using QTFK.Services;
using System.Linq;
using QTFK.Services.DBIO;
using System.Configuration;
using QTFK.Data.Factory;
using QTFK.Data.Factory.Metadata;
using QTFK.Data.Extensions;
using QTFK.Data.Storage;

namespace QTFK.Data.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private IExpensesDB db;

        public UnitTest1()
        {
            IMetadataBuilder metadataBuilder;
            IDbBuilder dbBuilder;
            IStorage driver;
            IDbMetadata<IExpensesDB> dbMetadata;
            string connectionString;

            connectionString = ConfigurationManager.ConnectionStrings["tests"]?.ConnectionString;
            Assert.IsTrue(string.IsNullOrWhiteSpace(connectionString), $"Invalid 'tests' connection string in app.config");

            metadataBuilder = new DefaultMetadataBuilder();
            driver = new SqlServerStorage(connectionString);
            dbBuilder = new SqlServerDbBuilder(driver);

            dbMetadata = metadataBuilder.scan<IExpensesDB>();
            this.db = dbBuilder.createDb(dbMetadata);
        }

        [TestMethod]
        public void userTestMethod1()
        {
            IEnumerable<IUser> users;
            IUser user;

            foreach (IUser user1 in this.db.Users)
            {
                Console.WriteLine($"{user1.Name} - {user1.Mail}");
            }

            user = this.db.Users.create(u =>
            {
                u.Name = "pepe";
                u.Mail = "pepe@tronco.es";
                u.SignDate = DateTime.Now;
            });

            user.Mail = "pepe@gmail.com";
            this.db.Users.update(user);

            this.db.Users.delete(user);

            users = this.db.Users.whereMailContains("Frank");
            foreach (IUser filteredUser in users)
            {
                filteredUser.Name += "_check";
                this.db.Users.update(filteredUser);
            }

        }

        [TestMethod]
        public void currencyTestMethod1()
        {
            ICurrency euroCurrency, dollardCurrency;
            ICurrencyConversion eurUsd;

            this.db.CurrencyExchanges.deleteAll();
            this.db.Currencies.deleteAll();

            euroCurrency = this.db.Currencies.create(euro => euro.Name = "Euro");
            dollardCurrency = this.db.Currencies.create(dollard => dollard.Name = "US Dollard");

            eurUsd = this.db.CurrencyExchanges.create(x =>
            {
                x.Date = DateTime.Now;
                x.From = euroCurrency;
                x.To = dollardCurrency;
                x.Value = 1.16m;
            });

            foreach (ICurrencyConversion exchange in this.db.Currencies
                .First(c => c.Name == "Euro")
                .Exchanges)
            {
                Console.WriteLine($"1 {exchange.From.Name} = {exchange.Value} {exchange.To.Name}s at {exchange.Date}");
            }

            foreach (ICurrencyConversion exchange in this.db.CurrencyExchanges)
            {
                exchange.Value += 0.00005m;
            }

            foreach (IExpense expense in this.db.Expenses)
            {
                Console.WriteLine($"{expense.Concept} - {expense.Date}");
            }
        }

        [TestMethod]
        public void paginationTests1()
        {
            IPageCollection<IExpense> pages;
            
            pages = this.db.Expenses.getPages(pageSize: 10);

            Assert.AreEqual(3, pages.Count);
            Assert.AreEqual(10, pages[0].Count);
            Assert.AreEqual(10, pages[1].Count);
            Assert.AreEqual(3, pages[2].Count);
            throw new NotImplementedException("This test method needs more test code.");
        }

    }
}
