namespace CustomerManagementSystem.Helpers
{
    public class AppConfig
    {
        #region Mailer
        public string SystemName => "Customer Management System";
        public string EmailServer => "smtp.gmail.com";
        public string EmailSender => "####";
        public string EmailUsername => "meckydrix@gmail.com";
        public string EmailPassword => "#####";
        public string SystemAuthor => "Meck Aaron";
        public string SystemDescription => "Customer Management System";
        public string TokenSecret => "THIS IS THE TOKEN SECRET FOR THE CUSTOMER MANAGEMENT SYSTEM";
        public int EmailPort => 25;
        
        #endregion

    }
}