using System.Diagnostics;
using System.IO;
using RadioLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace radioLog
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		int Counter = 0;
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Input_KeyUp(object sender, KeyEventArgs e)
		{
			//MessageBox.Show("typed");
			if (e.Key == Key.Enter) { Recoginize(Input.Text); }

		}

		void Recoginize(string InputString)
		{
			string[] Part = InputString.Split(' ');
			Inline inline = new();
			foreach (string part in Part)
			{
				//MessageBox.Show(part + "的ML预测结果是" + PredictKeyword(part));

				switch (PredictKeyword(part))
				{
					case "Callsign":
						inline.Callsign = part; break;
					case "RST":
						inline.RSTreport = part; break;
					case "RIG":
						inline.RIG = part; break;
					case "ANT":
						inline.ANT = part; break;
					case "QTH":
						inline.QTH = part; break;
					case "Power":
						inline.Power = part; break;

				}

				#region 传统判断
				//MatchCollection mc = Regex.Matches(part, "(B|b)[ADIadi][0-9][a-zA-Z]{3}");
				//if (mc.Count > 0)// callsigh
				//{
				//    foreach (Match m in mc)
				//    {
				//        //Console.WriteLine(m);
				//        //MessageBox.Show("callsign: " + m.Value);
				//    }
				//    //MessageBox.Show("callsigh: " + mc[0].Value);
				//    Log.Text += mc[0].Value + "\t";

				//}

				//mc = Regex.Matches(part, "(tx|TX)[1-5][1-9]");
				//if (mc.Count > 0)
				//{
				//    //MessageBox.Show("tx: " + mc[0].Value);
				//    Log.Text += mc[0].Value + "\t";
				//}

				//mc = Regex.Matches(part, "(rx|rX)[1-5][1-9]");
				//if (mc.Count > 0)
				//{
				//    //MessageBox.Show("rx: " + mc[0].Value);
				//    Log.Text += mc[0].Value + "\t";
				//}

				//string[] Brand = { "icom", "yaesu", "moto", "kenwood", "hytera", "xiegu", "zastone", "wouxun", "senhaix", "bf", "quansheng" };
				//foreach (string b in Brand)
				//{
				//    if (part.Contains(b))
				//    {
				//        //MessageBox.Show("rig: " + part);
				//        Log.Text += part + "\t";
				//    }
				//}
				#endregion

			}
			//MessageBox.Show(inline.ToString(), InputString);
			AddInlineToTable(inline);

			//Log.Text += "\r\n";
			Input.Text = string.Empty;
		}

		string PredictKeyword(string keyword)
		{
			//Load sample data
			var sampleData = new 业余无线电关键词分类.ModelInput()
			{
				//Keyword = @"BI7OXI",
				Keyword = keyword,
			};

			//Load model and predict output
			var result = 业余无线电关键词分类.Predict(sampleData);

			return result.PredictedLabel;

		}


		private void Confirm_Click(object sender, RoutedEventArgs e)
		{
			Recoginize(Input.Text);
		}
		void AddInlineToTable(Inline inline)
		{
			//Log.Text += inline.Callsign + "\t";
			//Log.Text += inline.RSTreport + "\t";
			////Log.Text += inline.RSTsituation + "\t";
			//Log.Text += inline.RIG + "\t";
			//Log.Text += inline.ANT + "\t";
			//Log.Text += inline.QTH + "\r\n";

			Log.Text += Counter.ToString() + ",";
			Log.Text += inline.Callsign + ",";
			Log.Text += inline.RSTreport + ",";
			//Log.Text += inline.RSTsituation + ",";
			Log.Text += inline.RIG + ",";
			Log.Text += inline.ANT + ",";
			Log.Text += inline.Power + ",";
			Log.Text += inline.QTH + ",";
			Log.Text += Input.Text + "\r\n";

			Counter++;

		}

		private void ClearCounter_Click(object sender, RoutedEventArgs e)
		{
			Counter = int.Parse(CounterPreset.Text);
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			if (!File.Exists("output.csv"))
				File.Create("output.csv").Close();

			File.WriteAllText("output.csv", Log.Text);
			//Process.Start("output.csv");
			var directory = Directory.GetCurrentDirectory();

            Process.Start("Explorer", "/select," + directory + "\\output.csv");
		}
	}

	class Inline
	{
		public string? Callsign { get; set; }
		public string? RSTreport { get; set; }
		public string? RSTsituation { get; set; }
		public string? RIG { get; set; }
		public string? ANT { get; set; }
		public string? Power { get; set; }
		public string? QTH { get; set; }

	}
}
