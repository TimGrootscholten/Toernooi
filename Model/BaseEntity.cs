using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class BaseEntity
    {
        public virtual Guid Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime Created { get; set; }
        public virtual DateTime Updated { get; set; }

        public virtual void SetUpdated()
        {
            this.Updated = DateTime.UtcNow;
        }
    }
}
