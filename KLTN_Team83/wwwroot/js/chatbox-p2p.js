// Trong wwwroot/js/chatbox-p2p.js (ví dụ)
document.addEventListener('DOMContentLoaded', function () {
    const messagesContainer = document.getElementById('p2p-chatbox-messages'); // ID mới
    const input = document.getElementById('p2p-chatbox-input');         // ID mới
    const sendBtn = document.getElementById('p2p-chatbox-send-btn');     // ID mới
    const expertList = document.getElementById('expert-list'); // Nơi hiển thị danh sách chuyên gia
    let currentExpertId = null; // ID của chuyên gia đang chat cùng
    let currentConversationId = null; // ID cuộc trò chuyện hiện tại

    // --- Kết nối đến SignalR Hub ---
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub") // URL đã map ở backend
        .configureLogging(signalR.LogLevel.Information) // Bật logging để debug
        .build();

    // --- Xử lý khi nhận được tin nhắn từ Hub ---
    connection.on("ReceiveMessage", (senderId, senderName, message, conversationId, timestamp) => {
        // Kiểm tra xem tin nhắn có thuộc về cuộc trò chuyện hiện tại không (nếu đã có)
        // Hoặc nếu là tin nhắn mới, bạn cần có logic để mở cửa sổ chat phù hợp
        console.log(`Message from ${senderName} (ID: ${senderId}) for conv ${conversationId}: ${message}`);

        // Chỉ hiển thị nếu tin nhắn thuộc cuộc trò chuyện đang mở (hoặc bạn có logic để mở)
        // Đây là ví dụ đơn giản, bạn cần logic phức tạp hơn để quản lý nhiều cuộc trò chuyện
        if (currentConversationId === null || currentConversationId === conversationId) {
            currentConversationId = conversationId; // Lưu lại nếu là tin nhắn đầu tiên trong cuộc chat mới
            displayMessageInP2PChat(senderName, message, senderId === getMyUserId()); // isMe = true nếu là tin nhắn của chính mình
        } else {
            // Thông báo có tin nhắn mới từ cuộc trò chuyện khác
            // Ví dụ: alert(`Tin nhắn mới từ ${senderName} trong cuộc trò chuyện khác.`);
        }
    });

    // --- Các hàm khác từ Hub ---
    // connection.on("UserConnected", (userId) => { console.log("User connected: " + userId); });
    // connection.on("ConversationStarted", (conversationId, expertName) => {
    //     console.log(`Conversation ${conversationId} started with ${expertName}`);
    //     currentConversationId = conversationId;
    //     // Hiển thị UI chat
    // });

    // --- Hàm hiển thị tin nhắn trên UI ---
    function displayMessageInP2PChat(senderName, message, isMe) {
        const messageDiv = document.createElement('div');
        messageDiv.classList.add('message', isMe ? 'user-message' : 'bot-message'); // Tái sử dụng CSS hoặc tạo mới
        // Cần sanitize 'message' nếu nó chứa HTML, nhưng thường là text thuần từ người dùng
        messageDiv.innerHTML = `<strong>${senderName}:</strong> ${DOMPurify.sanitize(message)}`; // Giả sử DOMPurify đã được thêm
        messagesContainer.appendChild(messageDiv);
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }

    // --- Gửi tin nhắn ---
    async function sendMessageToSelectedExpert() {
        const messageText = input.value.trim();
        if (!messageText || !currentExpertId) {
            if (!currentExpertId) alert("Vui lòng chọn một chuyên gia để chat.");
            return;
        }

        try {
            // Gọi phương thức "SendMessageToExpert" trên Hub
            await connection.invoke("SendMessageToExpert", currentExpertId, messageText);
            input.value = '';
        } catch (err) {
            console.error("Error sending message: ", err);
            alert("Không thể gửi tin nhắn. Lỗi: " + err.toString());
        }
    }

    sendBtn.addEventListener('click', sendMessageToSelectedExpert);
    input.addEventListener('keypress', (e) => {
        if (e.key === 'Enter') sendMessageToSelectedExpert();
    });


    // --- Hàm lấy UserId của người dùng hiện tại (cần truyền từ server hoặc lấy từ claim) ---
    function getMyUserId() {
        // Cách 1: Truyền từ server vào một element HTML (ví dụ data attribute)
        const userInfoElement = document.getElementById('user-info-data'); // <div id="user-info-data" data-userid="@UserManager.GetUserId(User)"></div>
        return userInfoElement ? userInfoElement.dataset.userid : null;

        // Cách 2: Nếu bạn có cách khác để JS biết UserId
    }

    // --- Logic chọn chuyên gia (ví dụ đơn giản) ---
    // Bạn cần fetch danh sách chuyên gia từ server và hiển thị
    // Ví dụ: giả sử có các nút chuyên gia
    if (expertList) {
        expertList.addEventListener('click', (event) => {
            if (event.target.classList.contains('expert-select-btn')) {
                currentExpertId = event.target.dataset.expertid;
                currentConversationId = null; // Reset conversation khi chọn expert mới
                messagesContainer.innerHTML = ''; // Xóa tin nhắn cũ
                alert(`Bạn đã chọn chat với chuyên gia ID: ${currentExpertId}. Giờ bạn có thể nhắn tin.`);
                // TODO: Nên có logic để tải lịch sử chat với chuyên gia này nếu có
                // connection.invoke("LoadChatHistoryWithExpert", currentExpertId);
            }
        });
    }


    // --- Khởi động kết nối ---
    async function startConnection() {
        try {
            await connection.start();
            console.log("SignalR Connected.");
            // Nếu là chuyên gia, tham gia nhóm chuyên gia
            // if (isCurrentUserExpert()) { // Cần hàm isCurrentUserExpert()
            //    await connection.invoke("JoinExpertGroup");
            // }
        } catch (err) {
            console.error("SignalR Connection Error: ", err);
            setTimeout(startConnection, 5000); // Thử kết nối lại sau 5 giây
        }
    }

    connection.onclose(async () => {
        console.log("SignalR Disconnected.");
        await startConnection(); // Tự động kết nối lại
    });

    // Bắt đầu kết nối khi trang tải xong
    startConnection();
});