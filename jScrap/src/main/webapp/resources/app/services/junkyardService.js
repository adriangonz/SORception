var module = angular.module( "junkyard.module", [] );

module.service( 'Junkyard', [ '$rootScope', function( $rootScope ) {
   var service = {
     parts: [ 	
     ],
 
     addPart: function ( part ) {
       service.parts.push( part );
      $rootScope.$broadcast( 'parts.update' );
     }
   }
 
   return service;
}]);

module.config(['$routeProvider',
  function($routeProvider) {
    $routeProvider.
      when('/main', {
        templateUrl: r_main,
        controller: 'JunkyardCtrl'
      }).
      when('/config', {
        templateUrl: r_config,
        controller: 'JunkyardCtrl'
      }).
      otherwise({
        redirectTo: '/main'
      });
  }]);
