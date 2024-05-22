using AutoMapper;
using Interior_Quotation_Ecommerce.Entities;
using Interior_Quotation_Ecommerce.Models.GET;
using Interior_Quotation_Ecommerce.Repository.Interfaces;
using Interior_Quotation_Ecommerce.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Interior_Quotation_Ecommerce.Services.Implements
{
    public class AccountsService : IAccountsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountsService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }
        public AccountsDTO? AuthenticateUser(AccountsDTO loginInfo)
        {
            AccountsDTO accountsView = null;
            try
            {
                string hashedPassword = HashPassword(loginInfo.Password);
                Accounts? account = _unitOfWork.AccountsRepository.Get(a => a.Username == loginInfo.Username && a.Password == hashedPassword).FirstOrDefault();
                if (account != null)
                {
                    accountsView = new AccountsDTO();
                    accountsView.Username = account.Username;
                    accountsView.Status = account.Status;
                    accountsView.Role = account.AccountRole.ToString();
                }
                return accountsView;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string? HashPassword(string password)
        {
            try
            {
                using (SHA512 sha512 = SHA512.Create())
                {
                    // Compute hash from the password bytes
                    byte[] hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));

                    // Convert the byte array to a hexadecimal string
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        stringBuilder.Append(hashBytes[i].ToString("x2"));
                    }

                    return stringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool CreateAccountContractor(AccountsDTO newAccount)
        {
            try
            {
                bool status = false;
                newAccount.Password = HashPassword(newAccount.Password);
                newAccount.Status = true;
                var account = _mapper.Map<Accounts>(newAccount);
                account.AccountRole = Accounts.Role.CONTRACTOR;

                _unitOfWork.AccountsRepository.Insert(account);
                _unitOfWork.Save();
                var insertedAccount = _unitOfWork.AccountsRepository.Get(a => a.Username == newAccount.Username).FirstOrDefault();
                if (insertedAccount != null)
                {
                    if (insertedAccount.AccountRole.Equals(Accounts.Role.CONTRACTOR))
                    {
                        var contractor = new Contractors
                        {
                            AccountId = insertedAccount.Id,
                            Name = newAccount.Name,
                            Email = newAccount.Email,
                            PhoneNumber = newAccount.PhoneNumber,
                            Address = newAccount.Address
                        };
                        _unitOfWork.ContractorsRepository.Insert(contractor);
                        _unitOfWork.Save();
                        status = true;
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                var insertedAccount = _unitOfWork.AccountsRepository.Get(a => a.Username == newAccount.Username).FirstOrDefault();
                if (insertedAccount != null)
                {
                    _unitOfWork.AccountsRepository.Delete(insertedAccount);
                    _unitOfWork.Save();
                }
                throw new Exception(ex.Message);
            }
        }

        public bool CreateAccountCustomer(AccountsDTO newAccount)
        {
            try
            {
                bool status = false;
                newAccount.Password = HashPassword(newAccount.Password);
                newAccount.Status = true;
                var account = _mapper.Map<Accounts>(newAccount);
                account.AccountRole = Accounts.Role.CUSTOMER;

                _unitOfWork.AccountsRepository.Insert(account);
                _unitOfWork.Save();

                var insertedAccount = _unitOfWork.AccountsRepository.Get(a => a.Username == newAccount.Username).FirstOrDefault();
                if (insertedAccount != null)
                {
                    if (insertedAccount.AccountRole.Equals(Accounts.Role.CUSTOMER))
                    {
                        var customer = new Customers
                        {
                            AccountId = insertedAccount.Id,
                            Name = newAccount.Name,
                            Email = newAccount.Email,
                            Address = newAccount.Address,
                            PhoneNumber = newAccount.PhoneNumber,
                        };
                        _unitOfWork.CustomersRepository.Insert(customer);
                        _unitOfWork.Save();
                        status = true;
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                var insertedAccount = _unitOfWork.AccountsRepository.Get(a => a.Username == newAccount.Username).FirstOrDefault();
                if (insertedAccount != null)
                {
                    _unitOfWork.AccountsRepository.Delete(insertedAccount);
                    _unitOfWork.Save();
                }
                throw new Exception(ex.Message);
            }
        }

        public string? GenerateToken(Accounts account)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var expirationTime = DateTime.UtcNow.AddHours(1);

                var claims = new List<Claim>
                {
                    new Claim("Role", account.AccountRole.ToString()),
                    new Claim("Username", account.Username.ToString())
                };

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: expirationTime, signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public (string accessToken, string refreshToken) GenerateTokens(AccountsDTO account)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var accessClaims = new List<Claim>
                {
                    new Claim("Role", account.Role.ToString()),
                    new Claim("Username", account.Username.ToString())
                };

                var accessExpiration = DateTime.UtcNow.AddHours(1);
                var accessJwt = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], accessClaims, expires: accessExpiration, signingCredentials: credentials);
                var accessToken = new JwtSecurityTokenHandler().WriteToken(accessJwt);

                var accounts = _unitOfWork.AccountsRepository.Get(a => a.Username == account.Username).FirstOrDefault();

                var refreshClaims = new List<Claim>
                {
                    new Claim("AccountId", accounts.Id.ToString())
                };
                var refreshExpiration = DateTime.UtcNow.AddDays(14);
                var refreshJwt = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], refreshClaims, expires: refreshExpiration, signingCredentials: credentials);
                var refreshToken = new JwtSecurityTokenHandler().WriteToken(refreshJwt);

                // Store refresh token in the database
                // For simplicity, let's assume there's a method to store it

                var token = new Tokens
                {
                    AccountId = accounts.Id,
                    RefreshToken = refreshToken,
                    ExpiredDate = refreshExpiration
                };

                _unitOfWork.TokensRepository.Insert(token);
                _unitOfWork.Save();

                return (accessToken, refreshToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Accounts GetAccountById(int AccountId)
        {
            throw new NotImplementedException();
        }

        public Accounts? GetAccountByUsername(string username)
        {
            try
            {
                var account_ = _unitOfWork.AccountsRepository.Get(a => a.Username == username).FirstOrDefault();
                return account_;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public string? GetAccountRole(string username, Accounts account)
        {
            try
            {
                if (account.AccountRole == Accounts.Role.CONTRACTOR)
                {
                    return "CONTRACTOR";
                }
                else if (account.AccountRole == Accounts.Role.CUSTOMER)
                {
                    return "CUSTOMER";
                }
                else if (account.AccountRole == Accounts.Role.ADMIN)
                {
                    return "ADMIN";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Contractors? GetContractorByAccount(Accounts account)
        {
            try
            {
                var contractor = _unitOfWork.ContractorsRepository.Get(c => c.AccountId == account.Id).FirstOrDefault();
                return contractor;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ContractorsDTO? GetContractorInformation(string username, Accounts account, Contractors contractor)
        {
            try
            {
                ContractorsDTO contractorsView = _mapper.Map<ContractorsDTO>(contractor);
                string url = null;
                if (!String.IsNullOrEmpty(contractor.AvatarUrl))
                {
                    url = $"https://localhost:7233/img/contractorAvatar/{contractor.AvatarUrl}";
                }
                contractorsView.AvatarUrl = url;
                return contractorsView;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public (double? TotalRevenue, int TotalRequests, int TotalSignedRequests, int TotalOnGoingRequests, int TotalRejectedRequests, int TotalConstructs, int TotalProducts) GetContractorStats(int contractorId)
        {
            try
            {
                var contractorRequests = _unitOfWork.RequestsRepository.Get().Where(r => r.ContractorId == contractorId).ToList();
                var contractorSignedRequests = contractorRequests.Where(r => r.Status == Requests.RequestsStatusEnum.SIGNED).ToList();
                var contractorOnGoingRequests = contractorRequests.Where(r => r.Status == Requests.RequestsStatusEnum.PENDING || r.Status == Requests.RequestsStatusEnum.ACCEPTED || r.Status == Requests.RequestsStatusEnum.COMPLETED).ToList();
                var contractorRejectedRequests = contractorRequests.Where(r => r.Status == Requests.RequestsStatusEnum.REJECTED).ToList();

                var totalRevenue = contractorSignedRequests.Sum(r => r.TotalPrice);
                var totalRequests = contractorRequests.Count;
                var totalSignedRequests = contractorSignedRequests.Count;
                var totalOnGoingRequests = contractorOnGoingRequests.Count;
                var totalRejectedRequests = contractorRejectedRequests.Count;
                var totalConstructs = _unitOfWork.ConstructsRepository.Get().Count(c => c.ContractorId == contractorId);
                var totalProducts = _unitOfWork.ProductsRepository.Get().Count(p => p.ContractorId == contractorId);

                return (totalRevenue, totalRequests, totalSignedRequests, totalOnGoingRequests, totalRejectedRequests, totalConstructs, totalProducts);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Customers? GetCustomerByUsername(Accounts account)
        {
            try
            {
                var customer = _unitOfWork.CustomersRepository.Get(c => c.AccountId == account.Id).FirstOrDefault();
                return customer;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public CustomerDTO? GetCustomersInformation(string username, Accounts account, Customers customer)
        {
            try
            {
                CustomerDTO customersView = _mapper.Map<CustomerDTO>(customer);
                return customersView;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public (double? TotalRevenue, double? TotalDepositRevenue, int TotalCustomers, int TotalFilterdCustomer, int TotalContractors, int TotalRequests, int TotalSignedRequests, int TotalOnGoingRequests, int TotalRejectedRequests, int TotalConstructs, int TotalProducts) GetPlatformStats()
        {
            try
            {
                var requestList = _unitOfWork.RequestsRepository.Get().ToList();
                double? revenue = 0;
                double depositRevenue = 0;
                if (requestList != null)
                {
                    if (requestList.Any())
                    {
                        foreach (var request in requestList)
                        {
                            if (request.Status == Requests.RequestsStatusEnum.SIGNED)
                            {
                                revenue += request.TotalPrice;
                            }
                            //var depositOrder = _unitOfWork.DepositOrdersRepository.Find(d => d.RequestId == request.Id && d.Status == DepositOrders.DepositOrderStatusEnum.COMPLETED).FirstOrDefault();
                            //if (depositOrder != null)
                            //{
                            //    depositRevenue += depositOrder.DepositPrice;
                            //}
                        }
                    }
                }


                var customers = _unitOfWork.CustomersRepository.Get().Count();

                int requestedCustomer = 0;
                var customerList = _unitOfWork.CustomersRepository.Get().ToList();
                if (customerList != null && requestList != null)
                {
                    if (customerList.Any() && requestList.Any())
                    {
                        var uniqueCustomer = customerList.Select(c => c.Id).Except(requestList.Select(r => r.CustomerId)).Count();
                        requestedCustomer = customers - uniqueCustomer;
                    }
                }

                var contractors = _unitOfWork.ContractorsRepository.Get().Count();

                var requests = 0;
                var signedRequests = 0;
                var onGoingRequests = 0;
                var rejectedRequests = 0;
                if (requestList != null)
                {
                    if (requestList.Any())
                    {
                        requests = requestList.Count();
                        signedRequests = requestList.Count(r => r.Status == Requests.RequestsStatusEnum.SIGNED);
                        onGoingRequests = requestList.Count(r => (r.Status == Requests.RequestsStatusEnum.PENDING || r.Status == Requests.RequestsStatusEnum.ACCEPTED || r.Status == Requests.RequestsStatusEnum.COMPLETED));
                        rejectedRequests = requestList.Count(r => r.Status == Requests.RequestsStatusEnum.REJECTED);
                    }
                }

                var constructs = _unitOfWork.ConstructsRepository.Get().Count();

                var products = _unitOfWork.ProductsRepository.Get().Count();

                return (revenue, depositRevenue, customers, requestedCustomer, contractors, requests, signedRequests, onGoingRequests, rejectedRequests, constructs, products);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Tokens? GetRefreshTokenByAccountId(int AccountId)
        {
            throw new NotImplementedException();
        }

        public bool IsExistedEmail(string email)
        {
            try
            {
                bool status = true;

                var contractor = _unitOfWork.ContractorsRepository.Get(c => c.Email == email).FirstOrDefault();

                var customer = _unitOfWork.ContractorsRepository.Get(c => c.Email == email).FirstOrDefault();

                if (contractor == null && customer == null)
                {
                    status = false;
                }

                return status;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public (bool isValid, string username) ValidateRefreshToken(string refreshToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false // Disable lifetime validation as refresh tokens may be long-lived
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out validatedToken);

                var accountIdClaim = principal.FindFirst("AccountId");
                if (accountIdClaim == null)
                {
                    // If AccountId claim is missing, return false
                    return (false, null);
                }

                // Extract the AccountId as a string
                var accountId = accountIdClaim.Value;

                // Check if the refresh token exists in the database and is not expired
                var token = _unitOfWork.TokensRepository.Get(t => t.AccountId == int.Parse(accountId) && t.RefreshToken == refreshToken).FirstOrDefault();
                if (token == null || token.ExpiredDate < DateTime.UtcNow)
                {
                    // If token not found or expired, return false
                    return (false, null);
                }

                // If the token is valid, return true and the associated AccountId
                return (true, accountId);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return (false, null);
            }
        }
    }
}
