namespace SearchAThing.Ext.Examples;

class Program
{
    static void Main(string[] args)
    {
        //! [example]
        foreach (var x in TailLike("/var/log/syslog"))
        {
            System.Console.WriteLine(x);
        }
        //! [example]
    }
}
