using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Logistics.Service.Helper;
using Logistics.Models;

namespace Logistics.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class RouteController : ControllerBase
{

    private readonly ILogger<RouteController> _logger;
    private readonly ISQLHelperService _sqlHelperService;
    private readonly IStatusHelperService _statusHelperService;

    public RouteController(ILogger<RouteController> logger, ISQLHelperService sqlHelperService, IStatusHelperService statusHelperService)
    {
        _logger = logger;
        _sqlHelperService = sqlHelperService;
        _statusHelperService = statusHelperService;
    }

    [HttpGet("GetRouteDetails")]
    public Task<LoadsRoute> GetRouteDetails(string routeId)
    {
        try
        {
            return _sqlHelperService.GetRouteDetails(routeId);
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error in GetRouteDetails: {ex.Message}");
            return Task.FromResult<LoadsRoute>(null);
        }

    }

    [HttpGet("GetRouteDetailsForStatus")]
    public Task<List<LoadsRoute>> GetRouteDetailsForStatus( string routeStatus, string? truckId = null)
    {
        try
        {
            return _statusHelperService.GetRouteDetailsForStatus(truckId, routeStatus);
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error in GetRouteDetailsForStatus: {ex.Message}");
            return Task.FromResult<List<LoadsRoute>>(null);
        }

    }

    [HttpGet("GetAllRoutes")]
    public Task<List<LoadsRoute>> GetAllRoutes()
    {
        try
        {
            return _sqlHelperService.GetAllRoutes();
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error in GetRouteDetailsForStatus: {ex.Message}");
            return Task.FromResult<List<LoadsRoute>>(null);
        }

    }

    [HttpPost("UpdateRouteStatus")]
    public async Task<Result> UpdateRouteStatus([FromBody] Dictionary<string, string> routeUpdate)
    {
        try{
            if (!routeUpdate.ContainsKey("routeId") || !routeUpdate.ContainsKey("routeStatus") )
            {
                return new Result
                {
                    IsSuccess = false,
                    Message = "Route details are incorrect",
                    StatusCode = 500,
                    Exception = "routeId, routeStatus are required"
                };
            }
            else{

                return await _statusHelperService.UpdateRouteStatus(routeUpdate);
            }
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error in UpdateRouteStatus: {ex.Message}");
            return new Result
            {
                IsSuccess = false,
                Message = "Route details update failed",
                StatusCode = 500,
                Exception = ex.Message
            };
        }

    }
}
