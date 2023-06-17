using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*[Authorize]*/
    public class TeacherController : Controller
    {
        private readonly MultipleChoiceContext _context;

        public TeacherController(MultipleChoiceContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get List Teacher
        [HttpGet]
        public JsonResult GetListTeacher(string keyword, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listTeacherFromDB = (from _teacher in _context.Teachers.Where(x => x.IsDelete != 1)
                                         select new
                                         {
                                             Id = _teacher.Id,
                                             TeacherId = _teacher.IdTeacher,
                                             TeacherName = _teacher.TeacherName,
                                             Phone = _teacher.Phone,
                                             Email = _teacher.Email,
                                             Address = _teacher.Address,
                                             IsLeader = _teacher.IsLeader,
                                         }).ToList();

                if (keyword != null)
                {
                    listTeacherFromDB = listTeacherFromDB.Where(x => x.TeacherName.ToLower().Contains(keyword.ToLower())
                    || x.Phone.ToLower().Contains(keyword.ToLower())
                    || x.Email.ToLower().Contains(keyword.ToLower())
                    || x.Address.ToLower().Contains(keyword.ToLower())
                    || x.TeacherId.ToLower().Contains(keyword.ToLower()))
                        .ToList();
                }

                var pageSize = listTeacherFromDB.Count % settingsPages == 0 ? listTeacherFromDB.Count / settingsPages : listTeacherFromDB.Count / settingsPages + 1;
                var listTeacher = listTeacherFromDB.Skip((page - 1) * settingsPages)
                                    .Take(settingsPages)
                                    .ToList();
                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách giáo viên thành công!",
                    pageSize,
                    data = listTeacher
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách giáo viên thất bại: " + ex.Message
                });
            }

        }

        //Get All Teacher
        [HttpGet]
        public JsonResult GetAllTeacher()
        {
            try
            {
                var listTeacher = _context.Teachers.Where(x => x.IsDelete != 1).ToList();
                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách giáo viên thành công!",
                    data = listTeacher
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách giáo viên thất bại: " + ex.Message
                });
            }
        }

        //Get Detail
        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            try
            {
                var _teacher = _context.Teachers.SingleOrDefault(x => x.Id == id);

                return Json(new
                {
                    code = 200,
                    message = "Lấy thông tin chi tiết giáo viên thành công!",
                    data = _teacher
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy thông tin chi tiết giáo viên thất bại: " + ex.Message
                });
            }
        }

        //Add Teacher
        [HttpPost]
        public JsonResult AddTeacher(string teacherId, string password, string teacherName, string phone, string email, string address, byte isLeader)
        {
            try
            {
                var _checkTeacher = _context.Teachers.FirstOrDefault(x => x.IdTeacher == teacherId || x.Email == email || x.Phone == phone);
                if (_checkTeacher != null)
                {
                    if (_checkTeacher.IdTeacher == teacherId)
                    {
                        return Json(new
                        {
                            code = 400,
                            message = "Mã giáo viên đã được sử dụng!"
                        });
                    }
                    else if (_checkTeacher.Email == email)
                    {
                        return Json(new
                        {
                            code = 400,
                            message = "Email đã được sử dụng!"
                        });
                    }
                    else if (_checkTeacher.Phone == phone)
                    {
                        return Json(new
                        {
                            code = 400,
                            message = "Số điện thoại đã được sử dụng!"
                        });
                    }
                }

                var _teacher = new Teacher();

                _teacher.IdTeacher = teacherId;
                _teacher.Password = password;
                _teacher.TeacherName = teacherName;
                _teacher.Phone = phone;
                _teacher.Email = email;
                _teacher.Address = address;
                _teacher.IsLeader = isLeader;

                _context.Teachers.Add(_teacher);
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Thêm giáo viên thành công!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Thêm giáo viên thất bại: " + ex.Message
                });
            }
        }

        //Update Teacher
        [HttpPost]
        public JsonResult UpdateTeacher(int id, string teacherId, string password, string teacherName, string phone, string email, string address, byte isLeader)
        {
            try
            {
                var _checkTeacher = _context.Teachers.FirstOrDefault(x => (x.IdTeacher == teacherId || x.Email == email || x.Phone == phone) && x.Id != id);

                if (_checkTeacher != null)
                {
                    if (_checkTeacher.IdTeacher == teacherId)
                    {
                        return Json(new
                        {
                            code = 400,
                            message = "Mã giáo viên đã được sử dụng!"
                        });
                    }
                    else if (_checkTeacher.Email == email)
                    {
                        return Json(new
                        {
                            code = 400,
                            message = "Email đã được sử dụng!"
                        });
                    }
                    else if (_checkTeacher.Phone == phone)
                    {
                        return Json(new
                        {
                            code = 400,
                            message = "Số điện thoại đã được sử dụng!"
                        });
                    }
                }

                //Find teacher by id
                var _teacher = _context.Teachers.SingleOrDefault(x => x.Id == id);

                //Set new value teacher
                _teacher.IdTeacher = teacherId;
                _teacher.Password = password;
                _teacher.TeacherName = teacherName;
                _teacher.Phone = phone;
                _teacher.Email = email;
                _teacher.Address = address;
                _teacher.IsLeader = isLeader;

                //Save data
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Cập nhật thông tin giáo viên thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Cập nhật thông tin giáo viên thất bại: " + ex.Message
                });
            }
        }

        //Delete Teacher
        [HttpPost]
        public JsonResult DeleteTeacher(int id)
        {
            try
            {
                //Find teacher by id
                var _teacher = _context.Teachers.SingleOrDefault(x => x.Id == id);
                _teacher.IsDelete = 1;

                //Save data
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Xóa giáo viên thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Xóa giáo viên thất bại: " + ex.Message
                });
            }
        }
    }
}
