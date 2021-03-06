using MediatR;
using StructureMap;
using Bodybuildr.Domain;
using Bodybuildr.Domain.Workouts;
using BodyBuildr.EventStore;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;
using Bodybuildr.Domain.CommandHandlers;
using StrongR.ReadStack.EventHandlers;
using Strongr.Web.Commands.CopyWorkout;
using Strongr.Web.Authentication;

namespace Strongr.Web
{
    public class StrongrRegistry : Registry
    {
        public StrongrRegistry()
        {
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => ctx.GetInstance);
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => ctx.GetAllInstances);
            //For(typeof(IPipelineBehavior<,>)).Add(typeof(LoggingBehavior<,>));
            //For(typeof(IPipelineBehavior<,>)).Add(typeof(InfluxCircuitBreakerBehavior<,>)).Singleton();

            Scan(scan =>
            {
                scan.AssemblyContainingType<WorkoutTableEventHandler>();
                scan.AssemblyContainingType<WorkoutCommandHandlers>();
                scan.AssemblyContainingType<CopyWorkoutRequestHandler>();
                scan.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>))
                    .OnAddedPluginTypes(t=>t.Singleton());
                scan.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<>))
                    .OnAddedPluginTypes(t => t.Singleton());
                scan.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>))
                    .OnAddedPluginTypes(t => t.Singleton());

                scan.AssemblyContainingType<Repository<Workout>>();
                scan.ConnectImplementationsToTypesClosing(typeof(IRepository<>));
            });

            For<IRepository<Workout>>().Use(x => new Repository<Workout>(x.GetInstance<EventStore>())).Singleton();
            For<EventStore>().Use(x => CreateEventStore(x)).Singleton();
            For<IEventStore>().Use<EventStore>().Singleton();
            For<CloudTableClient>().Use(x => CreateCloudTableClient(x)).Singleton();
            For<IMediator>().Use<Mediator>();
#if DEBUG
            For<IUserManager>().Use<DefaultingUserManager>().Singleton();
#else
            For<IUserManager>().Use<PassthroughUsermanager>().Singleton();
#endif
        }

        private EventStore CreateEventStore(IContext x)
        {
            var table = x.GetInstance<CloudTableClient>().GetTableReference("Streams");
            table.CreateIfNotExistsAsync().Wait();
            return new EventStore(table, x.GetInstance<IMediator>());
        }

        private static CloudTableClient CreateCloudTableClient(IContext x)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(
                            x.GetInstance<IConfiguration>().GetConnectionString("EventStore"));
            var client = account.CreateCloudTableClient();
            return client;
        }
    }
}
