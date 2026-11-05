using System.Reflection;

namespace Sergin.Platform;

public static class PlatformAssemblyReference
{
    public static readonly Assembly Assembly = typeof(PlatformAssemblyReference).Assembly;
}
