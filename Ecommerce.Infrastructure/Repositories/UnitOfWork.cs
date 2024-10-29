using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Persistence;

namespace Ecommerce.Infrastructure.Repositories;

public class UnitOfWork : IDisposable
{
    private EcommerceDbContext _context;
    private GenericRepository<Category> categoryRepository;
    private GenericRepository<Role> roleRepository;
    private GenericRepository<Invoice> invoiceRepository;
    private GenericRepository<Manufacturer> manufacturerRepository;

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

    public GenericRepository<Manufacturer> ManufacturerRepository {
        get{
            if (manufacturerRepository == null)
            {
                manufacturerRepository = new GenericRepository<Manufacturer>(_context);
            }

            return manufacturerRepository;
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