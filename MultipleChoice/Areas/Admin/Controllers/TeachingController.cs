using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*[Authorize]*/
    public class TeachingController : Controller
    {
        private readonly MultipleChoiceContext _context;

        public TeachingController(MultipleChoiceContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get List Teaching
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
                                          }).ToList();

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
                    message = "Lấy danh sách quá trình dạy của giáo viên thành công!",
                    pageSize,
                    data = listTeaching
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách quá trình dạy của giáo viên thất bại: " + ex.Message
                });
            }

        }

        //Get Detail
        [HttpGet]
        public JsonResult GetDetail(int id)
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

        //Add Teaching
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

        //Update Teaching
        [HttpPost]
        public JsonResult UpdateTeaching(int id, int teacherId, int subjectId, int classId, DateTime startingDate, DateTime endingDate)
        {
            try
            {
                //Find teaching by id
                var _teaching = _context.Teachings.SingleOrDefault(x => x.Id == id);

                //Set new value class
                _teaching.IdTeacher = teacherId;
                _teaching.IdSubject = subjectId;
                _teaching.IdClass = classId;
                _teaching.StartingDate = startingDate;
                _teaching.EndingDate = endingDate;

                //Save data
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Cập nhật quá trình dạy của giáo viên thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Cập nhật quá trình dạy của giáo viên thất bại: " + ex.Message
                });
            }
        }

        //Delete Teaching
        [HttpPost]
        public JsonResult DeleteTeaching(int id)
        {
            try
            {
                //Find teaching by id
                var _teaching = _context.Teachings.SingleOrDefault(x => x.Id == id);
                _teaching.IsDelete = 1;

                //Save data
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Xóa quá trình dạy của giáo viên thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Xóa quá trình dạy của giáo viên thất bại: " + ex.Message
                });
            }
        }
    }
}
