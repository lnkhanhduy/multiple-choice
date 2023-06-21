using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ChapterController : Controller
    {
        private readonly MultipleChoiceContext _context;

        private int GetNumber(string name)
        {
            string numberString = new string(name.Where(char.IsDigit).ToArray());
            return int.Parse(numberString);
        }

        public ChapterController(MultipleChoiceContext context)
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

        //Get list subject by grade
        [HttpGet]
        public JsonResult GetListSubject(int gradeId)
        {
            try
            {
                var listSubject = _context.Subjects.Where(x => x.IsDelete != 1 && x.IdGrade == gradeId).AsEnumerable()
                                    .OrderBy(x => x.SubjectName).ThenBy(x => GetNumber(x.SubjectName)).ToList();

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
        public JsonResult GetListChapter(int subjectId, string keyword, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listChapterFromDB = (from _chapter in _context.Chapters.Where(x => x.IsDelete != 1 && x.IdSubject == subjectId)
                                         join _subject in _context.Subjects on _chapter.IdSubject equals _subject.Id
                                         select new
                                         {
                                             Id = _chapter.Id,
                                             ChapterName = _chapter.ChapterName,
                                             Meta = _chapter.Meta,
                                             SubjectId = _chapter.IdSubject,
                                             SubjectName = _subject.SubjectName
                                         }).AsEnumerable()
                                         .OrderBy(x => GetNumber(x.ChapterName))
                                         .ThenBy(x => x.ChapterName).ToList();

                if (keyword != null)
                {
                    listChapterFromDB = listChapterFromDB.Where(x => x.ChapterName.ToLower().Contains(keyword.ToLower()) || x.Meta.ToLower().Contains(keyword.ToLower()) || x.SubjectName.ToLower().Contains(keyword.ToLower())).ToList();
                }

                var pageSize = listChapterFromDB.Count % settingsPages == 0 ? listChapterFromDB.Count / settingsPages : listChapterFromDB.Count / settingsPages + 1;
                var listChapter = listChapterFromDB.Skip((page - 1) * settingsPages)
                                    .Take(settingsPages).ToList();
                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách chương thành công!",
                    pageSize,
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

        //Get detail chapter
        [HttpGet]
        public JsonResult GetDetailChapter(int id)
        {
            try
            {
                var _chapter = _context.Chapters.SingleOrDefault(x => x.Id == id);

                return Json(new
                {
                    code = 200,
                    message = "Lấy thông tin chi tiết chương thành công!",
                    data = _chapter
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy thông tin chi tiết chương thất bại: " + ex.Message
                });
            }
        }

        //Add chapter
        [HttpPost]
        public JsonResult AddChapter(int subjectId, string chapterName, string chapterMeta)
        {
            try
            {
                var _chapter = new Chapter();
                _chapter.IdSubject = subjectId;
                _chapter.ChapterName = chapterName;
                _chapter.Meta = chapterMeta;

                _context.Chapters.Add(_chapter);
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Thêm chương thành công!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Thêm chương thất bại: " + ex.Message
                });
            }
        }

        //Update chapter
        [HttpPost]
        public JsonResult UpdateChapter(int id, int subjectId, string chapterName, string chapterMeta)
        {
            try
            {
                var _chapter = _context.Chapters.SingleOrDefault(x => x.Id == id);

                if (_chapter != null)
                {
                    _chapter.IdSubject = subjectId;
                    _chapter.ChapterName = chapterName;
                    _chapter.Meta = chapterMeta;

                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy chương",
                    });
                }

                return Json(new
                {
                    code = 200,
                    message = "Cập nhật chương thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Cập nhật chương thất bại: " + ex.Message
                });
            }
        }

        //Delete chapter
        [HttpPost]
        public JsonResult DeleteChapter(int id)
        {
            try
            {
                var _chapter = _context.Chapters.SingleOrDefault(x => x.Id == id);

                if (_chapter != null)
                {
                    _chapter.IsDelete = 1;

                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy chương",
                    });
                }

                return Json(new
                {
                    code = 200,
                    message = "Xóa chương thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Xóa chương thất bại: " + ex.Message
                });
            }
        }
    }
}

