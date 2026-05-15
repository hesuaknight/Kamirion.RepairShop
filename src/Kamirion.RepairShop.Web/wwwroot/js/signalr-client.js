// signalr-client.js
// Base template for OperationalHub connectivity.
//
// Prerequisites:
//   Include @microsoft/signalr before this script:
//   <script src="~/lib/signalr/dist/browser/signalr.min.js"></script>
//
// Usage in a Razor page:
//   @section Scripts {
//     <script src="~/lib/signalr/dist/browser/signalr.min.js"></script>
//     <script src="~/js/signalr-client.js" asp-append-version="true"></script>
//     <script>
//       RepairShopHub.on('SomeEvent', function(payload) { /* handle */ });
//       RepairShopHub.start();
//     </script>
//   }

(function (window) {
    'use strict';

    const HUB_URL = '/hubs/operational';

    let _connection = null;
    let _startPromise = null;

    function buildConnection() {
        return new signalR.HubConnectionBuilder()
            .withUrl(HUB_URL)
            .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
            .configureLogging(signalR.LogLevel.Warning)
            .build();
    }

    // Returns a Promise that resolves when connected.
    // Safe to call multiple times — only one connection is created.
    function start() {
        if (_startPromise) return _startPromise;

        _connection = buildConnection();

        _connection.onreconnecting(() => console.warn('[SignalR] Reconnecting...'));
        _connection.onreconnected(() => console.info('[SignalR] Reconnected.'));
        _connection.onclose(err => {
            console.error('[SignalR] Connection closed.', err);
            _startPromise = null;
        });

        _startPromise = _connection.start()
            .then(() => console.info('[SignalR] Connected.'))
            .catch(err => {
                console.error('[SignalR] Start failed.', err);
                _startPromise = null;
                throw err;
            });

        return _startPromise;
    }

    // Joins a fine-grained group (e.g. 'board:{tenantId}:{branchId}').
    // The server validates tenant ownership; invalid groups abort the connection.
    async function joinGroup(groupName) {
        await start();
        return _connection.invoke('JoinGroup', groupName);
    }

    async function leaveGroup(groupName) {
        await start();
        return _connection.invoke('LeaveGroup', groupName);
    }

    // Registers a handler for a server-pushed method.
    async function on(method, handler) {
        await start();
        _connection.on(method, handler);
    }

    window.RepairShopHub = { start, joinGroup, leaveGroup, on };

})(window);
