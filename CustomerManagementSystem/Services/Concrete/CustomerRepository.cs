using CustomerManagementSystem.Data;
using CustomerManagementSystem.Models.Entities;
using CustomerManagementSystem.Services.Abstract;

namespace CustomerManagementSystem.Services.Concrete
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}