using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Nếu muốn bảo vệ Hub
using System.Collections.Concurrent; // Cho Dictionary thread-safe
using KLTN_Team83.DataAccess.Data; // DbContext
using KLTN_Team83.Models; // Models (Message, Conversation, ApplicationUser)
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore; // Cho DateTime

namespace YourProjectName.Hubs
{
    [Authorize] // Chỉ người dùng đã đăng nhập mới kết nối được
    public class ChatHubController : Hub
    {
        // Lưu trữ kết nối của người dùng/chuyên gia
        // Key: UserId, Value: ConnectionId (của SignalR)
        // Cần cơ chế phức tạp hơn nếu 1 user có nhiều tab/thiết bị
        private static readonly ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatHubController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Khi client kết nối
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // Lấy UserId của người dùng đang kết nối
            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections[userId] = Context.ConnectionId;
            }
            // Thông báo cho các chuyên gia khác (ví dụ)
            // await Clients.Group("Experts").SendAsync("UserConnected", userId);
            await base.OnConnectedAsync();
        }

        // Khi client ngắt kết nối
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections.TryRemove(userId, out _);
            }
            // Thông báo cho các chuyên gia khác (ví dụ)
            // await Clients.Group("Experts").SendAsync("UserDisconnected", userId);
            await base.OnDisconnectedAsync(exception);
        }

        // Phương thức Client (người dùng) gọi để gửi tin nhắn đến chuyên gia
        public async Task SendMessageToExpert(string expertUserId, string message)
        {
            var senderUserId = Context.UserIdentifier; // ID người gửi (người dùng)
            var sender = await _userManager.FindByIdAsync(senderUserId);
            if (sender == null) return; // Không tìm thấy người gửi

            // TODO: Tìm hoặc tạo Conversation giữa senderUserId và expertUserId
            var conversation = await GetOrCreateConversationAsync(senderUserId, expertUserId);
            if (conversation == null) return; // Không thể tạo/tìm cuộc trò chuyện

            // Lưu tin nhắn vào DB
            var chatMessage = new Message
            {
                ConversationId = conversation.Id,
                SenderId = senderUserId,
                SenderName = sender.UserName, // Hoặc tên hiển thị
                ReceiverId = expertUserId,
                Content = message, // Cần sanitize trước khi hiển thị
                Timestamp = DateTime.UtcNow
            };
            _context.Messages.Add(chatMessage);
            await _context.SaveChangesAsync();

            // Gửi tin nhắn đến chuyên gia cụ thể (nếu họ đang online)
            if (UserConnections.TryGetValue(expertUserId, out string? expertConnectionId) && expertConnectionId != null)
            {
                // Client của chuyên gia cần có hàm "ReceiveMessage"
                await Clients.Client(expertConnectionId).SendAsync("ReceiveMessage", senderUserId, sender.UserName, message, conversation.Id, DateTime.UtcNow);
            }

            // Gửi lại tin nhắn cho chính người dùng đã gửi (để hiển thị trên UI của họ)
            await Clients.Caller.SendAsync("ReceiveMessage", senderUserId, sender.UserName, message, conversation.Id, DateTime.UtcNow);
            // Có thể thông báo cho chuyên gia về tin nhắn mới nếu họ không online (qua email, notification)
        }

        // Phương thức Client (chuyên gia) gọi để gửi tin nhắn đến người dùng
        public async Task SendMessageToUser(string targetUserId, string message)
        {
            var senderUserId = Context.UserIdentifier; // ID người gửi (chuyên gia)
            var sender = await _userManager.FindByIdAsync(senderUserId);
            if (sender == null || !await _userManager.IsInRoleAsync(sender, "Expert")) // Giả sử chuyên gia có role "Expert"
            {
                // Chỉ chuyên gia mới được gửi theo cách này
                return;
            }

            var conversation = await GetOrCreateConversationAsync(targetUserId, senderUserId); // User là người bắt đầu, expert là người join
            if (conversation == null) return;

            var chatMessage = new Message
            {
                ConversationId = conversation.Id,
                SenderId = senderUserId,
                SenderName = sender.UserName,
                ReceiverId = targetUserId,
                Content = message,
                Timestamp = DateTime.UtcNow
            };
            _context.Messages.Add(chatMessage);
            await _context.SaveChangesAsync();

            // Gửi tin nhắn đến người dùng cụ thể (nếu họ đang online)
            if (UserConnections.TryGetValue(targetUserId, out string? userConnectionId) && userConnectionId != null)
            {
                await Clients.Client(userConnectionId).SendAsync("ReceiveMessage", senderUserId, sender.UserName, message, conversation.Id, DateTime.UtcNow);
            }

            // Gửi lại tin nhắn cho chính chuyên gia đã gửi
            await Clients.Caller.SendAsync("ReceiveMessage", senderUserId, sender.UserName, message, conversation.Id, DateTime.UtcNow);
        }

        // TODO: Thêm các phương thức khác như:
        // - UserRequestChat(expertId) -> tạo Conversation, thông báo cho expert
        // - ExpertAcceptChat(conversationId) -> chuyên gia chấp nhận, bắt đầu chat
        // - LoadChatHistory(conversationId) -> tải lịch sử tin nhắn cho một cuộc trò chuyện

        private async Task<Conversation?> GetOrCreateConversationAsync(string userId1, string userId2)
        {
            // Sắp xếp ID để đảm bảo key duy nhất cho cặp user
            var userIds = new List<string> { userId1, userId2 }.OrderBy(id => id).ToList();
            var sortedUserId1 = userIds[0];
            var sortedUserId2 = userIds[1];

            var conversation = await _context.Conversations
                .FirstOrDefaultAsync(c =>
                    (c.User1Id == sortedUserId1 && c.User2Id == sortedUserId2) ||
                    (c.User1Id == sortedUserId2 && c.User2Id == sortedUserId1)
                );

            if (conversation == null)
            {
                conversation = new Conversation
                {
                    User1Id = sortedUserId1, // Có thể 1 là user, 1 là expert
                    User2Id = sortedUserId2,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Pending" // Hoặc "Active" nếu tự động chấp nhận
                };
                _context.Conversations.Add(conversation);
                await _context.SaveChangesAsync();
            }
            return conversation;
        }

        // Ví dụ: Chuyên gia tham gia vào một "nhóm" để nhận thông báo
        public async Task JoinExpertGroup()
        {
            var user = await _userManager.FindByIdAsync(Context.UserIdentifier);
            if (user != null && await _userManager.IsInRoleAsync(user, "Expert")) // Giả sử vai trò là "Expert"
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Experts");
            }
        }
    }
}
