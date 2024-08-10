using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Angel;
using EasyHook;
using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Enums;
using Newtonsoft.Json.Linq;

namespace GPMLoginActive;

public class Form1 : Form
{
	private IContainer components;

	private Guna2GradientButton activebtn;

	private Guna2TextBox tbKEY;

	private Label label1;

	private Label label2;

	private Guna2CircleButton guna2CircleButton1;

	private Guna2CircleButton guna2CircleButton2;

	private Guna2CircleButton guna2CircleButton3;

	private Guna2ProgressBar guna2ProgressBar1;

	private Timer timer1;

	private BackgroundWorker backgroundWorker1;

	public Form1()
	{
		InitializeComponent();
	}

	private void active()
	{
		Process[] processesByName = Process.GetProcessesByName(Path.GetFileNameWithoutExtension("GPMLogin.exe"));
		if (processesByName.Length == 0)
		{
			Console.WriteLine("Target process not found.");
			return;
		}
		int id = processesByName[0].Id;
		string RefChannelName = null;
		RemoteHooking.IpcCreateServer<ServerInterface>(ref RefChannelName, WellKnownObjectMode.SingleCall, Array.Empty<WellKnownSidType>());
		try
		{
			RemoteHooking.Inject(id, InjectionOptions.Default, typeof(Main).Assembly.Location, typeof(Main).Assembly.Location, RefChannelName);
		}
		catch (Exception ex)
		{
			Console.WriteLine("Failed to inject: " + ex.Message);
			return;
		}
		guna2ProgressBar1.Value = 100;
		label1.ForeColor = Color.GreenYellow;
		UpdateLabel("ĐÃ ACTIVE");
	}

	private void UpdateLabel(string text)
	{
		if (label1.InvokeRequired)
		{
			label1.Invoke((Action)delegate
			{
				UpdateLabel(text);
			});
		}
		else
		{
			label1.Text = text;
		}
	}

	private void Updatetb(string text)
	{
		if (tbKEY.InvokeRequired)
		{
			tbKEY.Invoke((Action)delegate
			{
				Updatetb(text);
			});
		}
		else
		{
			tbKEY.Text = text;
		}
	}

	public static string Decrypt(string text)
	{
		byte[] array = Convert.FromBase64String(text);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		byte[] bytes = Encoding.ASCII.GetBytes("0876166547273447");
		rijndaelManaged.KeySize = 128;
		rijndaelManaged.Padding = PaddingMode.PKCS7;
		rijndaelManaged.Mode = CipherMode.ECB;
		using ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor(bytes, null);
		byte[] bytes2 = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
		cryptoTransform.Dispose();
		return Encoding.UTF8.GetString(bytes2);
	}

	private void checkeydocs()
	{
		string text = serial.mamay();
		HttpClient httpClient = new HttpClient();
		string text2 = text;
		Updatetb(text);
		string requestUri = "https://docs.google.com/spreadsheets/d/1qwJdDLNhYbdDBXZDIQIuXvXC1El6ShD6-yy1UPqbzl4/edit?gid=0#gid=0";
		Match match = Regex.Match(httpClient.GetAsync(requestUri).Result.Content.ReadAsStringAsync().Result.ToString().ToString(), text2 + ".*?(?=ok)");
		if (match == Match.Empty)
		{
			UpdateLabel("Lh Duy milano");
			Updatetb(text);
			guna2ProgressBar1.Value = 0;
			timer1.Stop();
			return;
		}
		string[] array = match.ToString().Split('|');
		DateTime now = DateTime.Now;
		int day = now.Day;
		int month = now.Month;
		int year = now.Year;
		string[] array2 = array[1].ToString().Split('/');
		int day2 = int.Parse(array2[0]);
		int month2 = int.Parse(array2[1]);
		int year2 = int.Parse(array2[2]);
		DateTime value = new DateTime(year, month, day);
		if ((int)Math.Ceiling(new DateTime(year2, month2, day2).Subtract(value).TotalDays) <= 0)
		{
			label1.Text = "Lh duy milano";
		}
		else
		{
			active();
		}
	}

