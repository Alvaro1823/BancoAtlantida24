namespace API_TarjetaCredito_Atlantida.models
{
    public class TransaccionesTarjetas
    {
        public int NumeroAutorizacion { get; set; }
        public String NumeroTarjeta { get; set; }
        public String TipoTransaccion { get; set; }
        public Decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public String Descripcion { get; set; }
    }
}
