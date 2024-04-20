using OfficeOpenXml;

namespace PoLoAnalysisBusiness.Core.Services;

public interface IPoLoExcelServices
{
    void SetFilePath(string path,string id);
    void AnalyzeExcel();


}