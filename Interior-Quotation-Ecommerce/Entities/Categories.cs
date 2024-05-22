namespace Interior_Quotation_Ecommerce.Entities
{
    public class Categories : BaseEntity
    {
        //Properties
        public string? Name {  get; set; }

        //Relationship
        public ICollection<Constructs>? Constructs { get; set; }
    }
}