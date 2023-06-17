using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*[Authorize]*/
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

        //Get List Lesson
        [HttpGet]
        public JsonResult GetListLesson(string keyword, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listLessonFromDB = (from _lesson in _context.Lessons.Where(x => x.IsDelete != 1)
                                        join _chapter in _context.Chapters on _lesson.IdChapter equals _chapter.Id
                                        select new
                                        {
                                            Id = _lesson.Id,
                                            LessonName = _lesson.LessonName,
                                            Meta = _lesson.Meta,
                                            ChapterId = _lesson.IdChapter,
                                            ChapterName = _chapter.ChapterName,
                                        }).AsEnumerable()
                                         .OrderBy(x => GetNumber(x.ChapterName))
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

        //Get All Lesson
        [HttpGet]
        public JsonResult GetAllLesson()
        {
            try
            {
                var listLesson = _context.Lessons.Where(x => x.IsDelete != 1).ToList();
                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách bài thành công!",
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

        [HttpGet]
        public JsonResult GetListLessonByChapter(int id, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listLessonFromDB = (from _lesson in _context.Lessons.Where(x => x.IdChapter == id && x.IsDelete != 1)
                                        join _chapter in _context.Chapters on _lesson.IdChapter equals _chapter.Id
                                        select new
                                        {
                                            Id = _lesson.Id,
                                            LessonName = _lesson.LessonName,
                                            ChaperId = _lesson.IdChapter,
                                            ChapterName = _chapter.ChapterName,
                                            Meta = _lesson.Meta,
                                        }).AsEnumerable()
                                        .OrderBy(x => GetNumber(x.ChapterName))
                                         .ThenBy(x => GetNumber(x.LessonName)).ToList();

                var pageSize = listLessonFromDB.Count % settingsPages == 0 ? listLessonFromDB.Count / settingsPages : listLessonFromDB.Count / settingsPages + 1;
                var listLesson = listLessonFromDB.Skip((page - 1) * settingsPages)
                                    .Take(settingsPages).ToList();
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

        [HttpGet]
        public JsonResult GetListLessonByChapterFilter(int id)
        {
            try
            {
                var listLesson = (from _lesson in _context.Lessons.Where(x => x.IdChapter == id && x.IsDelete != 1)
                                  join _chapter in _context.Chapters on _lesson.IdChapter equals _chapter.Id
                                  select new
                                  {
                                      Id = _lesson.Id,
                                      LessonName = _lesson.LessonName,
                                      ChaperId = _lesson.IdChapter,
                                      ChapterName = _chapter.ChapterName,
                                      Meta = _lesson.Meta,
                                  }).AsEnumerable()
                         .OrderBy(x => GetNumber(x.ChapterName))
                          .ThenBy(x => GetNumber(x.LessonName)).ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách bài thành công!",
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

        //Get Detail
        [HttpGet]
        public JsonResult GetDetail(int id)
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

        //Add Lesson
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

        //Update Lesson
        [HttpPost]
        public JsonResult UpdateLesson(int id, int chapterId, string lessonName, string lessonMeta)
        {
            try
            {
                //Find lesson by id
                var _lesson = _context.Lessons.SingleOrDefault(x => x.Id == id);

                //Set new value lesson
                _lesson.IdChapter = chapterId;
                _lesson.LessonName = lessonName;
                _lesson.Meta = lessonMeta;

                //Save data
                _context.SaveChanges();

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

        //Delete Lesson
        [HttpPost]
        public JsonResult DeleteLesson(int id)
        {
            try
            {
                //Find lesson by id
                var _lesson = _context.Lessons.SingleOrDefault(x => x.Id == id);
                _lesson.IsDelete = 1;

                //Save data
                _context.SaveChanges();

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
