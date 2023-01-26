using System.Threading;

namespace SearchAThing.Ext.Examples;

class Program
{
    static void Main(string[] args)
    {
        //! [Exec example]
        Task.Run(async () =>
        {
            var q = await Exec("ls", new[] { "-la", "/etc/hosts" }, CancellationToken.None,
                sudo: false,
                redirectStdout: true,
                redirectStderr: false,
                verbose: false);

            if (q.ExitCode == 0)
            {
                var line = q.Output.Lines().First();
                System.Console.WriteLine(line);

                var ss = line.Split(' ');

                System.Console.WriteLine($"perm: {ss[0]}");
                System.Console.WriteLine($"owner: {ss[2]}");
                System.Console.WriteLine($"group: {ss[3]}");
                System.Console.WriteLine($"size: {ss[4]}");
            }

            // RESULT:
            //
            // -rw-r--r-- 1 root root 218 May 11  2020 /etc/hosts
            // perm: -rw-r--r--
            // owner: root
            // group: root
            // size: 218

        }).Wait();

        //! [Exec example]
    }
}
