namespace SerialCaller.Libs.Common.Clients;

/// <summary>
/// Extends IResponse objects.
/// </summary>
public static class IResponseExtensions
{
    /// <summary>
    /// Set the error in the response object.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="response"></param>
    /// <param name="errorCode"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public static TResponse Error<TResponse>(this TResponse response, string errorCode, string? errorMessage)
        where TResponse : class, IResponse, new()
    {
        response.ErrorCode = errorCode;
        response.ErrorMessage = errorMessage;

        return response;
    }
}