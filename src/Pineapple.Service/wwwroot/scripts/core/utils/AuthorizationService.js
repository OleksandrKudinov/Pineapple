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
