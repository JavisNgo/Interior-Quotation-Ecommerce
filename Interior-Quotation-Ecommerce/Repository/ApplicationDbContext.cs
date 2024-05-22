using Interior_Quotation_Ecommerce.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interior_Quotation_Ecommerce.Repository
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Accounts>? Accounts { get; set; }
        public DbSet<Categories>? Categories { get; set; }
        public DbSet<ConstructProducts>? ConstructProducts { get; set; }
        public DbSet<Constructs>? Constructs { get; set; }
        public DbSet<Contractors>? Contractors { get; set; }
        public DbSet<Contracts>? Contracts { get; set; }
        public DbSet<Customers>? Customers { get; set; }
        public DbSet<Messages>? Messages { get; set; }
        public DbSet<Products>? Products { get; set; }
        public DbSet<RequestDetails>? RequestDetails { get; set; }
        public DbSet<Requests>? Requests { get; set; }
        public DbSet<Tokens>? Token { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ConstructProducts
            modelBuilder.Entity<ConstructProducts>().HasOne(cp => cp.Construct).WithMany(c => c.ConstructProducts).HasForeignKey(cp => cp.ConstructId);
            modelBuilder.Entity<ConstructProducts>().HasOne(cp => cp.Product).WithMany(p => p.ConstructProducts).HasForeignKey(cp => cp.ProductId).OnDelete(DeleteBehavior.Restrict);

            //Constructs
            modelBuilder.Entity<Constructs>().HasOne(c => c.Category).WithMany(cate => cate.Constructs).HasForeignKey(c => c.CategoryId);
            modelBuilder.Entity<Constructs>().HasOne(c => c.Contractor).WithMany(c => c.Constructs).HasForeignKey(c => c.ContractorId);

            //Contractors
            modelBuilder.Entity<Contractors>().HasOne(c => c.Account).WithOne(a => a.Contractor).HasForeignKey<Contractors>(c => c.AccountId);

            //Contracts
            modelBuilder.Entity<Contracts>().HasOne(c => c.Request).WithOne(r => r.Contract).HasForeignKey<Contracts>(c => c.RequestId);

            //Customers
            modelBuilder.Entity<Customers>().HasOne(c => c.Account).WithOne(a => a.Customer).HasForeignKey<Customers>(c => c.AccountId);

            //Messages
            modelBuilder.Entity<Messages>().HasOne(m => m.Customer).WithMany(c => c.Messages).HasForeignKey(m => m.CustomerId);
            modelBuilder.Entity<Messages>().HasOne(m => m.Contractor).WithMany(c => c.Messages).HasForeignKey(m => m.ContractorId);

            //Products
            modelBuilder.Entity<Products>().HasOne(p => p.Contractor).WithMany(c => c.Products).HasForeignKey(p => p.ContractorId);

            //RequestDetails
            modelBuilder.Entity<RequestDetails>().HasOne(r => r.Product).WithMany(p => p.RequestDetails).HasForeignKey(r => r.ProductId);
            modelBuilder.Entity<RequestDetails>().HasOne(r => r.Request).WithMany(r => r.RequestDetails).HasForeignKey(r => r.RequestId).OnDelete(DeleteBehavior.Restrict);

            //Request
            modelBuilder.Entity<Requests>().HasOne(r => r.Customer).WithMany(c => c.Requests).HasForeignKey(r => r.CustomerId);
            modelBuilder.Entity<Requests>().HasOne(r => r.Contractor).WithMany(c => c.Requests).HasForeignKey(r => r.ContractorId);

            //Token
            modelBuilder.Entity<Tokens>().HasOne(t => t.Account).WithMany(a => a.Tokens).HasForeignKey(t => t.AccountId);
        }
    }
}
