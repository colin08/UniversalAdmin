using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using Universal.Entity;


namespace Universal.DataCore
{
    public class MPUserDoctorsMapping:EntityTypeConfiguration<MPUserDoctors>
    {
        public MPUserDoctorsMapping()
        {
            HasKey(u => u.ID).HasRequired(u => u.MPUser).WithOptional(u => u.DoctorsInfo).WillCascadeOnDelete(false);
        }
    }
}
