using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*[Authorize]*/
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

        //Get List Chapter
        [HttpGet]
        public JsonResult GetListChapter(string keyword, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listChapterFromDB = (from _chapter in _context.Chapters.Where(x => x.IsDelete != 1)
                                         join _subject in _context.Subjects on _chapter.IdSubject equals _subject.Id
                                         select new
                                         {
                                             Id = _chapter.Id,
                                             ChapterName = _chapter.ChapterName,
                                             Meta = _chapter.Meta,
                                             SubjectId = _chapter.IdSubject,
                                             SubjectName = _subject.SubjectName
                                         }).AsEnumerable()
                                         .OrderBy(x => GetNumber(x.SubjectName))
                                         .ThenBy(x => GetNumber(x.ChapterName)).ToList();

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

        //Get All Chapter
        [HttpGet]
        public JsonResult GetAllChapter()
        {
            try
            {
                var listChapter = _context.Chapters.Where(x => x.IsDelete != 1).AsEnumerable()
                    .OrderBy(x => GetNumber(x.ChapterName)).ToList();
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

        [HttpGet]
        public JsonResult GetListChapterBySubjectFilter(int id)
        {
            try
            {
                var listChapter = (from _chapter in _context.Chapters.Where(x => x.IdSubject == id && x.IsDelete != 1)
                                   select new
                                   {
                                       Id = _chapter.Id,
                                       ChapterName = _chapter.ChapterName,
                                   }).AsEnumerable().OrderBy(x => GetNumber(x.ChapterName)).ToList();

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

        [HttpGet]
        public JsonResult GetListChapterBySubject(int id, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listChapterFromDB = (from _chapter in _context.Chapters.Where(x => x.IdSubject == id && x.IsDelete != 1)
                                         join _subject in _context.Subjects on _chapter.IdSubject equals _subject.Id
                                         select new
                                         {
                                             Id = _chapter.Id,
                                             ChapterName = _chapter.ChapterName,
                                             SubjectId = _chapter.IdSubject,
                                             SubjectName = _subject.SubjectName,
                                             Meta = _chapter.Meta,
                                         }).AsEnumerable().OrderBy(x => GetNumber(x.ChapterName)).ToList();

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

        //Get Detail
        [HttpGet]
        public JsonResult GetDetail(int id)
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

        //Add Chapter
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

        //Update Chapter
        [HttpPost]
        public JsonResult UpdateChapter(int id, int subjectId, string chapterName, string chapterMeta)
        {
            try
            {
                //Find chapter by id
                var _chapter = _context.Chapters.SingleOrDefault(x => x.Id == id);

                //Set new value chapter
                _chapter.IdSubject = subjectId;
                _chapter.ChapterName = chapterName;
                _chapter.Meta = chapterMeta;

                //Save data
                _context.SaveChanges();

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

        //Delete Chapter
        [HttpPost]
        public JsonResult DeleteChapter(int id)
        {
            try
            {
                //Find chapter by id
                var _chapter = _context.Chapters.SingleOrDefault(x => x.Id == id);
                _chapter.IsDelete = 1;

                //Save data
                _context.SaveChanges();

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

