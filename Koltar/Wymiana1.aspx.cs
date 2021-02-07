using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Xml;

namespace stat2018
{
    public partial class Wymiana1 : System.Web.UI.Page
    {
        public ServiceReference test = new ServiceReference();
        public common Common = new common();
        public Class1 cl = new Class1();
        private dataReaders dR = new dataReaders();
        public string con_str = ConfigurationManager.ConnectionStrings["wap"].ConnectionString;
        public string con_str_wcyw = ConfigurationManager.ConnectionStrings["wcywConnectionString"].ConnectionString;
        public log_4_net log = new log_4_net();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                TextBox1.Text = "";
                lbRok.Items.Clear();
                int year = DateTime.Now.Year;
                for (int i = year; i > year - 30; i--)
                {
                    lbRok.Items.Add(i.ToString());
                }
                lbRok.SelectedIndex = 0;
                DataTable parameters = Common.makeParameterTable();

                DataTable dT1 = Common.getDataTable("SELECT distinct [wartosc], [ConnectionString] FROM [konfig] WHERE ([klucz] = 'KonfigRodzajSprawy') ORDER BY wartosc", con_str, parameters, "Wymiana");
                lbRodzajSprawy.Items.Clear();
                foreach (DataRow item in dT1.Rows)
                {
                    string Text = item[0].ToString();
                    DevExpress.Web.ListEditItem listEditItem = new DevExpress.Web.ListEditItem()
                    {
                        Text = item[0].ToString(),
                        Value = item[1].ToString()
                    };
                    string value = item[1].ToString();
                    lbRodzajSprawy.Items.Add(Text, value);
                }
                if (lbRodzajSprawy.SelectedIndex == -1)
                {
                    lbRodzajSprawy.SelectedIndex = 0;
                }
            }
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            string kwerendaOdczytujaca = string.Empty;
            string rodzaj = string.Empty;

            string connection = string.Empty;
            string url = string.Empty;
            string host = string.Empty;
            TextBox1.Text = "";

            ServiceReference1.SerwisWymianySoapClient serwisWymianySoapClient = new ServiceReference1.SerwisWymianySoapClient();

            rodzaj = lbRodzajSprawy.SelectedItem.Text.ToString();
            connection = lbRodzajSprawy.SelectedItem.Value.ToString();
            // walidacja po stronie clienta

            DataTable parametry = Common.makeParameterTable();
            parametry.Rows.Add("@rodzaj", rodzaj);
            DataTable kwerendaWalidująca = Common.getDataTable("SELECT distinct kwerendaOdczytujaca,  connection FROM wymiana where rodzaj = @rodzaj and typ=0", Common.con_str, parametry, "wymiana cleint: kwerendaWalidująca");
            if (kwerendaWalidująca == null)
            {
                log.Error("Wymiana odczyt danych: Brak kwerendy walidującej zapytanie po stronie klienta - rodzaj=0");
                TextBox1.Text = TextBox1.Text + "Wymiana odczyt danych: Brak kwerendy walidującej zapytanie po stronie klienta - rodzaj=0" + Environment.NewLine;
                return;
            }
            string kwerendaSprawdzajaca = kwerendaWalidująca.Rows[0][0].ToString();
            string CSkwerendySprawdzajacej = kwerendaWalidująca.Rows[0][1].ToString();

            parametry = Common.makeParameterTable();
            parametry.Rows.Add("@numer", TBNrSprawy.Text.Trim());
            parametry.Rows.Add("@rok", lbRok.SelectedItem.Text.Trim());
            parametry.Rows.Add("@rep", TBRepertorium.Text.Trim());
            parametry.Rows.Add("@wydzial", TBNrWydzialu.Text.Trim());
            string wynikWalidacji = Common.getQuerryValue(kwerendaSprawdzajaca, CSkwerendySprawdzajacej, parametry, "wymiana cleint");
            try
            {
                if (int.Parse(wynikWalidacji) == 0)
                {
                    log.Error("Brak spraw z repetytoriu, " + TBRepertorium.Text + ", Wydziału " + TBNrWydzialu.Text + ", o numerze " + TBNrSprawy.Text.Trim() + " z roku " + lbRok.SelectedItem.Text.Trim());
                    TextBox1.Text = TextBox1.Text + "Brak spraw z repetytoriu, " + TBRepertorium.Text + ", Wydziału " + TBNrWydzialu.Text + ", o numerze " + TBNrSprawy.Text.Trim() + " z roku " + lbRok.SelectedItem.Text.Trim() + Environment.NewLine;
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error("Wymiana: walidacja po stronie klienta " + ex.Message);
                TextBox1.Text = TextBox1.Text + "Wymiana: walidacja po stronie klienta " + ex.Message + Environment.NewLine;
                return;
            }
            // wyciągnięcie kwerendy i CS zapytujących

            parametry = Common.makeParameterTable();
            parametry.Rows.Add("@rodzaj", rodzaj);
            DataTable zestawZapytujacy = Common.getDataTable("SELECT distinct kwerendaOdczytujaca,  connection FROM wymiana where rodzaj = @rodzaj and typ=1", Common.con_str, parametry, "wymiana cleint: kwerendaWalidująca");

            string kwerendaZapytujaca = zestawZapytujacy.Rows[0][0].ToString();
            string CSkwerendyZapytujacej = zestawZapytujacy.Rows[0][1].ToString();

            //  serwisWymianySoapClient.Endpoint.Address = endpointAddress;
            string wynik = string.Empty;
            try
            {
                wynik = serwisWymianySoapClient.DaneWXml(TBNrWydzialu.Text.Trim(), TBRepertorium.Text.Trim(), int.Parse(TBNrSprawy.Text.Trim()), rodzaj, CSkwerendyZapytujacej, int.Parse(lbRok.SelectedItem.Text.Trim()), kwerendaZapytujaca);
                //                              DaneWXml(string NrWydzialu       , string repertorium       , int nrSprawy                     , string rodzaj,  string connection, int rok, string kwerendaZapytujaca)
                TextBox1.Text = wynik;
            }
            catch (Exception ex)
            {
                log.Error("Wymiana odczyt danych: " + ex.Message);
                TextBox1.Text = TextBox1.Text + ex.Message + Environment.NewLine;
            }

            string path = Server.MapPath("Wymiana\\Tmp\\odpowiedz") + DateTime.Now.ToString().Replace(" ", "_").Replace(".", "_").Replace(":", "_") + ".xml";

            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(wynik.ToString());
                xdoc.Save(path);
            }
            catch (Exception ex)
            {
                log.Error("Wymiana odczyt danych: " + ex.Message);
                TextBox1.Text = TextBox1.Text + ex.Message;
            }
        }
    }
}