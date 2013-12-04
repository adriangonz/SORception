var module = angular.module( "scrap.module", [] );

module.service( 'Scrap', [ '$rootScope', function( $rootScope ) {
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
        templateUrl: 'assets/app/templates/main.html',
        controller: 'ScrapCtrl'
      }).
      when('/config', {
        templateUrl: 'assets/app/templates/config.html',
        controller: 'ScrapCtrl'
      }).
      otherwise({
        redirectTo: '/config'
      });
  }]);
