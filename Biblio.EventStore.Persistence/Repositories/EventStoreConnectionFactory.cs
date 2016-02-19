using System.Net;
using Biblio.Configuration;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace Biblio.EventStore.Persistence.Repositories
{
    public class EventStoreConnectionFactory
    {
        private static IEventStoreConnection _eventStoreConnection;

        public static IEventStoreConnection EventStore => _eventStoreConnection ??
                                                          (_eventStoreConnection = CreateEventStoreConnection());

        private static IEventStoreConnection CreateEventStoreConnection()
        {
            var tcpEndPoint =
                new IPEndPoint(IPAddress.Parse(BiblioConfiguration.EventStoreSection.Uri),
                    BiblioConfiguration.EventStoreSection.Port);

            var connectionSettings = ConnectionSettings.Create();
            connectionSettings.SetDefaultUserCredentials(
                new UserCredentials(BiblioConfiguration.EventStoreSection.User,
                    BiblioConfiguration.EventStoreSection.Password));

            return EventStoreConnection.Create(connectionSettings, tcpEndPoint);
        }
    }
}
