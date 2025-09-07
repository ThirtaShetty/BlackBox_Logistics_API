using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Logistics.Service.Helper;
using Logistics.Models;
namespace Logistics.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class LoadController : ControllerBase
{

    private readonly ILogger<LoadController> _logger;
    private readonly ISQLHelperService _sqlHelperService;
    private readonly IStatusHelperService _statusHelperService;

    public LoadController(ILogger<LoadController> logger, ISQLHelperService sqlHelperService, IStatusHelperService statusHelperService)
    {
        _logger = logger;
        _sqlHelperService = sqlHelperService;
        _statusHelperService = statusHelperService;
    }

    [HttpGet("GetLoadDetails")]
    public Task<Load> GetLoadDetails(string loadId)
    {
        try{
            return _sqlHelperService.GetLoadDetails(loadId);
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error in GetLoadDetails: {ex.Message}");
            return Task.FromResult<Load>(null);
        }

    }

    [HttpGet("GetLoadDetailsForStatus")]
    public Task<List<Load>> GetLoadDetailsForStatus(string loadStatus)
    {
        try{
            return _statusHelperService.GetLoadDetailsForStatus(loadStatus);
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error in GetLoadDetailsForStatus: {ex.Message}");
            return Task.FromResult<List<Load>>(null);
        }

    }

    [HttpPut("CreateLoadDetails")]
    public async Task<Result> CreateLoadDetails([FromBody] Load load)
    {
        try
        {
            return await _sqlHelperService.CreateLoadDetails(load);

        }
        catch(Exception ex)
        {
            _logger.LogError($"Error in CreateLoadDetails: {ex.Message}");
            return new Result
            {
                IsSuccess = false,
                Message = "Load details creation failed",
                StatusCode = 500,
                Exception = ex.Message
            };
        }
    }
}
