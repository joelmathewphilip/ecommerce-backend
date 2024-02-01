using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {
        public IntegrationBaseEvent()
        {
            guid = new Guid();
            eventDatetime = DateTime.UtcNow;
        }
        public IntegrationBaseEvent(Guid guid, DateTime dateTime)
        {
            this.guid = guid;
            this.eventDatetime = dateTime;
        }
        public Guid guid { get; set; }
        public DateTime eventDatetime { get; set; }
    }
}
