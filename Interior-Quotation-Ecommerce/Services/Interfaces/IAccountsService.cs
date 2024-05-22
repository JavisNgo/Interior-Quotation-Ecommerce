using Interior_Quotation_Ecommerce.Entities;
using Interior_Quotation_Ecommerce.Models.GET;

namespace Interior_Quotation_Ecommerce.Services.Interfaces
{
    public interface IAccountsService
    {
        AccountsDTO? AuthenticateUser(AccountsDTO loginInfo);
        string? HashPassword(string password);
        (string accessToken, string refreshToken) GenerateTokens(AccountsDTO account);
        public string? GenerateToken(Accounts account);
        Tokens? GetRefreshTokenByAccountId(int AccountId);
        (bool isValid, string username) ValidateRefreshToken(string refreshToken);
        Accounts GetAccountById(int AccountId);
        bool IsExistedEmail(string email);
        Accounts? GetAccountByUsername(string username);
        bool CreateAccountCustomer(AccountsDTO newAccount);
        bool CreateAccountContractor(AccountsDTO newAccount);
        string? GetAccountRole(string username, Accounts account);
        Contractors? GetContractorByAccount(Accounts account);
        ContractorsDTO? GetContractorInformation(string username, Accounts account, Contractors contractor);
        Customers? GetCustomerByUsername(Accounts account);
        CustomerDTO? GetCustomersInformation(string username, Accounts account, Customers customer);
        (double? TotalRevenue, double? TotalDepositRevenue, int TotalCustomers, int TotalFilterdCustomer, int TotalContractors, int TotalRequests, int TotalSignedRequests, int TotalOnGoingRequests, int TotalRejectedRequests, int TotalConstructs, int TotalProducts) GetPlatformStats();
        (double? TotalRevenue, int TotalRequests, int TotalSignedRequests, int TotalOnGoingRequests, int TotalRejectedRequests, int TotalConstructs, int TotalProducts) GetContractorStats(int contractorId);

    }
}
