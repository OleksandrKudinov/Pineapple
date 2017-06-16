angular
    .module("PineappleModule")
    .directive("chatPanel",
    [
        "ChatService",
        function (chatService) {
            return {
                controllerAs: "chatController",
                templateUrl: "/templates/chatPanel.html",
                restrict: "A",
                controller: function ($scope, $rootScope) {
                    var context = this;

                    context.chats = [];

                    context._getChatsCallback = function (isOk, chats) {
                        if (isOk) {
                            context.chats = chats;
                            $scope.$apply();
                        } else {
                            console.log("chats: Error =(");
                            console.log(chats);
                        }
                    };

                    context.getChats = function () {
                        chatService.getChatsAsync(context._getChatsCallback);
                    };

                    context._onLogin = function () {
                        console.log("login!");
                        context.getChats();
                    };

                    context._onLogout = function () {
                        console.log("logout!");
                    };

                    $scope.$on(GLOBALEVENTS.login, context._onLogin);
                    $scope.$on(GLOBALEVENTS.logout, context._onLogout);
                }
            };
        }
    ]);