using GameGearMarket_Backend.Context;
using GameGearMarket_Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GameGearMarket_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }


        //Get para traer todos los productos
        [HttpGet]
        public ActionResult Get()
        {
            try 
            { 
                return Ok(_context.Productos.ToList());
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }


        //Buscar producto por folio
        [HttpGet("{id}", Name = "Productos")]
        public ActionResult Get(int id)
        {
            try
            {
                var producto = _context.Productos.FirstOrDefault(p => p.folio == id);
                if(producto == null)
                {
                    return NotFound();
                }
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Agregar un producto
        [HttpPost]
        public ActionResult Post([FromBody] Productos producto)
        {
            try
            {
                // Validaciones
                if (producto == null)
                {
                    return BadRequest("El objeto 'producto' no puede ser nulo.");
                }

                // Validar que los campos requeridos no sean nulos
                if (string.IsNullOrEmpty(producto.nombre) || producto.precio <= 0 
                    || string.IsNullOrEmpty(producto.descripcion)
                    || string.IsNullOrEmpty(producto.marca)
                    || string.IsNullOrEmpty(producto.clasificacion)
                    || string.IsNullOrEmpty(producto.categoria))
                {
                    return BadRequest("No se aceptan campos nulos o el precio en 0");
                }

                // Validar que el nombre no se repita en la base de datos
                if (_context.Productos.Any(p => p.nombre == producto.nombre))
                {
                    return BadRequest("Ya existe un producto con el mismo nombre.");
                }

                // Agregar el producto si las validaciones pasan
                _context.Productos.Add(producto);
                _context.SaveChanges();

                // Devolver la respuesta CreatedAtRoute
                return CreatedAtRoute("Productos", new { id = producto.folio }, producto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //Modificar un producto
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Productos producto)
        {
            try
            {
                // Verificar si el producto existe en la base de datos
                var existingProduct = _context.Productos.Find(id);
                if (existingProduct == null)
                {
                    return NotFound($"No se encontró un producto con el ID {id}");
                }

                // Validar que la propiedad folio coincida con el parámetro id
                if (producto.folio != id)
                {
                    return BadRequest("El folio del producto no coincide con el ID proporcionado.");
                }

                // Validar que los campos requeridos no sean nulos
                if (string.IsNullOrEmpty(producto.nombre) || producto.precio <= 0 
                    || string.IsNullOrEmpty(producto.descripcion)
                    || string.IsNullOrEmpty(producto.marca)
                    || string.IsNullOrEmpty(producto.clasificacion)
                    || string.IsNullOrEmpty(producto.categoria))
                {
                    return BadRequest("Son requridos los campos de nombre, precio, descripcion, marca, clasificacion y categoria.");
                }

                // Actualizar las propiedades del producto existente
                existingProduct.nombre = producto.nombre;
                existingProduct.precio = producto.precio;
                existingProduct.descripcion = producto.descripcion;
                existingProduct.marca = producto.marca;
                existingProduct.stock = producto.stock;
                existingProduct.categoria = producto.categoria;
                existingProduct.clasificacion = producto.clasificacion;

                // Marcar el estado deL producto modificado
                _context.Entry(existingProduct).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                // Hacer la actualizacion del producto
                _context.SaveChanges();

                // Devolver la respuesta
                return CreatedAtRoute("Productos", new { id = existingProduct.folio }, existingProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Eliminar un producto
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var producto = _context.Productos.FirstOrDefault(p => p.folio == id);
                if(producto != null)
                {
                    _context.Productos.Remove(producto);
                    _context.SaveChanges();
                    return Ok(id);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
