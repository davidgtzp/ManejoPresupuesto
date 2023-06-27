using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {        
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly ILogger<TiposCuentasController> _logger;

        public TiposCuentasController(
            IRepositorioTiposCuentas repositorioTiposCuentas,
            ILogger<TiposCuentasController> logger
        )
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            _logger = logger;
        }

        public IActionResult Crear()
        {
            _logger.LogInformation("Comienza");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            _logger.LogInformation("Crear - Post");
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            tipoCuenta.UsuarioId = 1;

            var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (yaExisteTipoCuenta)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe.");
                return View(tipoCuenta);
            }
            await repositorioTiposCuentas.Crear(tipoCuenta);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            var usuarioId = 1;
            _logger.LogInformation("Crear - Get");
            _logger.LogInformation(nombre);
            var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(nombre, usuarioId);
            _logger.LogInformation(yaExisteTipoCuenta.ToString());
            if (yaExisteTipoCuenta)
            {
                return Json($"El nombre {nombre} ya existe");
            }

            return Json(true);
        }
    }
}
