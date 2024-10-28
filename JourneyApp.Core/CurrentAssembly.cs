using System.Reflection;

namespace JourneyApp.Core;

public static class CurrentAssembly
{
    public static Assembly Current => typeof(CurrentAssembly).Assembly;
}