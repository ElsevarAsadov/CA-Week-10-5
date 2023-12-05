using Microsoft.AspNetCore.Hosting;
using Pustok.Business.Exceptions;
using Pustok.Business.Extensions;
using Pustok.Models;
using Pustok.Repositories.Interfaces;
using Pustok.Business.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Pustok.Business.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IBookTagsRepository _bookTagsRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IBookImagesRepository _bookImagesRepository;

        public BookService(IBookRepository bookRepository,
                           IGenreRepository genreRepository,
                           IAuthorRepository authorRepository,
                           ITagRepository tagRepository,
                           IBookTagsRepository bookTagsRepository,
                           IWebHostEnvironment env,
                           IBookImagesRepository bookImagesRepository)

        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _authorRepository = authorRepository;
            _tagRepository = tagRepository;
            _bookTagsRepository = bookTagsRepository;
            _env = env;
            _bookImagesRepository = bookImagesRepository;
        }

        public async Task CreateAsync(Book entity)
        {
            if (!_genreRepository.Table.Any(x => x.Id == entity.GenreId))
            {
                throw new NotFoundException("GenreId", "Genre not found!");
            }

            if (!_authorRepository.Table.Any(x => x.Id == entity.AuthorId))
            {
                throw new NotFoundException("AuthorId", "Author not found!");
            }


            bool check = false;

            if (entity.TagIds != null)
            {
                foreach (var tagId in entity.TagIds)
                {
                    if (!_tagRepository.Table.Any(x => x.Id == tagId))
                    {
                        check = true;
                        break;
                    }
                }
            }

            if (check)
            {
                throw new NotFoundException("TagId", "Tag not found!");
            }
            else
            {
                if (entity.TagIds != null)
                {
                    foreach (var tagId in entity.TagIds)
                    {
                        BookTag bookTag = new BookTag
                        {
                            Book = entity,
                            TagId = tagId
                        };

                        await _bookTagsRepository.CreateAsync(bookTag);
                    }
                }
            }

            if (entity.BookPosterImageFile != null)
            {
                if (entity.BookPosterImageFile.ContentType != "image/jpeg" && entity.BookPosterImageFile.ContentType != "image/png")
                {
                    throw new InvalidImageContentException("BookPosterImageFile", "File must be .png or .jpeg (.jpg)");
                }
                if (entity.BookPosterImageFile.Length > 2097152)
                {
                    throw new InvalidImageContentException("BookPosterImageFile", "File size must be lower than 2mb!");
                }

                BookImage bookImage = new BookImage
                {
                    Book = entity,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/Books", entity.BookPosterImageFile),
                    IsPoster = true
                };

                await _bookImagesRepository.CreateAsync(bookImage);
            }

            if (entity.BookHoverImageFile != null)
            {
                if (entity.BookHoverImageFile.ContentType != "image/jpeg" && entity.BookHoverImageFile.ContentType != "image/png")
                {
                    throw new InvalidImageContentException("BookHoverImageFile", "File must be .png or .jpeg");
                }
                if (entity.BookHoverImageFile.Length > 2097152)
                {
                    throw new InvalidImageContentException("BookHoverImageFile", "File size must be lower than 2mb)");
                }

                BookImage bookImage = new BookImage
                {
                    Book = entity,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", entity.BookHoverImageFile),
                    IsPoster = false
                };

                await _bookImagesRepository.CreateAsync(bookImage);
            }


            if (entity.ImageFiles != null)
            {
                foreach (var imageFile in entity.ImageFiles)
                {
                    if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
                    {
                        throw new InvalidImageContentException("ImageFiles", "File must be .png or .jpeg");
                    }
                    if (imageFile.Length > 2097152)
                    {
                        throw new InvalidImageContentException("ImageFiles", "File size must be lower than 2mb)");
                    }

                    BookImage bookImage = new BookImage
                    {
                        Book = entity,
                        ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", imageFile),
                        IsPoster = null
                    };

                    await _bookImagesRepository.CreateAsync(bookImage);
                }
            }

            await _bookRepository.CreateAsync(entity);
            await _bookRepository.CommitAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }



        public async Task<List<Book>> GetAllAsync()
        {
            return await _bookRepository.GetAllAsync(x => x.IsDeleted == false, "BookImages", "Author");
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            var entity = await _bookRepository.GetByIdAsync(x => x.Id == id && x.IsDeleted == false, "Author", "BookImages", "BookTags.Tag");

            if (entity is null) throw new NullReferenceException();

            return entity;
        }

        public async Task SoftDelete(int id)
        {
            var entity = await _bookRepository.GetByIdAsync(x => x.Id == id && x.IsDeleted == false);

            if (entity is null) throw new NullReferenceException();

            entity.IsDeleted = true;
            await _bookRepository.CommitAsync();
        }

        public async Task UpdateAsync(Book entity)
        {
            Book existBook = _bookRepository.Table
                          .Include(x => x.BookTags)
                          .Include(x => x.BookImages)
                          .Include(x => x.BookTags).ThenInclude(x => x.Tag)
                          .FirstOrDefault(x => x.Id == entity.Id);


            if (existBook == null) throw new ModelNotFoundException();

            var destination = existBook.GetType().GetProperties();
            var source = entity.GetType().GetProperties();

            for (int i = 0; i < destination.Length; i++)
            {
                destination[i].SetValue(existBook, source[i].GetValue(entity));
            }


            if (!_genreRepository.Table.Any(x => x.Id == entity.GenreId))
            {
                throw new NotFoundException("GenreId", "Genre not found");
            }


            if (!_authorRepository.Table.Any(x => x.Id == entity.AuthorId))
            {
                throw new NotFoundException("AuthorId", "Author not found");
            }

            existBook.BookTags.RemoveAll(bt => !entity.TagIds.Contains(bt.TagId));

            foreach (var tagId in entity.TagIds.Where(x => !existBook.BookTags.Any(bt => bt.TagId == x)))
            {
                BookTag bookTag = new BookTag
                {
                    Book = existBook,
                    TagId = tagId
                };
                existBook.BookTags.Add(bookTag);
            }



            if (entity.BookPosterImageFile != null)
            {
                if (entity.BookPosterImageFile.ContentType != "image/jpeg" && entity.BookPosterImageFile.ContentType != "image/png")
                {

                    throw new InvalidImageContentException("BookPosterImageFile", "File must be .png, .jpg");
                }
                if (entity.BookPosterImageFile.Length > 2097152)
                {

                    throw new InvalidImageSizeException("BookPosterImageFile", "File size must be lower than 2MB");
                }


                BookImage bookImage = new BookImage
                {
                    Book = entity,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/Books", entity.BookPosterImageFile),
                    IsPoster = true
                };

                existBook.BookImages.Add(bookImage);
             }




            if (entity.BookHoverImageFile != null)
            {
                if (entity.BookHoverImageFile.ContentType != "image/jpeg" && entity.BookHoverImageFile.ContentType != "image/png")
                {

                    throw new InvalidImageContentException("BookHoverImageFile", "File must be .png, .jpg");
                }
                if (entity.BookHoverImageFile.Length > 2097152)
                {

                    throw new InvalidImageSizeException("BookHoverImageFile", "File size must be lower than 2MB");
                }

                BookImage bookImage = new BookImage
                {
                    Book = entity,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", entity.BookHoverImageFile),
                    IsPoster = false
                };

                existBook.BookImages.Add(bookImage);
                
            }


            existBook.BookImages.RemoveAll(bi => !entity.BookImageIds.Contains(bi.Id) && bi.IsPoster == null);
            if (entity.ImageFiles != null)
            {
                foreach (var imageFile in entity.ImageFiles)
                {
                    if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
                    {

                        throw new InvalidImageContentException("ImageFiles", "File must be .png, .jpg");
                    }
                    if (imageFile.Length > 2097152)
                    {

                        throw new InvalidImageSizeException("ImageFiles", "File size must be lower than 2MB");
                    }

                    BookImage bookImage = new BookImage
                    {
                        Book = entity,
                        ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", imageFile),
                        IsPoster = null
                    };

                    await _bookImagesRepository.CreateAsync(bookImage);
                    existBook.BookImages.Add(bookImage);
                }
            }


            existBook.Name = entity.Name;
            existBook.Description = entity.Description;
            existBook.CostPrice = entity.CostPrice;
            existBook.SalePrice = entity.SalePrice;
            existBook.Code = entity.Code;
            existBook.DiscountPercent = entity.DiscountPercent;
            existBook.IsAvailable = entity.IsAvailable;
            existBook.Tax = entity.Tax;
            existBook.AuthorId = entity.AuthorId;
            existBook.GenreId = entity.GenreId;

            await _bookRepository.CommitAsync();


        }
    }
}
