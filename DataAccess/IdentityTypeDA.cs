using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StersTransport.Models;

namespace StersTransport.DataAccess
{
   public  class IdentityTypeDA
    {

        public List<IdentityType> GetIdentityTypes()
        {
            List<IdentityType> identityTypes = new List<IdentityType>();
            using (StersDB stersDB = new StersDB())
            {
                identityTypes = stersDB.IdentityTypes.ToList();
            }
            return identityTypes;
        }
    }
}
