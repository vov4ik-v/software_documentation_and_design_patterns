using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Netflix.BusinessLogic.Interfaces;
using Netflix.Presentation.Attributes;
using Netflix.Presentation.DTO;
using System.IO;

namespace Netflix.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataImportController(IDataImportService dataImportService) : ControllerBase
{
    [HttpPost]
    [ApiKey]
    public async Task<ActionResult<ImportResultDto>> Import(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new ImportResultDto { Message = "Please upload a valid CSV file." });

        var tempFilePath = Path.GetTempFileName();
        
        try
        {
            await using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            await dataImportService.ImportFromCsvAsync(tempFilePath);
            return Ok(new ImportResultDto { Message = "Import completed successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ImportResultDto { Message = $"Import failed: {ex.Message}" });
        }
        finally
        {
            if (System.IO.File.Exists(tempFilePath))
            {
                System.IO.File.Delete(tempFilePath);
            }
        }
    }
}
