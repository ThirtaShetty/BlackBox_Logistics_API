using Microsoft.AspNetCore.Mvc;
using Logistics.Service.Helper;
using Logistics.Models;

namespace Logistics.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TruckController : ControllerBase
{

    private readonly ILogger<TruckController> _logger;
    private readonly ISQLHelperService _sqlHelperService;
    private readonly IStatusHelperService _statusHelperService;

    public TruckController(ILogger<TruckController> logger, ISQLHelperService sqlHelperService, IStatusHelperService statusHelperService)
    {
        _logger = logger;
        _sqlHelperService = sqlHelperService;
        _statusHelperService = statusHelperService;
    }

    [HttpGet("GetTruckDetails")]
    public Task<Truck> GetTruckDetails(string truckId)
    {
        try{
            return _sqlHelperService.GetTruckDetails(truckId);
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error in GetTruckDetails: {ex.Message}");
            return Task.FromResult<Truck>(null);
        }

    }

    [HttpGet("GetTruckDetailsForStatus")]
    public Task<List<Truck>> GetTruckDetailsForStatus(int currentLocationPincode,string driverStatus)
    {
        try{
            return _statusHelperService.GetTruckDetailsForStatus(currentLocationPincode,driverStatus);
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error in GetTruckDetailsForStatus: {ex.Message}");
            return Task.FromResult<List<Truck>>(null);
        }

    }

    [HttpPut("CreateTruckDetails")]
    public async Task<Result> CreateTruckDetails([FromBody] Truck truck)
    {
        try
        {
            return await _sqlHelperService.CreateTruckDetails(truck);

        }
        catch(Exception ex)
        {
            _logger.LogError($"Error in CreateTruckDetails: {ex.Message}");
            return new Result
            {
                IsSuccess = false,
                Message = "Truck details creation failed",
                StatusCode = 500,
                Exception = ex.Message
            };
        }
    }

    [HttpPost("UpdateTruckStatus")]
    public async Task<Result> UpdateTruckStatus([FromBody] Dictionary<string, string> truckUpdate)
    {
        try{
            if (!truckUpdate.ContainsKey("truckId") || !truckUpdate.ContainsKey("driverStatus") || !truckUpdate.ContainsKey("currentLocationPincode") )
            {
                return new Result
                {
                    IsSuccess = false,
                    Message = "Truck details are incorrect",
                    StatusCode = 500,
                    Exception = "truckId, driverStatus and currentLocationPincode are required"
                };
            }
            else{

                return await _statusHelperService.UpdateTruckStatus(truckUpdate);
            }
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error in UpdateTruckStatus: {ex.Message}");
            return new Result
            {
                IsSuccess = false,
                Message = "Truck details update failed",
                StatusCode = 500,
                Exception = ex.Message
            };
        }

    }
}
