namespace vector3net7;

using System.Numerics;

class Program
{
    static void Main(string[] args)
    {
        var v = new[] { Vector3.Zero, Vector3.One }.Mean();
        System.Console.WriteLine(v);

    }
}
