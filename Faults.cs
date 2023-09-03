using Zs.Common.Models;

namespace DemoBot;

internal static class Faults
{
    public static Fault Unauthorized => new (nameof(Unauthorized));
}