using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;
using System.Net;
using Microsoft.CodeAnalysis.Scripting;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize]
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

        //Get list subject
        [HttpGet]
        public JsonResult GetListSubject()
        {
            try
            {
                var listSubject = (from _subject in _context.Subjects.Where(x => x.IsDelete != 1).OrderBy(x => x.SubjectName)
                                   select new
                                   {
                                       Id = _subject.Id,
                                       SubjectName = _subject.SubjectName,
                                   }).ToList();

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

        //Get list teacher
        [HttpGet]
        public JsonResult GetListTeacher(int subjectId, string keyword, int page)
        {
            try
            {
                var settingsPage = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listTeacherFromDB = (from _teacher in _context.Teachers.Where(x => x.IsDelete != 1 && x.IdSubject == subjectId).OrderBy(x => x.TeacherName)
                                         join _subject in _context.Subjects on _teacher.IdSubject equals _subject.Id
                                         select new
                                         {
                                             Id = _teacher.Id,
                                             TeacherId = _teacher.IdTeacher,
                                             TeacherName = _teacher.TeacherName,
                                             Phone = _teacher.Phone,
                                             Email = _teacher.Email,
                                             Address = _teacher.Address,
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

                var pageSize = listTeacherFromDB.Count % settingsPage == 0 ? listTeacherFromDB.Count / settingsPage : listTeacherFromDB.Count / settingsPage + 1;
                var listTeacher = listTeacherFromDB.Skip((page - 1) * settingsPage)
                                    .Take(settingsPage)
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

        //Get detail teacher
        [HttpGet]
        public JsonResult GetDetailTeacher(int id)
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

        //Add teacher
        [HttpPost]
        public JsonResult AddTeacher(int subjectId, string teacherId, string password, string teacherName, string phone, string email, string address)
        {
            try
            {
                var _checkTeacher = _context.Teachers.FirstOrDefault(x => x.IsDelete != 1 && (x.IdTeacher == teacherId || x.Email == email || x.Phone == phone));

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

                _teacher.IdSubject = subjectId;
                _teacher.IdTeacher = teacherId;
                _teacher.Password = BCrypt.Net.BCrypt.HashPassword(password);
                _teacher.TeacherName = teacherName;
                _teacher.Phone = phone;
                _teacher.Email = email;
                _teacher.Address = address;

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

        //Update teacher
        [HttpPost]
        public JsonResult UpdateTeacher(int id, int subjectId, string teacherId, string password, string teacherName, string phone, string email, string address, byte isLeader)
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

                var _teacher = _context.Teachers.SingleOrDefault(x => x.Id == id);

                if (_teacher != null)
                {
                    _teacher.IdSubject = subjectId;
                    _teacher.IdTeacher = teacherId;
                    _teacher.Password = BCrypt.Net.BCrypt.HashPassword(password);
                    _teacher.TeacherName = teacherName;
                    _teacher.Phone = phone;
                    _teacher.Email = email;
                    _teacher.Address = address;

                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy giáo viên!",
                    });
                }

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

        //Delete teacher
        [HttpPost]
        public JsonResult DeleteTeacher(int id)
        {
            try
            {
                var _teacher = _context.Teachers.SingleOrDefault(x => x.Id == id);

                if (_teacher != null)
                {
                    _teacher.IsDelete = 1;

                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy giáo viên!",
                    });
                }

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
