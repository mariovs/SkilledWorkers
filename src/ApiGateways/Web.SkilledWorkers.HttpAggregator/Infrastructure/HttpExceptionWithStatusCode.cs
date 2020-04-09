using System;
using System.Net;
using System.Net.Http;

namespace Web.SkilledWorkers.HttpAggregator.Infrastructure
{
	public class HttpExceptionWithStatusCode : Exception
	{
		public HttpExceptionWithStatusCode(HttpRequestException ex, HttpStatusCode statusCode)
		{
			HttpRequestException = ex;
			StatusCode = statusCode;
		}

		public HttpRequestException HttpRequestException { get; set; }
		public HttpStatusCode StatusCode { get; set; }
	}
}
