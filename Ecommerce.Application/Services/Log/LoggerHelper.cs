using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EcommerceSolution.Logs;

public class LoggerHelper<T>
{
    private readonly ILogger<T> _logger;

    private List<string> _entityNames =
    [
        "User",
        "Role",
        "Product",
        "Manufacturer",
        "Invoice",
        "Category",
        "ProductInvoice"
    ];

    private List<string> _genericRepoNames =
    [
        "GenericRepo.UserRepository",
        "GenericRepo.RoleRepository",
        "GenericRepo.ProductRepository",
        "GenericRepo.ManufacturerRepository",
        "GenericRepo.InvoiceRepository",
        "GenericRepo.CategoryRepository",
        "GenericRepo.ProductInvoiceRepository"
    ];

    private List<string> _servicesName =
    [
        "UserService",
        "RoleService",
        "ProductService",
        "ManufacturerService",
        "InvoiceService",
        "CategoryService",
        "ProductInvoiceService"
    ];

    private List<string> _genericRepoFunctionNames =
    [
        "GetByIdAsync",
        "GetByUniquePropertyAsync",
        "GetAsync",
        "Update",
        "DeleteByIdAsync",
        "Delete",
        "InsertAsync"
    ];

    private List<string> _userServiceFunctionNames =
    [
        "GetUserByIdAsync",
        "GetUserByUsernameAsync",
        "GetUserByEmailAsync",
        "GetUserByPhoneNumberAsync",
        "AddUserAsync",
        "UpdateUserAsync",
        "DeleteUserAsync",
        "GetAllUsersAsync",
        "GetAllUsersByNameAsync",
        "GetUserByRoleAsync",
        "AuthenticateUserAsync"
    ];

    public LoggerHelper(ILogger<T> logger)
    {
        _logger = logger;
    }

    //Function types :Service = 0, GenericRepository = 1, Controller = 2
    //RepoName : UserRepository, RoleRepository, InvoiceRepository, ProductRepository, CategoryRepository, ProductInvoiceRepository, ManufacturerRepository

    public void Log(int functionIndex, string entityName, int informationType,
        string input = "", string output = "", string message = "", int repoFunctionIndex = 0, string repoEntity = "")
    {
        int index  =  _entityNames.IndexOf(entityName);
        int repoIndex = !string.IsNullOrEmpty(repoEntity) ? _entityNames.IndexOf(repoEntity) : -1;
        var serviceName = _servicesName[index];
        var serviceFunctionName = _userServiceFunctionNames[functionIndex];
        var repositoryName = repoIndex!=-1 ? _genericRepoNames[repoIndex] : "";
        var repoFunctionName = _genericRepoFunctionNames[repoFunctionIndex];
        switch (informationType)
        {
            //Service Function Log
            case 0:
                _logger.LogInformation("{ServiceName} : {FunctionName} :  Inputs :{Inputs}", serviceName, serviceFunctionName,
                    input);
                break;
            case 1:
                _logger.LogInformation("{ServiceName} : {FunctionName} :  Outputs :{Outputs}", serviceName, serviceFunctionName,
                    output);
                break;
            case 2:
                _logger.LogInformation("{ServiceName} : {FunctionName} :  {Message}", serviceName, serviceFunctionName,
                    message);
                break;
            case 3:
                _logger.LogError("{ServiceName} : {FunctionName} :  {Message}", serviceName, serviceFunctionName,
                    message);
                break;
            //Repository Function Log 
            case 4:
                _logger.LogInformation("{ServiceName} : {FunctionName} : {RepoFunctionName} Inputs :{Inputs}",
                    serviceName, serviceFunctionName, $"{repositoryName}." + repoFunctionName,
                    input);
                break;
            case 5:
                _logger.LogInformation("{ServiceName} : {FunctionName} : {RepoFunctionName} Outputs :{Outputs}",
                    serviceName, serviceFunctionName, $"{repositoryName}." + repoFunctionName,
                    output);
                break;
            case 6:
                _logger.LogInformation("{ServiceName} : {FunctionName} : {RepoFunctionName} Inputs :{Inputs}",
                    serviceName, serviceFunctionName, $"{repositoryName}." + repoFunctionName,
                    input);
                break;
            case 7:
                _logger.LogInformation("{ServiceName} : {FunctionName} : {RepoFunctionName} :{Message}",
                    serviceName, serviceFunctionName, $"{repositoryName}." + repoFunctionName,
                    message);
                break;
            default:
                _logger.LogError("{LoggerHelper} : Invalid Log function call!", "LoggerHelper");
                break;
        }
    }
}