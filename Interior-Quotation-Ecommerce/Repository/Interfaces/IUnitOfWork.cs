using Interior_Quotation_Ecommerce.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interior_Quotation_Ecommerce.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Accounts> AccountsRepository { get; }
        public IGenericRepository<Categories> CategoriesRepository { get; }
        public IGenericRepository<ConstructProducts> ConstructProductsRepository { get; }
        public IGenericRepository<Constructs> ConstructsRepository { get; }
        public IGenericRepository<Contractors> ContractorsRepository { get; }
        public IGenericRepository<Contracts> ContractsRepository { get; }
        public IGenericRepository<Customers> CustomersRepository { get; }
        public IGenericRepository<Messages> MessagesRepository { get; }
        public IGenericRepository<Products> ProductsRepository { get; }
        public IGenericRepository<RequestDetails> RequestDetailsRepository { get; }
        public IGenericRepository<Requests> RequestsRepository { get; }
        public IGenericRepository<Tokens> TokensRepository { get; }

        void Save();
    }
}
