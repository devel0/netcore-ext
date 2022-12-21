using System.Linq;

using SearchAThing;

namespace example
{
    class Program
    {
        static void Main(string[] args)
        {
            var q = new[] { 1, 2, 4 };

            var last = 0d;

            // q2 : sum of all elements except last ( save last into `last` var )
            var q2 = q.WithIndexIsLast().Select(w =>
            {
                if (w.isLast)
                {
                    last = w.item;
                    return 0;
                }
                return w.item;
            }).Sum();

            if (q2 == 3 && last == 4)
                System.Console.WriteLine($"tests succeeded");
            else
                System.Console.WriteLine($"tests failed");
        }
    }
}
