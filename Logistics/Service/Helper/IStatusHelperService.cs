using Logistics.Models;
using Microsoft.AspNetCore.Mvc;
namespace Logistics.Service.Helper
{
    public interface IStatusHelperService
    {
        public  Task<List<Load>> GetLoadDetailsForStatus(string loadStatus);
        public  Task<Result> UpdateTruckStatus(Dictionary<string, string> truckUpdate);
        public  Task<List<Truck>> GetTruckDetailsForStatus(int currentLocationPincode, string driverStatus);
        public  Task<List<LoadsRoute>> GetRouteDetailsForStatus(string truckId , string routeStatus);
        public  Task<Result> UpdateRouteStatus(Dictionary<string, string> routeUpdate);

    }

}



