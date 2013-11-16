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
        templateUrl: '/resources/app/templates/main.html',
        controller: 'JunkyardCtrl'
      }).
      when('/add_agent', {
        templateUrl: '/resources/app/templates/add_agent.html',
        controller: 'AgentCtrl'
      }).
      otherwise({
        redirectTo: '/main'
      });
  }]);
