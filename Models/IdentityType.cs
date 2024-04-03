namespace StersTransport.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class IdentityType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IdentityType()
        {
            ClientCodes = new HashSet<ClientCode>();
        }

        [Key]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Shortcut { get; set; }

        [StringLength(50)]
        public string NameAr { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientCode> ClientCodes { get; set; }
    }
}
