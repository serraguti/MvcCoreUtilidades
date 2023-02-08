using Microsoft.AspNetCore.Mvc;
using MvcCoreUtilidades.Models;
using MvcCoreUtilidades.Services;

namespace MvcCoreUtilidades.Controllers
{
    public class AzureTablesController : Controller
    {
        private ServiceStorageTables service;

        public AzureTablesController(ServiceStorageTables service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<Cliente> clientes =
                await this.service.GetClientesAsync();
            return View(clientes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            await this.service.CreateClienteAsync(cliente.IdCliente
                , cliente.Empresa, cliente.Nombre, cliente.Salario
                , cliente.Edad);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string partitionkey, string rowkey)
        {
            await this.service.DeleteClientAsync(partitionkey, rowkey);
            return RedirectToAction("Index");
        }

        //UNA VISTA PARA BUSCAR CLIENTES POR EMPRESA
        public IActionResult ClientesEmpresa()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ClientesEmpresa(string empresa)
        {
            List<Cliente> clientes = this.service.GetClientesEmpresa(empresa);
            return View(clientes);
        }
    }
}
