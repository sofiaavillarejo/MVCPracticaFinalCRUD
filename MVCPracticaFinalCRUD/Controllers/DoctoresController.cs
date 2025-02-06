using Microsoft.AspNetCore.Mvc;
using MVCPracticaFinalCRUD.Models;
using MVCPracticaFinalCRUD.Repositories;

namespace MVCPracticaFinalCRUD.Controllers
{
    public class DoctoresController : Controller
    {
        RepositoryDoctor repo;

        public DoctoresController()
        {
            this.repo = new RepositoryDoctor();
        }

        public IActionResult Index()
        {
            List<Doctor> doctores = this.repo.GetDoctores();
            return View(doctores);
        }

        public IActionResult Detalles(int IdDoctor)
        {
            Doctor doc = this.repo.DetalleDoctor(IdDoctor);
            return View(doc);
        }

        public IActionResult Delete(int IdDoctor)
        {
            this.repo.Delete(IdDoctor);
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(int idHospital, int idDoctor, string apellido, string especialidad, int salario)
        {
            await this.repo.CreateDoctorAsync(idHospital, idDoctor, apellido, especialidad, salario);
            return RedirectToAction("Index");
        }

        public IActionResult BuscadorEspecialidad()
        {
            ViewData["ESPECIALIDAD"] = this.repo.GetEspecialidades();
            return View();
        }

        [HttpPost]
        public IActionResult BuscadorEspecialidad(string especialidad)
        {
            ViewData["ESPECIALIDAD"] = this.repo.GetEspecialidades();
            List<Doctor> doctores = this.repo.GetDoctorEspecialidad(especialidad);
            return View(doctores);

        }

    }
}
