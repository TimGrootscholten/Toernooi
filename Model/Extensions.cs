using System.Net;
using Microsoft.AspNetCore.Http;

namespace Models;

public static class Extensions
{
    public static void SetHttpStatusCode(this HttpResponse response, HttpStatusCode httpStatusCode)
    {
        response.OnStarting(() =>
        {
            response.StatusCode = (int) httpStatusCode;
            return Task.CompletedTask;
        });
    }

    public static string ToCommaSeparateString(this List<int> list)
    {
        return list.Count > 0 ? string.Join(",", list.ConvertAll(x => x.ToString())) : string.Empty;
    }

    public static List<int> CommaSeparateStringToList(this string? str)
    {
        return !string.IsNullOrEmpty(str) ? str.Split(",").Select(int.Parse).ToList() : new List<int>();
    }
}