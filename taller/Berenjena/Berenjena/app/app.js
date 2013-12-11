var module = angular.module( "taller.module", [] );

module.config(['$routeProvider',
  function($routeProvider) {
    $routeProvider.
      when('/main', {
        templateUrl: 'app/templates/main.html',
        controller: 'TallerCtrl'
      }).
      when('/config', {
        templateUrl: 'app/templates/config.html',
        controller: 'SettingsCtrl'
      }).
      otherwise({
        redirectTo: '/config'
      });
  }]);