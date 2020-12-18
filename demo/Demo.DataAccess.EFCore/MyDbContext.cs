using Microsoft.EntityFrameworkCore;
using Monica.EntityFrameworkCore;
using System;

namespace Demo.DataAccess.EFCore
{
    public class MyDbContext : DbContextBase
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
