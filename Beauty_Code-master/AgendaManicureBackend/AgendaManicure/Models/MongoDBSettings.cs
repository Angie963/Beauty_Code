namespace AgendaManicure.Models
{
    // Ajustado para evitar advertencias de nullability y dar valores por defecto razonables.
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;   // se llenará desde appsettings.json
        public string DatabaseName { get; set; } = null!;

        // Inicializamos Collections para evitar warnings y NREs
        public CollectionsSettings Collections { get; set; } = new CollectionsSettings();
    }

    public class CollectionsSettings
    {
        // Valores por defecto que coinciden con tu appsettings.json.
        public string Users { get; set; } = "usuarios";
        public string Servicios { get; set; } = "servicios";
        public string Categorias { get; set; } = "categorias";
        public string Citas { get; set; } = "agenda";
        public string Pagos { get; set; } = "pagos";
        public string Resenas { get; set; } = "resenas";
    }
}
