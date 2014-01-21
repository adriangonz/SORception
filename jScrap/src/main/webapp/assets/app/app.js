var module = angular.module( "scrap.module", ['ngCookies'] );

module.config(['$routeProvider',
  function($routeProvider) {
    $routeProvider.
      when('/login', {
        templateUrl: 'assets/app/templates/login.html',
      }).
      when('/config', {
        templateUrl: 'assets/app/templates/config.html',
        controller: 'SettingsCtrl'
      }).
      when('/orders', {
        templateUrl: 'assets/app/templates/orders.html',
        controller: 'OrdersCtrl'
      }).
      when('/offers', {
        templateUrl: 'assets/app/templates/offers.html',
        controller: 'OffersCtrl'
      }).
      when('/orders-received', {
        templateUrl: 'assets/app/templates/pedidos.html',
        controller: 'PedidosCtrl'
      }).
      when('/order/:id', {
        templateUrl: 'assets/app/templates/create-offer.html',
        controller: 'CreateOfferCtrl'
      }).
      when('/offer/:id', {
        templateUrl: 'assets/app/templates/view-offer.html',
        controller: 'ViewOfferCtrl'
      }).
      when('/orders-received/:id', {
        templateUrl: 'assets/app/templates/view-pedido.html',
        controller: 'ViewPedidoCtrl'
      }).
      otherwise({
        redirectTo: '/orders'
      });
  }]);