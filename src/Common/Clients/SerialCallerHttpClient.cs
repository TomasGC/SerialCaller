using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace SerialCaller.Libs.Common.Clients;

/// <summary>
/// Override of the HttpClient class to add common behavior for the different calls.
/// </summary>
/// <param name="httpClient">HttpClient from Microsoft.</param>
public class SerialCallerHttpClient(HttpClient httpClient)
{
    public async Task<TResponse> GetAsync<TResponse>(string? requestUri)
        where TResponse : class, IResponse, new()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
        return await SendAsync<TResponse>(httpRequestMessage);
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string? requestUri, TRequest request)
        where TRequest : class, IRequest, new()
        where TResponse : class, IResponse, new()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = JsonContent.Create(request, null, Constants.SERIALIZER_OPTIONS)
        };
        return await SendAsync<TResponse>(httpRequestMessage);
    }

    public async Task<TResponse> PutAsync<TRequest, TResponse>(string? requestUri, TRequest request)
        where TRequest : class, IRequest, new()
        where TResponse : class, IResponse, new()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri)
        {
            Content = JsonContent.Create(request, null, Constants.SERIALIZER_OPTIONS)
        };
        return await SendAsync<TResponse>(httpRequestMessage);
    }

    public async Task<TResponse> DeleteAsync<TResponse>(string? requestUri)
        where TResponse : class, IResponse, new()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);
        return await SendAsync<TResponse>(httpRequestMessage);
    }

    public async Task<TResponse> SendAsync<TResponse>(HttpRequestMessage request)
        where TResponse : class, IResponse, new()
    {
        HttpResponseMessage? httpResponseMessage;
        TResponse response = new();
        try
        {
            httpResponseMessage = await httpClient.SendAsync(request);
        }
        catch (Exception ex)
        {
            return response.Error($"{HttpStatusCode.NotFound}", $"Could not reach API {httpClient.BaseAddress}: [{ex.Message}].");
        }

        if (httpResponseMessage == null)
        {
            return response.Error($"{HttpStatusCode.NotFound}", $"Could not reach API {httpClient.BaseAddress}.");
        }

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            return response.Error($"{httpResponseMessage.StatusCode}", httpResponseMessage.ReasonPhrase);
        }

        var stream = await httpResponseMessage.Content.ReadAsStreamAsync();
        if (stream == null)
        {
            return response.Error($"{HttpStatusCode.NoContent}", $"Object not found for route {request.RequestUri!.AbsoluteUri}");
        }

        return (await JsonSerializer.DeserializeAsync<TResponse>(stream, Constants.SERIALIZER_OPTIONS))!;
    }
}
