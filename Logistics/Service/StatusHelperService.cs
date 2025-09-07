using Logistics.Data;
using Microsoft.EntityFrameworkCore;
using Logistics.Service.Helper;
using Logistics.Models;
using Microsoft.AspNetCore.Mvc;

namespace Logistics.Service;

public class StatusHelperService: IStatusHelperService
{
    private readonly ILogger<StatusHelperService> _logger;
    private readonly SQLDbContext _context;
    public StatusHelperService(SQLDbContext context,ILogger<StatusHelperService> logger )
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<Load>> GetLoadDetailsForStatus(string loadStatus)
    {
        return await _context.Load.Where(l => l.LoadStatus.ToLower() == loadStatus.ToLower()).ToListAsync();
    }

    public async Task<List<Truck>> GetTruckDetailsForStatus(int currentLocationPincode, string driverStatus)
    {
        var query = _context.Truck.AsQueryable();

        if (!string.IsNullOrEmpty(currentLocationPincode.ToString()))
            query = query.Where(t => t.CurrentLocationPincode == currentLocationPincode);

        if (!string.IsNullOrEmpty(driverStatus))
            query = query.Where(t => t.DriverStatus.ToLower() == driverStatus.ToLower());

        return await query.ToListAsync();
    }

    public async Task<Result> UpdateTruckStatus(Dictionary<string, string> truckUpdate)
    {
        try
        {
            string id = truckUpdate["truckId"];
            string status = truckUpdate["driverStatus"];
            string currentLocationPincode = truckUpdate["currentLocationPincode"];

            var truck = await _context.Truck.FindAsync(id);
            truck.DriverStatus = status;
            truck.CurrentLocationPincode = Convert.ToInt32(currentLocationPincode);
            await _context.SaveChangesAsync();

            if(status.ToLower() == "breakdown")
            {
                truckUpdate["routeStatus"] = "Disrupted";
                truckUpdate["routeTruckId"] = "TRK_NULL";
                await UpdateRouteStatus(truckUpdate);
            }
            return new Result
            {
                IsSuccess = true,
                Message = "Truck status updated successfully",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in UpdateTruckStatus: {ex.Message}");
             return new Result
            {
                IsSuccess = false,
                Message = "Truck status update failed",
                StatusCode = 500,
                Exception = ex.Message
            };
        }
    }

    public async Task<List<LoadsRoute>> GetRouteDetailsForStatus(string truckId, string routeStatus)
    {
        var query = _context.LoadsRoute.AsQueryable();

        if (!string.IsNullOrEmpty(truckId))
            query = query.Where(t => t.RouteTruckId == truckId);

        if (!string.IsNullOrEmpty(routeStatus))
            query = query.Where(t => t.RouteStatus.ToLower() == routeStatus.ToLower());

        return await query.ToListAsync();    
    }

    public async Task<Result> UpdateRouteStatus(Dictionary<string, string> routeUpdate)
    {
        try
        {
            string id = routeUpdate["routeId"];
            string status = routeUpdate["routeStatus"];
        
            var route = await _context.LoadsRoute.FindAsync(id);
            if(routeUpdate.ContainsKey("routeTruckId"))
            {
                string truckId = routeUpdate["routeTruckId"];
                route.RouteTruckId = truckId;

            }
            route.RouteStatus = status;
            await _context.SaveChangesAsync();
            return new Result
            {
                IsSuccess = true,
                Message = "Route status updated successfully",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in UpdateRouteStatus: {ex.Message}");
             return new Result
            {
                IsSuccess = false,
                Message = "Route status update failed",
                StatusCode = 500,
                Exception = ex.Message
            };
        }
    }
}
