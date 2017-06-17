angular
    .module("AuthorizationServiceModule")
    .factory("AuthorizationService",
    [
        "AuthorizedServiceProvider",
        "$rootScope",
        function (authServiceProv, $rootScope) {
            var service = authServiceProv;

            service.Setup = function (data) {
                localStorage.setItem("access_token", data.access_token);
                localStorage.setItem("expires", data.expires);

                $rootScope.$broadcast(GLOBALEVENTS.login, data);
            };

            service._isAurhorizedCallback = function (isOk) {
                if (isOk) {
                    var data = {
                        "access_token": localStorage.getItem("access_token"),
                        "expires": localStorage.getItem("expires")
                    };

                    $rootScope.$broadcast(GLOBALEVENTS.login, data);
                } else {
                    $rootScope.$broadcast(GLOBALEVENTS.logout, null);
                }
            };

            service.IsAuthorized = function (callback) {
                var request =
                    {
                        url: "api/accounts/whoami",
                        method: "get"
                    };
                service.SendAuth(request, function (isOk, data) {
                    console.log(data);
                    service._isAurhorizedCallback(isOk);
                    callback(isOk, data);
                });
            };

            service.Login = function (credentials, callback) {
                var request = {
                    url: "auth/token",
                    method: "post",
                    data: JSON.stringify(credentials),
                };

                service.Send(request, callback);
            };

            service.Logout = function () {
                $rootScope.$broadcast(GLOBALEVENTS.logout, null);
            };

            return service;
        }
    ]);
