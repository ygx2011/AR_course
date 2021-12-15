# Locate GLFW library
# This module defines
# GLFW_LIBRARY, the name of the library to link against
# GLFW_FOUND, if false, do not try to link to GLFW
# GLFW_INCLUDE_DIR, where to find GLFW.h
#
# This module responds to the the flag:
# GLFW_BUILDING_LIBRARY
# If this is defined, then no GLFW_main will be linked in because 
# only applications need main().
# Otherwise, it is assumed you are building an application and this
# module will attempt to locate and set the the proper link flags
# as part of the returned GLFW_LIBRARY variable.
#
# Don't forget to include GLFWmain.h and GLFWmain.m your project for the 
# OS X framework based version. (Other versions link to -lGLFWmain which
# this module will try to find on your behalf.) Also for OS X, this 
# module will automatically add the -framework Cocoa on your behalf.
#
#
# Additional Note: If you see an empty GLFW_LIBRARY_TEMP in your configuration
# and no GLFW_LIBRARY, it means CMake did not find your GLFW library 
# (GLFW.dll, libsdl.so, GLFW.framework, etc). 
# Set GLFW_LIBRARY_TEMP to point to your GLFW library, and configure again. 
# Similarly, if you see an empty GLFWMAIN_LIBRARY, you should set this value
# as appropriate. These values are used to generate the final GLFW_LIBRARY
# variable, but when these values are unset, GLFW_LIBRARY does not get created.
#
#
# $GLFWDIR is an environment variable that would
# correspond to the ./configure --prefix=$GLFWDIR
# used in building GLFW.
# l.e.galup  9-20-02
#
# Modified by Eric Wing. 
# Added code to assist with automated building by using environmental variables
# and providing a more controlled/consistent search behavior.
# Added new modifications to recognize OS X frameworks and 
# additional Unix paths (FreeBSD, etc). 
# Also corrected the header search path to follow "proper" GLFW guidelines.
# Added a search for GLFWmain which is needed by some platforms.
# Added a search for threads which is needed by some platforms.
# Added needed compile switches for MinGW.
#
# On OSX, this will prefer the Framework version (if found) over others.
# People will have to manually change the cache values of 
# GLFW_LIBRARY to override this selection or set the CMake environment
# CMAKE_INCLUDE_PATH to modify the search paths.
#
# Note that the header path has changed from GLFW/GLFW.h to just GLFW.h
# This needed to change because "proper" GLFW convention
# is #include "GLFW.h", not <GLFW/GLFW.h>. This is done for portability
# reasons because not all systems place things in GLFW/ (see FreeBSD).

#=============================================================================
# Copyright 2003-2009 Kitware, Inc.
#
# Distributed under the OSI-approved BSD License (the "License");
# see accompanying file Copyright.txt for details.
#
# This software is distributed WITHOUT ANY WARRANTY; without even the
# implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
# See the License for more information.
#=============================================================================
# (To distribute this file outside of CMake, substitute the full
#  License text for the above reference.)

FIND_PATH(GLFW_INCLUDE_DIR glfw3.h
  HINTS
  $ENV{GLFWDIR}
  PATH_SUFFIXES include/GL include
  PATHS
  ~/Library/Frameworks
  /Library/Frameworks
  /usr/local/include/GLFW
  /usr/include/GLFW
  /sw # Fink
  /opt/local # DarwinPorts
  /opt/csw # Blastwave
  /opt
)
#MESSAGE("GLFW_INCLUDE_DIR is ${GLFW_INCLUDE_DIR}")


# GLFW-1.1 is the name used by FreeBSD ports...
# don't confuse it for the version number.
FIND_LIBRARY(GLFW_LIBRARY_TEMP 
  NAMES GLFW
  HINTS
  $ENV{GLFWDIR}
  PATH_SUFFIXES lib64 lib
  PATHS
  /sw
  /opt/local
  /opt/csw
  /opt
)

