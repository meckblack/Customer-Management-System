using CustomerManagementSystem.Data;
using CustomerManagementSystem.Models.Entities;
using CustomerManagementSystem.Services.Abstract;

namespace CustomerManagementSystem.Services.Concrete
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}