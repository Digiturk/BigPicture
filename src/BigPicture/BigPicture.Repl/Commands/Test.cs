using BigPicture.IOC;
using Replify.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Repl.Commands
{
    [Command("test")]
    public class Test : BaseCommand
    {
        public override void Run(string param = "")
        {
            if (param == "repo")
            {
                
            }
            else
            {
                Console.Error.WriteLine("Unknown parameter. Test parameter should be one of these values [repo]");
            }
        }
    }

    public interface InterA
    {
        void Write();
    }

    public class ClassA : InterA
    {
        public void Write()
        {
            Console.WriteLine("AAAA");
        }
    }

    public interface InterB
    {
        void Write();
    }

    public class ClassB : InterB
    {
        public void Write()
        {
            Console.WriteLine("BBBB");
        }
    }
}
