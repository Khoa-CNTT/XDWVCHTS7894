using System.Text;
using KLTN_Team83.Models;
using Microsoft.AspNetCore.Mvc;

namespace KLTN_Team83.Controllers
{
    public class ChatbotController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string OpenAiApiKey = "AIzaSyAHE6r_TifCeDRJiKYR_VZkLN3WuwBcbyA";

        public ChatbotController()
        {
            _httpClient = new HttpClient();
        }

        //[HttpPost]
        //public async Task<IActionResult> GetResponse([FromBody] ChatRequest request)
        //{
        //    var apiUrl = "https://api.openai.com/v1/chat/completions";
        //    var requestBody = new
        //    {
        //        model = "gpt-3.5-turbo",
        //        messages = new[] { new { role = "user", content = request.Message } }
        //    };

        //    var jsonContent = JsonConvert.SerializeObject(requestBody);
        //    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        //    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {OpenAiApiKey}");

        //    var response = await _httpClient.PostAsync(apiUrl, content);
        //    var responseString = await response.Content.ReadAsStringAsync();

        //    return Ok(responseString);
        //}
    }

    //public class ChatRequest
    //{
    //    public string Message { get; set; }
    //}
}
