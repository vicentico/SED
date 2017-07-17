namespace Template.Engine.Data.Models
{
    public static class ResultJson
    {
	    public static dynamic MensajeExito(string Message_)
	    {
			return new
			{
				Error = false,
				Message = Message_
			};
	    }

		public static dynamic MensajeError(string Message_)
		{
			return new 
			{
				Error = true,
				Message = Message_
			};
		}

		public static dynamic MensajeDataExito(string Message_, object Data_)
		{
			return new
			{
				Error = false,
				Message = Message_,
				Data = Data_
			};
		}
    }
}
