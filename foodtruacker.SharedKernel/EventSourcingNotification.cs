using MediatR;

namespace foodtruacker.SharedKernel
{
    public abstract class EventSourcingNotification : INotification
    {
        public bool IsReplayingEvent { get; set; }
        public bool ShouldSerializeIsReplayingEvent() => false;
    }
}
