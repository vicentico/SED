using System.Web.Mvc;

namespace Template.Engine.Helper.Alert
{
    public static class FlashHelper
    {
        public static void Flash(this Controller Controller, string Message, MessageType MessageType = MessageType.Success, bool Confirmation = false)
        {
            Controller.TempData.Add("Flash", new FlashData(MessageType, Message, Confirmation));
        }
    }
}