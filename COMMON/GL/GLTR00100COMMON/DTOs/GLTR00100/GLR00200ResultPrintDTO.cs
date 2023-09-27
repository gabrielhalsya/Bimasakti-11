using System.Collections.Generic;
namespace GLTR00100COMMON
{
    public class GLTR00100ResultPrintDTO
    {
        public GLTR00101DTO HeaderData { get; set; }
        public List<GLTR00102DTO> ListDetail { get; set; }
    }

    public class GLTR00100ResultWithBaseHeaderPrintDTO : BaseHeaderReportCommon.BaseHeader.BaseHeaderResult
    {
        public GLTR00100ResultPrintDTO GLTR { get; set; }
    }
}
