using System.Text;
using System.Threading.Tasks;

namespace IpScanner;

internal static class Program
{
	private static bool IsDisplayUnknownHost = true;

	private static Action<string?> Print { get; } = Console.WriteLine;

	static void Main(string[] args)
	{
		try
		{
			if (args.Length < 2)
				throw new Exception("Need two arguments: <Start IP> <End IP>");

			if (args.Length == 3)
			{
				if (args[2] != "--unknown-hosts")
					throw new Exception("Invalid flag found.");
				IsDisplayUnknownHost = args[2] == "--unknown-hosts";
			}

			var startIp = IPAddress.Parse(args[0]);
			var endIp = IPAddress.Parse(args[1]);

			// TODO: Add IP validation (current ip + mask of network subset)

			DisplayHeader(startIp, endIp);
			DisplayHostDetails(Dns.GetHostEntry(Dns.GetHostName()));

			var result = Parallel.ForEach(
				NetworkUtils.GenerateIpAddressesList(startIp, endIp),
				CheckIpAddress);

			if (result.IsCompleted)
			{
				Console.WriteLine("Finish!");
			}

			Console.ReadKey();
		}
		catch (Exception e)
		{
			Print($"Error: {e}");
#if DEBUG
			Console.ReadKey();
#endif
		}
	}

	private static void DisplayHeader(IPAddress startIp, IPAddress endIp)
	{
		Print($"Start IP: {startIp}");
		Print($"End IP:   {endIp}\n");
	}

	private static void DisplayHostDetails(IPHostEntry host)
	{
		Print($"Current HOST:\n\t{host.HostName}\nIP addresses:");

		foreach (var ipAddress in host.AddressList)
		{
			Print($"\t{ipAddress}");
		}

		Print(string.Empty);
	}

	private static void CheckIpAddress(IPAddress address)
	{
		var isUnknown = false;
		var messageBuilder = new StringBuilder()
			.Append(address)
			.Append("\t-\t");

		try
		{
			var status = NetworkUtils.GetStatusOfPingSafely(address);

			messageBuilder
				.Append(status)
				.Append("  \t-\t");

			var host = Dns.GetHostEntry(address);

			messageBuilder.Append(host.HostName);
		}
		catch (Exception)
		{
			isUnknown = IsDisplayUnknownHost;
		}
		finally
		{
			if (!isUnknown)
				Print(messageBuilder.ToString());
		}
	}
}