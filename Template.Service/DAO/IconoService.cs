using System;
using System.Collections.Generic;
using System.Linq;
using Template.Domain.Model;

namespace Template.Service.DAO
{
	public static class IconoService
	{
		public static List<Icono> GetAll()
		{
			using (var Dc = new TemplateEntities())
			{
				var Iconos = Dc.Icono.ToList();

				return Iconos;
			}
		}
	}
}
