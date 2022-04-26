using System;
using System.Collections.Generic;
using System.Text;

namespace MargaritasAppClase.Models
{
    public class MetodosPagoListModel
    {
        public MetodosPagoListModel(string ID_Rel_Pago, string ID_Cliente, string ID_FormaPago, string Descripcion, string NumTcTd, string Titular, string Fecha_exp, string CCV, string Predeterminada)
        {
            this.ID_Rel_Pago = ID_Rel_Pago;
            this.ID_Cliente = ID_Cliente;
            this.ID_FormaPago = ID_FormaPago;
            this.Descripcion = Descripcion;
            this.NumTcTd = NumTcTd;
            this.Titular = Titular;
            this.Fecha_exp = Fecha_exp;
            this.CCV = CCV;
            this.Predeterminada = Predeterminada;
        }

        public string ID_Rel_Pago { get; set; }
        public string ID_Cliente { get; set; }
        public string ID_FormaPago { get; set; }
        public string Descripcion { get; set; }
        public string NumTcTd { get; set; }
        public string Titular { get; set; }
        public string Fecha_exp { get; set; }
        public string CCV { get; set; }
        public string Predeterminada { get; set; }
    }
}
