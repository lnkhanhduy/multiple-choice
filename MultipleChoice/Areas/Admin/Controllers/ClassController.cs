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
    /*[Authorize]*/
    public class ClassController : Controller
    {
        private readonly MultipleChoiceContext _context;

        public ClassController(MultipleChoiceContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get List Class
        [HttpGet]
        public JsonResult GetListClass(string keyword, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listClassFromDB = (from _class in _context.Classes.Where(x => x.IsDelete != 1)
                                       join _grade in _context.Grades on _class.IdGrade equals _grade.Id
                                       select new
                                       {
                                           Id = _class.Id,
                                           ClassName = _class.ClassName,
                                           GradeName = _grade.GradeName,
                                           Meta = _class.Meta
                                       }).ToList().OrderBy(x => x.ClassName).ToList();

                if (keyword != null)
                {
                    listClassFromDB = listClassFromDB.Where(x => x.ClassName.ToLower().Contains(keyword.ToLower()) || x.Meta.ToLower().Contains(keyword.ToLower()) || x.GradeName.ToLower().Contains(keyword.ToLower())).ToList();
                }

                var pageSize = listClassFromDB.Count % settingsPages == 0 ? listClassFromDB.Count / settingsPages : listClassFromDB.Count / settingsPages + 1;
                var listClass = listClassFromDB.Skip((page - 1) * settingsPages)
                                    .Take(settingsPages)
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

        //Get All Class
        public JsonResult GetAllClass()
        {
            try
            {
                var listClass = _context.Classes.Where(x => x.IsDelete != 1).ToList();
                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách lớp thành công!",
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

        [HttpGet]
        public JsonResult GetListClassByGrade(int id)
        {
            try
            {
                var listClass = (from _class in _context.Classes.Where(x => x.IdGrade == id && x.IsDelete != 1)
                                 select new
                                 {
                                     Id = _class.Id,
                                     ClassName = _class.ClassName,
                                 }).ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách lớp thành công!",
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

        //Get Detail
        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            try
            {
                var _class = _context.Classes.SingleOrDefault(x => x.Id == id);

                return Json(new
                {
                    code = 200,
                    message = "Lấy thông tin chi tiết của lớp thành công!",
                    data = _class
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy thông tin chi tiết của lớp thất bại: " + ex.Message
                });
            }
        }

        //Add Class
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

        //Update Class
        [HttpPost]
        public JsonResult UpdateClass(int id, int gradeId, string className, string classMeta)
        {
            try
            {
                //Find class by id
                var _class = _context.Classes.SingleOrDefault(x => x.Id == id);

                //Set new value class
                _class.IdGrade = gradeId;
                _class.ClassName = className;
                _class.Meta = classMeta;

                //Save data
                _context.SaveChanges();

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

        //Delete Class
        [HttpPost]
        public JsonResult DeleteClass(int id)
        {
            try
            {
                //Find class by id
                var _class = _context.Classes.SingleOrDefault(x => x.Id == id);
                _class.IsDelete = 1;

                //Save data
                _context.SaveChanges();

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
