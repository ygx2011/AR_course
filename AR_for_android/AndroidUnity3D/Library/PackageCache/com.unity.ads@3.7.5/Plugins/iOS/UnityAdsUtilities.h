const char * UnityAdsCopyString(const char * string);

/**
 * Returns the size of an Il2CppString
 */
size_t Il2CppStringLen(const ushort* str);

/**
 * Converts an ushort string to an NSString
 */
NSString* NSStringFromIl2CppString(const ushort* str);

/**
 * Converts a char string to an NSString. Does pre checks for null pointer
 */
NSString* NSStringFromCString(const char* string);

/**
 * Converts a NSString to a char string.Does pre checks for null pointer
 */
const char * CStringFromNSString(const NSString * string);

#define NSSTRING_OR_EMPTY(string) NSStringFromCString(string) ?: @""
