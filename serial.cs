using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace GPMLoginActive;

internal class serial
{
	public static string mamay()
	{
		return $"{cut($"{cpuId()}_{MainBoardSerialNumber()}_{biosSerialNumber()}_{Environment.MachineName}").Substring(0, 10).ToUpper()}";
	}

	public static string identifier(string wmiClass, string wmiProperty)
	{
		string text = "";
		try
		{
			ManagementClass managementClass = new ManagementClass(wmiClass);
			ManagementObjectCollection instances = managementClass.GetInstances();
			foreach (ManagementBaseObject item in instances)
			{
				ManagementObject managementObject = (ManagementObject)item;
				if (text == "")
				{
					try
					{
						text = managementObject[wmiProperty].ToString();
					}
					catch
					{
						continue;
					}
					break;
				}
			}
			return text;
		}
		catch
		{
		}
		return text;
	}

	public static string cut(string string_4)
	{
		try
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] bytes = Encoding.UTF8.GetBytes(string_4);
			byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes, 0, bytes.Length);
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				text += $"{array[i]:x2}";
			}
			return text;
		}
		catch
		{
			return "";
		}
	}

	public static string cpuId()
	{
		string text = identifier("Win32_Processor", "UniqueId");
		if (text == "")
		{
			text = identifier("Win32_Processor", "ProcessorId");
			if (text == "")
			{
				text = identifier("Win32_Processor", "Name");
				if (text == "")
				{
					text = identifier("Win32_Processor", "Manufacturer");
				}
				text += identifier("Win32_Processor", "MaxClockSpeed");
			}
		}
		return text;
	}

	public static string biosSerialNumber()
	{
		return identifier("Win32_BIOS", "SerialNumber");
	}

	public static string MainBoardSerialNumber()
	{
		return identifier("Win32_BaseBoard", "SerialNumber");
	}
}
