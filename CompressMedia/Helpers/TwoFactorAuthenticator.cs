
using QRCoder;
using System.Security.Cryptography;

namespace CompressMedia.Helpers
{
	public class TwoFactorAuthenticator
	{
		private HashType HashType { get; set; }
		private const int StepSeconds = 30;
		private const int OtpSize = 6;
		private static readonly long UnixEpochTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
		private static readonly long TicksToSeconds = TimeSpan.TicksPerSecond;

		public TwoFactorAuthenticator()
		{
			HashType = HashType.HMACSHA1;
		}

		/// <summary>
		/// Generates setup code including the QR code URL
		/// </summary>
		/// <param name="issuer"></param>
		/// <param name="accountTitle"></param>
		/// <param name="accountSecretKey"></param>
		/// <param name="qrPixelsPerModule"></param>
		/// <param name="generateQrCode"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public SetupCode GenerateSetupCode(string issuer, string accountTitle, byte[] accountSecretKey, int qrPixelsPerModule = 3, bool generateQrCode = true)
		{
			if (string.IsNullOrWhiteSpace(accountTitle))
				throw new ArgumentException("Account Title cannot be empty");

			accountTitle = Uri.EscapeDataString(accountTitle.Replace(" ", ""));
			var encodedSecretKey = Base32Encoding.ToString(accountSecretKey);
			var provisionUrl = string.IsNullOrWhiteSpace(issuer)
				? $"otpauth://totp/{accountTitle}?secret={encodedSecretKey}&algorithm={HashType}"
				: $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{accountTitle}?secret={encodedSecretKey}&issuer={Uri.EscapeDataString(issuer)}&algorithm={HashType}";

			return new SetupCode(accountTitle, encodedSecretKey, generateQrCode ? GenerateQrCodeUrl(qrPixelsPerModule, provisionUrl) : "");
		}

		/// <summary>
		/// Validates the OTP with a given time window
		/// </summary>
		/// <param name="secretKey"></param>
		/// <param name="userInputOtp"></param>
		/// <param name="timeWindow"></param>
		/// <returns></returns>
		public bool ValidateTwoFactorPIN(byte[] secretKey, string userInputOtp, TimeSpan timeWindow)
		{
			var currentTimeStep = GetCurrentTimeStepNumber();
			if (GenerateTotp(secretKey, currentTimeStep) == userInputOtp)
				return true;

			int timeWindowSteps = (int)(timeWindow.TotalSeconds / StepSeconds);
			for (int i = 1; i <= timeWindowSteps; i++)
			{
				if (GenerateTotp(secretKey, currentTimeStep - i) == userInputOtp || GenerateTotp(secretKey, currentTimeStep + i) == userInputOtp)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Tạo url Qr Code
		/// </summary>
		/// <param name="qrPixelsPerModule"></param>
		/// <param name="provisionUrl"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		private static string GenerateQrCodeUrl(int qrPixelsPerModule, string provisionUrl)
		{
			try
			{
				using (var qrGenerator = new QRCodeGenerator())
				using (var qrCodeData = qrGenerator.CreateQrCode(provisionUrl, QRCodeGenerator.ECCLevel.Q))
				using (var qrCode = new PngByteQRCode(qrCodeData))
				{
					var qrCodeImage = qrCode.GetGraphic(qrPixelsPerModule);
					return $"data:image/png;base64,{Convert.ToBase64String(qrCodeImage)}";
				}
			}
			catch
			{
				throw new Exception("Failed to generate QR Code.");
			}
		}
	
		/// <summary>
		/// Computes the current TOTP based on the secret key and time step
		/// </summary>
		/// <param name="secretKey"></param>
		/// <param name="timeStep"></param>
		/// <returns></returns>
		private string GenerateTotp(byte[] secretKey, long timeStep)
		{
			using var hmac = new HMACSHA1(secretKey);
			var timeStepBytes = GetBigEndianBytes(timeStep);
			var hash = hmac.ComputeHash(timeStepBytes);
			int offset = hash[hash.Length - 1] & 0x0F;
			int otp = (hash[offset] & 0x7F) << 24 | (hash[offset + 1] & 0xFF) << 16 | (hash[offset + 2] & 0xFF) << 8 | (hash[offset + 3] & 0xFF);
			otp %= (int)Math.Pow(10, OtpSize);
			return otp.ToString().PadLeft(OtpSize, '0');
		}

		/// <summary>
		/// Gets the current time step number
		/// </summary>
		/// <returns></returns>
		private long GetCurrentTimeStepNumber()
		{
			var elapsedSeconds = (DateTime.UtcNow.Ticks - UnixEpochTicks) / TicksToSeconds;
			return elapsedSeconds / StepSeconds;
		}

		/// <summary>
		/// Converts a long number to Big Endian byte array
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private byte[] GetBigEndianBytes(long input)
		{
			var bytes = BitConverter.GetBytes(input);
			if (BitConverter.IsLittleEndian)
				Array.Reverse(bytes);
			return bytes;
		}
	}
}
