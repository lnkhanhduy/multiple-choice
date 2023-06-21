using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;

namespace MultipleChoice.Areas.TeacherArea.Controllers
{
    [Area("TeacherArea")]
    [Authorize]
    public class ExamController : Controller
    {
        private readonly MultipleChoiceContext _context;

        private int GetNumber(string name)
        {
            string numberString = new string(name.Where(char.IsDigit).ToArray());
            return int.Parse(numberString);
        }

        public ExamController(MultipleChoiceContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get List Subject
        [HttpGet]
        public JsonResult GetListSubject()
        {
            try
            {
                var _idTeacher = HttpContext.Session.GetInt32("teacher");

                var listSubject = (from _subject in _context.Subjects
                                   join _teaching in _context.Teachings on _subject.Id equals _teaching.IdSubject
                                   where (_subject.IsDelete != 1 && _teaching.IdTeacher == _idTeacher && _teaching.StartingDate <= DateTime.Now && _teaching.EndingDate >= DateTime.Now)
                                   select new
                                   {
                                       Id = _subject.Id,
                                       SubjectName = _subject.SubjectName,
                                   }).OrderBy(x => x.SubjectName).ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách khối thành công!",
                    data = listSubject
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

        //Get List Duration
        [HttpGet]
        public JsonResult GetListDuration()
        {
            try
            {
                var listDuration = _context.ExamDurations.ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách thời lượng đề thi thành công!",
                    data = listDuration
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách thời lượng đề thi thất bại: " + ex.Message
                });
            }
        }

        //Get list exm
        [HttpGet]
        public JsonResult GetListExam(int subjectId, string keyword, int page)
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
                var listExamFromDB = (from _exam in _context.Exams.Where(x => x.IsDelete != 1 && x.IdSubject == subjectId).OrderBy(x => x.IsApprove).ThenByDescending(x => x.Id)
                                      join _examDuration in _context.ExamDurations on _exam.IdDuration equals _examDuration.Id
                                      join _teacher in _context.Teachers on _exam.Author equals _teacher.Id
                                      select new
                                      {
                                          Id = _exam.Id,
                                          DurationName = _examDuration.DurationName,
                                          DurationTime = _examDuration.DurationTime,
                                          ExamDate = _exam.ExamDate,
                                          Author = _teacher.TeacherName,
                                          IsApprove = _exam.IsApprove,
                                      }
                                      ).ToList();

                if (keyword != null)
                {
                    listExamFromDB = listExamFromDB.Where(
                        x => x.DurationName.ToLower().Contains(keyword.ToLower())
                        || x.DurationTime.ToString().Contains(keyword)
                        || x.Author.ToLower().Contains(keyword.ToLower())).ToList();
                }

                var pageSize = listExamFromDB.Count % settingsPages == 0 ? listExamFromDB.Count / settingsPages : listExamFromDB.Count / settingsPages + 1;
                var listExam = listExamFromDB.Skip((page - 1) * settingsPages)
                                   .Take(settingsPages)
                                   .ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách đề thi thành công!",
                    IsLeader = _isLeader,
                    pageSize,
                    data = listExam
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách đề thi thất bại: " + ex.Message
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

        //Get List Question By Lesson
        [HttpGet]
        public JsonResult GetListQuestion(int levelId, int lessonId)
        {
            try
            {
                var listQuestion = (from _question in _context.Questions.Where(x => x.IsDelete != 1 && x.IsApprove == 1 && x.IdLesson == lessonId && x.IdLevel == levelId)
                                    select new
                                    {
                                        Id = _question.Id,
                                        QuestionContent = _question.QuestionContent,
                                    }).OrderBy(x => x.QuestionContent).ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách đề thi thành công!",
                    data = listQuestion
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách đề thi thất bại: " + ex.Message
                });
            }

        }

        //Get List Question By Lesson
        [HttpGet]
        public JsonResult GetListLevel()
        {
            try
            {
                var listLevel = _context.Levels.Where(x => x.IsDelete != 1);

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách mức đọ thành công!",
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

        //Get List Question In Exam
        [HttpGet]
        public JsonResult GetListQuestionByExam(int examId)
        {
            try
            {
                var _isApprove = _context.Exams.SingleOrDefault(x => x.Id == examId).IsApprove == 1 ? true : false;
                var listQuestion = (from _questionExam in _context.ExamsQuestions.Where(x => x.IdExam == examId && x.IsDelete != 1).OrderByDescending(x => x.Id)
                                    join _question in _context.Questions on _questionExam.IdQuestion equals _question.Id
                                    join _answer in _context.Answers on _question.IdAnswer equals _answer.Id
                                    join _level in _context.Levels on _question.IdLevel equals _level.Id
                                    select new
                                    {
                                        Id = _questionExam.Id,
                                        QuestionContent = _question.QuestionContent,
                                        AnswerA = _question.AnswerA,
                                        AnswerB = _question.AnswerB,
                                        AnswerC = _question.AnswerC,
                                        AnswerD = _question.AnswerD,
                                        Answer = _answer.AnswerLabel,
                                        Level = _level.LevelName,
                                    }).ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách câu hỏi trong đề thi thành công!",
                    IsApprove = _isApprove,
                    data = listQuestion
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách câu hỏi trong đề thi thất bại: " + ex.Message
                });
            }

        }

        //Add Exam
        [HttpPost]
        public JsonResult AddExam(DateTime examDate, int subjectId, int durationId)
        {
            try
            {
                var _idTeacher = HttpContext.Session.GetInt32("teacher");

                var _exam = new Exam();
                _exam.ExamDate = examDate;
                _exam.IdSubject = subjectId;
                _exam.IdDuration = durationId;
                _exam.Author = _idTeacher;

                _context.Exams.Add(_exam);
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Tạo đề thi thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Tạo đề thi thất bại: " + ex.Message
                });
            }
        }

        //Approve Exam
        [HttpPost]
        public JsonResult ApproveExam(int[] listExam)
        {
            try
            {
                foreach (var id in listExam)
                {

                    var _exam = _context.Exams.SingleOrDefault(x => x.Id == id);
                    if (_exam != null)
                    {
                        _exam.IsApprove = 1;
                    }

                    _context.SaveChanges();
                }

                return Json(new
                {
                    code = 200,
                    message = "Phê duyệt đề thi thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Phê duyệt đề thi thất bại: " + ex.Message
                });
            }
        }

        //Add Question In Exam
        [HttpPost]
        public JsonResult AddQuestion(int examId, int questionId)
        {
            try
            {
                var _question = _context.ExamsQuestions.FirstOrDefault(x => x.IdExam == examId && x.IdQuestion == questionId);
                if (_question != null)
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Câu hỏi đã có trong đề thi!",
                    });
                }

                var _examQuestion = new ExamsQuestion();

                _examQuestion.IdQuestion = questionId;
                _examQuestion.IdExam = examId;

                _context.ExamsQuestions.Add(_examQuestion);
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Thêm câu hỏi vào đề thi thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Thêm câu hỏi vào đề thi thất bại: " + ex.Message
                });
            }
        }

        //Delete Question In Exam
        [HttpPost]
        public JsonResult DeleteQuestion(int id)
        {
            try
            {
                //Find question by id
                var _question = _context.ExamsQuestions.SingleOrDefault(x => x.Id == id);

                _question.IsDelete = 1;

                //Save data
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Xóa câu hỏi khỏi đề thi thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Xóa câu hỏi khỏi đề thi thất bại: " + ex.Message
                });
            }
        }
    }
}
