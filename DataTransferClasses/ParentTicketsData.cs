using SagErpBlazor.Models;

namespace SagErpBlazor.DataTransferClasses
{
    public class ParentTicketsData
    {
            public StsTicketsRegister TicketsRegister { get; set; }
            public List<StsTicketsRegisterAttachment> TicketsRegisterAttachment { get; set; }
            public string StatusColor { get; set; }
            public string StatusName { get; set; }
        
    }
}
