var GLOBALEVENTS =
    {
        login: "login",
        logout: "logout"
    };

angular.module("PineappleModule")
    .controller("AuthController",
    [
        "AuthorizationService",
        "$scope",
        function (authService, $scope) {
            var context = this;
            context.credentials = {};

            context._loginCallback = function (isOk, data) {
                if (isOk) {
                    authService.Setup(data);
                }
            };

            context.login = function () {
                console.log(context.credentials);
                authService.Login(context.credentials, context._loginCallback);
                context.credentials = {};
            };

            context.logout = function () {
                authService.Logout();
            };
        }]
    );