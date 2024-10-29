using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;
using Ecommerce.Infrastructure.Persistence;

namespace Ecommerce.Infrastructure.Repositories;

public class UnitOfWork : IDisposable
{
    private EcommerceDbContext _context;
    private GenericRepository<Category> categoryRepository;
    private GenericRepository<Role> roleRepository;
    private GenericRepository<Invoice> invoiceRepository;
    private GenericRepository<Manufacturer> manufacturerRepository;
    private GenericRepository<Product> productRepository;
    private GenericRepository<ProductInvoice> productInvoiceRepository;
    private GenericRepository<User> userRepository;

    public UnitOfWork(EcommerceDbContext context)
    {
        _context = context;
    }

    public GenericRepository<Category> CategoryRepository
    {
        get
        {
            if (categoryRepository == null)
            {
                categoryRepository = new GenericRepository<Category>(_context);
            }

            return categoryRepository;
        }
    }

    public GenericRepository<Role> RoleRepository
    {
        get
        {
            if (roleRepository == null)
            {
                roleRepository = new GenericRepository<Role>(_context);
            }

            return roleRepository;
        }
    }

    public GenericRepository<Invoice> InvoiceRepository
    {
        get
        {
            if (invoiceRepository == null)
            {
                invoiceRepository = new GenericRepository<Invoice>(_context);
            }

            return invoiceRepository;
        }
    }

    public GenericRepository<Manufacturer> ManufacturerRepository
    {
        get
        {
            if (manufacturerRepository == null)
            {
                manufacturerRepository = new GenericRepository<Manufacturer>(_context);
            }

            return manufacturerRepository;
        }
    }

    public GenericRepository<Product> ProductRepository
    {
        get
        {
            if (productRepository == null)
            {
                productRepository = new GenericRepository<Product>(_context);
            }

            return productRepository;
        }
    }

    public GenericRepository<ProductInvoice> ProductInvoiceRepository
    {
        get
        {
            if (productInvoiceRepository == null)
            {
                productInvoiceRepository = new GenericRepository<ProductInvoice>(_context);
            }

            return productInvoiceRepository;
        }
    }

    public GenericRepository<User> UserRepository
    {
        get
        {
            if (userRepository == null)
            {
                userRepository = new GenericRepository<User>(_context);
            }

            return userRepository;
        }
    }


    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
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