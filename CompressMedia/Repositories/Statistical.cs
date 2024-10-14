using CompressMedia.Data;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompressMedia.Repositories
{
	public class Statistical : IStatistical
	{
		private readonly ApplicationDbContext _context;

		public Statistical(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<Blob>> Get10MediaWithTheMostComments(Guid tenantId)
		{
			return await _context.Blobs
				.Include(c => c.Comments)
				.Where(x => x.TenantId == tenantId)
				.OrderByDescending(x => x.Comments!.Count)
				.Take(10)
				.ToListAsync();
		}

		public async Task<List<Blob>> Get10MediaWithTheMostLikes(Guid tenantId)
		{
			return await _context.Blobs
				.Where(x => x.TenantId == tenantId)
				//.OrderByDescending(x => x.)
				.Take(10)
				.ToListAsync();
		}
	}
}
