namespace API_TarjetaCredito_Atlantida.models
{
    public class Cliente
    {
        public int IDCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dui { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
