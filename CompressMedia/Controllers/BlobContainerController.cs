using AspNetCoreHero.ToastNotification.Abstractions;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompressMedia.Controllers
{
	public class BlobContainerController : Controller
	{
		private readonly IBlobContainerService _blobContainerService;
		private readonly INotyfService _notyfService;
		private readonly IUserService _userService;

		public BlobContainerController(IBlobContainerService blobContainerService, INotyfService notyfService, IUserService userService)
		{
			_blobContainerService = blobContainerService;
			_notyfService = notyfService;
			_userService = userService;
		}

		/// <summary>
		/// Lỗi truy cập
		/// </summary>
		/// <returns></returns>
		public IActionResult AccessDenied()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<User> users = await _userService.GetAllUser();
			if (users == null)
			{
				return RedirectToAction("AccessDenied");
			}

			IEnumerable<BlobContainer> blobContainers = await _blobContainerService.GetAsync();

			if (blobContainers == null || !blobContainers.Any())
			{
				_notyfService.Error("No blob container.");
				return View(Enumerable.Empty<ContainerDto>());
			}

			IEnumerable<ContainerDto> containerDtos = blobContainers.Select(container => new ContainerDto
			{
				ContainerId = container.ContainerId,
				ContainerName = container.ContainerName
			});

			return View(containerDtos);
		}

		[HttpGet]
		public async Task<IActionResult> CreateContainer()
		{
			IEnumerable<User> users = await _userService.GetAllUser();
			if (users == null)
			{
				return RedirectToAction("AccessDenied");
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateContainer(ContainerDto containerDto)
		{
			if (containerDto != null)
			{
				bool result = await _blobContainerService.SaveAsync(containerDto);
				if (result is false)
				{
					_notyfService.Error("Create container failed.");
				}
				_notyfService.Success("Create container successfully.");
				return RedirectToAction("Index");
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		[ActionName("DeleteContainer")]
		public async Task<IActionResult> DeleteContainerGet(int containerId)
		{
			IEnumerable<User> users = await _userService.GetAllUser();
			if (users == null)
			{
				return RedirectToAction("AccessDenied");
			}

			ContainerDto containerDto = new ContainerDto
			{
				ContainerId = containerId
			};
			return View(containerDto);
		}

		[HttpPost]
		[ActionName("DeleteContainer")]
		public async Task<IActionResult> DeleteContainerPost(int containerId)
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
	}
}
