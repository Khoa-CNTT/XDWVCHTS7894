document.addEventListener('DOMContentLoaded', function () {
    const chatbox = document.getElementById('gemini-chatbox');
    const messagesContainer = document.getElementById('chatbox-messages');
    const input = document.getElementById('chatbox-input');
    const sendBtn = document.getElementById('chatbox-send-btn');
    const toggleBtn = document.getElementById('chatbox-toggle-btn');
    const closeBtn = document.getElementById('chatbox-close-btn');
    const voiceBtn = document.getElementById('chatbox-voice-btn');

    const apiEndpoint = '/api/chat';

    // --- Hàm hiển thị tin nhắn ---
    function displayMessage(text, sender) {
        const messageDiv = document.createElement('div');
        messageDiv.classList.add('message', `${sender}-message`);
        messageDiv.innerHTML = text;
        messagesContainer.appendChild(messageDiv);
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
        return messageDiv;
    }

    // --- Hàm hiển thị trạng thái đang chờ ---
    function showThinkingIndicator() {
        const thinkingDiv = displayMessage('', 'bot');
        thinkingDiv.classList.add('thinking');
        return thinkingDiv;
    }

    // --- Hàm gửi tin nhắn đến Backend ---
    async function sendMessage() {
        const messageText = input.value.trim();
        if (!messageText) return;

        displayMessage(messageText, 'user');
        input.value = '';
        sendBtn.disabled = true;
        input.disabled = true;

        const thinkingIndicator = showThinkingIndicator();

        try {
            const response = await fetch(apiEndpoint, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ message: messageText })
            });

            messagesContainer.removeChild(thinkingIndicator);

            if (response.ok) {
                const data = await response.json();
                displayMessage(data.reply, 'bot');
            } else {
                const errorData = await response.json().catch(() => ({ reply: 'Lỗi không xác định từ server.' }));
                console.error("API Error:", response.status, errorData);
                displayMessage(`Lỗi: ${errorData.error || response.statusText || 'Không thể nhận phản hồi.'}`, 'bot');
            }
        } catch (error) {
            if (thinkingIndicator && messagesContainer.contains(thinkingIndicator)) {
                messagesContainer.removeChild(thinkingIndicator);
            }
            console.error("Fetch Error:", error);
            displayMessage('Lỗi kết nối đến máy chủ.', 'bot');
        } finally {
            sendBtn.disabled = false;
            input.disabled = false;
            input.focus();
        }
    }

    // --- Voice input ---
    let recognition;
    if ("webkitSpeechRecognition" in window) {
        recognition = new webkitSpeechRecognition();
        recognition.lang = "vi-VN";
        recognition.continuous = false;
        recognition.interimResults = false;

        recognition.onresult = (e) => {
            const transcript = e.results[0][0].transcript;
            input.value = transcript;
            sendMessage();
        };

        recognition.onerror = () => {
            console.warn("Không thể nhận diện giọng nói");
            voiceBtn.classList.remove("recording");
            displayMessage('Lỗi nhận diện giọng nói.', 'bot');
        };

        recognition.onend = () => {
            voiceBtn.classList.remove("recording");
        };

        voiceBtn.addEventListener("click", () => {
            if (!voiceBtn.classList.contains("recording")) {
                voiceBtn.classList.add("recording");
                recognition.start();
            } else {
                recognition.stop();
                voiceBtn.classList.remove("recording");
            }
        });
    } else {
        voiceBtn.style.display = "none";
        console.info("Trình duyệt không hỗ trợ Web Speech API");
    }

    // --- Xử lý sự kiện ---
    sendBtn.addEventListener('click', sendMessage);
    input.addEventListener('keypress', function (e) {
        if (e.key === 'Enter') sendMessage();
    });

    toggleBtn.addEventListener('click', () => {
        chatbox.classList.toggle('visible');
        if (chatbox.classList.contains('visible')) {
            input.focus();
            toggleBtn.style.display = 'none';
        } else {
            toggleBtn.style.display = 'block';
        }
    });

    closeBtn.addEventListener('click', () => {
        chatbox.classList.remove('visible');
        toggleBtn.style.display = 'block';
    });
});