namespace EventBus.Messages.Events
{
    public class AccountCreationEvent : IntegrationBaseEvent
    {
        public Guid AccountId { get; set; }
        public Guid CartId { get; set; } 
        
    }
}
