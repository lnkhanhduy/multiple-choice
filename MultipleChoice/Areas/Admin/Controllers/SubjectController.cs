using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SubjectController : Controller
    {
        private readonly MultipleChoiceContext _context;

        private int GetNumber(string name)
        {
            string numberString = new string(name.Where(char.IsDigit).ToArray());
            return int.Parse(numberString);
        }

        public SubjectController(MultipleChoiceContext context)
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
                var listGrade = _context.Grades.Where(x => x.IsDelete != 1).AsEnumerable().OrderBy(x => GetNumber(x.GradeName)).ToList();

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

        //Get list teacher
        [HttpGet]
        public JsonResult GetListTeacher()
        {
            try
            {
                var listTeacher = _context.Teachers.Where(x => x.IsDelete != 1).ToList();

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

        //Get list subject
        [HttpGet]
        public JsonResult GetListSubject(string keyword, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listSubjectFromDB = (from _subject in _context.Subjects.Where(x => x.IsDelete != 1)
                                         join _grade in _context.Grades on _subject.IdGrade equals _grade.Id
                                         select new
                                         {
                                             Id = _subject.Id,
                                             SubjectName = _subject.SubjectName,
                                             GradeId = _subject.IdGrade,
                                             GradeName = _grade.GradeName,
                                             Meta = _subject.Meta,
                                         }).AsEnumerable()
                                         .OrderBy(x => GetNumber(x.GradeName))
                                         .ThenBy(x => x.SubjectName).ToList();

                if (keyword != null)
                {
                    listSubjectFromDB = listSubjectFromDB.Where(x => x.GradeName.ToLower().Contains(keyword.ToLower()) || x.SubjectName.ToLower().Contains(keyword.ToLower()) || x.Meta.ToLower().Contains(keyword.ToLower())).ToList();
                }

                var pageSize = listSubjectFromDB.Count % settingsPages == 0 ? listSubjectFromDB.Count / settingsPages : listSubjectFromDB.Count / settingsPages + 1;
                var listSubject = listSubjectFromDB.Skip((page - 1) * settingsPages)
                                   .Take(settingsPages)
                                   .ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách môn học thành công!",
                    pageSize,
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

        //Get detail subject
        [HttpGet]
        public JsonResult GetDetailSubject(int id)
        {
            try
            {
                var _subject = _context.Subjects.SingleOrDefault(x => x.Id == id);

                return Json(new
                {
                    code = 200,
                    message = "Lấy thông tin chi tiết của môn học thành công!",
                    data = _subject
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy thông tin chi tiết của môn học thất bại: " + ex.Message
                });
            }
        }

        //Add subject
        [HttpPost]
        public JsonResult AddSubject(int gradeId, int leaderId, string subjectName, string subjectMeta)
        {
            try
            {
                var _subject = new Subject();
                _subject.IdLeader = leaderId;
                _subject.IdGrade = gradeId;
                _subject.SubjectName = subjectName;
                _subject.Meta = subjectMeta;

                _context.Subjects.Add(_subject);
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Thêm môn học thành công!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Thêm môn học thất bại: " + ex.Message
                });
            }
        }

        //Update subject
        [HttpPost]
        public JsonResult UpdateSubject(int id, int leaderId, int gradeId, string subjectName, string subjectMeta)
        {
            try
            {
                var _subject = _context.Subjects.SingleOrDefault(x => x.Id == id);

                if (_subject != null)
                {
                    _subject.IdLeader = leaderId;
                    _subject.IdGrade = gradeId;
                    _subject.SubjectName = subjectName;
                    _subject.Meta = subjectMeta;

                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy môn học!",
                    });
                }

                return Json(new
                {
                    code = 200,
                    message = "Cập nhật môn học thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Cập nhật môn học thất bại: " + ex.Message
                });
            }
        }

        //Delete subject
        [HttpPost]
        public JsonResult DeleteSubject(int id)
        {
            try
            {
                var _subject = _context.Subjects.SingleOrDefault(x => x.Id == id);

                if (_subject != null)
                {
                    _subject.IsDelete = 1;

                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy môn học!",
                    });
                }

                return Json(new
                {
                    code = 200,
                    message = "Xóa môn học thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Xóa môn học thất bại: " + ex.Message
                });
            }
        }
    }
}
