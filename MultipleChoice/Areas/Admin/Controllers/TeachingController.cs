using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MultipleChoice.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TeachingController : Controller
    {
        private readonly MultipleChoiceContext _context;

        private int GetNumber(string name)
        {
            string numberString = new string(name.Where(char.IsDigit).ToArray());
            return int.Parse(numberString);
        }

        public TeachingController(MultipleChoiceContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get list subject
        [HttpGet]
        public JsonResult GetListSubject()
        {
            try
            {
                var listSubject = (from _subject in _context.Subjects.Where(x => x.IsDelete != 1).OrderBy(x => x.SubjectName)
                                   select new
                                   {
                                       Id = _subject.Id,
                                       SubjectName = _subject.SubjectName,
                                   }).ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách môn học thành công!",
                    data = listSubject
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách môn học thất bại: " + ex.Message
                });
            }
        }

        //Get list teacher
        [HttpGet]
        public JsonResult GetListTeacher(int subjectId)
        {
            try
            {
                var listTeacher = (from _teacher in _context.Teachers.Where(x => x.IsDelete != 1 && x.IdSubject == subjectId).OrderBy(x => x.TeacherName)
                                   select new
                                   {
                                       Id = _teacher.Id,
                                       TeacherName = _teacher.TeacherName,
                                   }).ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách giáo viên thành công!",
                    data = listTeacher
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách giáo viên thất bại: " + ex.Message
                });
            }
        }

        //Get list class
        [HttpGet]
        public JsonResult GetListClass()
        {
            try
            {
                var listClass = (from _class in _context.Classes.Where(x => x.IsDelete != 1).AsEnumerable().OrderBy(x => GetNumber(x.ClassName))
                                 select new
                                 {
                                     Id = _class.Id,
                                     ClassName = _class.ClassName,
                                 }).ToList();

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

        //Get list teaching
        [HttpGet]
        public JsonResult GetListTeaching(string keyword, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listTeachingFromDB = (from _teaching in _context.Teachings.Where(x => x.IsDelete != 1)
                                          join _teacher in _context.Teachers on _teaching.IdTeacher equals _teacher.Id
                                          join _subject in _context.Subjects on _teaching.IdSubject equals _subject.Id
                                          join _class in _context.Classes on _teaching.IdClass equals _class.Id
                                          select new
                                          {
                                              Id = _teaching.Id,
                                              TeacherName = _teacher.TeacherName,
                                              TeacherId = _teacher.Id,
                                              UserIdTeacher = _teacher.IdTeacher,
                                              SubjectName = _subject.SubjectName,
                                              SubjectId = _subject.Id,
                                              ClassName = _class.ClassName,
                                              ClassId = _class.Id,
                                              StartingDate = _teaching.StartingDate,
                                              EndingDate = _teaching.EndingDate
                                          }).AsEnumerable().OrderBy(x => GetNumber(x.ClassName))
                                          .ThenBy(x => x.SubjectName).ToList();

                if (keyword != null)
                {
                    listTeachingFromDB = listTeachingFromDB.Where(x => x.UserIdTeacher.ToLower().Contains(keyword.ToLower()) || x.TeacherName.ToLower().Contains(keyword.ToLower()) || x.SubjectName.ToLower().Contains(keyword.ToLower()) || x.ClassName.ToLower().Contains(keyword.ToLower())).ToList();
                }

                var pageSize = listTeachingFromDB.Count % settingsPages == 0 ? listTeachingFromDB.Count / settingsPages : listTeachingFromDB.Count / settingsPages + 1;
                var listTeaching = listTeachingFromDB.Skip((page - 1) * settingsPages)
                                    .Take(settingsPages)
                                    .ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách quá trình dạy thành công!",
                    pageSize,
                    data = listTeaching
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách quá trình dạy thất bại: " + ex.Message
                });
            }
        }

        //Get detail teaching
        [HttpGet]
        public JsonResult GetDetailTeaching(int id)
        {
            try
            {
                var _teaching = _context.Teachings.SingleOrDefault(x => x.Id == id);

                return Json(new
                {
                    code = 200,
                    message = "Lấy thông tin chi tiết quá trình giảng dạy thành công!",
                    data = _teaching
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy thông tin chi tiết quá trình giảng dạy thất bại: " + ex.Message
                });
            }
        }

        //Add teaching
        [HttpPost]
        public JsonResult AddTeaching(int teacherId, int subjectId, int classId, DateTime startingDate, DateTime endingDate)
        {
            try
            {
                var _teaching = new Teaching();
                _teaching.IdTeacher = teacherId;
                _teaching.IdSubject = subjectId;
                _teaching.IdClass = classId;
                _teaching.StartingDate = startingDate;
                _teaching.EndingDate = endingDate;

                _context.Teachings.Add(_teaching);
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Thêm quá trình dạy của giáo viên thành công!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Thêm quá trình dạy của giáo viên thất bại: " + ex.Message
                });
            }
        }

        //Update teaching
        [HttpPost]
        public JsonResult UpdateTeaching(int id, int teacherId, int subjectId, int classId, DateTime startingDate, DateTime endingDate)
        {
            try
            {
                var _teaching = _context.Teachings.SingleOrDefault(x => x.Id == id);

                if (_teaching != null)
                {
                    _teaching.IdTeacher = teacherId;
                    _teaching.IdSubject = subjectId;
                    _teaching.IdClass = classId;
                    _teaching.StartingDate = startingDate;
                    _teaching.EndingDate = endingDate;

                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy quá trình giảng dạy!",
                    });
                }

                return Json(new
                {
                    code = 200,
                    message = "Cập nhật quá trình giảng dạy thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Cập nhật quá trình giảng dạy thất bại: " + ex.Message
                });
            }
        }

        //Delete teaching
        [HttpPost]
        public JsonResult DeleteTeaching(int id)
        {
            try
            {
                var _teaching = _context.Teachings.SingleOrDefault(x => x.Id == id);

                if (_teaching != null)
                {
                    _teaching.IsDelete = 1;

                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy quá trình giảng dạy!",
                    });
                }

                return Json(new
                {
                    code = 200,
                    message = "Xóa quá trình giảng dạy thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Xóa quá trình giảng dạy thất bại: " + ex.Message
                });
            }
        }
    }
}
