using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SettingController : Controller
    {
        private readonly MultipleChoiceContext _context;

        public SettingController(MultipleChoiceContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get list setting
        [HttpGet]
        public JsonResult GetListSetting()
        {
            try
            {
                var listSetting = _context.Settings.OrderBy(x => x.Keyword).ToList();

                return Json(new
                {
                    code = 200,
                    message = "Lấy danh sách cấu hình thành công!",
                    data = listSetting,
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy danh sách cấu hình thất bại: " + ex.Message
                });
            }
        }

        //Get detail setting
        [HttpGet]
        public JsonResult GetDetailSetting(string keyword)
        {
            try
            {
                var _setting = _context.Settings.SingleOrDefault(x => x.Keyword == keyword);

                return Json(new
                {
                    code = 200,
                    message = "Lấy chi tiết cấu hình thành công!",
                    data = _setting,
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Lấy chi tiết cấu hình thất bại: " + ex.Message
                });
            }
        }

        //Add setting
        [HttpPost]
        public JsonResult AddSetting(string keyword, string value, string description)
        {
            try
            {
                var _checkSetting = _context.Settings.FirstOrDefault(x => x.Keyword == keyword);

                if (_checkSetting != null)
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Từ khóa đã tồn tại"
                    });
                }

                var _setting = new Setting();
                _setting.Keyword = keyword;
                _setting.Value = value;
                _setting.Description = description;

                _context.Settings.Add(_setting);
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    message = "Thêm cấu hình thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Thêm cấu hình thất bại: " + ex.Message
                });
            }
        }

        //Update setting
        [HttpPost]
        public JsonResult UpdateSetting(string keyword, string value, string description)
        {
            try
            {
                var _setting = _context.Settings.SingleOrDefault(x => x.Keyword == keyword);

                if (_setting != null)
                {
                    _setting.Value = value;
                    _setting.Description = description;

                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy cấu hình!",
                    });
                }

                return Json(new
                {
                    code = 200,
                    message = "Cập nhật cấu hình thành công!",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Cập nhật cấu hình thất bại: " + ex.Message
                });
            }
        }
    }
}
