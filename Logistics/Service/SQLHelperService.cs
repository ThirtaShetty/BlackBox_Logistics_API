using Logistics.Data;
using Microsoft.EntityFrameworkCore;
using Logistics.Service.Helper;
using Logistics.Models;
using Microsoft.AspNetCore.Mvc;

namespace Logistics.Service;

public class SQLHelperService: ISQLHelperService
{
    private readonly ILogger<SQLHelperService> _logger;
    private readonly SQLDbContext _context;
    public SQLHelperService(SQLDbContext context,ILogger<SQLHelperService> logger )
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Load> GetLoadDetails(string loadId)
    {
        return await _context.Load.FirstOrDefaultAsync(l => l.LoadId == loadId);
    }
    
    public async Task<List<Load>> GetLoadDetailsForRoute(string[] loadIds)
    {
        return await _context.Load.Where(u => loadIds.Contains(u.LoadId)).ToListAsync();
;
    }

    public async Task<Result> CreateLoadDetails(Load load)
    {
        var result = new Result();
        try
        {
            await _context.Load.AddAsync(load);
            await _context.SaveChangesAsync();
            result = new Result
            {
                IsSuccess = true,
                Message = "Load details created successfully",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in CreateLoadDetails: {ex.Message}");
            result = new Result
            {
                IsSuccess = false,
                Message = "Load details creation failed",
                StatusCode = 500,
                Exception = ex.Message
            };
        }

        return result;
    }


    public async Task<Result> UpdateLoadDetails(string loadId, string routeId)
    {
        try
        {
            Load load = await _context.Load.FindAsync(loadId);
            load.LoadRouteId = routeId;
            load.LoadStatus = "Assigned";
            await _context.SaveChangesAsync();
            return new Result
            {
                IsSuccess = true,
                Message = "Load assigned to route successfully",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in UpdateRouteDetails: {ex.Message}");
             return new Result
            {
                IsSuccess = false,
                Message = "Load assignment to route failed",
                StatusCode = 500,
                Exception = ex.Message
            };
        }
    }

    public async Task<Truck> GetTruckDetails(string truckId)
    {
        return await _context.Truck.FirstOrDefaultAsync(l => l.TruckId == truckId);
    }

    public async Task<List<Truck>> GetAvailableTrucksForPincode(int currentPincode){
        
        string pincodePrefix = currentPincode.ToString().Substring(0, 3);  // Considering first 3 digits of pincode to perform District level search.
        var trucks = await _context.Truck.Where(t => t.DriverStatus == "Available" 
             && t.CurrentLocationPincode.ToString().StartsWith(pincodePrefix))
            .OrderBy(t => Math.Abs(t.CurrentLocationPincode - currentPincode))
            .ToListAsync();

        return trucks;
    }


    public async Task<Result> CreateTruckDetails(Truck truck)
    {
        var result = new Result();
        try
        {
            await _context.Truck.AddAsync(truck);
            await _context.SaveChangesAsync();
            result = new Result
            {
                IsSuccess = true,
                Message = "Truck details created successfully",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in CreateTruckDetails: {ex.Message}");
             result = new Result
            {
                IsSuccess = false,
                Message = "Truck details creation failed",
                StatusCode = 500,
                Exception = ex.Message
            };
        }

        return result;
    }

    public async Task<LoadsRoute> GetRouteDetails(string routeId)
    {
        LoadsRoute route = await _context.LoadsRoute.FirstOrDefaultAsync(l => l.RouteId == routeId);
        string[] routeLoadIds = route.RouteLoadIds?.Split(',') ?? Array.Empty<string>();
        List<Load> loads = await GetLoadDetailsForRoute(routeLoadIds);
        route.RoutePickupSpots = string.Join(", ", loads.Select(l => l.LoadPickupHubspot).Where(h => !string.IsNullOrEmpty(h)).Distinct());
        route.RouteDropSpots = string.Join(", ", loads.Select(l => l.LoadDropHubspot).Where(h => !string.IsNullOrEmpty(h)).Distinct());
        return route;

    }
    public async Task<List<LoadsRoute>> GetAllRoutes()
    {
        List<LoadsRoute> routes = await _context.LoadsRoute.Where(l => l.RouteStatus.ToLower() != "completed").ToListAsync();
        return routes ;
    }

    public async Task<List<LoadsRoute>> GetExistingRouteDetails(Dictionary<string, string> assignObject)
    {
        decimal newLoadWeight = Convert.ToDecimal(assignObject["loadWeight"]);
        int pickupPincode = Convert.ToInt32(assignObject["loadPickupHubspotPincode"]);
        int dropPincode = Convert.ToInt32(assignObject["loadDropHubspotPincode"]);

        string startPincodePrefix = assignObject["loadPickupHubspotPincode"].ToString().Substring(0, 4);  // Considering first 4 digits of pincode to perform District+ Nearby Posts level search.
        string endPincodePrefix = assignObject["loadDropHubspotPincode"].ToString().Substring(0, 4);  // Considering first 4 digits of pincode to perform District+ Nearby Posts level search.
        var maxCapacity = 900; //in kgs

        List<LoadsRoute> existingRoutes = await _context.LoadsRoute
        .Where(r => r.RouteStatus == "Created" && (r.RouteTotalWeight + newLoadWeight) <= maxCapacity) // Filter first on weight and status
        .Select(r => new
        {
            Route = r,
            PickupDistance = Math.Abs(r.RouteStartHubspotPincode - pickupPincode),
            DropDistance = Math.Abs(r.RouteEndHubspotPincode - dropPincode)
        })
        .Select(x => new
        {
            x.Route,
            TotalDistanceScore = x.PickupDistance + x.DropDistance
        })
        .OrderBy(x => x.TotalDistanceScore)
        .Take(10)
        .Select(x => x.Route)
        .ToListAsync();

        return existingRoutes;
    }

    public async Task<Result> UpdateRouteDetails(LoadsRoute route, Dictionary<string, string> assignObject)
    {
        try
        {
            string loadId = assignObject["loadId"];
            decimal loadWeight = Convert.ToDecimal(assignObject["loadWeight"]);
            route.RouteTotalWeight += loadWeight;
            route.RouteLoadIds += "," + loadId;

            await _context.SaveChangesAsync();
            return new Result
            {
                IsSuccess = true,
                Message = "Load assigned to route: " + route.RouteId + " successfully",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in UpdateRouteDetails: {ex.Message}");
             return new Result
            {
                IsSuccess = false,
                Message = "Load assignment to route failed",
                StatusCode = 500,
                Exception = ex.Message
            };
        }
    }

    public async Task<Result> CreateRouteDetails(LoadsRoute route)
    {
        var result = new Result();
        try
        {
            await _context.LoadsRoute.AddAsync(route);
            await _context.SaveChangesAsync();
            result = new Result
            {
                IsSuccess = true,
                Message = "Route details created successfully",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in CreateRouteDetails: {ex.Message}");
             result = new Result
            {
                IsSuccess = false,
                Message = "Route details creation failed",
                StatusCode = 500,
                Exception = ex.Message
            };
        }

        return result;
    }

    public async Task<Hubspot> GetHubspotDetails(int hubspotPincode)
    {
        return await _context.Hubspot.FirstOrDefaultAsync(l => l.HubspotPincode == hubspotPincode);
    }
}
