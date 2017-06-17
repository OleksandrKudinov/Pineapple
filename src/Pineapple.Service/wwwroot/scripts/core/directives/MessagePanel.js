angular
    .module("PineappleModule")
    .directive("messagePanel",
    [
        "ChatService",
        function (chatService) {
            return {
                controllerAs: "msgCtrl",
                templateUrl: "/templates/messagePanel.html",
                restrict: "A",
                controller: function ($scope, $rootScope) {
                    var context = this;

                    context.messages = [];

                    context._getMessagesFromChatCallback = function (isOk, messages) {
                        if (isOk) {
                            context.messages = messages;
                            $scope.$apply();
                        } else {
                            console.log("messages from chat: =(");
                            console.log(messages);
                        }
                    };

                    context.getMessagesFromChat = function (chatId) {
                        console.log("chatId: ");
                        console.log(chatId);
                        chatService.getMessagesFromChatAsync(chatId, context._getMessagesFromChatCallback);
                    };

                    $scope.$on(
                        GLOBALEVENTS.chatSelected,
                        function (event, args) {
                            context.getMessagesFromChat(args);
                        });
                }
            };
        }
    ]);