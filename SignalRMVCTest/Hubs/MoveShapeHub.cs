using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace SignalRMVCTest.Hubs
{

    public class MoveShapeHub : Hub
    {
        public void UpdateModel(ShapeModel shape)
        {
            shape.LastUpdatedBy = Context.ConnectionId;
            Clients.AllExcept(shape.LastUpdatedBy).updateShape(shape);
        }
    }

    public class ShapeModel
    {
        [JsonProperty("left")]
        public double Left { get; set; }
        [JsonProperty("top")]
        public double Top { get; set; }
        [JsonIgnore]
        public string LastUpdatedBy { get; set; }
    }
}