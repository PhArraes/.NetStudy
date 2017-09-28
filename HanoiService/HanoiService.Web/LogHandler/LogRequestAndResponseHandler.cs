using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HanoiService.Web.Log
{
    //font:https://stackoverflow.com/questions/23660340/need-to-log-asp-net-webapi-2-request-and-response-body-to-a-database
    public class LogRequestAndResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // log request body
            string requestBody = await request.Content.ReadAsStringAsync();

            Trace.WriteLine("Request ["+DateTime.Now+"] => URI: " + request.RequestUri + " RequestBody: " + requestBody);

            // let other handlers process the request
            var result = await base.SendAsync(request, cancellationToken);


            Trace.Write("Response [" + DateTime.Now + "] => StatusCode: " + result.StatusCode);


            if (result.Content != null)
            {
                // once response body is ready, log it
                var responseBody = await result.Content.ReadAsStringAsync();
                Trace.WriteLine(" ResponseBody: " + responseBody);
            }
            else { Trace.WriteLine(""); }

            Trace.Flush();
            return result;
        }
    }
}