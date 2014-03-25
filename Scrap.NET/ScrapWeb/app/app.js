var module = angular.module( "scrap.module", ['ngCookies', 'notificationWidget'] );

module.config(['$routeProvider',
  function($routeProvider) {
    $routeProvider.
      when('/login', {
        templateUrl: 'app/templates/login.html',
      }).
      when('/config', {
        templateUrl: 'app/templates/config.html',
        controller: 'SettingsCtrl'
      }).
      when('/orders', {
        templateUrl: 'app/templates/orders.html',
        controller: 'OrdersCtrl'
      }).
      when('/offers', {
        templateUrl: 'app/templates/offers.html',
        controller: 'OffersCtrl'
      }).
      when('/orders-received', {
        templateUrl: 'app/templates/pedidos.html',
        controller: 'PedidosCtrl'
      }).
      when('/order/:id', {
        templateUrl: 'app/templates/create-offer.html',
        controller: 'CreateOfferCtrl'
      }).
      when('/offer/:id', {
        templateUrl: 'app/templates/view-offer.html',
        controller: 'ViewOfferCtrl'
      }).
      when('/orders-received/:id', {
        templateUrl: 'app/templates/view-pedido.html',
        controller: 'ViewPedidoCtrl'
      }).
      otherwise({
        redirectTo: '/login'
      });
  }]);