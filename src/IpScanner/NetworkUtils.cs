using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace IpScanner;

public static class NetworkUtils
{
    public static IEnumerable<IPAddress> GenerateIpAddressesList(
        IPAddress startIp,
        IPAddress endIp)
    {
        yield return startIp;
        IPAddress iph = startIp;
        while (true)
        {
            iph = Increment(iph);

            if (iph.Equals(endIp))
                break;

            yield return iph;
        }
        yield return endIp;
    }

    public static IPAddress Increment(IPAddress ip)
    {
        var bytes = ip.GetAddressBytes();

        if (++bytes[3] == 0)
            if (++bytes[2] == 0)
                if (++bytes[1] == 0)
                    ++bytes[0];

        return new IPAddress(bytes);
    }

    public static string GetStatusOfPingSafely(IPAddress ip)
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
