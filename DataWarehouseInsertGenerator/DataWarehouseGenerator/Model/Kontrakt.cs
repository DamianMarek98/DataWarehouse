using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWarehouseGenerator.Model
{
    class Kontrakt
    {
        private int Id;
        private int AgentId;
        private int ZawodnikId;
        private int prowizjaAgenta;
        private int pensjaMiesieczna;
        private int DataStartu;
        private int DataZakonczenia;
        private int WartoscKontraktu;
        private int iloscMiesiecy;
        private int Zmienna;
        private bool CzyZmiennaWyplacona;

        public Kontrakt(int id, int agentId, int zawodnikId, int prowizjaAgenta, int pensjaMiesieczna, int dataStartu, int dataZakonczenia, int wartoscKontraktu, int iloscMiesiecy, int zmienna, bool czyZmiennaWyplacona)
        {
            Id = id;
            AgentId = agentId;
            ZawodnikId = zawodnikId;
            this.prowizjaAgenta = prowizjaAgenta;
            this.pensjaMiesieczna = pensjaMiesieczna;
            DataStartu = dataStartu;
            DataZakonczenia = dataZakonczenia;
            WartoscKontraktu = wartoscKontraktu;
            this.iloscMiesiecy = iloscMiesiecy;
            Zmienna = zmienna;
            CzyZmiennaWyplacona = czyZmiennaWyplacona;
        }

        override public string ToString()
        {
            string kontrakt = ("(" + Id.ToString() + ", " + AgentId.ToString() + ", " + ZawodnikId.ToString() + ", " + prowizjaAgenta.ToString() + "," + pensjaMiesieczna.ToString() + "," + DataStartu.ToString() + "," + DataZakonczenia.ToString() + "," + WartoscKontraktu.ToString() + "," + iloscMiesiecy.ToString() + "," + Zmienna.ToString() + "," + Convert.ToInt32(CzyZmiennaWyplacona).ToString() + ")");
            return kontrakt;
        }
    }
}
