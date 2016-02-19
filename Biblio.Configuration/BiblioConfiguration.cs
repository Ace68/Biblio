using System.Configuration;

namespace Biblio.Configuration
{
    public static class BiblioConfiguration
    {
        private const string SuiteCmGroupName = "Biblio/";

        private static EventStoreSectionHandler _eventStoreSection;
        public static EventStoreSectionHandler EventStoreSection => _eventStoreSection ??
                                                                    (_eventStoreSection =
                                                                        ConfigurationManager.GetSection(SuiteCmGroupName + "EventStore") as
                                                                            EventStoreSectionHandler);
    }
}
