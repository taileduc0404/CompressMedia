using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Volo.Abp.Imaging;

namespace CompressMedia.Imaging
{
	public class ImageResizeArgs
	{
		private int _width;
		private int _heigth;
		public int Width
		{
			get => _width;
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Width cannot be negative!", nameof(value));
				}
				_width = value;
			}
		}
		public int Height
		{
			get => _heigth;
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Width cannot be negative!", nameof(value));
				}
				_heigth = value;
			}
		}

		public ImageResizeMode Mode { get; set; } = ImageResizeMode.Default;
		public ImageResizeArgs(int? width = null, int? height = null, ImageResizeMode? mode = null)
		{
			if (mode.HasValue)
			{
				Mode = mode.Value;
			}

			Width = width ?? 0;
			Height = height ?? 0;
		}

	}
}
