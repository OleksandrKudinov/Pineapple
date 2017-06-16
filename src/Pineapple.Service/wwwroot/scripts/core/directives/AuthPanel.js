angular
    .module("PineappleModule")
    .directive("authPanel",
    [
        "AuthorizationService",
        function (authService) {
            return {
                controllerAs: "ctrl",
                templateUrl: "/templates/authPanel.html",
                restrict: "A",
                controller: function ($scope) {
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

                    context.checkAuth = function() {
                        authService.IsAuthorized(function(isOk, data) {
                            
                        });
                    };

                    context.checkAuth();
                }
            };
        }
    ]);