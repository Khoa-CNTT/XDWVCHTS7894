﻿using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; // Cần để đọc appsettings
using KLTN_Team83.Models.Gemini; // Namespace chứa model Gemini
using System.Collections.Generic; // Cho List
using Microsoft.AspNetCore.Http;
using KLTN_Team83.Models;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using KLTN_Team83.Utility;
using Microsoft.AspNetCore.Identity;
using KLTN_Team83.DataAccess.Repository.IRepository;
using System.Security.Claims;

namespace KLTN_Team83.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/chat")] // Định nghĩa route cho API
    [ApiController]
    [Authorize]
    public class ChatApiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _geminiApiKey;
        private readonly string _geminiApiUrl;
        private readonly IUnitOfWork _unitOfWork;

        public ChatApiController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _geminiApiKey = _configuration["Gemini:ApiKey"]; // Đọc API Key từ config
            _unitOfWork = unitOfWork;

            if (string.IsNullOrEmpty(_geminiApiKey))
            {
                // Nên ghi log lỗi ở đây
                throw new InvalidOperationException("Gemini API Key chưa được cấu hình trong appsettings.");
            }

            // Model 'gemini-pro' là phổ biến cho chat
            _geminiApiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_geminiApiKey}";
        }

        // --- Helper để quản lý lịch sử chat trong Session ---this

        private List<Content> GetChatHistory()
        {
            var historyJson = HttpContext.Session.GetString("ChatHistory");
            if (string.IsNullOrEmpty(historyJson))
            {
                return new List<Content>(); // Trả về list rỗng nếu chưa có
            }
            try
            {
                return JsonSerializer.Deserialize<List<Content>>(historyJson) ?? new List<Content>();
            }
            catch (JsonException) // Xử lý nếu JSON trong session bị lỗi
            {
                return new List<Content>();
            }
        }

        private void SaveChatHistory(List<Content> history)
        {
            var historyJson = JsonSerializer.Serialize(history);
            HttpContext.Session.SetString("ChatHistory", historyJson);
        }
        // ----------------------------------------------------


        [HttpPost] // Chỉ chấp nhận phương thức POST
        public async Task<IActionResult> PostMessage([FromBody] ChatInput input)
        {
            if (string.IsNullOrWhiteSpace(input?.Message))
            {
                return BadRequest(new { error = "Tin nhắn không được để trống." });
            }

            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                // 0. Lấy lịch sử chat từ Session
                var chatHistory = GetChatHistory();
                // 0.1. Lấy thông tin cá nhân từ database
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
                var name = user.Name;
                var gender = user.Gender;
                var state = user.State;
                var height = user.Height;
                var weight = user.Weight;

                // *** ÁP DỤNG PROMPT ENGINEERING ***

                // 1. Đảm bảo có prompt thiết lập vai trò/ràng buộc nếu là phiên mới
                if (!chatHistory.Any(c => c.Role == "model")) // Kiểm tra đơn giản xem có phải phiên mới không
                {
                    // Thêm persona/instructions vào ĐẦU danh sách
                    var initialInstructions = new List<Content> {
                new Content {
                    Role = "user",
                    Parts = new List<Part> { new Part { 
                        Text = "Bạn là WellnessBot – một trợ lý ảo chuyên gia dinh dưỡng và lối sống lành mạnh." +
                        "**Nhiệm vụ chính:**\r\n- Cung cấp thông tin chính xác và lời khuyên chi tiết về dinh dưỡng, calo, sức khỏe tổng quát và vận động.\r\n- Hỗ trợ người dùng xây dựng và duy trì thói quen tích cực." +
                        
                        "**Phong cách giao tiếp:**\r\n- Luôn bắt đầu bằng lời chào ấm áp, hỏi thăm "+name+" hôm nay cảm thấy thế nào hoặc cần hỗ trợ gì về sức khỏe và lối sống.\r\n- Sử dụng giọng điệu nhẹ nhàng, khích lệ, thân thiện, tích cực và không phán xét.\r\n- Trả lời ngắn gọn, rõ ràng, dễ hiểu, và dựa trên cơ sở khoa học.\r\n- Khi đưa ra gợi ý, trình bày mỗi ý hoặc gợi ý trên một dòng riêng biệt, bắt đầu bằng dấu '- '.\r\n- Có thể sử dụng một vài biểu tượng cảm xúc phù hợp (🌿💧😊) để tăng sự thân thiện, nhưng không lạm dụng.\r\n- Luôn cố gắng tóm tắt lời khuyên chính ở cuối mỗi câu trả lời." +

                        // *** Nhắc lại ràng buộc ***
                        "**Giới hạn và Ràng buộc (Rất Quan Trọng):**\r\n- **Chỉ tập trung tư vấn các chủ đề sau:**\r\n    - Gợi ý chế độ ăn uống cân bằng và bữa ăn lành mạnh.\r\n    - Gợi ý bài tập thể dục phù hợp với từng mức độ.\r\n    - Cách xây dựng thói quen tích cực và duy trì động lực."+

                        "**TUYỆT ĐỐI KHÔNG:**\r\n - Trả lời các câu hỏi nằm ngoài các chủ đề trên (ví dụ: không trả lời về lập trình, code, chính trị, giải trí, v.v.).\r\n - Chẩn đoán bệnh hoặc kê đơn thuốc. Nếu người dùng hỏi về vấn đề sức khỏe nghiêm trọng, hãy lịch sự khuyên họ liên hệ với bác sĩ hoặc chuyên gia y tế.\r\n- Nếu người dùng hỏi về thực phẩm chức năng, thuốc giảm cân, hoặc chế độ ăn đặc biệt, hãy cung cấp thông tin trung lập, nêu rõ ưu và nhược điểm (nếu có thông tin khoa học), và luôn nhắc họ tham khảo ý kiến chuyên gia y tế trước khi sử dụng hoặc bắt đầu."+

                        // *** Thêm thông tin về context cá nhân nếu có ***
                        "Hãy sử dụng thông tin cá nhân:"+"giới tính:"+gender+",chiều cao(cm):"+height+",cân nặng(kg):"+weight+",nơi ở::"+state+"được cung cấp trong database để đưa ra lời khuyên phù hợp."+
                        
                        // Ngôn ngữ
                        "**Ngôn ngữ:** - Trả lời bằng ngôn ngữ mà người dùng sử dụng để đặt câu hỏi (ví dụ: nếu hỏi bằng tiếng Anh, trả lời bằng tiếng Anh)." +
                        
                        "Hãy nhớ, bạn là người bạn đồng hành, hỗ trợ người dùng trên hành trình sống khỏe mỗi ngày! ✨" 
                        
                    } }
                },
                new Content {
                    Role = "model",
                    Parts = new List<Part> { new Part { Text = "Đã hiểu! Tôi sẽ sử dụng thông tin cá nhân của bạn (nếu có) để tư vấn. Bạn cần thông tin gì hôm nay?" } }
                }
            };
                    chatHistory.InsertRange(0, initialInstructions); // Chèn vào đầu
                }

                // 2. Thêm tin nhắn mới của người dùng vào lịch sử
                chatHistory.Add(new Content { Role = "user", Parts = new List<Part> { new Part { Text = input.Message } } });

                // 3. Chuẩn bị request body cho Gemini
                var requestPayload = new GeminiRequest
                {
                    Contents = chatHistory // Gửi toàn bộ lịch sử
                };

                var jsonPayload = JsonSerializer.Serialize(requestPayload, new JsonSerializerOptions
                {
                    // Bỏ qua các thuộc tính null để JSON gọn hơn (tùy chọn)
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });

                var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // 4. Gửi yêu cầu POST đến Gemini API
                var response = await httpClient.PostAsync(_geminiApiUrl, httpContent);

                // 5. Xử lý Response
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseBody);
                    

                    // Lấy câu trả lời đầu tiên (thường chỉ có 1 candidate tốt)
                    var botMessageContent = geminiResponse?.Candidates?.FirstOrDefault()?.Content;
                    if (botMessageContent != null && botMessageContent.Parts != null && botMessageContent.Parts.Any())
                    {
                        var botReplyText = botMessageContent.Parts.First().Text;
                        //Chuyển markdown về dạng text
                        string text = Markdown.ToHtml(botReplyText);
                        // 6. Thêm câu trả lời của bot vào lịch sử
                        // Quan trọng: Đảm bảo Role là "model"
                        botMessageContent.Role = "model"; // API có thể không trả về role, ta cần gán
                        chatHistory.Add(botMessageContent);

                        // 7. Lưu lại lịch sử vào Session
                        SaveChatHistory(chatHistory);

                        // 8. Trả về câu trả lời cho frontend
                        return Ok(new { reply = text });
                    }
                    else
                    {
                        // Trường hợp API trả về thành công nhưng không có nội dung mong đợi
                        SaveChatHistory(chatHistory); // Vẫn lưu lịch sử user hỏi
                        return Ok(new { reply = "Xin lỗi, tôi không thể tạo phản hồi lúc này." });
                    }
                }
                else
                {
                    // Xử lý lỗi từ Gemini API
                    var errorBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Lỗi từ Gemini API: {response.StatusCode} - {errorBody}"); // Log lỗi ra console server
                    // Không lưu lịch sử khi có lỗi API
                    return StatusCode((int)response.StatusCode, new { error = "Có lỗi xảy ra khi giao tiếp với AI.", details = errorBody });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi nội bộ server: {ex.Message}"); // Log lỗi
                // Cân nhắc không nên trả về chi tiết lỗi cho client trong production
                return StatusCode(500, new { error = "Lỗi máy chủ nội bộ." });
            }
        }
    }

    // Class đơn giản để nhận input từ frontend
    public class ChatInput
    {
        public string Message { get; set; }
    }
}
