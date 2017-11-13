using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace IpScanner
{
    internal class Program
    {
        private static bool IsDisplayUnknownHost => false; // TODO: add ability for change from args

        private static event EventHandler<string> Print;

        private static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2) throw new Exception("Need two arguments: <Start IP> <End IP>");

                Print += (sender, s) => Console.WriteLine(s);

                var startIp = IPAddress.Parse(args[0]);
                var endIp = IPAddress.Parse(args[1]);

                // TODO: Add IP validation (current ip + mask of network subset)

                DisplayHeader(startIp, endIp);
                DisplayHostDetails(Dns.GetHostEntry(Dns.GetHostName()));

                if (Parallel.ForEach(GenerateIpAddressesList(startIp, endIp), CheckIpAddress).IsCompleted)
                {
                    Console.WriteLine("Finish!");
                }

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
#if DEBUG
                Console.ReadKey();
#endif
            }
        }

        private static void CheckIpAddress(IPAddress address)
        {
            var isUnknown = false;
            var logMessage = address + "\t-\t";

            try
            {
                logMessage += GetStatusOfPingSafely(address) + "  \t-\t";

                var host = Dns.GetHostEntry(address);

                logMessage += host.HostName;
            }
            catch (Exception)
            {
                isUnknown = IsDisplayUnknownHost;
            }
            finally
            {
                if (!isUnknown)
                    Print?.Invoke(null, logMessage);
            }
        }

        private static IEnumerable<IPAddress> GenerateIpAddressesList(IPAddress startIp, IPAddress endIp)
        {
            yield return startIp;
            IPAddress iph = startIp;
            while (true)
            {
                iph = Increament(iph);

                if (iph.Equals(endIp))
                    break;

                yield return iph;
            }
            yield return endIp;
        }

        private static IPAddress Increament(IPAddress ip)
        {
            var bytes = ip.GetAddressBytes();

            if (++bytes[3] == 0)
                if (++bytes[2] == 0)
                    if (++bytes[1] == 0)
                        ++bytes[0];

            return new IPAddress(bytes);
        }

        private static void DisplayHeader(IPAddress startIp, IPAddress endIp)
        {
            Console.WriteLine($"Start IP: {startIp}");
            Console.WriteLine($"End IP:   {endIp}\n");
        }

        private static void DisplayHostDetails(IPHostEntry host)
        {
            Console.WriteLine($"Current HOST:\n\t{host.HostName}\nIP addresses:");

            foreach (var ipAddress in host.AddressList)
            {
                Console.WriteLine($"\t{ipAddress}");
            }

            Console.WriteLine(string.Empty);
        }

        private static string GetStatusOfPingSafely(IPAddress ip)
        {
            try
            {
                return new Ping().Send(ip).Status.ToString();
            }
            catch (Exception e)
            {
                return $"<ERROR> {e.Message}";
            }
        }
    }
}
