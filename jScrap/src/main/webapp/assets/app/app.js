var module = angular.module( "scrap.module", ['ngCookies'] );

module.config(['$routeProvider',
  function($routeProvider) {
    $routeProvider.
      when('/login', {
        templateUrl: 'assets/app/templates/login.html',
      }).
      when('/main', {
        templateUrl: 'assets/app/templates/main.html',
        controller: 'ScrapCtrl'
      }).
      when('/config', {
        templateUrl: 'assets/app/templates/config.html',
        controller: 'SettingsCtrl'
      }).
      when('/orders', {
        templateUrl: 'assets/app/templates/orders.html',
        controller: 'OrdersCtrl'
      }).
      when('/order/:id', {
        templateUrl: 'assets/app/templates/create-offer.html',
        controller: 'CreateOfferCtrl'
      }).
      otherwise({
        redirectTo: '/login'
      });
  }]);