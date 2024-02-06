namespace API_TarjetaCredito_Atlantida.models
{
    public class Tarjetas
    {
        public string NumeroTarjeta { get; set; }
        public int IDCliente { get; set; }
        public string NombreTarjeta { get; set; }
        public decimal LimiteCredito { get; set; }
        public decimal SaldoActual { get; set; }
        public decimal SaldoDisponible { get; set; }
        public int PIC { get; set; }
        public int PCSM { get; set; }
    }
}
