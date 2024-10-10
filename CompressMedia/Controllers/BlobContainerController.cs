using AspNetCoreHero.ToastNotification.Abstractions;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.PermissionRequirement;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompressMedia.Controllers
{
	public class BlobContainerController : Controller
	{
		private readonly IBlobContainerService _blobContainerService;
		private readonly INotyfService _notyfService;

		public BlobContainerController(IBlobContainerService blobContainerService, INotyfService notyfService)
		{
			_blobContainerService = blobContainerService;
			_notyfService = notyfService;
		}

		[HttpGet]
		[CustomPermission("CreateContainer")]
		public async Task<IActionResult> Index()
		{
			string? tenantIdString = HttpContext.User.FindFirstValue("TenantId");
			Guid? _tenantId = null;

			if (!string.IsNullOrEmpty(tenantIdString) && Guid.TryParse(tenantIdString, out Guid tenantId))
			{
				_tenantId = tenantId;
			}

			IEnumerable<BlobContainer> blobContainers = await _blobContainerService.GetAsync(_tenantId);

			if (blobContainers == null || !blobContainers.Any())
			{
				_notyfService.Error("No blob container.");
				return View(Enumerable.Empty<ContainerDto>());
			}

			IEnumerable<ContainerDto> containerDtos = blobContainers.Select(container => new ContainerDto
			{
				ContainerId = container.ContainerId,
				ContainerName = container.ContainerName,
				TenantName = container.Tenant?.TenantName
			});

			return View(containerDtos);
		}

		[HttpGet]
		[CustomPermission("CreateContainer")]
		public IActionResult CreateContainer()
		{
			return View();
		}

		[HttpPost]
		[CustomPermission("CreateContainer")]
		public async Task<IActionResult> CreateContainer(ContainerDto containerDto)
		{
			if (containerDto != null)
			{
				string result = await _blobContainerService.SaveAsync(containerDto);

				switch (result)
				{
					case "null":
						_notyfService.Error("Access Dinied");
						return RedirectToAction("Index");
					case "exist":
						_notyfService.Error("Container Name exist. Enter other Container Name");
						return RedirectToAction("Index");
				}

				_notyfService.Success("Create container successfully.");
				return RedirectToAction("Index");
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		[ActionName("DeleteContainer")]
		[CustomPermission("DeleteContainer")]
		public IActionResult DeleteContainerGet(int containerId)
		{
			try
			{
				ContainerDto containerDto = new ContainerDto
				{
					ContainerId = containerId
				};
				return View(containerDto);
			}
			catch (InvalidOperationException)
			{
				_notyfService.Error("You cannot do this action");
				return RedirectToAction("Index");
			}
		}

		[HttpPost]
		[ActionName("DeleteContainer")]
		[CustomPermission("DeleteContainer")]
		public async Task<IActionResult> DeleteContainerPost(int containerId)
		{
			try
			{
				if (containerId != 0)
				{
					bool result = await _blobContainerService.DeleteAsync(containerId);
					if (result is false)
					{
						_notyfService.Error("Delete container failed.");
					}
					_notyfService.Success("Delete container successfully.");
					return RedirectToAction("Index");
				}
				return RedirectToAction("Index");
			}
			catch (InvalidOperationException)
			{
				_notyfService.Error("You cannot do this action");
				return RedirectToAction("Index");
			}
		}
	}
}
