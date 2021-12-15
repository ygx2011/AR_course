// Include GLFW
#include <GLFW/glfw3.h>
extern GLFWwindow* window;

// Include GLM
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
using namespace glm;

#include "GL_Rendering/controls.hpp"

#include <glm/gtc/type_ptr.hpp>

glm::mat4 ModelMatrix;
glm::mat4 TranslateMatrix;
glm::mat4 ScalingMatrix;
glm::mat4 ProjectionMatrix;

glm::mat4 getModelMatrix(){
	return ModelMatrix;
}
glm::mat4 getProjectionMatrix(){
	return ProjectionMatrix;
}


float scale_factor = 1;
float rotation = 0;

float speed = 3.0f; // 3 units / second
float rotate_speed = 90.0f;



void computeMatricesFromInputs(){

    ModelMatrix = glm::rotate(glm::mat4(1.0), glm::radians(90.0f), glm::vec3( -1, 0, 0));

	// glfwGetTime is called only once, the first time this function is called
	static double lastTime = glfwGetTime();

	// Compute time difference between current and last frame
	double currentTime = glfwGetTime();
	float deltaTime = float(currentTime - lastTime);

	// Move forward
	if (glfwGetKey( window, GLFW_KEY_UP ) == GLFW_PRESS){
		scale_factor += deltaTime * speed;
	}
	// Move backward
	if (glfwGetKey( window, GLFW_KEY_DOWN ) == GLFW_PRESS){
		scale_factor -= deltaTime * speed;
	}
	// Strafe right
	if (glfwGetKey( window, GLFW_KEY_RIGHT ) == GLFW_PRESS){
		rotation += deltaTime * rotate_speed;
	}
	// Strafe left
	if (glfwGetKey( window, GLFW_KEY_LEFT ) == GLFW_PRESS){
        rotation -= deltaTime * rotate_speed;
	}
    
    float f_x = 657.342285;
    float f_y = 657.342285;
    float c_x = 319.647289;
    float c_y = 236.907530;
    
    float width = 640;
    float height = 480;
    
    float near_plane = 0.01;
    float far_plane = 100;
    
    float projection_matrix[16];
    
    projection_matrix[0] = 2*f_x/width;
    projection_matrix[1] = 0.0f;
    projection_matrix[2] = 0.0f;
    projection_matrix[3] = 0.0f;
    
    projection_matrix[4] = 0.0f;
    projection_matrix[5] = 2*f_y/height;
    projection_matrix[6] = 0.0f;
    projection_matrix[7] = 0.0f;
    
    projection_matrix[8] = 1.0f - 2*c_x/width;
    projection_matrix[9] = 2*c_y/height - 1.0f;
    projection_matrix[10] = -(far_plane + near_plane)/(far_plane - near_plane);
    projection_matrix[11] = -1.0f;
    
    projection_matrix[12] = 0.0f;
    projection_matrix[13] = 0.0f;
    projection_matrix[14] = -2.0f*far_plane*near_plane/(far_plane - near_plane);
    projection_matrix[15] = 0.0f;
    
    ProjectionMatrix = glm::make_mat4(projection_matrix);
    
    // Model matrix
    ModelMatrix = glm::rotate(ModelMatrix, glm::radians(rotation), glm::vec3( 0, 1, 0));
    TranslateMatrix = glm::translate(glm::mat4(1.0f), glm::vec3(3.5,3,0));
    ScalingMatrix = glm::scale(glm::mat4(1.0f), glm::vec3(scale_factor));
    ModelMatrix = TranslateMatrix * ModelMatrix * ScalingMatrix;

	// For the next frame, the "last time" will be "now"
	lastTime = currentTime;
}