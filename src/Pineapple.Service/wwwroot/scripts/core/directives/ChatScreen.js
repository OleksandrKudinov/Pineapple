angular
    .module("PineappleModule")
    .directive("chatScreen",
    [
        "ChatService",
        function (chatService) {
            return {
                controllerAs: "chatCtrl",
                templateUrl: "/templates/chatScreen.html",
                restrict: "A",
                controller: function ($scope, $rootScope) {
                    var context = this;

                    context.chat = {};

                    context.reset = function () {
                        context.chat = {};
                        context.messages = [];
                        context.users = [];
                    };

                    context._getUsersFromChatAsyncCallback = function (isOk, users) {
                        if (isOk) {
                            context.users = users;
                            $scope.$apply();
                        } else {
                            console.log("cannot get users from chat: ");
                            console.log(users);
                        }
                    };

                    context.getUsers = function () {
                        chatService.getAllUsersFromChatAsync(context.chat.chatId, context._getUsersFromChatAsyncCallback);
                    };

                    context._getMessagesFromChatCallback = function (isOk, messages) {
                        if (isOk) {
                            context.messages = messages;
                            $scope.$apply();
                        } else {
                            console.log("messages from chat: =(");
                            console.log(messages);
                        }
                    };

                    context.getMessages = function () {
                        chatService.getMessagesFromChatAsync(context.chat.chatId, context._getMessagesFromChatCallback);
                    };

                    context.addUser = function (user) {

                    };

                    context.removeUser = function (user) {

                    };

                    context._sendMessageCallback = function (isOk, data) {
                        if (isOk) {
                            context.message = {};
                            context.getMessages();
                        } else {
                            console.log("cannot send message");
                            console.log(data);
                        }
                    };

                    context.sendMessage = function (message) {
                        chatService.sendMessageToChatAsync(context.chat.chatId, context.message, context._sendMessageCallback);
                    };

                    context._onChatSelected = function (event, args) {
                        console.log("selected chat:");
                        console.log(args);
                        context.chat = args;
                        context.getUsers();
                        context.getMessages();
                    };

                    $scope.$on(
                        GLOBALEVENTS.chatSelected,
                        context._onChatSelected);

                    $scope.$on(
                        GLOBALEVENTS.logout,
                        function (event, args) {
                            // TODO logout logic
                        });
                }
            };
        }
    ]);