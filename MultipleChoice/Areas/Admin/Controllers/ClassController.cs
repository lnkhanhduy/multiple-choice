using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultipleChoice.Models;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ClassController : Controller
    {
        private readonly MultipleChoiceContext _context;

        private int GetNumber(string name)
        {
            string numberString = new string(name.Where(char.IsDigit).ToArray());
            return int.Parse(numberString);
        }

        public ClassController(MultipleChoiceContext context)
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
                var listGrade = (from _grade in _context.Grades.Where(x => x.IsDelete != 1).AsEnumerable().OrderBy(x => GetNumber(x.GradeName))
                                 select new
                                 {
                                     Id = _grade.Id,
                                     GradeName = _grade.GradeName,
                                 }).ToList();

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

        //Get list class
        [HttpGet]
        public JsonResult GetListClass(int gradeId, string keyword, int page)
        {
            try
            {
                var settingsPage = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);

                var listClassFromDB = (from _class in _context.Classes.Where(x => x.IsDelete != 1 && x.IdGrade == gradeId)
                                       join _grade in _context.Grades on _class.IdGrade equals _grade.Id
                                       select new
                                       {
                                           Id = _class.Id,
                                           ClassName = _class.ClassName,
                                           GradeName = _grade.GradeName,
                                           Meta = _class.Meta
                                       }).AsEnumerable()
                                       .OrderBy(x => GetNumber(x.ClassName)).ToList();

                if (keyword != null)
                {
                    listClassFromDB = listClassFromDB.Where(x => x.ClassName.ToLower().Contains(keyword.ToLower()) || x.Meta.ToLower().Contains(keyword.ToLower()) || x.GradeName.ToLower().Contains(keyword.ToLower())).ToList();
                }

                var pageSize = listClassFromDB.Count % settingsPage == 0 ? listClassFromDB.Count / settingsPage : listClassFromDB.Count / settingsPage + 1;
                var listClass = listClassFromDB.Skip((page - 1) * settingsPage)
                                    .Take(settingsPage)
                                    .ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách lớp thành công!",
                    pageSize,
                    data = listClass
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách lớp thất bại: " + ex.Message
                });
            }
        }

        //Get detail class
        [HttpGet]
        public JsonResult GetDetailClass(int id)
        {
            try
            {
                var _class = _context.Classes.SingleOrDefault(x => x.Id == id);

                return Json(new
                {
                    code = 200,
                    message = "Lấy thông tin chi tiết lớp thành công!",
                    data = _class
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy thông tin chi tiết lớp thất bại: " + ex.Message
                });
            }
        }

        //Add class
        [HttpPost]
        public JsonResult AddClass(int gradeId, string className, string classMeta)
        {
            try
            {
                var _class = new Class();
                _class.IdGrade = gradeId;
                _class.ClassName = className;
                _class.Meta = classMeta;

                _context.Classes.Add(_class);
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Thêm lớp thành công!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Thêm lớp thất bại: " + ex.Message
                });
            }
        }

        //Update class
        [HttpPost]
        public JsonResult UpdateClass(int id, int gradeId, string className, string classMeta)
        {
            try
            {
                var _class = _context.Classes.SingleOrDefault(x => x.Id == id);

                if (_class != null)
                {
                    _class.IdGrade = gradeId;
                    _class.ClassName = className;
                    _class.Meta = classMeta;

                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy lớp!",
                    });
                }

                return Json(new
                {
                    code = 200,
                    message = "Cập nhật lớp thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Cập nhật lớp thất bại: " + ex.Message
                });
            }
        }

        //Delete class
        [HttpPost]
        public JsonResult DeleteClass(int id)
        {
            try
            {
                var _class = _context.Classes.SingleOrDefault(x => x.Id == id);

                if (_class != null)
                {
                    _class.IsDelete = 1;
                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy lớp!",
                    });
                }

                return Json(new
                {
                    code = 200,
                    message = "Xóa lớp thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Xóa lớp thất bại: " + ex.Message
                });
            }
        }
    }
}
