namespace SagErpBlazor.Data
{
    public class STS_GLCompany
    {
        public int CompanyID { get; set; }
        public string Title { get; set; }   =string.Empty;
        public string ShortTitle { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ADDRESS { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string website { get; set; } = string.Empty;
        public string CompanyImage { get; set; } = string.Empty;
        public bool Inactive { get; set; } 
        public string SupportEmail { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string PoBoxNo { get; set; } = string.Empty;
        public string FaxNo { get; set; } = string.Empty;
        public string LoginBackground { get; set; } = string.Empty;
        public string CSSColorStyle { get; set; } = string.Empty;
    }
}
