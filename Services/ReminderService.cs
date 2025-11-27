
using Microsoft.AspNetCore.SignalR; 
using StudentTaskManager.Hubs;      
using Microsoft.EntityFrameworkCore;
using StudentTaskManager.Data;

namespace StudentTaskManager.Services
{
    
    public class ReminderService : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        private readonly ILogger<ReminderService> _logger;
       
        private readonly IServiceScopeFactory _scopeFactory;


        private readonly IHubContext<ReminderHub> _hubContext;  

        public ReminderService(ILogger<ReminderService> logger, IServiceScopeFactory scopeFactory, IHubContext<ReminderHub> hubContext)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reminder Service started.");

         
            _timer = new Timer(CheckForReminders, null, TimeSpan.Zero, TimeSpan.FromSeconds(120));

            return Task.CompletedTask;
        }


      

        private async void CheckForReminders(object? state)
        {
            
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var now = DateTime.Now;
                var reminderTime = now.AddMinutes(30); 

                
                var upcomingTasks = await context.Tasks
                    .Where(t => t.Status == "Pending" && t.DueDate >= now && t.DueDate <= reminderTime)
                    .Include(t => t.Category)
                    .ToListAsync();

                
                if (upcomingTasks.Any())
                {
                    _logger.LogWarning($"--- FOUND {upcomingTasks.Count} UPCOMING REMINDERS! ---");
                    foreach (var task in upcomingTasks)
                    {
                        string message = $"Task '{task.Title}' in category '{task.Category?.Name}' is due in 30 minutes!";

                     
                        await _hubContext.Clients.All.SendAsync("ReceiveReminder", message);

                        _logger.LogWarning($"REMINDER SENT: {message}");
                    }
                    _logger.LogWarning("---------------------------------------------------");
                }
            } 
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reminder Service stopped.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}