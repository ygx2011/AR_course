using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnityEditor.Purchasing")]
[assembly: InternalsVisibleTo("UnityEngine.Purchasing.Stores")]
[assembly: InternalsVisibleTo("UnityEngine.Purchasing.RuntimeTests")]
//Needed for Moq to generate mocks from internal interfaces
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
