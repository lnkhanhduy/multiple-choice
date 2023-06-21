using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MultipleChoice.Areas.TeacherArea.Controllers
{
    [Area("TeacherArea")]
    [Authorize]
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

        //Get list grade
        [HttpGet]
        public JsonResult GetListGrade()
        {
            try
            {
                var _idTeacher = HttpContext.Session.GetInt32("teacher");
                var listGrade = (from _teaching in _context.Teachings.Where(x => x.IdTeacher == _idTeacher && x.StartingDate <= DateTime.Now && x.EndingDate >= DateTime.Now)
                                 join _subject in _context.Subjects.Where(x => x.IsDelete != 1) on _teaching.IdSubject equals _subject.Id
                                 join _grade in _context.Grades.Where(x => x.IsDelete != 1) on _subject.IdGrade equals _grade.Id
                                 select new
                                 {
                                     Id = _grade.Id,
                                     GradeName = _grade.GradeName,
                                 }).AsEnumerable().OrderBy(x => GetNumber(x.GradeName)).ToList();

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
        public JsonResult GetListSubject(int id)
        {
            try
            {
                var _idTeacher = HttpContext.Session.GetInt32("teacher");
                var listSubject = (from _teaching in _context.Teachings.Where(x => x.IdTeacher == _idTeacher && x.StartingDate <= DateTime.Now && x.EndingDate >= DateTime.Now)
                                   join _subject in _context.Subjects.Where(x => x.IdGrade == id && x.IsDelete != 1) on _teaching.IdSubject equals _subject.Id
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
        public JsonResult GetListChapter(int id)
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
        public JsonResult GetListLesson(int id)
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
        public JsonResult GetListQuestion(int subjectId, string keyword, int page)
        {
            try
            {
                var _idTeacher = HttpContext.Session.GetInt32("teacher");
                var _checkLeader = _context.Subjects.SingleOrDefault(x => x.Id == subjectId);
                var _isLeader = false;
                if (_checkLeader != null && _checkLeader.IdLeader == _idTeacher)
                {
                    _isLeader = true;
                }

                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listQuestionFromDB = (from _question in _context.Questions.Where(x => x.IsDelete != 1).OrderByDescending(x => x.Id)
                                          join _teacher in _context.Teachers on _question.Author equals _teacher.Id
                                          join _editor in _context.Teachers on _question.Editor equals _editor.Id into editorJoin
                                          from _editor in editorJoin.DefaultIfEmpty()
                                          join _approver in _context.Teachers on _question.Approver equals _approver.Id into approverJoin
                                          from _approver in approverJoin.DefaultIfEmpty()
                                          join _lesson in _context.Lessons on _question.IdLesson equals _lesson.Id
                                          join _chapter in _context.Chapters on _lesson.IdChapter equals _chapter.Id
                                          join _subject in _context.Subjects on _chapter.IdSubject equals _subject.Id
                                          where (_subject.Id == subjectId)
                                          select new
                                          {
                                              Id = _question.Id,
                                              QuestionContent = _question.QuestionContent,
                                              Author = _teacher.TeacherName,
                                              CreationDate = _question.CreationDate,
                                              Editor = _editor.TeacherName,
                                              EditingDate = _question.EditingDate,
                                              IsApprove = _question.IsApprove,
                                              Approver = _approver.TeacherName,
                                              AnswerA = _question.AnswerA,
                                              AnswerB = _question.AnswerB,
                                              AnswerC = _question.AnswerC,
                                              AnswerD = _question.AnswerD,
                                              Note = _question.Note,
                                          }).ToList();

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
                                    .Take(settingsPages).ToList();
                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách câu hỏi thành công!",
                    isLeader = _isLeader,
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

        //Get list question not appove
        [HttpGet]
        public JsonResult GetListQuestionNotAppoveBySubject(int id)
        {
            try
            {
                var listQuestion = (from _question in _context.Questions.Where(x => x.IsDelete != 1 && x.IsApprove != 1).OrderByDescending(x => x.Id)
                                    join _lesson in _context.Lessons on _question.IdLesson equals _lesson.Id
                                    join _chapter in _context.Chapters on _lesson.IdChapter equals _chapter.Id
                                    join _subject in _context.Subjects on _chapter.IdSubject equals _subject.Id
                                    join _answer in _context.Answers on _question.IdAnswer equals _answer.Id
                                    join _level in _context.Levels on _question.IdLevel equals _level.Id
                                    where (_subject.Id == id)
                                    select new
                                    {
                                        Id = _question.Id,
                                        QuestionContent = _question.QuestionContent,
                                        AnswerA = _question.AnswerA,
                                        AnswerB = _question.AnswerB,
                                        AnswerC = _question.AnswerC,
                                        AnswerD = _question.AnswerD,
                                        Answer = _answer.AnswerLabel,
                                        Level = _level.LevelName,
                                        Note = _question.Note,
                                    }).ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách câu hỏi thành công!",
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

        //Get detail question
        [HttpGet]
        public JsonResult GetDetailQuestion(int id)
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

        //Approve question
        [HttpPost]
        public JsonResult ApproveQuestion(int id)
        {
            try
            {
                var _idTeacher = HttpContext.Session.GetInt32("teacher");

                var _question = _context.Questions.SingleOrDefault(x => x.Id == id);
                _question.IsApprove = 1;
                _question.Approver = _idTeacher;

                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Phê duyệt câu hỏi thành công!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Phê duyệt câu hỏi thất bại: " + ex.Message
                });
            }
        }

        //Add question
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

        //Update question
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

        //Delete question
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
