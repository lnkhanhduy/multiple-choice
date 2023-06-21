using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class LessonController : Controller
    {
        private readonly MultipleChoiceContext _context;

        private int GetNumber(string name)
        {
            string numberString = new string(name.Where(char.IsDigit).ToArray());
            return int.Parse(numberString);
        }

        public LessonController(MultipleChoiceContext context)
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

        //Get list subject
        [HttpGet]
        public JsonResult GetListSubject(int gradeId)
        {
            try
            {
                var listSubject = _context.Subjects.Where(x => x.IsDelete != 1 && x.IdGrade == gradeId).AsEnumerable()
                                    .OrderBy(x => x.SubjectName)
                                    .ThenBy(x => GetNumber(x.SubjectName)).ToList();

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

        //Get list chapter
        [HttpGet]
        public JsonResult GetListChapter(int subjectId)
        {
            try
            {
                var listChapter = _context.Chapters.Where(x => x.IsDelete != 1 && x.IdSubject == subjectId).AsEnumerable()
                                    .OrderBy(x => GetNumber(x.ChapterName))
                                    .ThenBy(x => x.ChapterName).ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách chương thành công!",
                    data = listChapter
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách chương thất bại: " + ex.Message
                });
            }
        }

        //Get list lesson
        [HttpGet]
        public JsonResult GetListLesson(int chapterId, string keyword, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listLessonFromDB = (from _lesson in _context.Lessons.Where(x => x.IsDelete != 1 && x.IdChapter == chapterId)
                                        join _chapter in _context.Chapters on _lesson.IdChapter equals _chapter.Id
                                        select new
                                        {
                                            Id = _lesson.Id,
                                            LessonName = _lesson.LessonName,
                                            Meta = _lesson.Meta,
                                            ChapterId = _lesson.IdChapter,
                                            ChapterName = _chapter.ChapterName,
                                        }).AsEnumerable()
                                         .OrderBy(x => x.LessonName)
                                         .ThenBy(x => GetNumber(x.LessonName)).ToList();

                if (keyword != null)
                {
                    listLessonFromDB = listLessonFromDB.Where(x => x.ChapterName.ToLower().Contains(keyword.ToLower()) || x.Meta.ToLower().Contains(keyword.ToLower()) || x.LessonName.ToLower().Contains(keyword.ToLower())).ToList();
                }

                var pageSize = listLessonFromDB.Count % settingsPages == 0 ? listLessonFromDB.Count / settingsPages : listLessonFromDB.Count / settingsPages + 1;
                var listLesson = listLessonFromDB.Skip((page - 1) * settingsPages)
                                    .Take(settingsPages)
                                    .ToList();
                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách bài thành công!",
                    pageSize,
                    data = listLesson
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách bài thất bại: " + ex.Message
                });
            }
        }

        //Get detail lesson
        [HttpGet]
        public JsonResult GetDetailLesson(int id)
        {
            try
            {
                var _lesson = _context.Lessons.SingleOrDefault(x => x.Id == id);

                return Json(new
                {
                    code = 200,
                    message = "Lấy thông tin chi tiết bài thành công!",
                    data = _lesson
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy thông tin chi tiết bài thất bại: " + ex.Message
                });
            }
        }

        //Add lesson
        [HttpPost]
        public JsonResult AddLesson(int chapterId, string lessonName, string lessonMeta)
        {
            try
            {
                var _lesson = new Lesson();
                _lesson.IdChapter = chapterId;
                _lesson.LessonName = lessonName;
                _lesson.Meta = lessonMeta;

                _context.Lessons.Add(_lesson);
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Thêm bài thành công!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Thêm bài thất bại: " + ex.Message
                });
            }
        }

        //Update lesson
        [HttpPost]
        public JsonResult UpdateLesson(int id, int chapterId, string lessonName, string lessonMeta)
        {
            try
            {
                var _lesson = _context.Lessons.SingleOrDefault(x => x.Id == id);

                if (_lesson != null)
                {
                    _lesson.IdChapter = chapterId;
                    _lesson.LessonName = lessonName;
                    _lesson.Meta = lessonMeta;

                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy bài!",
                    });
                }

                return Json(new
                {
                    code = 200,
                    message = "Cập nhật bài thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Cập nhật bài thất bại: " + ex.Message
                });
            }
        }

        //Delete lesson 
        [HttpPost]
        public JsonResult DeleteLesson(int id)
        {
            try
            {
                var _lesson = _context.Lessons.SingleOrDefault(x => x.Id == id);

                if (_lesson != null)
                {
                    _lesson.IsDelete = 1;

                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy bài!",
                    });
                }

                return Json(new
                {
                    code = 200,
                    message = "Xóa bài thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Xóa bài thất bại: " + ex.Message
                });
            }
        }
    }
}
