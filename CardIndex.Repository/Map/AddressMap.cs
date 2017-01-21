using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CardIndex.Domain.Models;


namespace CardIndex.Repository.Map
{
    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            ToTable("Address");

            HasKey(t => t.Id);
            Property(t => t.Street).IsOptional().HasMaxLength(50);
            Property(t => t.ZipCode).IsOptional().HasMaxLength(10);
            Property(t => t.City).IsOptional().HasMaxLength(30);

        }
    }
}