#MESSAGE("GLFW_LIBRARY_TEMP is ${GLFW_LIBRARY_TEMP}")

IF(NOT GLFW_BUILDING_LIBRARY)
  IF(NOT ${GLFW_INCLUDE_DIR} MATCHES ".framework")
    # Non-OS X framework versions expect you to also dynamically link to 
    # GLFWmain. This is mainly for Windows and OS X. Other (Unix) platforms 
    # seem to provide GLFWmain for compatibility even though they don't
    # necessarily need it.
    FIND_LIBRARY(GLFWMAIN_LIBRARY 
      NAMES glfw
      HINTS
      $ENV{GLFWDIR}
      PATH_SUFFIXES lib64 lib
      PATHS
      /sw
      /opt/local
      /opt/csw
      /opt
    )
  ENDIF(NOT ${GLFW_INCLUDE_DIR} MATCHES ".framework")
ENDIF(NOT GLFW_BUILDING_LIBRARY)

# GLFW may require threads on your system.
# The Apple build may not need an explicit flag because one of the 
# frameworks may already provide it. 
# But for non-OSX systems, I will use the CMake Threads package.
IF(NOT APPLE)
  FIND_PACKAGE(Threads)
ENDIF(NOT APPLE)

# MinGW needs an additional library, mwindows
# It's total link flags should look like -lmingw32 -lGLFWmain -lGLFW -lmwindows
# (Actually on second look, I think it only needs one of the m* libraries.)
IF(MINGW)
  SET(MINGW32_LIBRARY mingw32 CACHE STRING "mwindows for MinGW")
ENDIF(MINGW)

SET(GLFW_FOUND "NO")
IF(GLFW_LIBRARY_TEMP)
  # For GLFWmain
  IF(NOT GLFW_BUILDING_LIBRARY)
    IF(GLFWMAIN_LIBRARY)
      SET(GLFW_LIBRARY_TEMP ${GLFWMAIN_LIBRARY} ${GLFW_LIBRARY_TEMP})
    ENDIF(GLFWMAIN_LIBRARY)
  ENDIF(NOT GLFW_BUILDING_LIBRARY)

  # For OS X, GLFW uses Cocoa as a backend so it must link to Cocoa.
  # CMake doesn't display the -framework Cocoa string in the UI even 
  # though it actually is there if I modify a pre-used variable.
  # I think it has something to do with the CACHE STRING.
  # So I use a temporary variable until the end so I can set the 
  # "real" variable in one-shot.
  IF(APPLE)
    SET(GLFW_LIBRARY_TEMP ${GLFW_LIBRARY_TEMP} "-framework Cocoa")
  ENDIF(APPLE)
    
  # For threads, as mentioned Apple doesn't need this.
  # In fact, there seems to be a problem if I used the Threads package
  # and try using this line, so I'm just skipping it entirely for OS X.
  IF(NOT APPLE)
    SET(GLFW_LIBRARY_TEMP ${GLFW_LIBRARY_TEMP} ${CMAKE_THREAD_LIBS_INIT})
  ENDIF(NOT APPLE)

  # For MinGW library
  IF(MINGW)
    SET(GLFW_LIBRARY_TEMP ${MINGW32_LIBRARY} ${GLFW_LIBRARY_TEMP})
  ENDIF(MINGW)

  # Set the final string here so the GUI reflects the final state.
  SET(GLFW_LIBRARY ${GLFW_LIBRARY_TEMP} CACHE STRING "Where the GLFW Library can be found")
  # Set the temp variable to INTERNAL so it is not seen in the CMake GUI
  SET(GLFW_LIBRARY_TEMP "${GLFW_LIBRARY_TEMP}" CACHE INTERNAL "")

  SET(GLFW_FOUND "YES")
ENDIF(GLFW_LIBRARY_TEMP)

#MESSAGE("GLFW_LIBRARY is ${GLFW_LIBRARY}")
