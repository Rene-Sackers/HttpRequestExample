using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Neo.IronLua;

namespace FiveRebornHttp
{
    public class HttpHandler
    {
        public static void MakeRequest(string url, Func<object, object, object, LuaResult> callback, string method = null, string data = null, LuaTable headers = null)
        {
            using (var httpClient = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage(StringToHttpMethod(method), url);

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        var key = header.Key.ToString();
                        var value = header.Value.ToString();

                        if (key.ToLowerInvariant() == "content-type")
                        {
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(value));
                            continue;
                        }

                        httpRequestMessage.Headers.Add(header.Key.ToString(), header.Value.ToString());
                    }
                }

                httpRequestMessage.Content = new StringContent(data);

                var response = httpClient.SendAsync(httpRequestMessage).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                var statusCode = (int)response.StatusCode;

                var responseHeaders = new LuaTable();
                foreach (var header in response.Headers)
                {
                    var values = new LuaTable();
                    foreach (var value in header.Value)
                    {
                        values.ArrayList.Add(value);
                    }

                    responseHeaders[header.Key] = values;
                }

                callback(content, statusCode, responseHeaders);
            }
        }

        private static HttpMethod StringToHttpMethod(string method)
        {
            switch (method.ToLowerInvariant())
            {
                case "delete":
                    return HttpMethod.Delete;
                case "head":
                    return HttpMethod.Head;
                case "options":
                    return HttpMethod.Options;
                case "post":
                    return HttpMethod.Post;
                case "put":
                    return HttpMethod.Put;
                case "trace":
                    return HttpMethod.Trace;
            }

            return HttpMethod.Get;
        }
    }
}
