module.service( 'Taller', [ '$rootScope', function( $rootScope ) {
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

