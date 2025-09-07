using Logistics.Models;
using Microsoft.AspNetCore.Mvc;

namespace Logistics.Service.Helper
{
    public interface IAssignmentHelperService
    {
        public  Task<Result> AssignTruckForLocation(Dictionary<string, string> assignObject);
        public  Task<Result> AssignLoadsToRoute(Dictionary<string, string> assignObject);
        public  Task<Result> UpdateRouteDetailsInLoad(Dictionary<string, string> assignObject, Result routeAssigned, string routeId);

    }

}



