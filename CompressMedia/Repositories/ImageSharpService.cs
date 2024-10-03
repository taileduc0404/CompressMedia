﻿using CompressMedia.Imaging;
using CompressMedia.Repositories.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace CompressMedia.Repositories
{
	public class ImageSharpService : IImageSharpService
	{
		public async Task<Stream> ResizeAsync(
			Stream inputStream,
			ImageResizeArgs resizeArgs,
			CancellationToken cancellationToken = default
			)
		{
			using (Image image = await Image.LoadAsync(inputStream, cancellationToken))
			{
				MemoryStream outputStream = new MemoryStream();
				image.Mutate(x => x.Resize(resizeArgs.Width, resizeArgs.Height));
				await image.SaveAsJpegAsync(outputStream);

				if (image is null)
				{
					return inputStream;
				}
				outputStream.Seek(0, SeekOrigin.Begin);
				return outputStream;
			}
		}
	}
}
