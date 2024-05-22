using Interior_Quotation_Ecommerce.Entities;
using Interior_Quotation_Ecommerce.Repository.Interfaces;

namespace Interior_Quotation_Ecommerce.Repository.Implements
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext context;
        private GenericRepository<Accounts> _accountRepository;
        private GenericRepository<Categories> _categoryRepository;
        private GenericRepository<Contractors> _contractorsRepository;
        private GenericRepository<Customers> _customersRepository;
        private GenericRepository<Messages> _messagesRepository;
        private GenericRepository<Products> _productsRepository;
        private GenericRepository<Constructs> _constructsRepository;
        private GenericRepository<Contracts> _contractsRepository;
        private GenericRepository<ConstructProducts> _constructProductsRepository;
        public GenericRepository<Requests> _requestsRepository;
        public GenericRepository<RequestDetails> _requestDetailsRepository;
        public GenericRepository<Tokens> _tokenRepository;

        public UnitOfWork(ApplicationDbContext context) {
            this.context = context;
        }
        public IGenericRepository<Accounts> AccountsRepository => _accountRepository ??= new GenericRepository<Accounts>(context);

        public IGenericRepository<Categories> CategoriesRepository => _categoryRepository ??= new GenericRepository<Categories>(context);

        public IGenericRepository<ConstructProducts> ConstructProductsRepository => _constructProductsRepository ??= new GenericRepository<ConstructProducts>(context);

        public IGenericRepository<Constructs> ConstructsRepository => _constructsRepository ??= new GenericRepository<Constructs>(context);

        public IGenericRepository<Contractors> ContractorsRepository => _contractorsRepository ??= new GenericRepository<Contractors>(context);

        public IGenericRepository<Contracts> ContractsRepository => _contractsRepository ??= new GenericRepository<Contracts>(context);

        public IGenericRepository<Customers> CustomersRepository => _customersRepository ??= new GenericRepository<Customers>(context);

        public IGenericRepository<Messages> MessagesRepository => _messagesRepository ??= new GenericRepository<Messages>(context);

        public IGenericRepository<Products> ProductsRepository => _productsRepository ??= new GenericRepository<Products>(context);

        public IGenericRepository<RequestDetails> RequestDetailsRepository => _requestDetailsRepository ??= new GenericRepository<RequestDetails>(context);

        public IGenericRepository<Requests> RequestsRepository => _requestsRepository ??= new GenericRepository<Requests>(context);

        public IGenericRepository<Tokens> TokensRepository => _tokenRepository ??= new GenericRepository<Tokens>(context);

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
