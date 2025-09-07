using Logistics.Data;
using Microsoft.EntityFrameworkCore;
using Logistics.Service.Helper;
using Logistics.Models;
using Microsoft.AspNetCore.Mvc; 

namespace Logistics.Service;

public class AssignmentHelperService: IAssignmentHelperService
{
    private readonly ILogger<AssignmentHelperService> _logger;
    private readonly IStatusHelperService _statusHelperService;
    private readonly ISQLHelperService _sqlHelperService;

    public AssignmentHelperService(ILogger<AssignmentHelperService> logger, IStatusHelperService statusHelperService, ISQLHelperService sqlHelperService)
    {
        _sqlHelperService = sqlHelperService;
        _statusHelperService = statusHelperService;
        _logger = logger;
    }

    public async Task<Result> AssignTruckForLocation(Dictionary<string, string> assignObject)
    {
        try
        {
            int currentPincode = Convert.ToInt32(assignObject["currentLocationPincode"]);
            //List<Truck> availableTruckList = await _statusHelperService.GetTruckDetailsForStatus(Convert.ToInt32(assignObject["currentLocationPincode"]),"Available");
            List<Truck> availableTruckList = await _sqlHelperService.GetAvailableTrucksForPincode(currentPincode);

            if(availableTruckList.Count != 0)
            {
                var isTruckUpdated = new Result{ IsSuccess = false };
                var isRouteUpdated = new Result{ IsSuccess = false };
                Dictionary<string, string> truckUpdate = new Dictionary<string, string>();
                truckUpdate["truckId"] = availableTruckList[0].TruckId;
                truckUpdate["driverStatus"] = "In Transit";
                truckUpdate["currentLocationPincode"] = assignObject["currentLocationPincode"];
                isTruckUpdated = await _statusHelperService.UpdateTruckStatus(truckUpdate);
                if(isTruckUpdated.IsSuccess)
                {
                    Dictionary<string, string> routeUpdate = new Dictionary<string, string>();
                    routeUpdate["routeId"] = assignObject["routeId"];
                    routeUpdate["routeStatus"] = "Initiated";
                    routeUpdate["routeTruckId"] = availableTruckList[0].TruckId;
                    isRouteUpdated = await _statusHelperService.UpdateRouteStatus(routeUpdate);
                    if(isRouteUpdated.IsSuccess)
                    {
                        return new Result
                        {
                            IsSuccess = true,
                            Message = "Truck assignment was successful",
                            StatusCode = 200
                        };
                    }

                }

                return new Result
                {
                    IsSuccess = false,
                    Message = "Truck assignment failed",
                    StatusCode = 500,
                    Exception = isTruckUpdated.Exception
                };
                
            }
            else{
                return new Result
                {
                    IsSuccess = false,
                    Message = "No trucks available at the location",
                    StatusCode = 500
                };
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

    public async Task<Result> AssignLoadsToRoute(Dictionary<string, string> assignObject)
    {
        try
        {
            assignObject["routeStatus"] = "Created";
            List<LoadsRoute> existingRoutes = await _sqlHelperService.GetExistingRouteDetails(assignObject);

            if(existingRoutes.Count > 0)
            {
                //Assign to the first existing route
                string routeId = existingRoutes[0].RouteId;
                var route = await _sqlHelperService.GetRouteDetails(routeId);
                var routeAssigned =  await _sqlHelperService.UpdateRouteDetails(route, assignObject);
                return await UpdateRouteDetailsInLoad(assignObject, routeAssigned, routeId);
            }
            else
            {
                //Create a new route
                Hubspot startHubspot = await _sqlHelperService.GetHubspotDetails(Convert.ToInt32(assignObject["loadPickupHubspot"]));
                Hubspot endHubspot = await _sqlHelperService.GetHubspotDetails(Convert.ToInt32(assignObject["loadDropHubspot"]));

                var newRoute = new LoadsRoute
                {
                    RouteId = Guid.NewGuid().ToString(),
                    RouteStartHubspot = startHubspot.HubspotName,
                    RouteEndHubspot = endHubspot.HubspotName,
                    RouteStatus = "Created",
                    RouteTotalWeight = Convert.ToInt32(assignObject["loadWeight"]),
                    RouteTruckId = "TRK_NULL",
                    RouteLoadIds = assignObject["loadId"],
                    RouteStartHubspotPincode = startHubspot.HubspotPincode,
                    RouteEndHubspotPincode = endHubspot.HubspotPincode,
                    RouteCoordinatorName = startHubspot.HubspotInchargeName,
                    RouteCoordinatorContactNo = startHubspot.HubspotContactNo
                };

                var routeAssigned =  await _sqlHelperService.CreateRouteDetails(newRoute);
                return await UpdateRouteDetailsInLoad(assignObject, routeAssigned, newRoute.RouteId);
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

    public async Task<Result> UpdateRouteDetailsInLoad(Dictionary<string, string> assignObject, Result routeAssigned, string routeId)
    {
        if(routeAssigned.IsSuccess)
        {
            return await _sqlHelperService.UpdateLoadDetails(assignObject["loadId"], routeId);
        }
        else
        {
            return new Result
            {
                IsSuccess = false,
                Message = "Load assignment to route failed",
                StatusCode = 500,
                Exception = routeAssigned.Exception
            };
        }
    }



}