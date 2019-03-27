using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FamilySys.Services {
	internal class TimedTask: IHostedService, IDisposable {
		private readonly ILogger logger;
		private Timer timer;

		public IServiceProvider Services { get; }

		public TimedTask(ILogger<TimedTask> _logger, IServiceProvider _services) {
			logger = _logger;
			Services = _services;
		}

		public Task StartAsync(CancellationToken cancellation) {
			logger.LogInformation("\n-----------------------------------------------------\n");
			logger.LogInformation("Starting Timed Background Task\n");

			timer = new Timer(DoWork, null, TimeSpan.Zero,TimeSpan.FromDays(1));

			return Task.CompletedTask;
		}

		public void DoWork(object state) {
			if (DateTime.Now.Day != 1) {
				logger.LogInformation("-----------------------------------------------------\n");
				logger.LogInformation("Today is not the first day of this Month\n");
				return;
			}

			using (var MonthlyRankTask = Services.CreateScope()) {
				var NewTask = MonthlyRankTask.ServiceProvider.GetRequiredService<GenerateMonthlyRank.IMonthlyRank>();

				NewTask.Generate();
			}

			logger.LogInformation("\n-----------------------------------------------------\n");
			logger.LogInformation("Monthly Rank Generated!\n");
		}

		public Task StopAsync(CancellationToken cancellation) {
			logger.LogInformation("\n-----------------------------------------------------\n");
			logger.LogInformation("Stopping Timed Background Task\n");

			timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose() {
			timer?.Dispose();
		}
	}
}
