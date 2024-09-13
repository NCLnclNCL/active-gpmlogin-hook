using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using EasyHook;
using HarmonyLib;

namespace Angel;

public class Main : IEntryPoint
{
	[HarmonyPatch]
	private class EncryptPatch
	{
		private static MethodBase TargetMethod()
		{
			return typeof(RSACryptoServiceProvider).GetMethod("Encrypt", new Type[2]
			{
				typeof(byte[]),
				typeof(bool)
			});
		}

		private static void Prefix(byte[] rgb, bool fOAEP)
		{
			string[] array = Encoding.UTF8.GetString(rgb).Split(new string[1] { "otp\":\"" }, StringSplitOptions.None)[1].Split(new string[1] { "\"" }, StringSplitOptions.None);
			File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\tokenfb2", array[0]);
		}
	}
		[HarmonyPatch]
		private class AntiCheckHookOrPatch
		{
			// Token: 0x06000006 RID: 6 RVA: 0x000021D4 File Offset: 0x000003D4
			private static MethodBase TargetMethod()
			{
				return typeof(AppDomain).GetMethod("GetAssemblies");
			}

			// Token: 0x06000007 RID: 7 RVA: 0x000021FC File Offset: 0x000003FC
			private static bool Prefix(ref Assembly[] __result)
			{
				Assembly assembly = typeof(object).Assembly;
				Assembly[] array = new Assembly[] { assembly };
				File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\ncl", "Ok " + assembly.FullName);
				__result = array;
				return false;
			}
		}
 [HarmonyPatch(typeof(StringReader))]
		[HarmonyPatch(MethodType.Constructor)]
		[HarmonyPatch(new Type[] { typeof(string) })]
	public static class StringReaderConstructorPatch
	{
		private static void Prefix(ref string s)
		{
			try
			{
				if (s.Contains("License kh\\u00f4ng t\\u1ed3n t\\u1ea1i"))
				{
					string text = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\tokenfb2");
					string text2 = RSAEncrypt("{\"success\":true,\"otp\":\"" + text + "\"}");
					string text3 = "3.0.2-stable-10";
					s = "{\r\n    \"data\": \"" + text2 + "\",\r\n    \"addination_data\": {\r\n      \"message\": \"Khách hàng: ADMIN. Sử dụng: 9 / 9\",\r\n      \"project\": {\r\n        \"name\": \"GPM Login\",\r\n        \"version\": \"" + text3 + "\",\r\n        \"update_url\": \"/storage/updates/gpm login_update_" + text3 + "\",\r\n        \"update_file_name\": \"gpm login_update_" + text3 + "\",\r\n        \"message\": \"" + text + "\",\r\n        \"meta_data\": \"{\\n  \\\"version\\\": \\\"4.5\\\",\\n  \\\"url\\\": \\\"https://assets.giaiphapmmo.net/gpm_browsers/gpm_browser_v105.zip\\\",\\n  \\\"other_versions\\\": [\\n    {\\n      \\\"name\\\": \\\"gpm_browser_chromium_core_124\\\",\\n      \\\"version\\\": \\\"1.1\\\",\\n      \\\"url\\\": \\\"https://assets.giaiphapmmo.net/gpm_browsers/gpm_browser_v124.zip\\\"\\n    },\\n    {\\n      \\\"name\\\": \\\"gpm_browser_chromium_core_121\\\",\\n      \\\"version\\\": \\\"1.2\\\",\\n      \\\"url\\\": \\\"https://assets.giaiphapmmo.net/gpm_browsers/gpm_browser_v121.zip\\\"\\n    },\\n    {\\n      \\\"name\\\": \\\"gpm_browser_chromium_core_119\\\",\\n      \\\"version\\\": \\\"1.5\\\",\\n      \\\"url\\\": \\\"https://assets.giaiphapmmo.net/gpm_browsers/gpm_browser_v119.zip\\\"\\n    },\\n    {\\n      \\\"name\\\": \\\"gpm_browser_firefox_core_119\\\",\\n      \\\"version\\\": \\\"1.3\\\",\\n      \\\"url\\\": \\\"https://assets.giaiphapmmo.net/gpm_browsers/gpm_firefox_v119.zip\\\"\\n    },\\n    {\\n      \\\"name\\\": \\\"gpm_browser_chromium_core_115\\\",\\n      \\\"version\\\": \\\"1.3\\\",\\n      \\\"url\\\": \\\"https://assets.giaiphapmmo.net/gpm_browsers/gpm_browser_v115.zip\\\"\\n    },\\n    {\\n      \\\"name\\\": \\\"gpm_browser_chromium_core_111\\\",\\n      \\\"version\\\": \\\"1.8\\\",\\n      \\\"url\\\": \\\"https://assets.giaiphapmmo.net/gpm_browsers/gpm_browser_v111.zip\\\"\\n    },\\n    {\\n      \\\"name\\\": \\\"gpm_browser_chromium_core_107\\\",\\n      \\\"version\\\": \\\"1.8\\\",\\n      \\\"url\\\": \\\"https://assets.giaiphapmmo.net/gpm_browsers/gpm_browser_v107.zip\\\"\\n    }\\n  ],\\n  \\\"up_device_rules\\\": [\\n    {\\n      \\\"from\\\": 1,\\n      \\\"to\\\": 5,\\n      \\\"price\\\": 1500\\n    },\\n    {\\n      \\\"from\\\": 6,\\n      \\\"to\\\": 10,\\n      \\\"price\\\": 1200\\n    },\\n    {\\n      \\\"from\\\": 11,\\n      \\\"to\\\": 10000,\\n      \\\"price\\\": 1000\\n    }\\n  ],\\n  \\\"up_device_max\\\": 20,\\n  \\\"global_messages\\\": [\\n    {\\n      \\\"type\\\": 1,\\n      \\\"message\\\": \\\"3.846.475 profiles đang được sử dụng bởi GPM-Login (số liệu 26-04-2024)\\\",\\n      \\\"details_url\\\": \\\"https://facebook.com/giaiphapmmodotnet\\\"\\n    },\\n    {\\n      \\\"type\\\": 2,\\n      \\\"message\\\": \\\"Phần mềm có thể hoạt động không ổn định nếu chưa thêm ngoại lệ vào antivirus\\\",\\n      \\\"details_url\\\": \\\"https://facebook.com/giaiphapmmodotnet\\\"\\n    },\\n    {\\n      \\\"type\\\": 1,\\n      \\\"message\\\": \\\"Chúng tôi là một team nhỏ, vì vậy, trong một số thời điểm, thời gian hỗ trợ sẽ không được nhanh chóng như các bên hoạt động theo mô hình công ty, đổi lại chúng tôi hướng tới chất lượng phần mềm và giá cả phải chăng nhất cho người sử dụng.\\\",\\n      \\\"details_url\\\": \\\"https://gpmloginapp.com\\\"\\n    }\\n  ],\\n  \\\"update_option\\\": {\\n    \\\"require_update_versions\\\": [\\n      \\\"3.0.0-beta-10\\\",\\n      \\\"3.0.0-beta-10.01\\\",\\n      \\\"3.0.0-beta-10.02\\\",\\n      \\\"3.0.0-beta-10.03\\\",\\n      \\\"3.0.0-beta-10.04\\\",\\n      \\\"3.0.0-beta-10.05\\\",\\n      \\\"3.0.0-stable\\\",\\n      \\\"3.0.0-stable-01\\\",\\n      \\\"3.0.0-stable-02\\\",\\n      \\\"3.0.0-stable-03\\\",\\n      \\\"3.0.0-stable-04\\\",\\n      \\\"3.0.0-stable-05\\\",\\n      \\\"3.0.0-stable-06\\\",\\n      \\\"3.0.0-stable-07\\\",\\n      \\\"3.0.0-stable-08\\\",\\n      \\\"3.0.0-stable-09\\\",\\n      \\\"3.0.1-stable-01\\\",\\n      \\\"3.0.1-stable-02\\\",\\n      \\\"3.0.1-stable-03\\\",\\n      \\\"3.0.1-stable-04\\\",\\n      \\\"3.0.1-stable-05\\\",\\n      \\\"3.0.1-stable-06\\\",\\n      \\\"3.0.1-stable-07\\\",\\n      \\\"3.0.1-stable-08\\\",\\n      \\\"3.0.1-stable-09\\\",\\n      \\\"3.0.1-stable-10\\\", \\\"3.0.2-stable-01\\\",\\\"3.0.2-stable-02\\\",\\\"3.0.2-stable-03\\\",\\\"3.0.2-stable-04\\\"\\n    ]\\n  }\\n}\"\r\n      },\r\n      \"customer_name\": \"ADMIN\",\r\n      \"active_modules\": [\"private_server\"]\r\n    },\r\n    \"message\": \"OK\"\r\n}";
				}
			}
			catch (Exception ex)
			{
				File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\tokenfb3.txt", ex.Message);
			}
		}

