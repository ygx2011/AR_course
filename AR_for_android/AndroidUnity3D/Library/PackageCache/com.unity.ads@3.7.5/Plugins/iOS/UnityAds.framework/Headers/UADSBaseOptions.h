
@protocol UADSDictionaryConvertible <NSObject>
- (NSDictionary *)dictionary;
@end

@interface UADSBaseOptions : NSObject<UADSDictionaryConvertible>

@property (nonatomic, strong, readonly) NSDictionary *dictionary;
@property (nonatomic, readwrite) NSString *objectId;

- (instancetype)  init;

@end
