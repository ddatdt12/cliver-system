using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliverSystem.Models
{
    public class Parameters
    {
        public int NumDaysReturnMoney { get; set; }


    }

    public class ParametersConfiguration : IEntityTypeConfiguration<Parameters>
    {
        public void Configure(EntityTypeBuilder<Parameters> builder)
        {
            builder.HasKey(p => new { p.NumDaysReturnMoney });

            builder.Property(m => m.NumDaysReturnMoney)
             .ValueGeneratedNever();
        }
    }
}
