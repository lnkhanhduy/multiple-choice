﻿using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*[Authorize]*/
    public class StudentController : Controller
    {
        private readonly MultipleChoiceContext _context;

        public StudentController(MultipleChoiceContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get List Student
        [HttpGet]
        public JsonResult GetListStudent(string keyword, int page)
        {
            try
            {
                var settingsPages = int.Parse(_context.Settings.SingleOrDefault(x => x.Keyword == "LinesPerPage").Value);
                var listStudentFromDB = (from _student in _context.Students.Where(x => x.IsDelete != 1)
                                         select new
                                         {
                                             StudentId = _student.IdStudent,
                                             StudentName = _student.StudentName,
                                             Birthday = _student.Birthday,
                                             Phone = _student.Phone,
                                             Email = _student.Email,
                                             Address = _student.Address,
                                         }).ToList();

                if (keyword != null)
                {
                    listStudentFromDB = listStudentFromDB.Where(x => x.StudentName.ToLower().Contains(keyword.ToLower())
                    || x.Phone.ToLower().Contains(keyword.ToLower())
                    || x.Email.ToLower().Contains(keyword.ToLower())
                    || x.Address.ToLower().Contains(keyword.ToLower())
                    || x.StudentId.ToLower().Contains(keyword.ToLower()))
                        .ToList();
                }

                var pageSize = listStudentFromDB.Count % settingsPages == 0 ? listStudentFromDB.Count / settingsPages : listStudentFromDB.Count / settingsPages + 1;
                var listStudent = listStudentFromDB.Skip((page - 1) * settingsPages)
                                    .Take(settingsPages)
                                    .ToList();
                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách học sinh thành công!",
                    pageSize,
                    data = listStudent
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách học sinh thất bại: " + ex.Message
                });
            }

        }

        //Get All Student
        public JsonResult GetAllStudent()
        {
            try
            {
                var listStudent = _context.Students.Where(x => x.IsDelete != 1).ToList();
                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách học sinh thành công!",
                    data = listStudent
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách học sinh thất bại: " + ex.Message
                });
            }
        }

        //Get Detail
        [HttpGet]
        public JsonResult GetDetail(string id)
        {
            try
            {
                var _student = _context.Students.SingleOrDefault(x => x.IdStudent == id);

                return Json(new
                {
                    code = 200,
                    message = "Lấy thông tin chi tiết học sinh thành công!",
                    data = _student
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy thông tin chi tiết học sinh thất bại: " + ex.Message
                });
            }
        }

        //Add Student
        [HttpPost]
        public JsonResult AddStudent(string studentId, string password, string studentName, DateTime birthday, string phone, string email, string address)
        {
            try
            {
                var _checkStudent = _context.Students.FirstOrDefault(x => x.IdStudent == studentId);
                if (_checkStudent != null && _checkStudent.IdStudent == studentId)
                {
                    return Json(new
                    {
                        code = 400,
                        message = "Mã học sinh đã được sử dụng!"
                    });

                }

                var _student = new Student();
                _student.IdStudent = studentId;
                _student.Password = password;
                _student.StudentName = studentName;
                _student.Birthday = birthday;
                _student.Phone = phone;
                _student.Email = email;
                _student.Address = address;

                _context.Students.Add(_student);
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Thêm học sinh thành công!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Thêm học sinh thất bại: " + ex.Message
                });
            }
        }

        //Update Student
        [HttpPost]
        public JsonResult UpdateStudent(string studentId, string password, string studentName, DateTime birthday, string phone, string email, string address)
        {
            try
            {
                //Find student by id
                var _student = _context.Students.SingleOrDefault(x => x.IdStudent == studentId);

                //Set new value teacher
                _student.Password = password;
                _student.StudentName = studentName;
                _student.Birthday = birthday;
                _student.Phone = phone;
                _student.Email = email;
                _student.Address = address;

                //Save data
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Cập nhật thông tin học sinh thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Cập nhật thông tin học sinh thất bại: " + ex.Message
                });
            }
        }

        //Delete Student
        [HttpPost]
        public JsonResult DeleteStudent(string id)
        {
            try
            {
                //Find teacher by id
                var _student = _context.Students.SingleOrDefault(x => x.IdStudent == id);
                _student.IsDelete = 1;

                //Save data
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Xóa học sinh thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Xóa học sinh thất bại: " + ex.Message
                });
            }
        }
    }
}
