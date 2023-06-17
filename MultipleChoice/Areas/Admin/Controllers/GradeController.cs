using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*[Authorize]*/
    public class GradeController : Controller
    {
        private readonly MultipleChoiceContext _context;

        public GradeController(MultipleChoiceContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get List Grade
        [HttpGet]
        public JsonResult GetListGrade(string keyword, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listGradeFromDB = (from _grade in _context.Grades.Where(x => x.IsDelete != 1)
                                       select new
                                       {
                                           Id = _grade.Id,
                                           GradeName = _grade.GradeName,
                                           Meta = _grade.Meta
                                       }).ToList();

                if (keyword != null)
                {
                    listGradeFromDB = listGradeFromDB.Where(x => x.GradeName.ToLower().Contains(keyword.ToLower())).ToList();
                }

                var pageSize = listGradeFromDB.Count % settingsPages == 0 ? listGradeFromDB.Count / settingsPages : listGradeFromDB.Count / settingsPages + 1;
                var listClasses = listGradeFromDB.Skip((page - 1) * settingsPages)
                                    .Take(settingsPages)
                                    .ToList();
                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách khối thành công!",
                    pageSize,
                    data = listClasses
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

        //Get Detail
        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            try
            {
                var _grade = _context.Grades.SingleOrDefault(x => x.Id == id);

                return Json(new
                {
                    code = 200,
                    message = "Lấy thông tin chi tiết của khối thành công!",
                    data = _grade
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy thông tin chi tiết của khối thất bại: " + ex.Message
                });
            }
        }

        //Add Grade
        [HttpPost]
        public JsonResult AddGrade(string gradeName, string gradeMeta)
        {
            try
            {
                var _grade = new Grade();
                _grade.GradeName = gradeName;
                _grade.Meta = gradeMeta;

                _context.Grades.Add(_grade);
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Thêm khối thành công!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Thêm khối thất bại: " + ex.Message
                });
            }
        }

        //Update Grade
        [HttpPost]
        public JsonResult UpdateGrade(int id, string gradeName, string gradeMeta)
        {
            try
            {
                //Find grade by id
                var _grade = _context.Grades.SingleOrDefault(x => x.Id == id);

                //Set new value class
                _grade.GradeName = gradeName;
                _grade.Meta = gradeMeta;

                //Save data
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Cập nhật khối thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Cập nhật khối thất bại: " + ex.Message
                });
            }
        }

        //Delete Grade
        [HttpPost]
        public JsonResult DeleteGrade(int id)
        {
            try
            {
                //Find class by id
                var _grade = _context.Grades.SingleOrDefault(x => x.Id == id);
                _grade.IsDelete = 1;

                //Save data
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Xóa khối thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Xóa khối thất bại: " + ex.Message
                });
            }
        }
    }

}
