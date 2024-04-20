using Microsoft.AspNetCore.Mvc;
using Novacode;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using PoLoAnalysisBusiness.Core.Models;
using PoLoAnalysisBusiness.Core.Repositories;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.Core.UnitOfWorks;
using PoLoAnalysisBusiness.DTO.Responses;
using Spire.Xls;
using File = System.IO.File;
using LicenseContext = OfficeOpenXml.LicenseContext;
using static Novacode.DocX;

namespace PoLoAnalysisBusiness.Services.Services;

public class ResultService:GenericService<Result>,IResultService
{
    private readonly string _resultFilename = "result.xlsx";
    private readonly IResultRepository _resultRepository;
    private readonly IUnitOfWork _unitOfWork;
    private ExcelWorkbook _workbook;
    private ExcelPackage _excelPackage;
    private ExcelWorksheets _worksheets;
    private const string QuestionsSearchTerm = "q1";
    private const string PoSearchTerm = "PÇ";
    private const string LoSearchTerm = "ÖÇ";
    private const string PoloAllKeyword = "TÜMÜ";
    private string _filePath;
    private List<List<float>> _questionPointMatrix = new();
    private List<float> _questionPoints = new() ;
    private Dictionary<string,string> _q1StartPos =  new ();
    private Dictionary<string,string> _qEndPos =  new ();
    private Dictionary<string,string> _studentEndcol =  new ();
    private Dictionary<string,string> _loPos =  new ();
    private Dictionary<string,List<string>> _poQuestionMatrix =  new ();
    private Dictionary<string,string> _poPos =  new ();
    private Dictionary<string,List<string>> _loQuestionMatrix =  new ();
    private List<float> _pointsAverageList = new();
    private List<float> _pointsMatchingPercentageList = new();
    public string ResultPath { get; set; }
    
