﻿
namespace CompressMedia.CustomAuthentication
{
	public static class PasswordHasher
	{
		public static string Hash(string password)
		{
			return BCrypt.Net.BCrypt.HashPassword(password);
		}

		public static bool VerifyPassword(string password, string hashedPassword)
		{
			return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
		}
	}
}
