module.service( 'SettingsService', [ '$rootScope', '$http', function( $rootScope, $http ) {
   var service = {
     settings: {
     	"name": "",
     	"tokens": {}
 		},
 
     getSettings: function () {
        $http({method: 'GET', url: '/api/token'}).
		  success(function(data, status, headers, config) {
		    service.settings.name="DesguaceGET";
       		service.settings.tokens=data;
       		$rootScope.$broadcast( 'settings.update' );
		  }).
		  error(function(data, status, headers, config) {
		  	alert(status+"Valores por defecto");
		  	service.settings.name="DesguaceGET";
		  	service.settings.tokens = [
			   {
			        "Id": 1,
			        "Token": "TOKEN_1",
			        "Status": "REQUESTED", // 'REQUESTED', 'VALID' o 'EXPIRED'
			        "Created": "FECHA_1"
			   },
			   {
			        "Id": 2,
			        "Token": "TOKEN_2",
			        "Status": "EXPIRED", // 'REQUESTED', 'VALID' o 'EXPIRED'
			        "Created": "FECHA_2"
			   },
			   {
			        "Id": 3,
			        "Token": "TOKEN_3",
			        "Status": "VALID", // 'REQUESTED', 'VALID' o 'EXPIRED'
			        "Created": "FECHA_4"
			   }
			];
       		$rootScope.$broadcast( 'settings.update' );
		  });



      	

     },

     postSettings: function(){
     	  $http({method: 'POST', url: '/api/token'}).
          success(function(data, status, headers, config) {
            SettingsService.settings.name="DesguacePOST";
            SettingsService.settings.tokens=data;
       		$rootScope.$broadcast( 'settings.update' );
          }).
          error(function(data, status, headers, config) {
            alert(status);
          });
     }

   }
 

   return service;
}]);

