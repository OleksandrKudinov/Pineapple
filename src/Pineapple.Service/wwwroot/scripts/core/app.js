var app = angular.module("PineappleModule");

app.controller("ChatController",
    [
        "ChatService",
        "AuthorizationService",
        "$scope",
        function (chatService, authService, $scope) {
            var context = this;
            context.chats = [];
            context.credentials = {};

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

            context._loginCallback = function(isOk, data) {
                console.log("login: " + isOk);
                console.log(data);
                authService.Setup(data);
                context.getChats();
            };

            context.login = function () {
                console.log(context.credentials);
                authService.Login(context.credentials, context._loginCallback);
                context.credentials = {};
            };
        }]
);