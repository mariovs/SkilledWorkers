using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.SkilledWorkers.HttpAggregator.Infrastructure
{
	public class ServicesExceptionFilter : IActionFilter, IOrderedFilter
	{
		public int Order { get; set; } = int.MaxValue - 10;

		public void OnActionExecuted(ActionExecutedContext context)
		{
			if (context.Exception is HttpExceptionWithStatusCode exception)
			{
				context.Result = new StatusCodeResult((int)exception.StatusCode);
				context.ExceptionHandled = true;
			}
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
		}
	}
}
