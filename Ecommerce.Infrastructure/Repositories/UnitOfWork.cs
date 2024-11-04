using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;
using Ecommerce.Core.Log;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.Repositories;

public class UnitOfWork : IDisposable
{
    private EcommerceDbContext _context;
    public readonly GenericRepository<Category> categoryRepository;
    public readonly GenericRepository<Role> roleRepository;
    public readonly GenericRepository<Invoice> invoiceRepository;
    public readonly GenericRepository<Manufacturer> manufacturerRepository;
    public readonly GenericRepository<Product> productRepository;
    public readonly GenericRepository<ProductInvoice> productInvoiceRepository;
    public readonly GenericRepository<User> userRepository;
    private readonly ILogger<UnitOfWork> _logger;

    
    public UnitOfWork(EcommerceDbContext context,ILogger<UnitOfWork>logger
        ,GenericRepository<Category> categoryRepository,
        GenericRepository<Role> roleRepository,
        GenericRepository<Invoice> invoiceRepository,
        GenericRepository<Manufacturer> manufacturerRepository,
        GenericRepository<Product> productRepository,
        GenericRepository<ProductInvoice> productInvoiceRepository,
        GenericRepository<User>userRepository)
    {
        _context = context;
        _logger = logger;
        this.categoryRepository = categoryRepository;
        this.roleRepository = roleRepository;
        this.invoiceRepository = invoiceRepository;
        this.manufacturerRepository = manufacturerRepository;
        this.productRepository = productRepository;
        this.productInvoiceRepository = productInvoiceRepository;
        this.userRepository = userRepository;
    }
    

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
        LoggerHelper.LogWithDetails(_logger,"Saved Successfully.");
    }

    
    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        disposed = true;
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}