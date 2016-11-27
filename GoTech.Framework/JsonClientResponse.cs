using System;

namespace GoTech.Framework
{
    public class JsonClientResponse
    {
        public long ID { get; set; }
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public JsonClientResponse(bool isSuccess = true, string Message = null, long ID = 0)
        {
            this.isSuccess = isSuccess;
            this.Message = Message;
            this.ID = ID;
        }
        public JsonClientResponse(Exception ex)
        {
            this.isSuccess = false;
            this.Message = GetMessage(ex);
        }

        private string GetMessage(Exception ex)
        {
            if (ex.InnerException != null)
                return ex.Message + GetMessage(ex.InnerException);
            else return ex.Message;
        }
    }
}
