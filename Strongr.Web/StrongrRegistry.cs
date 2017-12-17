using MediatR;
using StructureMap;
using Bodybuildr.Domain.Commands;
using Bodybuildr.CommandStack.CommandHandlers;
using Bodybuildr.Domain;
using Bodybuildr.Domain.Workouts;
using BodyBuildr.EventStore;
using System;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.Extensions.Configuration;

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
                scan.AssemblyContainingType<WorkoutCommandHandlers>();
                scan.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                scan.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<>));

                scan.AssemblyContainingType<Repository<Workout>>();
                scan.ConnectImplementationsToTypesClosing(typeof(IRepository<>));
            });

            For<IRepository<Workout>>().Use(x => new Repository<Workout>(x.GetInstance<EventStore>())).Singleton();
            For<EventStore>().Use(x => CreateEventStore(x)).Singleton();
            For<IEventStore>().Use<EventStore>().Singleton();

            For<IMediator>().Use<Mediator>();

        }

        private EventStore CreateEventStore(IContext x)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(
                x.GetInstance<IConfiguration>().GetConnectionString("EventStore"));
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("Streams");
            table.CreateIfNotExistsAsync().Wait();
            return new EventStore(table, x.GetInstance<IMediator>());
        }
    }
}
