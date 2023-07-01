using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using Microsoft.Win32;

namespace RequestRepealWB
{
    public partial class MainWindow : Window
    {
        private readonly XmlData xmlData = new XmlData();

        private readonly SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
        public MainWindow()
        {
            InitializeComponent();
            SaveFileDialog1.Filter = "Xml файлы(*.xml)|*.xml";
            SaveFileDialog1.FileName = "RequestRepealWB";
        }


        public void ValidationFsrar()
        {
            if (ulong.TryParse(FsrarId.Text, out _))
            {
                if (FsrarId.Text.Length < 12)
                {
                    FsrarId.Background = Brushes.Red;
                    FsrarId.ToolTip = "Введено некорректное значение, должно быть 12 цифр";
                }
                else
                {
                    xmlData.FsrarIDStatus = true;
                    FsrarId.Background = default;
                    FsrarId.ToolTip = default;
                }
            }
            else
            {
                FsrarId.Background = Brushes.Red;
                FsrarId.ToolTip = "Введено некорректное значение, используйте только цифры";
            }
        }

        public void ValidationTtn()
        {
            if (ulong.TryParse(TtnId.Text, out _))
            {
                if (TtnId.Text.Length < 10)
                {
                    TtnId.Background = Brushes.Red;
                    TtnId.ToolTip = "Введено некорректное значение, должно быть 10 цифр";
                }
                else
                {
                    xmlData.TtnIDStatus = true;
                    TtnId.Background = default;
                    TtnId.ToolTip = default;
                }

            }
            else
            {
                TtnId.Background = Brushes.Red;
                TtnId.ToolTip = "Введено некорректное значение, используйте только цифры";
            }
        }
        public void Validation()
        {
            ValidationFsrar();
            ValidationTtn();

            if (xmlData.FsrarIDStatus && xmlData.TtnIDStatus)
            {
                if (SaveFileDialog1.ShowDialog() == true)
                {
                    string filename = SaveFileDialog1.FileName;
                    xmlData.FsrarID = FsrarId.Text;
                    xmlData.TtnID = TtnId.Text;
                    xmlData.Number = DateTime.Now.ToString("0000-ddMMyyyy");
                    xmlData.Date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    GenerateXml(filename);
                    MessageBox.Show($"Файл сохранен\nПуть: {filename}");
                }
                xmlData.FsrarIDStatus = false;
                xmlData.TtnIDStatus = false;
            }
            else
                MessageBox.Show("Устраните ошибки заполнения");      
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Validation();
        }

        class XmlData
        {
            public string FsrarID { get; set; }
            public string TtnID { get; set; }

            public string Number { get; set; }

            public string Date { get; set; }

            public bool FsrarIDStatus { get; set; }
            public bool TtnIDStatus { get; set; }
        }

        public void GenerateXml(string path)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                writer.WriteLine(@"<ns:Documents Version=""1.0""");
                writer.WriteLine(@"xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""");
                writer.WriteLine(@"xmlns:ns=""http://fsrar.ru/WEGAIS/WB_DOC_SINGLE_01""");
                writer.WriteLine(@"xmlns:qp=""http://fsrar.ru/WEGAIS/RequestRepealWB"">");
                writer.WriteLine(@"<ns:Owner>");
                writer.WriteLine($@"<ns:FSRAR_ID>{xmlData.FsrarID}</ns:FSRAR_ID>");
                writer.WriteLine(@"</ns:Owner>");
                writer.WriteLine(@"<ns:Document>");
                writer.WriteLine(@"<ns:RequestRepealWB>");
                writer.WriteLine($@"<qp:ClientId>{xmlData.FsrarID}</qp:ClientId>");
                writer.WriteLine($@"<qp:RequestNumber>{xmlData.Number}</qp:RequestNumber>");
                writer.WriteLine($@"<qp:RequestDate>{xmlData.Date}</qp:RequestDate>");
                writer.WriteLine($@"<qp:WBRegId>TTN-{xmlData.TtnID}</qp:WBRegId>");
                writer.WriteLine(@"</ns:RequestRepealWB>");
                writer.WriteLine(@"</ns:Document>");
                writer.Write(@"</ns:Documents>");
            }
        }

        private void FsrarId_TextChanged(object sender, TextChangedEventArgs e) => ValidationFsrar();

        private void TtnId_TextChanged(object sender, TextChangedEventArgs f) => ValidationTtn();
    }
}
