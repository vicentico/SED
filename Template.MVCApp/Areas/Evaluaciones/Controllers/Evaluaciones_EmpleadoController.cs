using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Template.Domain.Model;
using Template.Service;

namespace Template.MVCApp.Areas.Evaluaciones.Controllers
{
    public class Evaluaciones_EmpleadoController : Controller
    {
        private TemplateEntities db = new TemplateEntities();

        // GET: Evaluaciones/Evaluaciones_Empleado
        public ActionResult Index()
        {
            var emp_Eval = db.Emp_Eval.Include(e => e.Empleado).Include(e => e.Evaluacion);
            return View(emp_Eval.ToList());
        }

        // GET: Evaluaciones/Evaluaciones_Empleado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emp_Eval emp_Eval = db.Emp_Eval.Find(id);
            if (emp_Eval == null)
            {
                return HttpNotFound();
            }
            return View(emp_Eval);
        }

        // GET: Evaluaciones/Evaluaciones_Empleado/Create
        public ActionResult Create()
        {
            ViewBag.idEmpleado = new SelectList(db.Empleado, "id", "nombre");
            ViewBag.idEvaluacion = new SelectList(db.Evaluacion, "id", "nombre");
            return View();
        }

        // POST: Evaluaciones/Evaluaciones_Empleado/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEmpleado,idEvaluacion,comentario,activo")] Emp_Eval emp_Eval)
        {
            if (ModelState.IsValid)
            {
                db.Emp_Eval.Add(emp_Eval);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idEmpleado = new SelectList(db.Empleado, "id", "nombre", emp_Eval.idEmpleado);
            ViewBag.idEvaluacion = new SelectList(db.Evaluacion, "id", "nombre", emp_Eval.idEvaluacion);
            return View(emp_Eval);
        }

        // GET: Evaluaciones/Evaluaciones_Empleado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emp_Eval emp_Eval = db.Emp_Eval.Find(id);
            if (emp_Eval == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEmpleado = new SelectList(db.Empleado, "id", "nombre", emp_Eval.idEmpleado);
            ViewBag.idEvaluacion = new SelectList(db.Evaluacion, "id", "nombre", emp_Eval.idEvaluacion);
            return View(emp_Eval);
        }

        // POST: Evaluaciones/Evaluaciones_Empleado/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEmpleado,idEvaluacion,comentario,activo")] Emp_Eval emp_Eval)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emp_Eval).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idEmpleado = new SelectList(db.Empleado, "id", "nombre", emp_Eval.idEmpleado);
            ViewBag.idEvaluacion = new SelectList(db.Evaluacion, "id", "nombre", emp_Eval.idEvaluacion);
            return View(emp_Eval);
        }

        // GET: Evaluaciones/Evaluaciones_Empleado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emp_Eval emp_Eval = db.Emp_Eval.Find(id);
            if (emp_Eval == null)
            {
                return HttpNotFound();
            }
            return View(emp_Eval);
        }

        // POST: Evaluaciones/Evaluaciones_Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Emp_Eval emp_Eval = db.Emp_Eval.Find(id);
            db.Emp_Eval.Remove(emp_Eval);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
