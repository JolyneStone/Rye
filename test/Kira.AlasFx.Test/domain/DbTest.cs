using Kira.AlasFx.Domain;
using System;

namespace Kira.AlasFx.Test.domain
{
    public class DbTest : EntityBase<int>
    {
        public override int Key { get => Id; }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}