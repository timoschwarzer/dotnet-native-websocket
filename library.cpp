#include <iostream>
#include <macros.h>
#include <queue>

#include <ixwebsocket/IXNetSystem.h>
#include <ixwebsocket/IXWebSocket.h>

struct PendingMessage {
    std::string data;
};

ix::WebSocket websocket;
std::queue<PendingMessage> message_queue;

C_DLLEXPORT void initialize_network() { ix::initNetSystem(); }

C_DLLEXPORT void finalize_network() { ix::uninitNetSystem(); }

C_DLLEXPORT void set_url(const char* url) { websocket.setUrl(url); }

C_DLLEXPORT void start() {
    websocket.start();
    websocket.setOnMessageCallback([](const ix::WebSocketMessagePtr& msg) {
        if (msg->type == ix::WebSocketMessageType::Message) {
            message_queue.emplace(msg->str);
        } else if (msg->type == ix::WebSocketMessageType::Open) {
            std::cout << "[NativeWebSocket] Connection established" << std::endl;
        } else if (msg->type == ix::WebSocketMessageType::Error) {
            std::cout << "[NativeWebSocket] Connection error: " << msg->errorInfo.reason << std::endl;
        }
    });
}

C_DLLEXPORT void send_binary(const char* data, const int length) {
    websocket.sendBinary(std::string(data, length));
}

C_DLLEXPORT void stop() { websocket.stop(); }

C_DLLEXPORT int get_state() { return static_cast<int>(websocket.getReadyState()); }

C_DLLEXPORT bool has_pending_message() { return !message_queue.empty(); }

C_DLLEXPORT const char* get_pending_message(int* length) {
    const auto& message = message_queue.front().data;
    *length = static_cast<int>(message.length());
    return message.c_str();
}

C_DLLEXPORT void pop_pending_message() { return message_queue.pop(); }
