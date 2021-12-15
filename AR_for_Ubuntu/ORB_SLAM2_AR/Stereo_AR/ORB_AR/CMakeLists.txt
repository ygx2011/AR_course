cmake_minimum_required(VERSION 3.3)
project(BOB_AR)

include_directories(${PROJECT_SOURCE_DIR}/include)
include_directories(${PROJECT_SOURCE_DIR}/temp)

find_package( OpenCV REQUIRED )
find_package( OpenGL REQUIRED )
include_directories(${OPENGL_INCLUDE_DIR})

find_package( GLUT REQUIRED )
include_directories(${GLUT_INCLUDE_DIR})

find_package( GLEW REQUIRED )
include_directories(${GLEW_INCLUDE_DIR})

find_package( GLM REQUIRED )
include_directories(${GLM_INCLUDE_DIR})

find_package( PNG REQUIRED )
include_directories(${PNG_INCLUDE_DIR})

LIST(APPEND CMAKE_MODULE_PATH "${PROJECT_SOURCE_DIR}/cmake_modules" ${CMAKE_MODULE_PATH})
find_package(GLFW REQUIRED)
include_directories(${GLFW_INCLUDE_DIRS})

find_package(Eigen3 3.1.0 REQUIRED)
find_package(Pangolin REQUIRED)
include_directories(
        ${PROJECT_SOURCE_DIR}
        ${EIGEN3_INCLUDE_DIR}
        ${Pangolin_INCLUDE_DIRS})




set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++11")

set(SOURCE_FILES
        src/main.cpp
        src/shader.cpp
        src/controls.cpp
        src/texture.cpp
        src/objloader.cpp
        src/orbslam.cpp
        #temp/track_chessboard/track_chessboard.cpp
        #temp/track_chessboard/track_chessboard.hpp
        src/planar.cpp)
add_executable(BOB_AR ${SOURCE_FILES})
target_link_libraries(BOB_AR
        ${OPENGL_LIBRARIES}
        ${GLUT_LIBRARIES}
        ${OpenCV_LIBS}
        ${GLEW_LIBRARIES}
        ${GLFW_LIBRARY}
        ${PNG_LIBRARY}
        ${EIGEN3_LIBS}
        ${Pangolin_LIBRARIES}
        ${PROJECT_SOURCE_DIR}/lib/libORB_SLAM2.dylib)