using Microsoft.AspNetCore.Mvc;
using Logistics.Models;
using Logistics.Service.Helper;
namespace Logistics.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AssignmentController : ControllerBase
{

    private readonly ILogger< AssignmentController> _logger;
    private readonly  IAssignmentHelperService _assignmentHelperService;

    public  AssignmentController(ILogger< AssignmentController> logger,  IAssignmentHelperService assignmentHelperService)
    {
        _assignmentHelperService = assignmentHelperService;
        _logger = logger;
    }

    [HttpPost("AssignTruckForLocation")]
    public async Task<Result> AssignTruckForLocation([FromBody] Dictionary<string, string> assignObject)
    {
        try{
            if (!assignObject.ContainsKey("routeId") || !assignObject.ContainsKey("currentLocationPincode") )
            {
                return new Result
                {
                    IsSuccess = false,
                    Message = "Truck details are incorrect",
                    StatusCode = 500,
                    Exception = "routeId and currentLocationPincode are required"
                };
            }
            else
            {

                return await _assignmentHelperService.AssignTruckForLocation(assignObject);
            }
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error in AssignTruckForLocation: {ex.Message}");
            return new Result
            {
                IsSuccess = false,
                Message = "Truck assignment failed",
                StatusCode = 500,
                Exception = ex.Message
            };
        }

    }

    [HttpPost("AssignLoadsToRoute")]
    public async Task<Result> AssignLoadsToRoute([FromBody] Dictionary<string, string> assignObject)
    {
        try{
            if (!assignObject.ContainsKey("loadId") || !assignObject.ContainsKey("loadPickupHubspot") || !assignObject.ContainsKey("loadDropHubspot") || !assignObject.ContainsKey("loadWeight") )
            {
                return new Result
                {
                    IsSuccess = false,
                    Message = "Load details are incorrect",
                    StatusCode = 500,
                    Exception = "loadId, loadPickupHubspot, loadDropHubspot and loadWeight are required"
                };
            }
            else
            {
                return await _assignmentHelperService.AssignLoadsToRoute(assignObject);
            }

                
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error in AssignLoadsToRoute: {ex.Message}");
            return new Result
            {
                IsSuccess = false,
                Message = "Load assignment failed",
                StatusCode = 500,
                Exception = ex.Message
            };
        }
    }
}
