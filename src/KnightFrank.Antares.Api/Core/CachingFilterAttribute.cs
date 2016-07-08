namespace KnightFrank.Antares.Api.Core
{
	using System;
	using System.Net.Http.Headers;
	using System.Web.Http.Filters;

	/// <summary>
	/// CachingFilterAttribute
	/// </summary>
	public class CachingFilterAttribute : ActionFilterAttribute
    {
		/// <summary>
		/// OnActionExecuted
		/// </summary>
		/// <param name="context"></param>
		public override void OnActionExecuted(HttpActionExecutedContext context)
		{
			if (context.Response != null)
			{
				context.Response.Headers.CacheControl = new CacheControlHeaderValue()
				{
					NoCache = true
				};
			}

			base.OnActionExecuted(context);
		}
	}
}