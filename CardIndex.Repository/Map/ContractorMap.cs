using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CardIndex.Domain.Models;


namespace CardIndex.Repository.Map
{
    public class ContractorMap : EntityTypeConfiguration<Contractor>
    {
        public ContractorMap()
        {
            ToTable("Contractor");

            HasKey(t => t.Id);
            Property(t => t.Name).IsRequired().HasMaxLength(30);
            Property(t => t.Surname).IsOptional().HasMaxLength(30);
            Property(t => t.Email).IsRequired().HasMaxLength(50);
            Property(t => t.PhoneNumber).IsOptional().HasMaxLength(15);
            Property(t => t.DateOfBirth).IsOptional();

            Property(t => t.AddressId).IsOptional();

            HasOptional(p => p.Address).WithMany().HasForeignKey(x => x.AddressId).WillCascadeOnDelete(true);
        }
    }
}
