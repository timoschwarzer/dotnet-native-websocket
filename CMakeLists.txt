cmake_minimum_required(VERSION 3.28)

project(NativeWebSocket)

set(CMAKE_CXX_STANDARD 23)
# set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -m32")
# set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -m32")

set(CMAKE_ROOT_DIRECTORY ${PROJECT_SOURCE_DIR})
set(CMAKE_BUILD_DIRECTORY ${PROJECT_BINARY_DIR})
set(CMAKE_CONFIG_DIRECTORY ${CMAKE_BUILD_DIRECTORY}/config)
set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_BUILD_DIRECTORY}/bin)
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_BUILD_DIRECTORY}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BUILD_DIRECTORY}/bin)

message(STATUS "Windows Target version: ${CMAKE_VS_WINDOWS_TARGET_PLATFORM_VERSION}")

add_library(
        NativeWebSocket
        SHARED
        library.cpp
)

find_package(ixwebsocket CONFIG REQUIRED)
find_path(IXWEBSOCKET_INCLUDE_DIR ixwebsocket/IXWebSocket.h)
target_link_libraries(
        ${PROJECT_NAME}
        wsock32
        ws2_32
        ixwebsocket::ixwebsocket
)

target_include_directories(
        NativeWebSocket
        PUBLIC
        .
        ${CMAKE_CONFIG_DIRECTORY}
        ${CMAKE_ENGINE_DIRECTORY}
        ${IXWEBSOCKET_INCLUDE_DIR}
        SYSTEM
)
