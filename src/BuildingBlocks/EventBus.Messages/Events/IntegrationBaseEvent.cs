namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {
        public IntegrationBaseEvent()
        {
            guid = Guid.NewGuid();
            eventDatetime = DateTime.UtcNow;
        }
        public IntegrationBaseEvent(Guid guid, DateTime dateTime)
        {
            this.guid = guid;
            this.eventDatetime = dateTime;
        }
        public Guid guid { get; private set; }
        public DateTime eventDatetime { get; private set; }
    }
}
