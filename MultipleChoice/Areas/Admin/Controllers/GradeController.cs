using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class GradeController : Controller
    {
        private readonly MultipleChoiceContext _context;

        private int GetNumber(string name)
        {
            string numberString = new string(name.Where(char.IsDigit).ToArray());
            return int.Parse(numberString);
        }

        public GradeController(MultipleChoiceContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get list grade
        [HttpGet]
        public JsonResult GetListGrade(string keyword, int page)
        {
            try
            {
                var settingsPage = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);

                var listGradeFromDB = _context.Grades.Where(x => x.IsDelete != 1).AsEnumerable()
                                      .OrderBy(x => GetNumber(x.GradeName)).ToList();

                if (keyword != null)
                {
                    listGradeFromDB = listGradeFromDB.Where(x => x.GradeName.ToLower().Contains(keyword.ToLower())).ToList();
                }

                var pageSize = listGradeFromDB.Count % settingsPage == 0 ? listGradeFromDB.Count / settingsPage : listGradeFromDB.Count / settingsPage + 1;
                var listGrade = listGradeFromDB.Skip((page - 1) * settingsPage)
                                    .Take(settingsPage)
                                    .ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách khối thành công!",
                    pageSize,
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

        //Get detail grade
        [HttpGet]
        public JsonResult GetDetailGrade(int id)
        {
            try
            {
                var _grade = _context.Grades.SingleOrDefault(x => x.Id == id);

                return Json(new
                {
                    code = 200,
                    message = "Lấy thông tin chi tiết khối thành công!",
                    data = _grade
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy thông tin chi tiết khối thất bại: " + ex.Message
                });
            }
        }

        //Add grade
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

        //Update grade
        [HttpPost]
        public JsonResult UpdateGrade(int id, string gradeName, string gradeMeta)
        {
            try
            {
                var _grade = _context.Grades.SingleOrDefault(x => x.Id == id);

                if (_grade != null)
                {
                    _grade.GradeName = gradeName;
                    _grade.Meta = gradeMeta;

                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy khối!",
                    });
                }

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

        //Delete grade
        [HttpPost]
        public JsonResult DeleteGrade(int id)
        {
            try
            {
                var _grade = _context.Grades.SingleOrDefault(x => x.Id == id);

                if (_grade != null)
                {
                    _grade.IsDelete = 1;
                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy khối!",
                    });
                }

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
