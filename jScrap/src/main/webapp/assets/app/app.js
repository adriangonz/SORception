var module = angular.module( "scrap.module", [] );

module.config(['$routeProvider',
  function($routeProvider) {
    $routeProvider.
      when('/login', {
        templateUrl: 'assets/app/templates/login.html',
        controller: 'ScrapCtrl'
      }).
      when('/main', {
        templateUrl: 'assets/app/templates/main.html',
        controller: 'ScrapCtrl'
      }).
      when('/config', {
        templateUrl: 'assets/app/templates/config.html',
        controller: 'SettingsCtrl'
      }).
      otherwise({
        redirectTo: '/login'
      });
  }]);