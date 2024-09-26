using AspNetCoreHero.ToastNotification.Abstractions;

namespace CompressMedia.Middlewares
{
	public class Mp4FileValidationMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<Mp4FileValidationMiddleware> _logger;
		private readonly IServiceScopeFactory _scopeFactory;

		public Mp4FileValidationMiddleware(RequestDelegate requestDelegate, ILogger<Mp4FileValidationMiddleware> logger, IServiceScopeFactory scopeFactory)
		{
			_next = requestDelegate;
			_logger = logger;
			_scopeFactory = scopeFactory;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.HasFormContentType && context.Request.Form.Files.Count > 0)
			{
				foreach (var item in context.Request.Form.Files)
				{
					string fileExtension = Path.GetExtension(item.FileName).ToLowerInvariant();
					if (fileExtension != ".mp4")
					{
						Console.OutputEncoding = System.Text.Encoding.UTF8;
						_logger.LogWarning("Invalid file format, only .mp4 files are allowed");

						using (var scope = _scopeFactory.CreateScope())
						{
							var notifyService = scope.ServiceProvider.GetRequiredService<INotyfService>();
							notifyService.Error("Invalid file format, only .mp4 files are allowed");
						}

						context.Response.StatusCode = StatusCodes.Status400BadRequest;
						context.Response.ContentType = "text/html";

						var errorMessageHtml = "<html><body><h3>Invalid file format, only .mp4 files are allowed</p></body></html>";
						await context.Response.WriteAsync(errorMessageHtml);
						return;
					}
				}
			}

			await _next(context);
		}
	}
}
