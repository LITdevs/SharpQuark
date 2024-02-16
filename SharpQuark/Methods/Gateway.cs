using System.Diagnostics;
using Newtonsoft.Json;
using SharpQuark.Objects;
using Websocket.Client;

namespace SharpQuark;

public partial class Lightquark
{
    private void GatewayMessage(ResponseMessage message)
    {
        Task.Run(() =>
        {
            if (message.Text == null) return;
            var baseMessage = JsonConvert.DeserializeObject<GatewayEventBase>(message.Text);
            if (baseMessage == null) throw new Exception($"Invalid event from gateway {message.Text}");

            switch (baseMessage.Event)
            {
                case "heartbeat":
                    Debug.WriteLine("Heartbeat received");
                    return;
                default:
                    Console.WriteLine($"Unimplemented event received {baseMessage.Event}");
                    break;
            }
        });
    }

    private void SendGateway(GatewayMessage message)
    {
        if (_client == null) _connectGateway();
        _client!.Send(JsonConvert.SerializeObject(message));
    }
    
}