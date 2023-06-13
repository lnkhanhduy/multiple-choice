using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MultipleChoice.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*[Authorize]*/
    public class EnrollController : Controller
    {
        private readonly MultipleChoiceContext _context;

        public EnrollController(MultipleChoiceContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get List Class
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

        //Get All Class
        public JsonResult GetAllClass()
        {
            try
            {
                var listClasses = _context.Classes.Where(x => x.IsDelete != 1).ToList();
                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách lớp thành công!",
                    data = listClasses
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

        //Get Detail
        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            try
            {
                var _class = _context.Classes.SingleOrDefault(x => x.Id == id);

                return Json(new
                {
                    code = 200,
                    message = "Lấy thông tin chi tiết của lớp thành công!",
                    data = _class
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy thông tin chi tiết của lớp thất bại: " + ex.Message
                });
            }
        }


        //Get Student Not Enroll
        [HttpGet]
        public JsonResult GetStudentNotEnrollByClass(string keyword, int page)
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

        //Update Enrollment
        [HttpPost]
        public JsonResult UpdateEnroll(int id, string className, string classMeta)
        {
            try
            {
                //Find class by id
                var _class = _context.Classes.SingleOrDefault(x => x.Id == id);

                //Set new value class
                _class.ClassName = className;
                _class.Meta = classMeta;

                //Save data
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Cập nhật lớp thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Cập nhật lớp thất bại: " + ex.Message
                });
            }
        }

        //Delete Class
        [HttpPost]
        public JsonResult DeleteClass(int id)
        {
            try
            {
                //Find class by id
                var _class = _context.Classes.SingleOrDefault(x => x.Id == id);
                _class.IsDelete = 1;

                //Save data
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Xóa lớp thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Xóa lớp thất bại: " + ex.Message
                });
            }
        }
    }
}

