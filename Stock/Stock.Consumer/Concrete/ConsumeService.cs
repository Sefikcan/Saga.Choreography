using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Consumer.Concrete
{
    public class ConsumeService : BackgroundService
    {
        private readonly AutoSubscriber _autoSubscriber;

        public ConsumeService(AutoSubscriber autoSubscriber)
        {
            _autoSubscriber = autoSubscriber;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _autoSubscriber.SubscribeAsync(new Assembly[] { Assembly.GetExecutingAssembly() }, stoppingToken);
        }
    }
}
