namespace Biblio.Infrastructures.Core
{
    public static class EventStoreCore
    {
        public const string EventClrTypeHeader = "BiblioEventName";             // EventClrTypeName
        public const string AggregateClrTypeHeader = "BiblioAggregateName";     // AggregateClrTypeName
        public const string CommitIdHeader = "CommitId";
        public const int WritePageSize = 500;
        public const int ReadPageSize = 500;
    }
}
