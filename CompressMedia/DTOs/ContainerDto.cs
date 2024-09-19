using System.ComponentModel.DataAnnotations;

namespace CompressMedia.DTOs
{
    public class ContainerDto
    {
        [Required]
        public int ContainerId { get; set; }
        [Required]
        public string? ContainerName { get; set; }
        [Required]
        public string? UserId { get; set; }
        public string? Status { get; set; }
    }
}
