using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnityEditor.Purchasing")]
[assembly: InternalsVisibleTo("UnityEditor.Purchasing.EditorTests")]
[assembly: InternalsVisibleTo("UnityEngine.Purchasing.RuntimeTests")]
//Needed for Moq to generate mocks from internal interfaces
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
