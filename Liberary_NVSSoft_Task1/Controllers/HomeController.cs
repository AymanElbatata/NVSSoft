using Liberary_NVSSoft_Task1.BLL.Interfaces;
using Liberary_NVSSoft_Task1.BLL.Repositories;
using Liberary_NVSSoft_Task1.DAL.Entities;
using Liberary_NVSSoft_Task1.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Liberary_NVSSoft_Task1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }


        #region Users
        public IActionResult Users(string PartialError = "")
        {
            try
            {
                ViewData["PartialError"] = PartialError;
                return View(unitOfWork.UserRepository.GetAll());
            }
            catch (Exception ex)
            {
                return View("Errors", ex.Message);
            }
        }

        [HttpPost]
        public IActionResult DeleteUsers(int userId)
        {
            try
            {
                var user = unitOfWork.UserRepository.GetById(userId);
                if (user != null)
                {
                    var Confirmation = unitOfWork.UserRepository.Delete(user);
                    if (!Confirmation)
                    {
                        return RedirectToAction("Users", new { PartialError = "Deletion was Canceled, Invalid User!" });
                    }
                }

                return View("Users", unitOfWork.UserRepository.GetAll());

            }
            catch (Exception ex)
            {
                return View("Errors", ex.Message);
            }
        }


        public JsonResult GetUserJsonData(int userId)
        {
            var user = unitOfWork.UserRepository.GetById(userId);

            var jsonData = new { userName = user.UserName, userPassword = user.UserPW }; // Sample JSON data
            return Json(jsonData);
        }


        public JsonResult GetAllUsersForDDL()
        {
            // Retrieve data from your data source, for example, a database
            var AllAuthors = unitOfWork.UserRepository.GetAll();
            var MyList = new List<object>();
            MyList.Add(new { key = 0, value = "Select" });

            foreach (var item in AllAuthors)
            {
                MyList.Add(new { key = item.UserId, value = item.UserName });
            }
            // Here, data should be in the form of a collection of objects with 'key' and 'value' properties
            // You can format your data accordingly before returning it

            // Return data as JSON
            return Json(MyList);
        }


        [HttpPost]
        public IActionResult AddUpdateUsers(string Username, string userPassword, int processType, int userId = 0)
        {
            try
            {
                if (processType == 1)
                {
                    var user = new User()
                    {
                        UserName = Username,
                        UserPW = userPassword,
                        IsDeleted = false
                    };
                    var Confirmation = unitOfWork.UserRepository.Add(user);
                    if (!Confirmation)
                    {
                        return RedirectToAction("Users", new { PartialError = "Adding New User Was Canceled, UserName is Taken!" });
                    }
                }
                else
                {
                    var user = unitOfWork.UserRepository.GetById(userId);
                    if (user != null)
                    {
                        user.UserName = Username;
                        user.UserPW = userPassword;
                        var Confirmation = unitOfWork.UserRepository.Update(user);
                        if (!Confirmation)
                        {
                            return RedirectToAction("Users", new { PartialError = "Update was Canceled, Invalid User!" });
                        }
                    }
                }


                return View("Users", unitOfWork.UserRepository.GetAll());

            }
            catch (Exception ex)
            {
                return View("Errors", ex.Message);
            }
        }
        #endregion

        #region Authors
        public IActionResult Authors(string PartialError = "")
        {
            try
            {
                ViewData["PartialError"] = PartialError;
                return View(unitOfWork.AuthorRepository.GetAll());
            }
            catch (Exception ex)
            {
                return View("Errors", ex.Message);
            }
        }

        [HttpPost]
        public IActionResult DeleteAuthors(int AuthorId)
        {
            try
            {
                var Author = unitOfWork.AuthorRepository.GetById(AuthorId);
                if (Author != null)
                {
                    var Confirmation = unitOfWork.AuthorRepository.Delete(Author);
                    if (!Confirmation)
                    {
                        return RedirectToAction("Authors", new { PartialError = "Deletion was Canceled, Invalid Author!" });
                    }
                }

                return View("Authors", unitOfWork.AuthorRepository.GetAll());

            }
            catch (Exception ex)
            {
                return View("Errors", ex.Message);
            }
        }


        public JsonResult GetAuthorJsonData(int AuthorId)
        {
            var author = unitOfWork.AuthorRepository.GetById(AuthorId);

            var jsonData = new { AuthorName = author.AuthorName }; // Sample JSON data
            return Json(jsonData);
        }

        public JsonResult GetAllAuthorsForDDL()
        {
            // Retrieve data from your data source, for example, a database
            var AllAuthors = unitOfWork.AuthorRepository.GetAll();
            var MyList = new List<object>();
            MyList.Add(new { key = 0, value = "Select" });

            foreach (var item in AllAuthors)
            {
                MyList.Add(new { key = item.AuthorId, value = item.AuthorName });
            }
            // Here, data should be in the form of a collection of objects with 'key' and 'value' properties
            // You can format your data accordingly before returning it

            // Return data as JSON
            return Json(MyList);
        }

        [HttpPost]
        public IActionResult AddUpdateAuthors(string Authorname, int processType, int AuthorId = 0)
        {
            try
            {
                if (processType == 1)
                {
                    var Author = new Author()
                    {
                        AuthorName = Authorname,
                        IsDeleted = false
                    };
                    var Confirmation = unitOfWork.AuthorRepository.Add(Author);
                    if (!Confirmation)
                    {
                        return RedirectToAction("Authors", new { PartialError = "Adding New Author Was Canceled, Author Name is Taken!" });
                    }
                }
                else
                {
                    var Author = unitOfWork.AuthorRepository.GetById(AuthorId);
                    if (Author != null)
                    {
                        Author.AuthorName = Authorname;
                        var Confirmation = unitOfWork.AuthorRepository.Update(Author);
                        if (!Confirmation)
                        {
                            return RedirectToAction("Authors", new { PartialError = "Update was Canceled, Invalid Author!" });
                        }
                    }
                }


                return View("Authors", unitOfWork.AuthorRepository.GetAll());

            }
            catch (Exception ex)
            {
                return View("Errors", ex.Message);
            }
        }
        #endregion

        #region Books
        public IActionResult Books(string PartialError = "")
        {
            try
            {
                ViewData["PartialError"] = PartialError;
                return View(unitOfWork.BookRepository.GetAll());
            }
            catch (Exception ex)
            {
                return View("Errors", ex.Message);
            }
        }

        [HttpPost]
        public IActionResult DeleteBooks(int BookId)
        {
            try
            {
                var Book = unitOfWork.BookRepository.GetById(BookId);
                if (Book != null)
                {
                    var Confirmation = unitOfWork.BookRepository.Delete(Book);
                    if (!Confirmation)
                    {
                        return RedirectToAction("Books", new { PartialError = "Deletion was Canceled, Invalid Book!" });
                    }
                }

                return View("Books", unitOfWork.BookRepository.GetAll());

            }
            catch (Exception ex)
            {
                return View("Errors", ex.Message);
            }
        }


        public JsonResult GetBookJsonData(int BookId)
        {
            var book = unitOfWork.BookRepository.GetById(BookId);

            var jsonData = new { ISBN = book.ISBN, Title = book.Title, AuthorId = book.AuthorId }; // Sample JSON data
            return Json(jsonData);
        }

        public JsonResult GetAllBooksForDDL()
        {
            // Retrieve data from your data source, for example, a database
            var AllAuthors = unitOfWork.BookRepository.GetAll();
            var MyList = new List<object>();
            MyList.Add(new { key = 0, value = "Select" });

            foreach (var item in AllAuthors)
            {
                MyList.Add(new { key = item.BookId, value = item.ISBN+"||"+item.Title });
            }
            // Here, data should be in the form of a collection of objects with 'key' and 'value' properties
            // You can format your data accordingly before returning it

            // Return data as JSON
            return Json(MyList);
        }

        [HttpPost]
        public IActionResult AddUpdateBooks(string ISBN, string Title, int AuthorId, int processType, int BookId = 0)
        {
            try
            {
                if (processType == 1)
                {
                    var book = new Book()
                    {
                        ISBN = ISBN,
                        Title = Title,
                        AuthorId = AuthorId,
                        IsDeleted = false
                    };
                    var Confirmation = unitOfWork.BookRepository.Add(book);
                    if (!Confirmation)
                    {
                        return RedirectToAction("Books", new { PartialError = "Adding New Book Was Canceled, ISBN is Taken!" });
                    }
                }
                else
                {
                    var book = unitOfWork.BookRepository.GetById(BookId);
                    if (book != null)
                    {
                        book.ISBN = ISBN;
                        book.Title = Title;
                        book.AuthorId = AuthorId;
                        var Confirmation = unitOfWork.BookRepository.Update(book);
                        if (!Confirmation)
                        {
                            return RedirectToAction("Books", new { PartialError = "Update was Canceled, Invalid Book!" });
                        }
                    }
                }


                return View("Books", unitOfWork.BookRepository.GetAll());

            }
            catch (Exception ex)
            {
                return View("Errors", ex.Message);
            }
        }
        #endregion

        #region Borrowing Requests
        public IActionResult Index(string PartialError = "", string ISBNbook = null)
        {
            try
            {
                if (ISBNbook != null)
                {

                }
               ViewData["PartialError"] = PartialError;
                return View(GetBookWithStatus());
            }
            catch (Exception ex)
            {
                return View("Errors", ex.Message);
            }
        }

        public IActionResult Borrowings(string PartialError = "")
        {
            try
            {
                ViewData["PartialError"] = PartialError;
                return View(unitOfWork.BorrowingRepository.GetAll());
            }
            catch (Exception ex)
            {
                return View("Errors", ex.Message);
            }
        }

        public JsonResult GetBorrowingJsonData(int BorrowingId)
        {
            var book = unitOfWork.BorrowingRepository.GetById(BorrowingId);

            var jsonData = new { BorrowingId = book.BorrowingId, BookId = book.BookId, UserId = book.UserId, IsDone = book.IsDone == false ? 0 : 1 }; // Sample JSON data
            return Json(jsonData);
        }

        [HttpPost]
        public IActionResult AddUpdateBorrowings(int BookId, int UserId, int processType, int BorrowingId, int IsDone = 0, int Place =0)
        {
            try
            {
                if (processType == 1)
                {
                    var borrowing = new Borrowing()
                    {                       
                        BookId = BookId,
                        UserId = UserId,
                        IsDone = false,
                        IsDeleted = false
                    };
                    var Confirmation = unitOfWork.BorrowingRepository.Add(borrowing);
                    if (!Confirmation)
                    {
                        return RedirectToAction("Index", new { PartialError = "Adding New Borrowing Was Canceled, Request already made!" });
                    }
                }
                else
                {
                    var borrowing = unitOfWork.BorrowingRepository.GetById(BorrowingId);
                    if (borrowing != null)
                    {
                        borrowing.BookId = BookId;
                        borrowing.UserId = UserId;
                        borrowing.IsDone = IsDone == 1? true: false;
                        var Confirmation = unitOfWork.BorrowingRepository.Update(borrowing);
                        if (!Confirmation)
                        {
                            return RedirectToAction("Index", new { PartialError = "Update was Canceled, Invalid Borrowing Request!" });
                        }
                    }
                }
                if (Place == 1)
                {
                    return View("Borrowings", unitOfWork.BorrowingRepository.GetAll());
                }
                return View("Index", GetBookWithStatus());
            }
            catch (Exception ex)
            {
                return View("Errors", ex.Message);
            }
        }


        [HttpPost]
        public IActionResult DeleteBorrowings(int BorrowingId)
        {
            try
            {
                var borrowing = unitOfWork.BorrowingRepository.GetById(BorrowingId);
                if (borrowing != null)
                {
                    var Confirmation = unitOfWork.BorrowingRepository.Delete(borrowing);
                    if (!Confirmation)
                    {
                        return RedirectToAction("Index", new { PartialError = "Deletion was Canceled, Invalid Borrowing!" });
                    }
                }

                return View("Borrowings", unitOfWork.BorrowingRepository.GetAll());

            }
            catch (Exception ex)
            {
                return View("Errors", ex.Message);
            }
        }


        private List<BorrowBookStatusAllVM> GetBookWithStatus()
        {
            List<BorrowBookStatusAllVM> BookWithStatus = new List<BorrowBookStatusAllVM>();

            var AllBooks = unitOfWork.BookRepository.GetAll();
            foreach (var item in AllBooks)
            {
                var BorrowBookStatusAllVM = new BorrowBookStatusAllVM()
                {
                    BookId = item.BookId,
                    AuthorId = item.AuthorId,
                    Title = item.Title,
                    Author = item.Author,
                    ISBN = item.ISBN,
                    IsDeleted = item.IsDeleted,
                    IsOut = unitOfWork.BorrowingRepository.GetBookBorrowStatusByBookId((int)item.BookId)
                };
                BookWithStatus.Add(BorrowBookStatusAllVM);
            }
            return BookWithStatus;
        }

        #endregion


        public IActionResult Errors(string ex)
        {
            return View(ex);
        }



    }
}