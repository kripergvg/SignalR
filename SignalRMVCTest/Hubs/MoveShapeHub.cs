using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace SignalRMVCTest.Hubs
{
    public class Broadcaster
    {
        private static readonly Lazy<Broadcaster> _istance = new Lazy<Broadcaster>(() => new Broadcaster());
        private readonly TimeSpan _broadcastIterval = TimeSpan.FromMilliseconds(1);
        private readonly IHubContext _hubContext;
        private Timer _broadcastLoop;
        private ShapeModel _model;
        private bool _modelUpdated;

        public Broadcaster()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<MoveShapeHub>();
            _model = new ShapeModel();
            _modelUpdated = false;
            _broadcastLoop = new Timer(BroadcastShape, null, _broadcastIterval, _broadcastIterval);
        }

        public void BroadcastShape(object state)
        {
            if (_modelUpdated)
            {
                _hubContext.Clients.AllExcept(_model.LastUpdatedBy).updateShape(_model);
                _modelUpdated = false;
            }
        }

        public void UpdateShape(ShapeModel clientModel)
        {
            _model = clientModel;
            _modelUpdated = true;
        }

        public static Broadcaster Instance
        {
            get { return _istance.Value; }
        }
    }

    public class MoveShapeHub : Hub
    {
        private Broadcaster _broadcaster;

        public MoveShapeHub()
            :this(Broadcaster.Instance)
        {
            
        }

        public MoveShapeHub(Broadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public void UpdateModel(ShapeModel shape)
        {
            shape.LastUpdatedBy = Context.ConnectionId;
            _broadcaster.UpdateShape(shape);
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