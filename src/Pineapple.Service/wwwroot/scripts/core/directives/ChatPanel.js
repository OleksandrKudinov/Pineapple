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

                    context.selectChat = function (chat) {
                        $rootScope.$broadcast(GLOBALEVENTS.chatSelected, chat.chatId);
                    };

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
                    
                    $scope.$on(
                        GLOBALEVENTS.login,
                        function (event, args) {
                            console.log("login!");
                            context.getChats();
                        });

                    $scope.$on(
                        GLOBALEVENTS.logout,
                        function (event, args) {
                            console.log("logout!");
                        });
                }
            };
        }
    ]);