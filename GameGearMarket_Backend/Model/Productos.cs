using System.ComponentModel.DataAnnotations;

namespace GameGearMarket_Backend.Model
{
    public class Productos
    {
        [Key]
        public int folio { get; set; }
        public string nombre { get; set; }
        public double precio { get; set; }
        public string descripcion {  get; set; }
        public string marca { get; set; }
        public int stock { get; set; }
        public string clasificacion { get; set; }
        public string categoria { get; set; }
    }
}
