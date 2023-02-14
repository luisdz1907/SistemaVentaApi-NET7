namespace SistemaVenta.API.Utilidad
{
    public class Response<T>
    {
        public bool Estatus { get; set; }
        public T Value { get; set; }
        public string message { get; set; }
    }
}