    public ResultService(IResultRepository resultRepository, IUnitOfWork unitOfWork) : base(resultRepository, unitOfWork)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        _resultRepository = resultRepository;
        _unitOfWork = unitOfWork;
    }

    public string GetResultPath()
    {
        return ResultPath;
    }

    public async Task<FileStreamResult> GetFileStream(string fileId)
    {
        var entity = await _resultRepository.GetById(fileId); 
            
        if (entity == null)
            throw new Exception("File not found."); 

        var filePath = entity.Path+$"//result.docx"; 

        if (!System.IO.File.Exists(filePath))
            throw new Exception("File not found on disk.");

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var contentType = "application/octet-stream"; 


        return new FileStreamResult(fileStream, contentType)
        {
            FileDownloadName = Path.GetFileName(entity.Id+".docx")
        };    
    }

    public void SetFilePath(string filePath,string id)
    {
        _filePath = filePath;
        _excelPackage  = new ExcelPackage(new FileInfo(_filePath));
        _workbook = _excelPackage.Workbook;
        _worksheets = _workbook.Worksheets;
        
        var tempPath = Path.Combine(Directory.GetCurrentDirectory(), "../ResultFiles/");
        ResultPath = tempPath+$"/{id}";
        CreateResultFolder(ResultPath);
    }
    private string CreateResultFolder(string path = @"C:\\Users\\Lenovo\\Downloads\\")
    {
        try
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            return path;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "";
        }
        
    }
    private void Dispose()
    {
        _q1StartPos.Clear();
        _qEndPos.Clear();
        _studentEndcol.Clear();
        _loPos.Clear();
        _poPos.Clear();
        _poQuestionMatrix.Clear();
        _loQuestionMatrix.Clear();
        _questionPointMatrix.Clear();
        _questionPoints.Clear();
        _pointsAverageList.Clear();
        _pointsMatchingPercentageList.Clear();
    }
    public void AnalyzeExcel()
    {
        foreach (var worksheet in _worksheets)
        {
            SetFirstQuestionPosition(worksheet);
            SetLastQuestionPosition(worksheet);
            SetStudentPos(worksheet);
            SetPointMatrix(worksheet);
            CalculatePointResults();
            SetPoloPosition(worksheet);
            SetPoMatrix(worksheet);
            SetLoMatrix(worksheet);
            SetPoloGraph(worksheet,_loPos,_loQuestionMatrix,"Öğrenim Çıktıları Karşılama Yüzdesi");
            SetPoloGraph(worksheet,_poPos,_poQuestionMatrix,"Program Çıktıları Karşılama Yüzdesi");
            SaveImagesFromWorkSheet(worksheet);
            Dispose();
        }
        AppendImagesToTheWordFile();

    }

    public async Task<CustomResponseDto<Result?>> AddAsync(string fileId, string path)
    {
        var entity = new Result()
        {
            Id = Guid.NewGuid().ToString(),
            FileId = fileId,
            Path = ResultPath
        };
        return await AddAsync(entity, "mahmut");
        
    }

    private void SetFirstQuestionPosition(ExcelWorksheet worksheet)
    {
        var rowCount = worksheet.Dimension.Rows;
        var colCount = worksheet.Dimension.Columns;

        for (var row = 1; row <= rowCount; row++)
        {
            for (var col = 1; col <= colCount; col++)
            {
                var cellValue = worksheet.Cells[row, col].Value;

                if (cellValue == null ) 
                    continue;
                
                var cellString = cellValue.ToString().ToLower();

                if (cellString != QuestionsSearchTerm && (_q1StartPos.Count != 0 || cellString != "total"))
                    continue;
                _q1StartPos["row"] = (row).ToString();
                _q1StartPos["col"] = (col).ToString();
                _q1StartPos["addr"] = ExcelCellBase.GetAddress(row, col);
                return;
            }
        }

    }

    private void SetLastQuestionPosition(ExcelWorksheet worksheet)
    {
        if (_q1StartPos.Count == 0)
            return;
        var colCount = worksheet.Dimension.Columns;
        var colStart = int.Parse(_q1StartPos["col"]);
        
        for (var col =colStart; col <= colCount-1; col++)
        {
            var cellValue = worksheet.Cells[int.Parse(_q1StartPos["row"]), col].Value;
            if (cellValue == null ) 
                continue;
            
            var questionPoint = worksheet.Cells[int.Parse(_q1StartPos["row"])+1, col].Value;
            _questionPoints.Add(float.Parse(questionPoint.ToString()));
            _qEndPos["row"] = int.Parse(_q1StartPos["row"]).ToString();
            _qEndPos["col"] = col.ToString();
            _qEndPos["addr"] = ExcelCellBase.GetAddress(int.Parse(_q1StartPos["row"]), col);

        }
    }

    private void SetStudentPos(ExcelWorksheet worksheet)
    {
        /*
         * I know it seems ugly but when i tried to remove inner while loop somehow it crashes so if you find a solution feel free to implement it
         */
        var rowCount = worksheet.Dimension.Rows;
        var col = int.Parse(_q1StartPos["col"])-1;
        var row = int.Parse(_q1StartPos["row"]);

            while (row <= rowCount)
            {
                var cellValue = worksheet.Cells[row++, col].Value;
                if (cellValue == null ) 
                    continue;
                
                var cellString = cellValue.ToString().ToLower();
                
                if (!cellString.Contains("yüzdesi")) 
                    continue;
                
                _studentEndcol["row"] = (row-1).ToString();
                _studentEndcol["col"] = col.ToString();
                _studentEndcol["addr"] = ExcelCellBase.GetAddress(row-1, col);
                
                return;
            }
        
    }

    private void SetPointMatrix(ExcelWorksheet worksheet)
    {
        if (_q1StartPos.Count == 0 || _qEndPos.Count == 0 || _studentEndcol.Count == 0)
            return;
        
        var colStart = int.Parse(_q1StartPos["col"]);
        var colEnd = int.Parse(_qEndPos["col"]);
        var rowStart = int.Parse(_q1StartPos["row"]) + 2;
        var rowEnd = int.Parse(_studentEndcol["row"]) - 2;
        var columnPoints = new List<float>();
        
        for (var col = colStart; col <= colEnd; col++)
        {
            for (var row = rowStart; row <= rowEnd; row++)
            {
                var cellValue = worksheet.Cells[row, col].Value;
                    
                if (cellValue is not null and not "" and not " ")
                    columnPoints.Add(float.Parse(cellValue.ToString()));
            }
            _questionPointMatrix.Add(new List<float>(columnPoints)); 
            columnPoints.Clear();
        }
    }

    private void CalculatePointResults()
    {
        for (var i = 0; i < _questionPoints.Count; ++i)
        {
            var average = (float)Math.Round(_questionPointMatrix[i].Average()*10)/10;
            _pointsAverageList.Add(average);
            _pointsMatchingPercentageList.Add(average/_questionPoints[i]);
        }
    }

    private void SetPoloPosition(ExcelWorksheet worksheet)
    {
        
        var rowCount = worksheet.Dimension.Rows;
        var colCount = worksheet.Dimension.Columns;

        for (var row = 1; row <= rowCount; row++)
        {
            for (var col = 1; col <= colCount; col++)
            {
                var cellValue = worksheet.Cells[row, col].Value;
                if (cellValue == null ) 
                    continue;
                
                var cellString = cellValue.ToString().ToLower();
                
                if (cellString.Contains(LoSearchTerm.ToLower()) && _loPos.Count == 0)
                {
                    _loPos["row"] = (row).ToString();
                    _loPos["col"] = (col).ToString();
                    _loPos["addr"] = ExcelCellBase.GetAddress(row, col);
                } 
                if (cellString.Contains(PoSearchTerm.ToLower()) && _poPos.Count == 0)
                {
                    _poPos["row"] = (row).ToString();
                    _poPos["col"] = (col).ToString();
                    _poPos["addr"] = ExcelCellBase.GetAddress(row, col);
                }

                if (_poPos.Count != 0 && _loPos.Count != 0)
                    return;
                
            }
        }

    }

    private void SetPoLoMatchingPercentage(ref Dictionary<string,List<string>> dict)
    {
        if(dict.Count == 0)
            return;

        var keys = dict.Keys.ToList();

        foreach (var key in keys)
        {
            var questions = dict[key];
            var average = 0f;
            
            if (questions.First().ToLower() == PoloAllKeyword.ToLower())
            {
                average = _pointsMatchingPercentageList.Average();
                dict[key].Add(average.ToString());
                continue;
            }
           
            foreach (var question in questions)
                average += _pointsMatchingPercentageList[int.Parse(question)-1];

            average /= questions.Count;
            dict[key].Add(average.ToString());

        }
    }
    private void SetPoMatrix(ExcelWorksheet worksheet)
    {
        if (_poPos.Count == 0)
            return;
        
        var colStart = int.Parse(_poPos["col"]);
        var rowStart = int.Parse(_poPos["row"]) + 1;
        var rowEnd = worksheet.Dimension.Rows;
        
        for (var row = rowStart; row <= rowEnd; row++)
        {
            var poCellValue = worksheet.Cells[row, colStart].Value;
            var questionCellValue = worksheet.Cells[row, colStart+1].Value;
            var poMatchCellValue = worksheet.Cells[row, colStart+2].Value;
            if (poCellValue is null || questionCellValue is null || poMatchCellValue is null)
                continue;

            var poCellString = poCellValue.ToString().ToLower();
            var questionCellString = questionCellValue.ToString().ToLower();
            var poCount = poCellString.Split("-").Last();

            if (questionCellString.Contains(PoloAllKeyword.ToLower()))
                _poQuestionMatrix[poCount] = [PoloAllKeyword];
            else
            {
                var questionNo = questionCellString.Split(",").Select(question =>
                {
                    try
                    {
                        return question.Substring(1); // Return the result of Substring(1)
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("no problem question padding error");
                        return "wrongIndex"; 
                    }
                }).Where(result => result != "wrongIndex").ToList();                
                _poQuestionMatrix[poCount] = questionNo;
            }
        }
        SetPoLoMatchingPercentage(ref _poQuestionMatrix);

    }

    private void SetLoMatrix(ExcelWorksheet worksheet)
    {
        if (_loPos.Count == 0)
            return;
        
        var colStart = int.Parse(_loPos["col"]);
        var rowStart = int.Parse(_loPos["row"]) + 1;
        var rowEnd = worksheet.Dimension.Rows;
        
        for (var row = rowStart; row <= rowEnd; row++)
        {
            var loCellValue = worksheet.Cells[row, colStart].Value;
            var questionCellValue = worksheet.Cells[row, colStart+1].Value;
            var loMatchCellValue = worksheet.Cells[row, colStart+2].Value;
            if (loCellValue is null || questionCellValue is null || loMatchCellValue is null)
                break;

            var loCellString = loCellValue.ToString().ToLower();
            var questionCellString = questionCellValue.ToString().ToLower();
            var loCount = loCellString.Split("-").Last();
            
            if (questionCellString.Contains(PoloAllKeyword.ToLower()))
                _loQuestionMatrix[loCount] = [PoloAllKeyword];
            else
            {
                var questionNo = questionCellString.Split(",").Select(question =>
                {
                    try
                    {
                        return question.Substring(1); // Return the result of Substring(1)
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error Like q1, continue");
                        return "missingQuestion"; // Return a special value to indicate an error
                    }
                }).Where(result => result != "missingQuestion").ToList();

                _loQuestionMatrix[loCount] = questionNo;
            }
        }
        SetPoLoMatchingPercentage(ref _loQuestionMatrix);
    }

    private void SetPoloGraph(ExcelWorksheet worksheet,Dictionary<string,string>dict,Dictionary<string,List<string>> matrix,string title)
    {
        if (dict.Count == 0)
            return;
        
        var colStart = int.Parse(dict["col"]);
        var rowIter = int.Parse(dict["row"]) + 1;
        var rowStart = int.Parse(dict["row"]) + 1;
        var minValue = float.MaxValue;
        var maxValue = float.MinValue;
        
        foreach (var (_, value) in matrix)
        {
            var floatValue = float.Parse(value.Last());
            worksheet.Cells[rowIter, colStart + 3].Value = floatValue;
            minValue = Math.Min(minValue, floatValue);
            maxValue = Math.Max(maxValue, floatValue);
            rowIter++;
        }

        
        var chart = worksheet.Drawings.AddChart(title+Guid.NewGuid(), eChartType.ColumnClustered) as ExcelBarChart;
        chart.Title.Text = title;
        chart.SetSize(400, 250);
        chart.SetPosition(rowStart-3,0,int.Parse(dict["col"])+5,0);

        var yRange = $"{worksheet.Cells[rowStart, colStart + 3]}:{worksheet.Cells[rowIter - 1, colStart + 3].Address}";
        var xRange = $"{worksheet.Cells[rowStart, colStart]}:{worksheet.Cells[rowIter - 1, colStart].Address}";
        chart.YAxis.MaxValue = 1.0f;
        var series = chart.Series.Add(worksheet.Cells[yRange], worksheet.Cells[xRange]) as ExcelBarChartSerie;

        series.DataLabel.ShowValue = true;
        series.DataLabel.Format = "0.000";
        _excelPackage.SaveAs($@"{ResultPath}/{_resultFilename}");

    }
    private void SaveImagesFromWorkSheet(ExcelWorksheet worksheet)
    {
        var workbook = new Workbook();
        workbook.LoadFromFile($@"{ResultPath}\{_resultFilename}", ExcelVersion.Version2010);
        var sheet = workbook.Worksheets[worksheet.Name];
        var poImage = workbook.SaveChartAsImage(sheet, 1);
        var loImage = workbook.SaveChartAsImage(sheet, 0);

        using (var fileStream = File.Create($"{ResultPath}\\{worksheet.Name}_po.png"))
        {
            poImage.CopyTo(fileStream);
        }

        using (var fileStream2 = File.Create($"{ResultPath}\\{worksheet.Name}_lo.png"))
        {
            loImage.CopyTo(fileStream2);
        }

    }

    private void AppendImagesToTheWordFile()
    {
        var worksheets = _worksheets.Select(item => item.Name).ToList();

        foreach (var worksheet in worksheets)
        {
            using var document = DocX.Create($"{ResultPath}\\{worksheet}.docx");
            var imagePoPath = Path.Combine(ResultPath, worksheet+"_po.png");
            
            var documentImagePo = document.AddImage(imagePoPath);
            var documentPicturePo = documentImagePo.CreatePicture();

            
            var poTitle = document.InsertParagraph().Append($"{worksheet} Programming Outcomes Result");
            poTitle.Alignment = Alignment.center;

            var poParagraph = document.InsertParagraph();
            poParagraph.AppendLine().AppendPicture(documentPicturePo);
            poParagraph.AppendLine();
            
            
            var imageLoPath =  Path.Combine(ResultPath, worksheet+"_lo.png");
            var documentImageLo = document.AddImage(imageLoPath);
            var documentPictureLo = documentImageLo.CreatePicture();
            var loTitle = document.InsertParagraph().Append($"{worksheet} Learning Outcomes Result");
            loTitle.Alignment = Alignment.center;

            var loParagraph = document.InsertParagraph();
            loParagraph.AppendLine().AppendPicture(documentPictureLo);
            loParagraph.AppendLine();
            document.Save();
        }

    }
}