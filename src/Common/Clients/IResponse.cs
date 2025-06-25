namespace SerialCaller.Libs.Common.Clients;

/// <summary>
/// Response interface with the default properties.
/// </summary>
public interface IResponse
{
    /// <summary>
    /// The error code we get from the http call.
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// The error message we get from the http call.
    /// </summary>
    public string? ErrorMessage { get; set; }

    // Is the http call a success?
    public bool Success => ErrorCode == null;
}