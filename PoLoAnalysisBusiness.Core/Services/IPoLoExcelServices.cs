using OfficeOpenXml;

namespace PoLoAnalysisBusiness.Core.Services;

public interface IPoLoExcelServices
{ 
    string CreateResultFolder(string path = @"C:\\Users\\Lenovo\\Downloads\\");
    void Dispose();
    void AnalyzeExcel();
    void SetFirstQuestionPosition(ExcelWorksheet worksheet);
    void SetLastQuestionPosition(ExcelWorksheet worksheet);
    void SetStudentPos(ExcelWorksheet worksheet);
    void SetPointMatrix(ExcelWorksheet worksheet);
    void CalculatePointResults();
    void SetPoloPosition(ExcelWorksheet worksheet);
    void SetPoLoMatchingPercentage(ref Dictionary<string, List<string>> dict);
    void SetPoMatrix(ExcelWorksheet worksheet);
    void SetLoMatrix(ExcelWorksheet worksheet);

    void SetPoloGraph(ExcelWorksheet worksheet, Dictionary<string, string> dict,
        Dictionary<string, List<string>> matrix, string title);

    void ConvertWorkSheetToWord(ExcelWorksheet worksheet);

}