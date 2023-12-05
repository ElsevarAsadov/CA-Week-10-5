using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok.Models;

public class Slider : BaseEntity
{
    [Required]
    [StringLength(maximumLength: 50)]
    public string Title { get; set; }
    [Required]
    [StringLength(maximumLength: 250)]
    public string Description { get; set; }
    [StringLength(maximumLength:100)]
    public string? Image { get; set; }
    public string RedirectUrl { get; set; }
    public string ButtonText { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}
