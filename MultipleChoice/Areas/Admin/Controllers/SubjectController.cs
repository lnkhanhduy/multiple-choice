﻿using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*[Authorize]*/
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

        //Get List Subject
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
                                         .ThenBy(x => GetNumber(x.SubjectName)).ToList();

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

        //Get All Subject
        [HttpGet]
        public JsonResult GetAllSubject()
        {
            try
            {
                var listSubject = _context.Subjects.Where(x => x.IsDelete != 1).ToList();
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

        //Get Detail
        [HttpGet]
        public JsonResult GetDetail(int id)
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

        //Add Subject
        [HttpPost]
        public JsonResult AddSubject(int gradeId, string subjectName, string subjectMeta)
        {
            try
            {
                var _subject = new Subject();
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

        //Update Subject
        [HttpPost]
        public JsonResult UpdateSubject(int id, int gradeId, string subjectName, string subjectMeta)
        {
            try
            {
                //Find subject by id
                var _subject = _context.Subjects.SingleOrDefault(x => x.Id == id);

                //Set new value subject
                _subject.IdGrade = gradeId;
                _subject.SubjectName = subjectName;
                _subject.Meta = subjectMeta;

                //Save data
                _context.SaveChanges();

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

        //Delete Class
        [HttpPost]
        public JsonResult DeleteSubject(int id)
        {
            try
            {
                //Find subject by id
                var _subject = _context.Subjects.SingleOrDefault(x => x.Id == id);
                _subject.IsDelete = 1;

                //Save data
                _context.SaveChanges();

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
