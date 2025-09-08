using Logistics.Models;
using Microsoft.AspNetCore.Mvc;
namespace Logistics.Service.Helper
{
    public interface ISQLHelperService
    {
        public  Task<Load> GetLoadDetails(string loadId);
        public Task<List<Load>> GetLoadDetailsForRoute(string[] loadIds);

        public Task<Result> CreateLoadDetails(Load load);
        public  Task<Result> UpdateLoadDetails(string loadId, string routeId);

        public  Task<Truck> GetTruckDetails(string truckId);
        public  Task<List<Truck>> GetAvailableTrucksForPincode(int currentPincode);
        public  Task<Result> CreateTruckDetails(Truck truck);

        public  Task<LoadsRoute> GetRouteDetails(string routeId);
        public Task<List<LoadsRoute>> GetAllRoutes();
        public Task<Result> CreateRouteDetails(LoadsRoute route);
        public  Task<List<LoadsRoute>> GetExistingRouteDetails(Dictionary<string, string> assignObject);
        public  Task<Result> UpdateRouteDetails(LoadsRoute route, Dictionary<string, string> assignObject);
        
        public  Task<Hubspot> GetHubspotDetails(int hubspotPincode);



    }

}



