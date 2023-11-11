using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Solution1
{
    public partial class Form1 : Form
    {
        private DataBaseWorker _db = new DataBaseWorker("test_db");
        private List<Card> _cards;
        private DataTable dataTable;

        public Form1()
        {
            InitializeComponent();
            LoadDateDB(dataGridViewXml);
        }

        private void LoadDateXml(List<Card> cards)
        {
            if (cards != null)
            {
                dataGridViewXml.Rows.Clear();

                for (int i = 0; i < (int)XmlCardEnum.TURNOVER; i++)
                {
                    dataGridViewXml.Columns.Add(((XmlCardEnum)i).ToString(), ((XmlCardEnum)i).ToString());
                }

                foreach (var card in cards)
                {
                    ReadSingleRow(dataGridViewXml, card);
                }
            }
        }

        private void LoadDateDB(DataGridView dataGridView)
        {
            _db.CheckTable();
            dataTable = new DataTable();
            dataTable = _db.LoadDataFromDatabase(dataTable);
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = dataTable;
            dataGridViewXml.DataSource = bindingSource;
        }

        private void ReadSingleRow(DataGridView dataGridView, Card card)
        {
            dataGridView.Rows.Add(card.cardCode, card.startDate, card.finishDate, card.lastName, card.firstName, card.surName,
                card.fullName, card.genderId, card.birthday, card.phoneHome, card.phoneMobil, card.email, card.city, card.street,
                card.house, card.apartment, card.alltaddress, card.cardType, card.ownerguId, card.cardper, card.turnover);
        }

        private string OpenDialog()
        {
            var opd = new OpenFileDialog();
            opd.Filter = "*.xml | *.xml";
            opd.ShowDialog();
            return opd.FileName;
        }

        private List<Card> ReadXmlFile()
        {
            List<Card> cards = new List<Card>();

            XmlDocument xDoc = new XmlDocument();
            
            var path = OpenDialog();

            if (path == null && path == "")
                return null;

            xDoc.Load(path);

            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlNode xNode in xRoot)
            {
                if (xNode.Attributes.Count > 0)
                {
                    List<string> prefabCard = new List<string>();

                    for (int i = 0; i < xNode.Attributes.Count; i++)
                    {
                        XmlCardEnum xmlCard = (XmlCardEnum)i;
                        XmlNode attr = xNode.Attributes.GetNamedItem(xmlCard.ToString());

                        if(attr != null)
                        {
                            prefabCard.Add(attr.Value);
                        }
                    }

                    cards.Add(
                        new Card(
                            Convert.ToInt64(prefabCard[0]),
                            CheckDateTime(prefabCard[1]),
                            CheckDateTime(prefabCard[2]),
                            prefabCard[3],
                            prefabCard[4],
                            prefabCard[5],
                            prefabCard[6],
                            Convert.ToInt32(prefabCard[7]),
                            CheckDateTime(prefabCard[8]),
                            prefabCard[9], prefabCard[10],
                            prefabCard[11], prefabCard[12],
                            prefabCard[13],
                            prefabCard[14],
                            prefabCard[15],
                            prefabCard[16],
                            prefabCard[17],
                            prefabCard[18],
                            Convert.ToInt64(prefabCard[19]),
                            Convert.ToDouble(prefabCard[20].Replace(".", ","))
                            ));
                }
            }
            return cards;
        }

        private DateTime? CheckDateTime(string dateTimeString)
        {
            DateTime? dt = null;

            if (dateTimeString == null || dateTimeString == "")
                return null;
            
            dt = DateTime.Parse(dateTimeString);

            return dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Card> xmlCards = ReadXmlFile();
            _db.ImportXml(xmlCards);
        }

        private void dataGridViewXml_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine($"Ячейка изменена: RowIndex={e.RowIndex}, ColumnIndex={e.ColumnIndex}");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool isComplite = false;
            if(dataTable != null)
               isComplite = _db.SaveChangesToDatabase(dataTable);

            if (isComplite)
            {
                MessageBox.Show("Изменения внесены корректно");
            }
            else
            {
                MessageBox.Show("Изменения не внесены");
            }
        }

    }   
}