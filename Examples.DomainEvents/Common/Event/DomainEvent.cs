using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Common.Event
{
    public abstract class DomainEvent:INotification
    {
        protected DomainEvent()
        {
            OccuredOn = DateTime.UtcNow;
        }

        public DateTime OccuredOn { get; private set; }
    }
}
