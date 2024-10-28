using System.Reflection;

namespace JourneyApp.Application;

public static class CurrentAssembly
{
    public static Assembly Current => typeof(Assembly).Assembly;
}