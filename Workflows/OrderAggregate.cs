using AsyncHandler.EventSourcing;
using AsyncHandler.EventSourcing.Events;
using AsyncHandler.EventSourcing.Extensions;

public class OrderAggregate(long id) : AggregateRoot(id)
{
    protected override void Apply(SourceEvent e)
    {
        this.InvokeApply(e);
    }
    public void Apply(OrderPlaced e)
    {

    }
    public void PlaceOrder(OrderPlaced e)
    {
        RaiseEvent(e);
    }
}
public record OrderPlaced : SourceEvent;