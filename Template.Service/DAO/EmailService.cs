using System;
using System.Linq;
using Template.Domain.Model;

namespace Template.Service.DAO
{
	public static class EmailService
	{
		public static Email GetById(int? Id)
		{
			if (!Id.HasValue) throw new Exception("Debe especificar el Identificador de la Plantilla de Correo.");

            using (var Dc = new TemplateEntities())
			{
				var Email_ = Dc
					.Email
					.FirstOrDefault(X =>
						X.Id == Id
						&& X.Activo
					);

				return Email_;
			}
		}







	}
}
