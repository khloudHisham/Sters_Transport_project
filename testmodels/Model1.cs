using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace StersTransport.testmodels
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<CODE_LIST> CODE_LIST { get; set; }
        public virtual DbSet<IdentityType> IdentityTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CODE_LIST>()
                .Property(e => e.stamp)
                .IsFixedLength();

            modelBuilder.Entity<IdentityType>()
                .HasMany(e => e.CODE_LIST)
                .WithOptional(e => e.IdentityType)
                .HasForeignKey(e => e.Sender_ID_Type);
        }
    }
}
