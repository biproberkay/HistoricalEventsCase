using HistoricalEventsCase.Data;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace HistoricalEventsCase.Services
{
    public interface IUserService
    {
        bool IsValidUser(string userName, string password);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly ApplicationDbContext _context;
        // inject database for user validation
        public UserService(ILogger<UserService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public bool IsValidUser(string userName, string password)
        {
            if(!_context.AppUsers.Any())
            {
                var seed = new Models.AppUser { UserName = "admin", Password = "12345" };
                _context.AppUsers.Add(seed);
                _context.SaveChanges();
            }
            _logger.LogInformation($"Validating user [{userName}]");
            if (!_context.AppUsers.Any(u=>u.UserName==userName && u.Password==password))
            {
                return false;
            }

            return true;
        }
    }
}
