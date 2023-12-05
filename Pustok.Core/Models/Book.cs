using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok.Models
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Tax { get; set; }
        public string Code { get; set; }
        public bool IsAvailable { get; set; }
        public double CostPrice { get; set; }
        public double SalePrice { get; set; }
        public double DiscountPercent { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsNew { get; set; }
        public bool IsBestseller { get; set; }

        public int GenreId { get; set; }
        public Genre? Genre { get; set; }

        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        public List<BookTag>? BookTags { get; set; }
       
        [NotMapped]
        public List<int>? TagIds { get; set; }

        public List<BookImage>? BookImages { get; set; }

        [NotMapped]
        public List<IFormFile>? ImageFiles { get; set; }
        [NotMapped]
        public IFormFile? BookPosterImageFile { get; set; }
        [NotMapped]
        public IFormFile? BookHoverImageFile { get; set; }
        [NotMapped]
        public List<int>? BookImageIds { get; set; }
    }
}
