﻿using QTFK.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTFK.Data.Tests.Models
{
    public class SampleClass
    {
        [Key]
        public int? ID { get; set; }

        [Unique]
        public string Name { get; set; }

        public decimal WalletCash { get; set; }
    }
}