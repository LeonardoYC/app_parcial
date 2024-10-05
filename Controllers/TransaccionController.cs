using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using app_parcial.Data;
using app_parcial.Models;

namespace app_parcial.Controllers
{
    public class TransaccionController : Controller
    {
private readonly ILogger<TransaccionController> _logger;
        private readonly ApplicationDbContext _context;

        public TransaccionController(ILogger<TransaccionController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Vista principal
        public IActionResult Index()
        {
            return View();
        }

        // Mostrar el formulario de creación
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Transaccion transaccion)
        {
            if (ModelState.IsValid)
            {
                // Convertir la fecha a UTC
                transaccion.Fecha = DateTime.UtcNow;

                // Asignar estado predeterminado
                transaccion.Estado = "Pendiente"; 

                // Calcular el MontoFinal
                if (transaccion.MonedaOrigen == "USD" && transaccion.MonedaDestino == "BTC")
                {
                    transaccion.MontoFinal = transaccion.MontoEnviado / transaccion.TasaCambio;
                }
                else if (transaccion.MonedaOrigen == "BTC" && transaccion.MonedaDestino == "USD")
                {
                    transaccion.MontoFinal = transaccion.MontoEnviado * transaccion.TasaCambio;
                }
                else
                {
                    transaccion.MontoFinal = transaccion.MontoEnviado; // Si ambas son iguales, no se realiza conversión
                }

                // Guardar en la base de datos
                _context.DataTransaccion.Add(transaccion);
                _context.SaveChanges();
            }

            return View(transaccion); // Volver a mostrar el formulario en caso de error
        }


        // Listar transacciones
        public IActionResult Listar()
        {
            var transacciones = _context.DataTransaccion.ToList();
            return View(transacciones);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}