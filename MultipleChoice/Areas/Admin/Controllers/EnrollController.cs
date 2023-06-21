using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MultipleChoice.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class EnrollController : Controller
    {
        private readonly MultipleChoiceContext _context;

        private int GetNumber(string name)
        {
            string numberString = new string(name.Where(char.IsDigit).ToArray());
            return int.Parse(numberString);
        }

        public EnrollController(MultipleChoiceContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get list grade
        [HttpGet]
        public JsonResult GetListGrade()
        {
            try
            {
                var listGrade = _context.Grades.Where(x => x.IsDelete != 1).AsEnumerable()
                                .OrderBy(x => GetNumber(x.GradeName)).ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách khối thành công!",
                    data = listGrade
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách khối thất bại: " + ex.Message
                });
            }
        }

        //Get list Class
        [HttpGet]
        public JsonResult GetListClass(int id)
        {
            try
            {
                var listClass = _context.Classes.Where(x => x.IsDelete != 1 && x.IdGrade == id).ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách lớp thành công!",
                    data = listClass
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách lớp thất bại: " + ex.Message
                });
            }
        }

        //Get list enroll
        [HttpGet]
        public JsonResult GetListEnroll(int year, string keyword, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listEnrollFromDB = (from _enroll in _context.Learnings
                                        join _student in _context.Students on _enroll.IdStudent equals _student.IdStudent
                                        where (_enroll.Year == year)
                                        select new
                                        {
                                            StudentId = _student.IdStudent,
                                            StudentName = _student.StudentName,
                                            Birthday = _student.Birthday,
                                            Phone = _student.Phone,
                                            Email = _student.Email,
                                            Address = _student.Address,
                                        }).ToList();

                if (keyword != null)
                {
                    listEnrollFromDB = listEnrollFromDB.Where(x => x.StudentName.ToLower().Contains(keyword.ToLower()) || x.Phone.ToLower().Contains(keyword.ToLower()) || x.Email.ToLower().Contains(keyword.ToLower()) || x.StudentId.ToLower().Contains(keyword.ToLower()) || x.Address.ToLower().Contains(keyword.ToLower())).ToList();
                }

                var pageSize = listEnrollFromDB.Count % settingsPages == 0 ? listEnrollFromDB.Count / settingsPages : listEnrollFromDB.Count / settingsPages + 1;
                var listEnroll = listEnrollFromDB.Skip((page - 1) * settingsPages)
                                    .Take(settingsPages)
                                    .ToList();
                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách học sinh đã xếp lớp thành công!",
                    pageSize,
                    data = listEnroll
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách học sinh đã xếp lớp thất bại: " + ex.Message
                });
            }

        }


        //Get list not enroll
        [HttpGet]
        public JsonResult GetListNotEnroll(string keyword, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var _listNotEnrollFromDB = (from _studentNotEnroll in ((from _student in _context.Students.Where(x => x.IsDelete != 1)
                                                                        select new
                                                                        {
                                                                            Id = _student.IdStudent
                                                                        }).ToList()
                   .Except(from _enroll in _context.Learnings.Where(x => x.Year == DateTime.Now.Year)
                           select new
                           {
                               Id = _enroll.IdStudent
                           }).ToList())
                                            join _student in _context.Students on _studentNotEnroll.Id equals _student.IdStudent
                                            select new
                                            {
                                                StudentId = _studentNotEnroll.Id,
                                                StudentName = _student.StudentName,
                                                Birthday = _student.Birthday,
                                                Phone = _student.Phone,
                                                Email = _student.Email,
                                                Address = _student.Address,
                                            }).ToList();

                if (keyword != null)
                {
                    _listNotEnrollFromDB = _listNotEnrollFromDB.Where(x => x.StudentName.ToLower().Contains(keyword.ToLower()) || x.Phone.ToLower().Contains(keyword.ToLower()) || x.Email.ToLower().Contains(keyword.ToLower()) || x.StudentId.ToLower().Contains(keyword.ToLower()) || x.Address.ToLower().Contains(keyword.ToLower())).ToList();
                }

                var pageSize = _listNotEnrollFromDB.Count % settingsPages == 0 ? _listNotEnrollFromDB.Count / settingsPages : _listNotEnrollFromDB.Count / settingsPages + 1;
                var listNotEnroll = _listNotEnrollFromDB.Skip((page - 1) * settingsPages)
                                                        .Take(settingsPages)
                                                        .ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách học sinh chưa nhập học thành công!",
                    pageSize,
                    data = listNotEnroll
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách học sinh chưa nhập học thất bại: " + ex.Message
                });
            }
        }

        //Enroll
        [HttpPost]
        public JsonResult Enroll(string studentId, int classId)
        {
            try
            {
                var _enroll = new Learning();
                _enroll.IdStudent = studentId;
                _enroll.IdClass = classId;
                _enroll.Year = DateTime.Now.Year;

                _context.Learnings.Add(_enroll);
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Nhập học thành công!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Nhập học thất bại: " + ex.Message
                });
            }
        }
    }
}

