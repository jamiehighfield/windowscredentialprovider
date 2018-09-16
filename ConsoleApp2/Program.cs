using JamieHighfield.CredentialProvider.Sample;
using System;

namespace ConsoleApp2
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            CredentialProviderSample s = new CredentialProviderSample();

            s.AddControls(new JamieHighfield.CredentialProvider.CredentialControlCollection(s));

            Console.ReadLine();
        }
    }
}
