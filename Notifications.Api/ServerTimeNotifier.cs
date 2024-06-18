using Microsoft.AspNetCore.SignalR;
using Notifications.Api.Models;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Notifications.Api;

public class ServerTimeNotifier : BackgroundService
{
    private static readonly TimeSpan Period = TimeSpan.FromSeconds(1);
    private readonly ILogger<ServerTimeNotifier> _logger;
    private readonly IHubContext<NotificationsHub, INotificationClient> _context;
    private readonly ConcurrentBag<Employee> _employees = new();
    private int _nextId = 0;
    public ServerTimeNotifier(ILogger<ServerTimeNotifier> logger, IHubContext<NotificationsHub, INotificationClient> context)
    {
        _logger = logger;
        _context = context;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Period);

        while (!stoppingToken.IsCancellationRequested &&
               await timer.WaitForNextTickAsync(stoppingToken))
        {
            var dateTime = DateTime.Now;

            _logger.LogInformation("Executing {Service} {Time}", nameof(ServerTimeNotifier), dateTime);
            var newEmployee = new Employee
            {
                Id = _nextId++,
                Name = $"Employee {_nextId}",
                Designation = "New Hire",
                DOJ = DateOnly.FromDateTime(DateTime.Now),
                IsActive = true
            };
            await _context.Clients
                .User("f45fe475-8466-484f-af69-a2658a8ee915")
                .ReceiveNotification(JsonSerializer.Serialize(newEmployee));
        }
    }
}
