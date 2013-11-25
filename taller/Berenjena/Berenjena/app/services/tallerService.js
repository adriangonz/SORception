var module = angular.module("Taller.module", []);

module.service('Taller', ['$rootScope', function ($rootScope) {
   var service = {
   }
 
   return service;
}]);

module.config(['$routeProvider',
  function($routeProvider) {
    $routeProvider.
      when('/', {
        templateUrl: 'app/templates/main.html',
        controller: 'TallerCtrl'
      }).
      when('/config', {
          templateUrl: 'app/templates/config.html',
        controller: 'TallerCtrl'
      }).
      otherwise({
        redirectTo: '/'
      });
  }]);
