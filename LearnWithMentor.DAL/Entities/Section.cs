using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LearnWithMentor.DAL.Entities
{
    public class Section
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Section()
        {
            PlanTasks = new HashSet<PlanTask>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanTask> PlanTasks { get; set; }
    }
}
