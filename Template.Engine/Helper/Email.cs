using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using Template.Engine.Security;

namespace Template.Engine.Helper
{
    public static class Email
    {
        public static bool EnviarEmail(string NombreEmisor = "", string DireccionEmisor = "", string Asunto = "", string NombreDestinatario = "", List<string> Destinatarios = null, string Mensaje = "", List<string> Adjuntos = null, List<string> Copias = null, List<string> CopiasOcultas = null)
        {
            #region Configuración del Servidor SMTP.
            var ServidorSMTP = ConfigurationManager.AppSettings["ServidorSMTP"];
            var PuertoSMTP = Convert.ToInt32(ConfigurationManager.AppSettings["PuertoSMTP"]);
            var UsaSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["UsaSSL"]);
            var UsuarioSMTP = ConfigurationManager.AppSettings["UsuarioSMTP"];
            var PasswordSMTP = ConfigurationManager.AppSettings["PasswordSMTP"];
            #endregion

            using (var ClienteSMTP = new SmtpClient(ServidorSMTP))
            {
                ClienteSMTP.UseDefaultCredentials = true;
                ClienteSMTP.Port = PuertoSMTP;
                ClienteSMTP.EnableSsl = UsaSSL;

                if (!string.IsNullOrEmpty(UsuarioSMTP) && !string.IsNullOrEmpty(PasswordSMTP))
                {
                    var CredencialesRed = new NetworkCredential(UsuarioSMTP, Coder.Decode(PasswordSMTP));

                    ClienteSMTP.Credentials = CredencialesRed;
                }

                using (var MensajeEmail = new MailMessage())
                {
                    #region Configuración del Mensaje.
                    MensajeEmail.IsBodyHtml = true;
                    MensajeEmail.SubjectEncoding = Encoding.UTF8;
                    MensajeEmail.BodyEncoding = Encoding.UTF8;
                    MensajeEmail.Subject = Asunto;
                    MensajeEmail.Body = Mensaje;

                    if (Copias != null && Copias.Count > 0)
                    {
                        foreach (var Copia in Copias) 
                            if (!string.IsNullOrEmpty(Copia)) MensajeEmail.CC.Add(new MailAddress(Copia));
                    }

                    if (CopiasOcultas != null && CopiasOcultas.Count > 0)
                    {
                        foreach (var CopiaOculta in CopiasOcultas)
                            if (!string.IsNullOrEmpty(CopiaOculta)) MensajeEmail.Bcc.Add(new MailAddress(CopiaOculta));
                    }

                    if (!string.IsNullOrEmpty(DireccionEmisor) && !string.IsNullOrEmpty(NombreEmisor))
                    {
                        MensajeEmail.From = new MailAddress(DireccionEmisor, NombreEmisor);
                        MensajeEmail.Sender = new MailAddress(DireccionEmisor, NombreEmisor);
                        MensajeEmail.ReplyToList.Add(new MailAddress(DireccionEmisor, NombreEmisor));
                    }

                    if (Destinatarios != null && Destinatarios.Count > 0)
                    {
                        foreach (var Destinatario in Destinatarios)
                            if (!string.IsNullOrEmpty(Destinatario)) MensajeEmail.To.Add(new MailAddress(Destinatario));
                    }
                    #endregion

                    #region Congiguración de los archivos adjuntos.
                    if (Adjuntos != null && Adjuntos.Count > 0)
                    {
                        foreach (var adjunto in Adjuntos)
                            using (var data = new Attachment(adjunto)) MensajeEmail.Attachments.Add(data);
                    }
                    #endregion

                    ClienteSMTP.Send(MensajeEmail);

                    return true;
                }
            }
        }

        public static string ObtenerHtml(string plantilla, string rutaPlantilla)
        {

            var rutaPlantillasHtml = rutaPlantilla;

            rutaPlantillasHtml = rutaPlantillasHtml.EndsWith("/") ? rutaPlantillasHtml : rutaPlantillasHtml + "/";
            var rutaFisica = string.Format("{0}{1}", rutaPlantillasHtml, plantilla);

            return File.ReadAllText(rutaFisica);
        }
    }
}
