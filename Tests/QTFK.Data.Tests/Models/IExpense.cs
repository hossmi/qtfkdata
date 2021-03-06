﻿using System;
using QTFK.Data.Attributes;

namespace QTFK.Data.Tests.Models
{
    public interface IExpense : IEntity
    {
        [Id]
        int Id { get; set; }
        string Concept { get; set; }
        DateTime Date { get; set; }

        IView<ICategory> Categories { get; }
        IView<IUser> Debtors { get; }
        IView<IPayment> Contributors { get; }
    }
}