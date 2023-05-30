﻿using Rye.DataAccess;
using Rye.Entities;
using System;

namespace Rye.Test.Domain
{
    public class DbTest : EntityBase<int>
    {
        //public override int Key { get => Id; }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}