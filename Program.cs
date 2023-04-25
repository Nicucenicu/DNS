using System;
using System.Net;
using DnsClient;
namespace DNSResolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var lookup = new LookupClient();
            string dnsServer = string.Empty; // DNS server-ul folosit initial va fi cel indicat de sistem

            Console.WriteLine("Introduceti o comanda:\nresolve domain\nresolve ip\nuse dns <ip>");
            while (true)
            {
                string command = Console.ReadLine();
                string[] commandParts = command.Split(' ');

                if (commandParts.Length >= 2)
                {
                    string cmd = commandParts[0].ToLower();
                    string param = commandParts[1];

                    switch (cmd)
                    {
                        case "resolve":
                            if (param == "domain")
                            {
                                Console.WriteLine("Introduceti numele de domeniu:");
                                string domain = Console.ReadLine();
                                IPHostEntry entry = lookup.GetHostEntry(domain);
                                Console.WriteLine("Adrese IP asociate domeniului {0}:", domain);
                                foreach (IPAddress ip in entry.AddressList)
                                {
                                    Console.WriteLine(ip.ToString());
                                }
                            }
                            else if (param == "ip")
                            {
                                Console.WriteLine("Introduceti adresa IP:");
                                string ip = Console.ReadLine();
                                IPAddress ipAddress = IPAddress.Parse(ip);
                                IPHostEntry entry = lookup.GetHostEntry(ipAddress);
                                Console.WriteLine("Domenii asociate adresei IP {0}:\n", ipAddress);
                                Console.WriteLine(lookup.GetHostName(ipAddress).ToString());
                                foreach (var name in entry.Aliases)
                                {
                                    Console.WriteLine(name.ToString());
                                }

                            }
                            break;
                        case "use":
                            if (param == "dns")
                            {
                                if (commandParts.Length >= 3)
                                {
                                    dnsServer = commandParts[2];
                                    var endpoint = new IPEndPoint(IPAddress.Parse(dnsServer), 53);
                                    lookup = new LookupClient(endpoint);
                                    Console.WriteLine("DNS server-ul a fost schimbat la {0}", dnsServer);
                                }
                                else
                                {
                                    Console.WriteLine("Comanda nu a fost utilizata corect. Utilizare: use dns <ip>");
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("Comanda invalida.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Comanda invalida.");
                }
            }
        }
    }
}
