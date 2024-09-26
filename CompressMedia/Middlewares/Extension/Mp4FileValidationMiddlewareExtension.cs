namespace CompressMedia.Middlewares.Extension
{
	public static class Mp4FileValidationMiddlewareExtension
	{
		public static IApplicationBuilder UseMp4FileValidationMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<Mp4FileValidationMiddleware>();
		}
	}
}
