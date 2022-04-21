namespace BuildingBlocks.InternalProcessor;

public class InternalProcessorOptions
{
    public bool Enabled { get; set; } = true;
    public TimeSpan? Interval { get; set; }
    public bool UseBackgroundDispatcher { get; set; } = true;
}
