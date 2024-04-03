using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace StersTransport.Models
{
    public partial class StersDB : DbContext
    {
        public StersDB()
            : base("name=StersDB")
            //: base("StersDB")
        {

            Database.SetInitializer<StersDB>(null);


            // this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<ClientCode> ClientCode { get; set; }
        public virtual DbSet<Agent> Agent { get; set; }

        public virtual DbSet<Agent_Prices> Agent_Prices { get; set; }
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<IdentityType> IdentityTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ClientCode>()
               .Property(e => e.stamp)
               .IsFixedLength();


            modelBuilder.Entity<IdentityType>()
                .HasMany(e => e.ClientCodes)
                .WithOptional(e => e.IdentityType)
                .HasForeignKey(e => e.Sender_ID_Type);



            modelBuilder.Entity<Agent>()
                .HasMany(e => e.ClientCodes)
                .WithOptional(e => e.virtual_Agent)
                .HasForeignKey(e => e.AgentId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Agent>()
              .HasMany(e => e.ClientCodes1)
              .WithOptional(e => e.virtual_Branch)
              .HasForeignKey(e => e.BranchId);


            modelBuilder.Entity<Agent>()
               .HasMany(e => e.Agent_Prices)
               .WithOptional(e => e.virtual_Agent)
               .HasForeignKey(e => e.Agent_Id)
               .WillCascadeOnDelete();

            modelBuilder.Entity<Agent>()
               .HasMany(e => e.Agent_Prices1)
               .WithOptional(e => e.virtual_Agent1)
               .HasForeignKey(e => e.Agent_Id_Destination);



            /*
            modelBuilder.Entity<Branch>()
                .HasMany(e => e.ClientCodes)
                .WithOptional(e => e.Branch)
                .HasForeignKey(e => e.BranchId);
            */

            modelBuilder.Entity<Country>()
                .Property(e => e.Zip_Code_LINK)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.ClientCodes)
                .WithOptional(e => e.Country)
                .HasForeignKey(e => e.CountryAgentId);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.Agents)
                .WithOptional(e => e.Country)
                .HasForeignKey(e => e.CountryId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Country>()
                .HasMany(e => e.Cities)
                .WithOptional(e => e.Country)
                .HasForeignKey(e => e.CountryId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Currency>()
                .HasMany(e => e.Agents)
                .WithOptional(e => e.Currency)
                .HasForeignKey(e => e.CurrencyId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClientCodes)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Person_in_charge_Id)
                .WillCascadeOnDelete();
        }
    }
}
