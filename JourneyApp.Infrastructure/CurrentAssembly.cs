using System.Reflection;

namespace JourneyApp.Infrastructure;

public static class CurrentAssembly
{
    public static Assembly Current => typeof(CurrentAssembly).Assembly;
}