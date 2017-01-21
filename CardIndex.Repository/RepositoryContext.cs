using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Conventions;
using CardIndex.Domain.Models;
using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;

namespace CardIndex.Repository
{
    public class RepositoryContext : DbContext, IRepositoryContext
    {
        #region Ctor

        public RepositoryContext() { }

        public RepositoryContext(string connectionString) : base(connectionString) { }

        #endregion

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        //public DbSet Set { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Potrzebne dla klas Identity
            //base.OnModelCreating(modelBuilder);
            //modelBuilder. = false;
            //Database.SetInitializer<RepositoryContext>(null);

            // using System.Data.Entity.ModelConfiguration.Conventions;
            //Wyłącza konwencję, która automatycznie tworzy liczbę mnogą dla nazw tabel w bazie danych
            // Zamiast Kategorie zostałaby stworzona tabela o nazwie Kategories
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Wyłącza konwencję CascadeDelete
            // CascadeDelete zostanie włączone za pomocą Fluent API
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Używa się Fluent API, aby ustalić powiązanie pomiędzy tabelami 
            // i włączyć CascadeDelete dla tego powiązania
            //modelBuilder.Entity<Ogloszenie>().HasRequired(x => x.Uzytkownik)
            //    .WithMany(x => x.Ogloszenia)
            //    .HasForeignKey(x => x.UzytkownikId)
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<CVApplication>().Ignore(e => e.CVFile);
            //modelBuilder.Entity<User>().ToTable("User");
            // Database.SetInitializer<RepositoryContext>(null);

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(type => !String.IsNullOrEmpty(type.Namespace))
.Where(type => type.BaseType != null && type.BaseType.IsGenericType
    && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
