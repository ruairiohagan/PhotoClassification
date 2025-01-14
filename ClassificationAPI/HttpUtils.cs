using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Json;
using System.Reflection.Metadata;

namespace ClassificationAPI
{
    /// <summary>
    /// Helper class with performing http requests
    /// </summary>

    internal class HttpUtils
    {
        public static async Task<GetResponse> GetRequest(string url)
        {
            using (var client = new HttpClient())
            {
                HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, url);
                client.Timeout = TimeSpan.FromMilliseconds(30000);
                HttpResponseMessage response = await client.SendAsync(msg);
                string content = await response.Content.ReadAsStringAsync();
                return new GetResponse() { status = response.StatusCode, content = content ?? "" };
            }
        }

        public static async Task<PostResponse> PostRequest(string reference, string url, string body, int timeoutMS)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, url);
                    msg.Method = HttpMethod.Post;
                    ByteArrayContent bac = new ByteArrayContent(Encoding.UTF8.GetBytes(body));
                    bac.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                    msg.Content = bac;
                    client.Timeout = TimeSpan.FromMilliseconds(timeoutMS);
                    HttpResponseMessage response = await client.SendAsync(msg);
                    string content = await response.Content.ReadAsStringAsync();
                    return new PostResponse() { reference = reference, status = response.StatusCode, content = content ?? "" };
                }
                catch (Exception ex)
                {
                    return new PostResponse() { reference = reference, status = HttpStatusCode.InternalServerError, content = ex.Message };
                }
            }
        }

        public static string ConvertImageToBase64(string filePath)
        {
            byte[] imageBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(imageBytes);
        }

    }
    internal class GetResponse
    {
        public HttpStatusCode status { get; set; }
        public string? content { get; set; }
    }
    internal class PostResponse
    {
        public string? reference { get; set; }
        public HttpStatusCode status { get; set; }
        public string? content { get; set; }
    }
}
