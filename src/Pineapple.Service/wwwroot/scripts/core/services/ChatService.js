angular
    .module("ChatServiceModule")
    .factory("ChatService",
        [
            "RequestCallbackInjector",
            "AuthorizationService",
            "EntryPointProvider",
            function (requestCallbackInjector, authorizeService, entryPointProvider) {
                var service = {};
                
                service.getChatsAsync = function (callback) {

                    var url = entryPointProvider.GetEntryPoint() +
                        "api/chats";

                    var request = {
                        method: "get",
                        url: url
                    };

                    requestCallbackInjector.InjectCallbackToRequest(request, callback);
                    authorizeService.AuthorizeRequest(request);
                    $.ajax(request);
                };
                
                return service;
            }
        ]);
