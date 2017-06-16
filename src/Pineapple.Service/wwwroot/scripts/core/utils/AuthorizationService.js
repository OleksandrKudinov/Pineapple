angular
    .module("AuthorizationServiceModule")
    .factory("AuthorizationService",
    [
        "RequestCallbackInjector",
        "EntryPointProvider",
        "$rootScope",
        function (requestCallbackInjector, entryPointProvider, $rootScope) {
            var service = {};
            service.AuthorizeRequest = function (request) {

                if (!request.headers) {
                    request.headers = {};
                }

                var token = localStorage.getItem("access_token");

                if (token === null) {
                    console.log("NON AUTHORIZED!");
                    return;
                }

                request.headers["Authorization"] = "Bearer " + token;
                
                //TODO refactor this
                if (request.method == "post" || request.method == "POST") {
                    request.headers["Content-type"] = "application/json";
                }
            };

            service.Setup = function (data) {
                localStorage.setItem("access_token", data.access_token);
                localStorage.setItem("expires", data.expires);

                // TODO : may set auth data?
                $rootScope.$broadcast(GLOBALEVENTS.login, null);
            };

            service.Login = function (credentials, callback) {
                var url = entryPointProvider.GetEntryPoint() +
                    "auth/token";

                var request = {
                    data: JSON.stringify(credentials),
                    method: "post",
                    url: url
                };
                //TODO refactor this
                if (request.method == "post" || request.method == "POST") {
                    if (!request.headers) {
                        request.headers = {};
                    }
                    request.headers["Content-type"] = "application/json";
                }

                requestCallbackInjector.InjectCallbackToRequest(request, callback);

                $.ajax(request);
            };

            service.Logout = function () {
                $rootScope.$broadcast(GLOBALEVENTS.logout, null);
            }

            return service;
        }]
    );
