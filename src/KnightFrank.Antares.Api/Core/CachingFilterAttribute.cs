namespace KnightFrank.Antares.Api.Core
{
    using System.Web.Http.Filters;

	/// <summary>
	/// CachingFilterAttribute
	/// </summary>
	public class CachingFilterAttribute : ExceptionFilterAttribute
    {
		/// <summary>
		/// OnResultExecuted
		/// </summary>
		/// <param name="context"></param>
		public void OnResultExecuted(HttpActionExecutedContext context)
		{
			context.Response.Headers.Add("Cache-Control", "no-cache");
		}
	}
}