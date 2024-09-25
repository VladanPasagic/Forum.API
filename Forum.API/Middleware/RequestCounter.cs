namespace Forum.API.Middleware;

public class RequestCounter
{
    public int Count { get; private set; }
    private DateTime _firstRequestTime;

    public bool Increment(TimeSpan timeWindow, int maxRequests)
    {
        var now = DateTime.UtcNow;

        if (_firstRequestTime == DateTime.MinValue || now - _firstRequestTime > timeWindow)
        {
            _firstRequestTime = now;
            Count = 1;
            return true;
        }

        if (Count < maxRequests)
        {
            Count++;
            return true;
        }

        return false;
    }
}
