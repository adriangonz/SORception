var module = angular.module( "scrap.module", ['ngCookies', 'notificationWidget'] );

module
.run(function($http, $cookies) {
  var header = angular.element('meta[name=_csrf_header]').attr('content'),
      token = angular.element('meta[name=_csrf]').attr('content');
  $http.defaults.headers.common[header] = token;
})
.config(['$routeProvider',
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