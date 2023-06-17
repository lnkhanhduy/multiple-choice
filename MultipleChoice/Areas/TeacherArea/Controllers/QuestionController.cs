using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MultipleChoice.Areas.TeacherArea.Controllers
{
    [Area("TeacherArea")]
    //[Authorize]
    public class QuestionController : Controller
    {
        private readonly MultipleChoiceContext _context;

        private int GetNumber(string name)
        {
            string numberString = new string(name.Where(char.IsDigit).ToArray());
            return int.Parse(numberString);
        }

        public QuestionController(MultipleChoiceContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get All Grade
        [HttpGet]
        public JsonResult GetAllGrade()
        {
            try
            {
                var listGrade = _context.Grades.Where(x => x.IsDelete != 1).ToList();
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

        //Get List Subject By Grade
        [HttpGet]
        public JsonResult GetListSubjectByGrade(int id)
        {
            try
            {
                var listSubject = (from _subject in _context.Subjects.Where(x => x.IdGrade == id && x.IsDelete != 1)
                                   select new
                                   {
                                       Id = _subject.Id,
                                       SubjectName = _subject.SubjectName,
                                   }).AsEnumerable()
                          .OrderBy(x => GetNumber(x.SubjectName)).ToList();

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

        //Get List Chapter By Subject
        [HttpGet]
        public JsonResult GetListChapterBySubject(int id)
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

        //Get List Lesson By Chapter
        [HttpGet]
        public JsonResult GetListLessonByChapter(int id)
        {
            try
            {
                var listLesson = (from _lesson in _context.Lessons.Where(x => x.IdChapter == id && x.IsDelete != 1)
                                  join _chapter in _context.Chapters on _lesson.IdChapter equals _chapter.Id
                                  select new
                                  {
                                      Id = _lesson.Id,
                                      LessonName = _lesson.LessonName,
                                  }).AsEnumerable()
                                    .OrderBy(x => GetNumber(x.LessonName)).ToList();

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

        //Get List Level
        [HttpGet]
        public JsonResult GetListLevel()
        {
            try
            {
                var listLevel = _context.Levels.Where(x => x.IsDelete != 1).ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách mức độ thành công!",
                    data = listLevel
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách mức độ thất bại: " + ex.Message
                });
            }
        }

        //Get List Answer
        [HttpGet]
        public JsonResult GetListAnswer()
        {
            try
            {
                var listAnswer = _context.Answers.Where(x => x.IsDelete != 1).OrderBy(x => x.AnswerLabel).ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách đáp án thành công!",
                    data = listAnswer
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách đáp án thất bại: " + ex.Message
                });
            }
        }

        //Get List Question
        [HttpGet]
        public JsonResult GetListQuestion(string keyword, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listQuestionFromDB = _context.Questions.Where(x => x.IsDelete != 1).OrderByDescending(x => x.Id).ToList();

                if (keyword != null)
                {
                    listQuestionFromDB = listQuestionFromDB.Where(
                        x => x.QuestionContent.ToLower().Contains(keyword.ToLower())
                        || x.AnswerA.ToLower().Contains(keyword.ToLower())
                        || x.AnswerB.ToLower().Contains(keyword.ToLower())
                        || x.AnswerC.ToLower().Contains(keyword.ToLower())
                        || x.AnswerD.ToLower().Contains(keyword.ToLower())
                        || (x.Note != null && x.Note.ToLower().Contains(keyword.ToLower()))).ToList();
                }

                var pageSize = listQuestionFromDB.Count % settingsPages == 0 ? listQuestionFromDB.Count / settingsPages : listQuestionFromDB.Count / settingsPages + 1;
                var listQuestion = listQuestionFromDB.Skip((page - 1) * settingsPages)
                                   .Take(settingsPages)
                                   .ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách môn học thành công!",
                    pageSize,
                    data = listQuestion
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

        //Get List Subject
        [HttpGet]
        public JsonResult GetListQuestionBySubject(int id, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listQuestionFromDB = (from _question in _context.Questions.OrderByDescending(x => x.Id)
                                          join _lesson in _context.Lessons on _question.IdLesson equals _lesson.Id
                                          join _chapter in _context.Chapters on _lesson.IdChapter equals _chapter.Id
                                          join _subject in _context.Subjects on _chapter.IdSubject equals _subject.Id
                                          where (_question.IsDelete != 1 && _subject.Id == id)
                                          select new
                                          {
                                              Id = _question.Id,
                                              QuestionContent = _question.QuestionContent,
                                          }).ToList();

                var pageSize = listQuestionFromDB.Count % settingsPages == 0 ? listQuestionFromDB.Count / settingsPages : listQuestionFromDB.Count / settingsPages + 1;
                var listQuestion = listQuestionFromDB.Skip((page - 1) * settingsPages)
                                    .Take(settingsPages).ToList();
                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách câu hỏi thành công!",
                    pageSize,
                    data = listQuestion
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách câu hỏi thất bại: " + ex.Message
                });
            }
        }

        //Get Detail
        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            try
            {
                var _question = from _questionDB in _context.Questions.Where(x => x.Id == id)
                                join _lesson in _context.Lessons on _questionDB.IdLesson equals _lesson.Id
                                join _chapter in _context.Chapters on _lesson.IdChapter equals _chapter.Id
                                join _subject in _context.Subjects on _chapter.IdSubject equals _subject.Id
                                join _grade in _context.Grades on _subject.IdGrade equals _grade.Id
                                select new
                                {
                                    Id = _questionDB.Id,
                                    LessonId = _questionDB.IdLesson,
                                    ChapterId = _chapter.Id,
                                    SubjectId = _subject.Id,
                                    GradeId = _grade.Id,
                                    QuestionContent = _questionDB.QuestionContent,
                                    AnswerA = _questionDB.AnswerA,
                                    AnswerB = _questionDB.AnswerB,
                                    AnswerC = _questionDB.AnswerC,
                                    AnswerD = _questionDB.AnswerD,
                                    Answer = _questionDB.IdAnswer,
                                    Level = _questionDB.IdLevel,
                                    Note = _questionDB.Note
                                };

                return Json(new
                {
                    code = 200,
                    message = "Lấy thông tin chi tiết câu hỏi thành công!",
                    data = _question
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy thông tin chi tiết câu hỏi thất bại: " + ex.Message
                });
            }
        }

        //Add Question
        [HttpPost]
        public JsonResult AddQuestion(string questionContent, string answerA, string answerB, string answerC, string answerD, int answer, int lesson, int level, string note)
        {
            try
            {
                var _idTeacher = HttpContext.Session.GetInt32("teacher");

                var _question = new Question();
                _question.QuestionContent = questionContent;
                _question.AnswerA = answerA;
                _question.AnswerB = answerB;
                _question.AnswerC = answerC;
                _question.AnswerD = answerD;
                _question.IdAnswer = answer;
                _question.IdLesson = lesson;
                _question.IdLevel = level;
                _question.Note = note;
                _question.Author = _idTeacher;
                _question.CreationDate = DateTime.Now;

                _context.Questions.Add(_question);
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Thêm câu hỏi thành công!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Thêm câu hỏi thất bại: " + ex.Message
                });
            }
        }

        //Update Question
        [HttpPost]
        public JsonResult UpdateQuestion(int id, string questionContent, string answerA, string answerB, string answerC, string answerD, int answer, int lesson, int level, string note)
        {
            try
            {
                var _idTeacher = HttpContext.Session.GetInt32("teacher");

                //Find question by id
                var _question = _context.Questions.SingleOrDefault(x => x.Id == id);

                //Set new value question
                _question.QuestionContent = questionContent;
                _question.AnswerA = answerA;
                _question.AnswerB = answerB;
                _question.AnswerC = answerC;
                _question.AnswerD = answerD;
                _question.IdAnswer = answer;
                _question.IdLesson = lesson;
                _question.IdLevel = level;
                _question.Note = note;
                _question.Editor = _idTeacher;
                _question.EditingDate = DateTime.Now;

                //Save data
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Cập nhật câu hỏi thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Cập nhật câu hỏi thất bại: " + ex.Message
                });
            }
        }

        //Delete Question
        [HttpPost]
        public JsonResult DeleteQuestion(int id)
        {
            try
            {
                var _idTeacher = HttpContext.Session.GetInt32("teacher");

                //Find question by id
                var _question = _context.Questions.SingleOrDefault(x => x.Id == id);
                _question.Eraser = _idTeacher;
                _question.DeletionDate = DateTime.Now;
                _question.IsDelete = 1;

                //Save data
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Xóa câu hỏi thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Xóa câu hỏi thất bại: " + ex.Message
                });
            }
        }
    }
}
