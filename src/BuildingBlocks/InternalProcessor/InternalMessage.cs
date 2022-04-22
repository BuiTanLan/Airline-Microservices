namespace BuildingBlocks.InternalProcessor;

public class InternalMessage
{
    public Guid EventId { get; private set; }

    /// <summary>
    /// Gets name of message.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the date the message occurred.
    /// </summary>
    public DateTime OccurredOn { get; private set; }

    /// <summary>
    /// Gets the command type full name.
    /// </summary>
    public string CommandType { get; private set; }

    /// <summary>
    /// Gets the command data - serialized to JSON.
    /// </summary>
    public string Data { get; private set; }

    /// <summary>
    /// Gets the date the message processed.
    /// </summary>
    public DateTime? ProcessedOn { get; private set; }


    /// <summary>
    /// Gets the CorrelationId of our command.
    /// </summary>
    public Guid? CorrelationId { get; private set; }


    public InternalMessage(
        Guid eventId,
        DateTime occurredOn,
        string commandType,
        string name,
        string data,
        Guid? correlationId = null)
    {
        OccurredOn = occurredOn;
        CommandType = commandType;
        Data = data;
        EventId = eventId;
        Name = name;
        CorrelationId = correlationId;
    }

    public void MarkAsProcessed()
    {
        ProcessedOn = DateTime.Now;
    }

    public bool Validate()
    {
        if (EventId == Guid.Empty)
        {
            throw new System.ComponentModel.DataAnnotations.ValidationException(
                "Id of the InternalMessage entity couldn't be null.");
        }

        if (string.IsNullOrEmpty(CommandType))
        {
            throw new System.ComponentModel.DataAnnotations.ValidationException(
                "Type of the InternalMessage entity couldn't be null or empty.");
        }

        if (Data is null)
        {
            throw new System.ComponentModel.DataAnnotations.ValidationException(
                "Payload of the InternalMessage entity couldn't be null (should be an Avro format).");
        }

        return true;
    }
}
