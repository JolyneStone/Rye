using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Monica.Test.Domain
{
    public class DbTestConfiguration : IEntityTypeConfiguration<DbTest>
    {
        public void Configure(EntityTypeBuilder<DbTest> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Ignore(d => d.Key);
            builder.Property(d => d.Id).ValueGeneratedOnAdd();
        }
    }
}