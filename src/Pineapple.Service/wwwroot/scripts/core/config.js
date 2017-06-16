angular.module("EntryPointProviderModule", []);
angular.module("RequestCallbackInjectorModule", []);
angular.module("AuthorizationServiceModule", ["EntryPointProviderModule", "RequestCallbackInjectorModule"]);

angular.module("ChatServiceModule", ["AuthorizationServiceModule", "RequestCallbackInjectorModule", "EntryPointProviderModule"]);

angular.module("PineappleModule", ["ChatServiceModule", "AuthorizationServiceModule"]);