		public static string RSAEncrypt(string text)
		{
			string xmlString = "<RSAKeyValue><Modulus>mQtu+HUWB5yVVaX3FX7E/RsSEgK0mYp37KSl/Qgx0KZPRH1Y7/dZiYg6lT+syNDmjwvvXwPlSk6V0SRdCVAIry0IzvfecajBgNJVW3g4/Cc9usmlxI0FrMoayb06Cl+lorFr7sp0Ou8LGoxAX4qtQhHUlF8JQXupe/57Ygf5DYNuCA5iQ1CFqylIGPLeU85gmnnTdqbIQpMw+i7tiKZ9P9oiJ3aj9yqo3kel+N7vsHUvbEv65Lnp7cKAHqdEYhfpDTEkG1kys4bgSnfcfa69BDcCUnksurd0MUsh+8TqTilmiABed1c8984/h5pJnOLSLC/SA4y3g+mPJTi5GZcssGag69hm+UU1Jq/cB5kJa8PsZnX/ol5s1yvmzsqlwETLEUFUV1NsZraA3gjUckX86bDTUNW8hQcg4/ZhqykNklqcqdf3ApoKln7EgIJbQpyKwz1QyRGsOKgn7Tb2OGjWUxCAjYeC9L3oVGmh7qaefDUufr/izsrR+/56yh7s6FsV4v8edqbzddSmEcAZXK3LrgwwgxP3BDvZAZ4C7OHOO9GwrAGEWHkIbu3pmS4qf/OXlHetEJaI8NQhPYOA0H5G5VZySJawsEtQC43enpnJ6f+KafrtFeE7lwMA8gMjDf03QQ+i6yc5Q6ySdWQT5D6WVf/54/MXgIP7fReQsMRYM+0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
			RSA rSA = RSA.Create();
			rSA.FromXmlString(xmlString);
			return Convert.ToBase64String(rSA.Encrypt(Encoding.UTF8.GetBytes(text), RSAEncryptionPadding.Pkcs1));
		}
	}

	private readonly string _channelName;

	private LocalHook _hook;

	private LocalHook _encryptHook;

	public Main(RemoteHooking.IContext context, string channelName)
	{
		_channelName = channelName;
	}

	public void Run(RemoteHooking.IContext context, string channelName)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		try
		{
			Harmony val = new Harmony("Destroy");
			val.PatchAll(Assembly.GetExecutingAssembly());
			RemoteHooking.WakeUpProcess();
		}
		catch (Exception)
		{
		}
		while (true)
		{
			Thread.Sleep(100);
		}
	}
}
