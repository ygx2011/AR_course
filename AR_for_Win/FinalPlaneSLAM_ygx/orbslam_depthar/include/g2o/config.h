#ifndef G2O_CONFIG_H
#define G2O_CONFIG_H


#define G2O_HAVE_OPENGL 1
#define G2O_OPENGL_FOUND 1
/* #undef G2O_OPENMP */
#define G2O_SHARED_LIBS 1
#define G2O_LGPL_SHARED_LIBS 1

// available sparse matrix libraries
/* #undef G2O_HAVE_CHOLMOD */
#define G2O_HAVE_CSPARSE 1

#define G2O_CXX_COMPILER "MSVC c:/Program Files (x86)/Microsoft Visual Studio 10.0/VC/bin/cl.exe"

#ifdef __cplusplus
#include <g2o/core/eigen_types.h>
#endif

#endif
