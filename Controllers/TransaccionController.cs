using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using app_parcial.Data;
using app_parcial.Models;
using app_parcial.Services;


namespace app_parcial.Controllers
{
    public class TransaccionController : Controller
    {
        private readonly ILogger<TransaccionController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly CoinMarketCapService _coinMarketCapService;

        public TransaccionController(ILogger<TransaccionController> logger, ApplicationDbContext context, CoinMarketCapService coinMarketCapService)
        {
            _logger = logger;
            _context = context;
            _coinMarketCapService = coinMarketCapService;
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
        public async Task<IActionResult> Crear(Transaccion transaccion)
        {
            if (ModelState.IsValid)
            {
                // Validación de moneda
                if (transaccion.MonedaOrigen == null || transaccion.MonedaDestino == null)
                {
                    ModelState.AddModelError("", "Las monedas de origen y destino son requeridas.");
                    return View(transaccion);
                }

                try
                {
                    transaccion.Fecha = DateTime.UtcNow;
                    transaccion.Estado = "Pendiente";

                    // Lógica de conversión
                    if (transaccion.MonedaOrigen == "USD" && transaccion.MonedaDestino == "BTC")
                    {
                        transaccion.TasaCambio = await _coinMarketCapService.ConvertUsdToBtcAsync(transaccion.MontoEnviado);
                        transaccion.MontoFinal = transaccion.MontoEnviado / transaccion.TasaCambio;
                    }
                    else if (transaccion.MonedaOrigen == "BTC" && transaccion.MonedaDestino == "USD")
                    {
                        transaccion.TasaCambio = await _coinMarketCapService.ConvertBtcToUsdAsync(transaccion.MontoEnviado);
                        transaccion.MontoFinal = transaccion.MontoEnviado * transaccion.TasaCambio;
                    }
                    else
                    {
                        transaccion.MontoFinal = transaccion.MontoEnviado; // Sin conversión si son iguales
                    }

                    await GuardarHistorialConversion(transaccion.TasaCambio);
                    _context.DataTransaccion.Add(transaccion);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Crear)); // Redirigir a la lista de transacciones
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al procesar la transacción: {ex.Message}");
                }
            }

            return View(transaccion); // Volver a mostrar el formulario si hay errores
        }


        // Método para guardar el historial de conversiones
        private async Task GuardarHistorialConversion(decimal tasaCambio)
        {
            var historialConversion = new HistorialConversion
            {
                TasaUSDaBTC = tasaCambio,
                Fecha = DateTime.UtcNow
            };
            _context.DataHistorialConversion.Add(historialConversion);
            await _context.SaveChangesAsync();
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
            return View("Error!"); // Vista de error
        }
    }
}