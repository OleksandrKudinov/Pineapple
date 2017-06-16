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

            return service;
        }
    ]);
