namespace Model
{
    public class BaseEntity
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Updated { get; set; }

        public virtual void SetUpdated()
        {
            this.Updated = DateTime.Now;
        }
    }
}
