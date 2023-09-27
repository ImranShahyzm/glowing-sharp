namespace SagErpBlazor.ExtendedClasses
{
   
        public interface IMailInterface
        {
            Task<string> SendEmailAsync(string ToEmail, string Subject, string HTMLBody);

        }
    
}
