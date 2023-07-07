using CliverSystem.Models;
using Hangfire;

namespace CliverSystem.Services
{
    public class ScheduleTask
    {
        private DataContext _context{ get; set; }
        public ScheduleTask(DataContext context)
        {
            _context = context;
        }

        public async Task SendEmail()
        {
        }
    }
}
