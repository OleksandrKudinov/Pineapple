angular
    .module("ChatServiceModule")
    .factory("ChatService",
    [
        "AuthorizedServiceProvider",
        function (serviceProvider) {
            var service = serviceProvider;

            service.getChatsAsync = function (callback) {
                var request = {
                    url: "api/chats",
                    method: "get"
                };

                service.SendAuth(request, callback);
            };

            service.getGetMessagesFromChatAsync = function (chatId, callback) {
                var request = {
                    url: "api/chats/" + chatId + "/messages",
                    method:"get"
                }

                service.SendAuth(request, callback);
            };
            
            service.getAllUsersFromChatAsync = function (chatId, callback) {
                var request = {
                    url: "api/chats/" + chatId + "/users",
                    method: "get"
                };

                service.SendAuth(request, callback);
            };

            service.SendMessageToChatAsync = function(chatId, message, callback) {
                var request = {
                    url: "api/chats/" + chatId + "/messages",
                    method: "post",
                    data: JSON.stringify(message)
                };

                service.SendAuth(request, callback);
            };

            return service;
        }
    ]);