	private void checkkeyweb()
	{
		string value = null;
		if (Dns.GetHostAddresses("http://tooldoithongtin.liveblog365.com/")[1].ToString() == "127.0.0.1")
		{
			MessageBox.Show("Lỗi kết nối", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			Environment.Exit(1);
		}
		string text = serial.mamay();
		HttpWebRequest obj = (HttpWebRequest)WebRequest.Create("https://shop.phanmemgiare.net/api2?code=60444b73f24c62409634019bbb296a10&key_active=" + text);
		WebRequest.DefaultWebProxy = new WebProxy();
		obj.Method = "GET";
		HttpWebResponse httpWebResponse = (HttpWebResponse)obj.GetResponse();
		if (httpWebResponse.StatusCode == HttpStatusCode.OK)
		{
			using Stream stream = httpWebResponse.GetResponseStream();
			using StreamReader streamReader = new StreamReader(stream);
			value = streamReader.ReadToEnd();
		}
		httpWebResponse.Close();
		if (!(new X509Certificate2(ServicePointManager.FindServicePoint(new Uri("http://tooldoithongtin.liveblog365.com/")).Certificate).GetPublicKeyString() == "3082010A02820101009DAA1A2832ED5EF005CE9967CF7BC065981BEC7CE17EA06693E88934D5159126E9F59032714FF3F108FD4B430B6D65372C07DBB484C1EB1AB0027540FBFD71B7712DBFF8BA13812AB76E1CEE966A5363B8538AEBED94962B291F3DC859C5DDDCD690E642A6B1F6D343ACB2AF40F73F473FCDE9AD921035B8E75B8D1BAE096A5109B03477E0227274251923CEB8D084CFA65EAFDB529A4303077843E407556F3D533816BCBC6DE714ED4D1A9BEC45268B3DACD00438DA88344C80D87E2EAF1BBFB9F77EF42BFBDA1D4FBC8C9D937C0EC25317B6099E403A23EFE324AF5BD31ED26EE800682D74A013152B6158A43D9D972FAB3BF5228EF8B64C20E085C49B7C9F0203010001"))
		{
			return;
		}
		if (!string.IsNullOrEmpty(value))
		{
			JObject jObject = JObject.Parse(Decrypt(value));
			if (!(jObject["status"].ToString() == "true") || !(jObject["expired"].ToString() == "false"))
			{
				UpdateLabel("Lh duy milano");
				guna2ProgressBar1.Value = 0;
				timer1.Stop();
				return;
			}
			bool flag;
			if (jObject["key_active"].ToString() == text)
			{
				JToken jToken = jObject["days"];
				flag = jToken != null && jToken.Value<int>() > 0;
			}
			else
			{
				flag = false;
			}
			if (flag)
			{
				active();
			}
			else
			{
				UpdateLabel("Lh duy milano");
			}
		}
		else
		{
			UpdateLabel("Lh duy milano");
			guna2ProgressBar1.Value = 0;
			timer1.Stop();
		}
	}

	private void Form1_Load(object sender, EventArgs e)
	{
	}

	private void activebtn_Click(object sender, EventArgs e)
	{
		active();
	}

	private void guna2CircleButton1_Click(object sender, EventArgs e)
	{
		Process.Start("https://www.facebook.com/profile.php?id=100004387351588&mibextid=LQQJ4d");
	}

	private void guna2CircleButton2_Click(object sender, EventArgs e)
	{
		Process.Start("https://zalo.me/0846701333");
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		guna2ProgressBar1.Value += 1;
	}

	private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
	{
		checkeydocs();
	}

	private void guna2CircleButton3_Click(object sender, EventArgs e)
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GPMLoginActive.Form1));
		this.activebtn = new Guna.UI2.WinForms.Guna2GradientButton();
		this.tbKEY = new Guna.UI2.WinForms.Guna2TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.guna2CircleButton1 = new Guna.UI2.WinForms.Guna2CircleButton();
		this.guna2CircleButton2 = new Guna.UI2.WinForms.Guna2CircleButton();
		this.guna2CircleButton3 = new Guna.UI2.WinForms.Guna2CircleButton();
		this.guna2ProgressBar1 = new Guna.UI2.WinForms.Guna2ProgressBar();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		base.SuspendLayout();
		this.activebtn.Animated = true;
		this.activebtn.AutoRoundedCorners = true;
		this.activebtn.BorderRadius = 35;
		this.activebtn.FillColor = System.Drawing.Color.FromArgb(128, 255, 128);
		this.activebtn.FillColor2 = System.Drawing.Color.Cyan;
		this.activebtn.Font = new System.Drawing.Font("Century Gothic", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.activebtn.ForeColor = System.Drawing.Color.White;
		this.activebtn.Location = new System.Drawing.Point(416, 35);
		this.activebtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.activebtn.Name = "activebtn";
		this.activebtn.Size = new System.Drawing.Size(214, 72);
		this.activebtn.TabIndex = 7;
		this.activebtn.Text = "ACTIVE";
		this.activebtn.Click += new System.EventHandler(activebtn_Click);
		this.tbKEY.Animated = true;
		this.tbKEY.AutoRoundedCorners = true;
		this.tbKEY.BackColor = System.Drawing.Color.Transparent;
		this.tbKEY.BorderColor = System.Drawing.SystemColors.ControlDark;
		this.tbKEY.BorderRadius = 36;
		this.tbKEY.BorderThickness = 2;
		this.tbKEY.Cursor = System.Windows.Forms.Cursors.IBeam;
		this.tbKEY.DefaultText = "";
		this.tbKEY.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
		this.tbKEY.DisabledState.FillColor = System.Drawing.Color.FromArgb(226, 226, 226);
		this.tbKEY.DisabledState.ForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
		this.tbKEY.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
		this.tbKEY.FocusedState.BorderColor = System.Drawing.Color.FromArgb(94, 148, 255);
		this.tbKEY.Font = new System.Drawing.Font("Century Gothic", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tbKEY.ForeColor = System.Drawing.Color.FromArgb(255, 128, 128);
		this.tbKEY.HoverState.BorderColor = System.Drawing.Color.FromArgb(94, 148, 255);
		this.tbKEY.IconLeft = (System.Drawing.Image)resources.GetObject("tbKEY.IconLeft");
		this.tbKEY.IconLeftOffset = new System.Drawing.Point(5, 0);
		this.tbKEY.Location = new System.Drawing.Point(28, 34);
		this.tbKEY.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
		this.tbKEY.Name = "tbKEY";
		this.tbKEY.PasswordChar = '\0';
		this.tbKEY.PlaceholderForeColor = System.Drawing.SystemColors.ControlDarkDark;
		this.tbKEY.PlaceholderText = "KEY";
		this.tbKEY.SelectedText = "";
		this.tbKEY.Size = new System.Drawing.Size(374, 75);
		this.tbKEY.TabIndex = 6;
		this.tbKEY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Century Gothic", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.LightCoral;
		this.label1.Location = new System.Drawing.Point(183, 145);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(150, 28);
		this.label1.TabIndex = 8;
		this.label1.Text = "CHƯA ACTIVE";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Century Gothic", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.DimGray;
		this.label2.Location = new System.Drawing.Point(26, 145);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(103, 28);
		this.label2.TabIndex = 9;
		this.label2.Text = "trạng thái :";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.guna2CircleButton1.BorderColor = System.Drawing.Color.FromArgb(59, 89, 152);
		this.guna2CircleButton1.BorderThickness = 3;
		this.guna2CircleButton1.FillColor = System.Drawing.Color.Empty;
		this.guna2CircleButton1.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.guna2CircleButton1.ForeColor = System.Drawing.Color.White;
		this.guna2CircleButton1.Image = (System.Drawing.Image)resources.GetObject("guna2CircleButton1.Image");
		this.guna2CircleButton1.Location = new System.Drawing.Point(405, 128);
		this.guna2CircleButton1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.guna2CircleButton1.Name = "guna2CircleButton1";
		this.guna2CircleButton1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
		this.guna2CircleButton1.Size = new System.Drawing.Size(64, 62);
		this.guna2CircleButton1.TabIndex = 16;
		this.guna2CircleButton1.Click += new System.EventHandler(guna2CircleButton1_Click);
		this.guna2CircleButton2.BorderColor = System.Drawing.Color.DodgerBlue;
		this.guna2CircleButton2.BorderThickness = 3;
		this.guna2CircleButton2.FillColor = System.Drawing.Color.Empty;
		this.guna2CircleButton2.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.guna2CircleButton2.ForeColor = System.Drawing.Color.White;
		this.guna2CircleButton2.Image = (System.Drawing.Image)resources.GetObject("guna2CircleButton2.Image");
		this.guna2CircleButton2.Location = new System.Drawing.Point(490, 128);
		this.guna2CircleButton2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.guna2CircleButton2.Name = "guna2CircleButton2";
		this.guna2CircleButton2.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
		this.guna2CircleButton2.Size = new System.Drawing.Size(64, 62);
		this.guna2CircleButton2.TabIndex = 17;
		this.guna2CircleButton2.Click += new System.EventHandler(guna2CircleButton2_Click);
		this.guna2CircleButton3.BorderColor = System.Drawing.Color.DarkTurquoise;
		this.guna2CircleButton3.BorderThickness = 3;
		this.guna2CircleButton3.FillColor = System.Drawing.Color.Empty;
		this.guna2CircleButton3.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.guna2CircleButton3.ForeColor = System.Drawing.Color.White;
		this.guna2CircleButton3.Image = (System.Drawing.Image)resources.GetObject("guna2CircleButton3.Image");
		this.guna2CircleButton3.Location = new System.Drawing.Point(578, 128);
		this.guna2CircleButton3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.guna2CircleButton3.Name = "guna2CircleButton3";
		this.guna2CircleButton3.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
		this.guna2CircleButton3.Size = new System.Drawing.Size(64, 62);
		this.guna2CircleButton3.TabIndex = 18;
		this.guna2CircleButton3.Click += new System.EventHandler(guna2CircleButton3_Click);
		this.guna2ProgressBar1.AutoRoundedCorners = true;
		this.guna2ProgressBar1.BorderRadius = 3;
		this.guna2ProgressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.guna2ProgressBar1.Location = new System.Drawing.Point(0, 213);
		this.guna2ProgressBar1.Name = "guna2ProgressBar1";
		this.guna2ProgressBar1.ProgressColor = System.Drawing.Color.Lime;
		this.guna2ProgressBar1.ProgressColor2 = System.Drawing.Color.Cyan;
		this.guna2ProgressBar1.Size = new System.Drawing.Size(668, 9);
		this.guna2ProgressBar1.TabIndex = 19;
		this.guna2ProgressBar1.Text = "guna2ProgressBar1";
		this.guna2ProgressBar1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
		this.timer1.Interval = 70;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 20f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Control;
		base.ClientSize = new System.Drawing.Size(668, 222);
		base.Controls.Add(this.guna2ProgressBar1);
		base.Controls.Add(this.guna2CircleButton3);
		base.Controls.Add(this.guna2CircleButton2);
		base.Controls.Add(this.guna2CircleButton1);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.activebtn);
		base.Controls.Add(this.tbKEY);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "Form1";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "ACTIVE_GPM_Duy Milano";
		base.Load += new System.EventHandler(Form1_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
