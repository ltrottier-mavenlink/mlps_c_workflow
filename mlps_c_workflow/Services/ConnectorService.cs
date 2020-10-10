using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc;
using Microsoft.Extensions.Logging;

namespace mlps_c_workflow
{
    public class ConnectorService : Connector.ConnectorBase
    {
        private readonly ILogger<ConnectorService> _logger;
        public ConnectorService(ILogger<ConnectorService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// used by Workflows to get triggers defined by the Connector. Run when a user registers or refreshes a Connector.
        /// </summary>
        /// <param name="request">The request received from the client.</param>
        /// <param name="context">The context of the server-side call handler being invoked.</param>
        /// <returns>The response to send back to the client (wrapped by a task).</returns>
        public override Task<TriggersResponse> triggers(TriggersRequest request, ServerCallContext context)
        {
            //return null;
            //return new Task<TriggersResponse>(() => { return new TriggersResponse(); });
            return Task.FromResult(new TriggersResponse());
        }

        /// <summary>
        /// used by Workflows to check for trigger events. Polled periodically.
        /// </summary>
        /// <param name="request">The request received from the client.</param>
        /// <param name="context">The context of the server-side call handler being invoked.</param>
        /// <returns>The response to send back to the client (wrapped by a task).</returns>
        public override Task<TriggerResponse> perform_trigger(TriggerRequest request, ServerCallContext context)
        {
            //return null;
            //return new Task<TriggerResponse>(() => { return new TriggerResponse(); });
            return Task.FromResult(new TriggerResponse());
        }

        /// <summary>
        /// used by Workflows to get get actions defined by the Connector. Run when a user registers or refreshes a Connector.
        /// </summary>
        /// <param name="request">The request received from the client.</param>
        /// <param name="context">The context of the server-side call handler being invoked.</param>
        /// <returns>The response to send back to the client (wrapped by a task).</returns>
        public override Task<ActionsResponse> actions(ActionsRequest request, ServerCallContext context)
        {

            //throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
            Field fullName = new Field
            {
                DisplayName = "Full Name",
                Key = "full_name",
                Description = "User's full name",
                Type = "text"
            };

            Field salutation = new Field
            {
                DisplayName = "Salutation",
                Key = "custom_field_value",
                Description = "User-specific salutation",
                Type = "text"
            };

            Grpc.Action sayHello = new Grpc.Action
            {
                DisplayName = "Say Hello",
                Description = "Returns a friendly hello"
            };
            sayHello.Inputs.Add(fullName);
            sayHello.Outputs.Add(salutation);

            Grpc.ActionsResponse resp = new ActionsResponse();
            resp.Actions.Add(sayHello);

            //return new Task<ActionsResponse>(() => { return resp; });
            //_logger.LogInformation($"Sending hello to {request.Name}");
            return Task.FromResult(resp);
        }

        /// <summary>
        /// used by Workflows to perform an action defined by the Connector. Run when a Workflow containing the defined action is run.
        /// </summary>
        /// <param name="request">The request received from the client.</param>
        /// <param name="context">The context of the server-side call handler being invoked.</param>
        /// <returns>The response to send back to the client (wrapped by a task).</returns>
        public override Task<ActionResponse> perform_action(ActionRequest request, ServerCallContext context)
        {
            //throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
            ActionResponse resp = new ActionResponse
            {
                Status = ActionResponse.Types.Status.Success
            };
            resp.Outputs.Add("custom_field_value", "Hello there " + request.Params["full_name"].ToString());

            //return new Task<ActionResponse>(() => { return resp; });
            //_logger.LogInformation($"Sending hello to {request.Name}");
            return Task.FromResult(resp);
        }
    }
}
