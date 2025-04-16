namespace KLTN_Team83.Models
{
    public class Plan
    {
        public string GetResponse(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return "Xin hãy nhập tin nhắn!";

            message = message.ToLower();
            if (message.Contains("xin chào"))
                return "Chào bạn! Tôi có thể giúp gì?";
            if (message.Contains("bạn tên gì"))
                return "Tôi là chatbot WellNess!";
            if (message.Contains("dinh dưỡng") || message.Contains("thực đơn"))
                return "Vui lòng cung cấp chiều cao (cm), cân nặng (kg), độ tuổi và mục tiêu (tăng cân, giảm cân, duy trì).";

            return "Xin lỗi, tôi không hiểu câu hỏi của bạn.";
        }

        public string GetDietPlan(int height, int weight, int age, string goal)
        {
            double bmi = weight / Math.Pow(height / 100.0, 2);
            string advice;

            if (bmi < 18.5)
                advice = "Bạn cần tăng cường dinh dưỡng với thực phẩm giàu protein, carb và chất béo lành mạnh.";
            else if (bmi >= 18.5 && bmi < 24.9)
                advice = "Bạn có cân nặng lý tưởng! Duy trì chế độ ăn cân bằng và tập luyện thường xuyên.";
            else
                advice = "Bạn nên cắt giảm calo với thực phẩm ít béo, nhiều chất xơ và tập luyện đều đặn.";

            return $"BMI của bạn là {bmi:F1}. {advice}";
        }
    }
}
